using System.Collections;
using GameManagementSpace;
using Unity.VisualScripting;
using UnityEngine;

namespace EnemySpace
{
    public class Enemy : MonoBehaviour
    {
        [Header("Moving")]
        [SerializeField] private float _speedMin = 1f;
        [SerializeField] private float _speedMax = 4f;
        [SerializeField] private float _rotationY = -180f;

        [Header("Tilt")]
        [SerializeField] private float _tilt;
        [SerializeField] private float _tiltMin;
        [SerializeField] private float _tiltMax;
        
        [Header("Trajectory")]
        [SerializeField] private float _dodgeMin;
        [SerializeField] private float _dodgeMax;
        [SerializeField] private float _smoothing;
        [SerializeField] private Vector2 _startWait;
        [SerializeField] private Vector2 _waveTimer;
        [SerializeField] private Vector2 _waveWait;

        private Rigidbody _rigidBody;
        private float _speed;
        private float _targetWave;
        
        public delegate void OnEnemyTookDamage(Enemy enemy);
        public static event OnEnemyTookDamage OnEnemyTookDamageEvent;
        
        public delegate void OnEnemyCollision(Enemy enemy, GameObject other);
        public static event OnEnemyCollision OnEnemyCollisionEvent;
        
        public delegate void OnEnemyDestroyed();
        public static event OnEnemyDestroyed OnEnemyDestroyedEvent;
        
        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _speed = Random.Range(_speedMin, _speedMax);
            
            StartCoroutine(Evade());
        }

        private IEnumerator Evade()
        {
            yield return new WaitForSeconds(Random.Range(_startWait.x, _startWait.y));

            while (!gameObject.IsDestroyed())
            {
                _targetWave = Random.Range(_dodgeMin, _dodgeMax) * -Mathf.Sign(transform.position.x);
                yield return new WaitForSeconds(Random.Range(_waveTimer.x, _waveTimer.y));
                
                _targetWave = 0;
                yield return new WaitForSeconds(Random.Range(_waveWait.x, _waveWait.y));
            }
        }
        
        private void FixedUpdate()
        {
            var maneuver = Mathf.MoveTowards(_rigidBody.velocity.x, _targetWave, _smoothing * Time.deltaTime);
            var boundary = GameManagement.Instance.MovingEnemyBoundary;

            _rigidBody.velocity = new Vector3(maneuver, 0.0f, -_speed);
            _rigidBody.position = new Vector3(Mathf.Clamp(_rigidBody.position.x, boundary.xMin, boundary.xMax), 0f, _rigidBody.position.z);
            _rigidBody.rotation = Quaternion.Euler(0, _rotationY, Mathf.Clamp(_rigidBody.velocity.x * 10 * _tilt, _tiltMin, _tiltMax));
        }
        
        private void OnTriggerEnter(Collider other)
        {
            OnEnemyCollisionEvent?.Invoke(this, other.gameObject);
        }
        
        public void Die()
        {
            OnEnemyDestroyedEvent?.Invoke();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        
        public void TakeDamage()
        {
            OnEnemyTookDamageEvent?.Invoke(this);
            Die();
        }
    }
}
