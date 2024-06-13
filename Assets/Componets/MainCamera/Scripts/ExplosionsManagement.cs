using System.Collections;
using AsteroidSpace;
using EnemySpace;
using PlayerSpace;
using UnityEngine;

namespace GameManagementSpace
{
    public class ExplosionsManagement : MonoBehaviour
    {
        [SerializeField] private float _explosionLiveTime = 2f;
        
        [Header("Explosions")]
        [SerializeField] private GameObject _explosionAsteroidPrefab;
        [SerializeField] private GameObject _explosionEnemyPrefab;
        [SerializeField] private GameObject _explosionPlayerPrefab;

        private void OnEnable()
        {
            Asteroid.OnAsteroidTookDamageEvent += OnAsteroidTookDamage;
            Player.OnPlayerTookDamageEvent += OnPlayerTookDamage;
            Enemy.OnEnemyTookDamageEvent += OnEnemyTookDamage;
        }

        private void OnAsteroidTookDamage(Asteroid asteroid)
        {
            ShowExplosion(_explosionAsteroidPrefab, asteroid.gameObject);
        }
        
        private void OnPlayerTookDamage(Player player)
        {
            ShowExplosion(_explosionPlayerPrefab, player.gameObject);
        }
        
        private void OnEnemyTookDamage(Enemy enemy)
        {
            ShowExplosion(_explosionEnemyPrefab, enemy.gameObject);
        }

        private void ShowExplosion(GameObject explosionPrefab, GameObject sourceObj)
        {
            GameObject explosionObj = Instantiate(explosionPrefab, sourceObj.transform.position, sourceObj.transform.rotation);

            StartCoroutine(DestroyExplosion(explosionObj));
        }

        private IEnumerator DestroyExplosion(GameObject obj)
        {
            yield return new WaitForSeconds(_explosionLiveTime);
            Destroy(obj);
        }
        
        private void OnDisable()
        {
            Asteroid.OnAsteroidTookDamageEvent -= OnAsteroidTookDamage;
            Player.OnPlayerTookDamageEvent -= OnPlayerTookDamage;
            Enemy.OnEnemyTookDamageEvent -= OnEnemyTookDamage;
        }
    }  
}

