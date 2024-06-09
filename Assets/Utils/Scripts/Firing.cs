using System.Collections.Generic;
using BoltSpace;
using UnityEngine;

namespace UtilsSpace
{
    public abstract class Firing: MonoBehaviour
    {
        [SerializeField] private GameObject _boltPrefab;
        [SerializeField] private GameObject _boltsPrefab;
        [SerializeField] private Transform _boltStartPosition;
        [SerializeField] private int _initialPoolSize = 5;
        [SerializeField] private float _spawnInterval = 1f;
        
        private List<GameObject> _boltsPool;
        private GameObject _bolts;
        private float _timer;
        private bool _canFire;
        
        private void Start()
        {
            _bolts = Instantiate(_boltsPrefab);
            
            BoltsInit();
        }
        
        private void Update()
        {
            if (CheckCanFiring())
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

        protected abstract bool CheckCanFiring();
        
        private void SpawnBolt()
        {
            GameObject bolt = GetBolt();

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
        
        private GameObject GetBolt()
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