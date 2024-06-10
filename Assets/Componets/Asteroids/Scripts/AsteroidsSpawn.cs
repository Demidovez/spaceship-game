using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GameManagementSpace;
using Unity.VisualScripting;
using UnityEngine;

namespace AsteroidSpace
{
    public class AsteroidsSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject _asteroid1Prefab;
        [SerializeField] private GameObject _asteroid2Prefab;
        [SerializeField] private GameObject _asteroid3Prefab;
        
        [SerializeField] private int _initialPoolSize = 15;
        [SerializeField] private float _spawnInterval = 2f;
        
        private List<GameObject> _asteroidsPool;
        private float _timer;
        private Bounds _spawnBounds;
        private bool _isSpawnCompleted;
        private int _countDestroyedAsteroids;
        
        public delegate void OnAsteroidsPoolEmpty();
        public static event OnAsteroidsPoolEmpty OnAsteroidsPoolEmptyEvent;
        
        private void OnEnable()
        {
            Asteroid.OnAsteroidDestroyedEvent += OnAsteroidDestroyed;
        }
        
        private void Start()
        {
            _spawnBounds = GetComponent<Collider>().bounds;
            
            AsteroidsInit();
        }
        
        private void Update()
        {
            if (_isSpawnCompleted || !GameManagement.Instance.CanSpawnAsteroids)
            {
                _timer = _spawnInterval;
                return;
            }
            
            _timer += Time.deltaTime;

            if (_timer >= _spawnInterval)
            {
                _timer = 0;
                SpawnAsteroid();
            }
        }

        private void OnAsteroidDestroyed()
        {
            _countDestroyedAsteroids++;

            if (_countDestroyedAsteroids >= _initialPoolSize)
            {
                OnAsteroidsPoolEmptyEvent?.Invoke();
            }
        }
        
        private void SpawnAsteroid()
        {
            GameObject asteroid = GetAsteroid();

            if (asteroid)
            {
                asteroid.transform.position = new Vector3(Random.Range(_spawnBounds.min.x, _spawnBounds.max.x), transform.position.y, transform.position.z);
                asteroid.transform.rotation = transform.rotation;
                asteroid.SetActive(true);
            }
            else
            {
                _isSpawnCompleted = true;
            }
        }
        
        private void AsteroidsInit()
        {
            _asteroidsPool = new List<GameObject>();

            for (int i = 0; i < _initialPoolSize; i++)
            {
                CreateAsteroid(i);
            }
        }
        
        private GameObject GetAsteroid()
        {
            foreach (var asteroid in _asteroidsPool)
            {
                if (!asteroid.IsDestroyed() && !asteroid.activeInHierarchy)
                {
                    return asteroid;
                }
            }

            return null;
        }
        
        private void CreateAsteroid(int index)
        {
            GameObject asteroidPrefab = (index % 3) switch
            {
                0 => _asteroid1Prefab,
                1 => _asteroid2Prefab,
                2 => _asteroid3Prefab,
                _ => _asteroid1Prefab
            };

            GameObject newAsteroid = Instantiate(asteroidPrefab, transform);
            newAsteroid.SetActive(false);
                
            _asteroidsPool.Add(newAsteroid);
        }
        
        private void OnDisable()
        {
            Asteroid.OnAsteroidDestroyedEvent -= OnAsteroidDestroyed;
        }
    }
}

