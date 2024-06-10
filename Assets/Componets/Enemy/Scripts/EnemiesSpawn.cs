using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GameManagementSpace;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace EnemySpace
{
    public class EnemiesSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyPrefab;
        
        [SerializeField] private int _initialPoolSize = 15;
        [SerializeField] private float _spawnInterval = 2f;
        
        private List<GameObject> _enemiesPool;
        private float _timer;
        private Bounds _spawnBounds;
        private bool _isSpawnCompleted;
        private int _countDestroyedEnemies;
        
        public delegate void OnEnemiesPoolEmpty();
        public static event OnEnemiesPoolEmpty OnEnemiesPoolEmptyEvent;

        private void OnEnable()
        {
            Enemy.OnEnemyDestroyedEvent += OnEnemyDestroyed;
        }

        private void Start()
        {
            _spawnBounds = GetComponent<Collider>().bounds;
            
            EnemiesInit();
        }
        
        private void Update()
        {
            if (_isSpawnCompleted || !GameManagement.Instance.CanSpawnEnemies)
            {
                _timer = _spawnInterval;
                return;
            }
            
            _timer += Time.deltaTime;

            if (_timer >= _spawnInterval)
            {
                _timer = 0;
                SpawnEnemy();
            }
        }

        private void OnEnemyDestroyed()
        {
            _countDestroyedEnemies++;

            if (_countDestroyedEnemies >= _initialPoolSize)
            {
                OnEnemiesPoolEmptyEvent?.Invoke();
            }
        }
        
        private void SpawnEnemy()
        {
            GameObject enemy = GetEnemy();

            if (enemy)
            {
                enemy.transform.position = new Vector3(Random.Range(_spawnBounds.min.x, _spawnBounds.max.x), transform.position.y, transform.position.z);
                enemy.SetActive(true);
            }
            else
            {
                _isSpawnCompleted = true;
            }
        }
        
        private void EnemiesInit()
        {
            _enemiesPool = new List<GameObject>();

            for (int i = 0; i < _initialPoolSize; i++)
            {
                CreateEnemy();
            }
        }
        
        private GameObject GetEnemy()
        {
            foreach (var enemy in _enemiesPool)
            {
                if (!enemy.IsDestroyed() && !enemy.activeInHierarchy)
                {
                    return enemy;
                }
            }

            return null;
        }
        
        private void CreateEnemy()
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, transform);
            newEnemy.SetActive(false);
                
            _enemiesPool.Add(newEnemy);
        }

        private void OnDisable()
        {
            Enemy.OnEnemyDestroyedEvent -= OnEnemyDestroyed;
        }
    }
}

