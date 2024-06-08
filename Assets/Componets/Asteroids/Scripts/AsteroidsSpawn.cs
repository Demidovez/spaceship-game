using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        private void Start()
        {
            _timer = _spawnInterval;
            _spawnBounds = GetComponent<Collider>().bounds;
            
            AsteroidsInit();
        }
        
        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _spawnInterval)
            {
                _timer = 0;
                SpawnAsteroid();
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
                if (!asteroid.activeInHierarchy)
                {
                    return asteroid;
                }
            }

            return CreateAsteroid(_asteroidsPool.Count);
        }
        
        private GameObject CreateAsteroid(int index)
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

            return newAsteroid;
        }
    }
}

