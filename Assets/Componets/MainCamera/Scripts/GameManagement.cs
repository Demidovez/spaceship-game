using System;
using AsteroidSpace;
using BoltSpace;
using EnemySpace;
using PlayerSpace;
using PopupSpace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagementSpace
{
	[Serializable]
    public class MovingBoundary
    {
        public float xMin = -6f, xMax = 6f, zMin = -7f, zMax = 0f;
    }

    public class GameManagement: MonoBehaviour
    {
        [SerializeField] private MovingBoundary _movingPlayerBoundary;
        [SerializeField] private MovingBoundary _movingEnemyBoundary;

        public static GameManagement Instance { get; private set; }
        public MovingBoundary MovingPlayerBoundary => _movingPlayerBoundary;
        public MovingBoundary MovingEnemyBoundary => _movingEnemyBoundary;
        private bool _allEnemiesDestroyed;
        private bool _allAsteroidsDestroyed;
        private bool _isWin;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            LiveBoundary.OnLiveBoundaryCollisionEvent += OnLiveBoundaryCollision;
            Bolt.OnBoltCollisionEvent += OnBoltCollision;
            Asteroid.OnAsteroidCollisionEvent += OnAsteroidCollision;
            Enemy.OnEnemyCollisionEvent += OnEnemyCollision;
            Player.OnPlayerDeadEvent += OnPlayerDead;
            PopupsManagement.OnPopupNewGameEvent += OnStartNewGame;
            EnemiesSpawn.OnEnemiesPoolEmptyEvent += OnEnemiesPoolEmpty;
            AsteroidsSpawn.OnAsteroidsPoolEmptyEvent += OnAsteroidsPoolEmpty;
        }
        
        private void Update()
        {
            if (_allEnemiesDestroyed && _allAsteroidsDestroyed && !_isWin)
            {
                _isWin = true;
                PopupsManagement.Instance.ShowGameWinPopup();
            }
        }

        private static void OnLiveBoundaryCollision(GameObject other)
        {
            if (other.TryGetComponent(out Bolt bolt))
            {
                bolt.SetInactive();
                return;
            }
            
            if (other.TryGetComponent(out Asteroid asteroid))
            {
                asteroid.Die();
                return;
            }
            
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.Die();
                return;
            }
        }
        
        private static void OnBoltCollision(Bolt bolt, GameObject other)
        {
            if (other.TryGetComponent(out Asteroid asteroid))
            {
                bolt.SetInactive();
                asteroid.TakeDamage();
                return;
            }
            
            if (bolt.SourceType != Bolt.ESourceType.Enemy && other.TryGetComponent(out Enemy enemy))
            {
                bolt.SetInactive();
                enemy.TakeDamage();
                return;
            }
            
            if (bolt.SourceType != Bolt.ESourceType.Player && other == Player.Instance.gameObject)
            {
                bolt.SetInactive();
                Player.Instance.TakeDamage();
            }
        }
        
        private static void OnAsteroidCollision(Asteroid asteroid, GameObject other)
        {
            if (other == Player.Instance.gameObject)
            {
                asteroid.TakeDamage();
                Player.Instance.TakeDamage();
            }
        }
        
        private static void OnEnemyCollision(Enemy enemy, GameObject other)
        {
            if (other.TryGetComponent(out Asteroid asteroid))
            {
                enemy.TakeDamage();
                asteroid.TakeDamage();

                return;
            }
            
            if (other == Player.Instance.gameObject)
            {
                enemy.TakeDamage();
                Player.Instance.TakeDamage();
            }
        }
        
        private static void OnStartNewGame()
        {
            PopupsManagement.Instance.HidePopups();

            DOTween.Clear(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private static void OnPlayerDead(Player player)
        {
            PopupsManagement.Instance.ShowGameOverPopup();
        }
        
        private void OnEnemiesPoolEmpty()
        {
            _allEnemiesDestroyed = true;
        }
        
        private void OnAsteroidsPoolEmpty()
        {
            _allAsteroidsDestroyed = true;
        }
        
        private void OnDisable()
        {
            LiveBoundary.OnLiveBoundaryCollisionEvent -= OnLiveBoundaryCollision;
            Bolt.OnBoltCollisionEvent -= OnBoltCollision;
            Asteroid.OnAsteroidCollisionEvent -= OnAsteroidCollision;
            Enemy.OnEnemyCollisionEvent -= OnEnemyCollision;
            Player.OnPlayerDeadEvent -= OnPlayerDead;
            PopupsManagement.OnPopupNewGameEvent -= OnStartNewGame;
            EnemiesSpawn.OnEnemiesPoolEmptyEvent -= OnEnemiesPoolEmpty;
            AsteroidsSpawn.OnAsteroidsPoolEmptyEvent -= OnAsteroidsPoolEmpty;
        }
    }
}