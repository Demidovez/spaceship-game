using System;
using System.Collections;
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
        [Header("Boundaries")]
        [SerializeField] private MovingBoundary _movingPlayerBoundary;
        [SerializeField] private MovingBoundary _movingEnemyBoundary;

        [Header("Spawns")] 
        [SerializeField] private float _secondsToStartWave = 2f;
        [SerializeField] private float _secondsToSpawnAsteroids = 3f;
        [SerializeField] private float _secondsToSpawnEnemies = 3f;
        [SerializeField] private float  _secondsToWaitWave = 3f;

        public static GameManagement Instance { get; private set; }
        public MovingBoundary MovingPlayerBoundary => _movingPlayerBoundary;
        public MovingBoundary MovingEnemyBoundary => _movingEnemyBoundary;
        private bool _allEnemiesDestroyed;
        private bool _allAsteroidsDestroyed;
        private bool _isWin;
        public bool CanSpawnAsteroids { get; private set; }
        public bool CanSpawnEnemies { get; private set; }

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

        private void Start()
        {
            StartCoroutine(SpawnsPermissions());
        }

        private void Update()
        {
            if (_allEnemiesDestroyed && _allAsteroidsDestroyed && !_isWin)
            {
                _isWin = true;
                PopupsManagement.Instance.ShowGameWinPopup();
            }
        }
        
        private IEnumerator SpawnsPermissions()
        {
            yield return new WaitForSeconds(_secondsToStartWave);
            
            while (!Player.Instance.IsDead)
            {
                CanSpawnAsteroids = true;
                
                yield return new WaitForSeconds(_secondsToSpawnAsteroids);
                CanSpawnAsteroids = false;
                CanSpawnEnemies = true;
                
                yield return new WaitForSeconds(_secondsToSpawnEnemies);
                CanSpawnAsteroids = false;
                CanSpawnEnemies = false;
                
                yield return new WaitForSeconds(_secondsToWaitWave);
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

        private void ShowGameOverPopup()
        {
            PopupsManagement.Instance.ShowGameOverPopup();
        }

        private void OnPlayerDead(Player player)
        {
            Invoke(nameof(ShowGameOverPopup), 1.5f);
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