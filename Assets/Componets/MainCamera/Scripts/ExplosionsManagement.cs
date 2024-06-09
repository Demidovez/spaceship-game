using AsteroidSpace;
using EnemySpace;
using PlayerSpace;
using UnityEngine;

namespace GameManagementSpace
{
    public class ExplosionsManagement : MonoBehaviour
    {
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
            Instantiate(_explosionAsteroidPrefab, asteroid.transform.position, asteroid.transform.rotation);
        }
        
        private void OnPlayerTookDamage(Player player)
        {
            Instantiate(_explosionPlayerPrefab, player.transform.position, player.transform.rotation);
        }
        
        private void OnEnemyTookDamage(Enemy enemy)
        {
            Instantiate(_explosionEnemyPrefab, enemy.transform.position, enemy.transform.rotation);
        }
        
        private void OnDisable()
        {
            Asteroid.OnAsteroidTookDamageEvent -= OnAsteroidTookDamage;
            Player.OnPlayerTookDamageEvent -= OnPlayerTookDamage;
            Enemy.OnEnemyTookDamageEvent -= OnEnemyTookDamage;
        }
    }  
}

