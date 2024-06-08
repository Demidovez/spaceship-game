using System;
using AsteroidSpace;
using BoltSpace;
using PlayerSpace;
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
        [SerializeField] private MovingBoundary _movingBoundary;

        public static GameManagement Instance { get; private set; }
        public MovingBoundary MovingBoundary => _movingBoundary;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            LiveBoundary.OnLiveBoundaryCollisionEvent += OnLiveBoundaryCollision;
            Bolt.OnBoltCollisionEvent += OnBoltCollision;
            Asteroid.OnAsteroidCollisionEvent += OnAsteroidCollision;
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
                asteroid.SetInactive();
                return;
            }
        }
        
        private static void OnBoltCollision(Bolt bolt, GameObject other)
        {
            if (other.TryGetComponent(out Asteroid asteroid))
            {
                bolt.SetInactive();
                asteroid.SetInactive();
                return;
            }
        }
        
        private static void OnAsteroidCollision(Asteroid asteroid, GameObject other)
        {
            if (other == Player.Instance.gameObject)
            {
                asteroid.SetInactive();
                
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        
        private void OnDisable()
        {
            LiveBoundary.OnLiveBoundaryCollisionEvent -= OnLiveBoundaryCollision;
            Bolt.OnBoltCollisionEvent -= OnBoltCollision;
            Asteroid.OnAsteroidCollisionEvent -= OnAsteroidCollision;
        }
    }
}