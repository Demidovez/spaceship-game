using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public class PlayerFiring: MonoBehaviour
    {
        [SerializeField] private GameObject _boltPrefab;
        [SerializeField] private GameObject _boltsPrefab;
        [SerializeField] private Transform _boltStartPosition;
        [SerializeField] private int _initialPoolSize = 20;
        [SerializeField] private float _spawnInterval = 0.5f;
        
        private List<GameObject> _boltsPool;
        private GameObject _bolts;
        private float _timer;
        
        private void Start()
        {
            _bolts = Instantiate(_boltsPrefab);
            
            BoltsInit();
        }
        
        private void Update()
        {
            if (Player.Instance.IsFiring)
            {
                _timer += Time.deltaTime;

                if (_timer >= _spawnInterval)
                {
                    _timer = 0;
                    SpawnBolt();
                }
            } else {
                _timer = _spawnInterval;
            }
        }
        
        private void SpawnBolt()
        {
            GameObject bolt = GetBullet();

            if (bolt)
            {
                bolt.transform.position = _boltStartPosition.position;
                bolt.transform.rotation = _boltStartPosition.rotation;
                bolt.SetActive(true);
            }
        }
        
        private void BoltsInit()
        {
            _boltsPool = new List<GameObject>();

            for (int i = 0; i < _initialPoolSize; i++)
            {
                CreateBolt();
            }
        }
        
        private GameObject GetBullet()
        {
            foreach (var bolt in _boltsPool)
            {
                if (!bolt.activeInHierarchy)
                {
                    return bolt;
                }
            }

            return CreateBolt();
        }
        
        private GameObject CreateBolt()
        {
            GameObject newBolt = Instantiate(_boltPrefab, _bolts.transform);
            newBolt.SetActive(false);
                
            _boltsPool.Add(newBolt);

            return newBolt;
        }
    }
}