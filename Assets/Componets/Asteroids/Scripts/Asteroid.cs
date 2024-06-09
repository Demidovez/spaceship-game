using UnityEngine;

namespace AsteroidSpace
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private float _speedMin = 1f;
        [SerializeField] private float _speedMax = 4f;

        private Rigidbody _rigidBody;
        private float _speed;
        private bool _isMoved;
        
        public delegate void OnAsteroidCollision(Asteroid asteroid, GameObject other);
        public static event OnAsteroidCollision OnAsteroidCollisionEvent;
        
        public delegate void OnAsteroidTookDamage(Asteroid asteroid);
        public static event OnAsteroidTookDamage OnAsteroidTookDamageEvent;
        
        public delegate void OnAsteroidDestroyed();
        public static event OnAsteroidDestroyed OnAsteroidDestroyedEvent;
        
        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }
        
        private void FixedUpdate()
        {
            if (!_isMoved)
            {
                _speed = Random.Range(_speedMin, _speedMax);
                
                _rigidBody.velocity = transform.forward * -_speed;
                _isMoved = true;
            }
        }
        
        public void Die()
        {
            OnAsteroidDestroyedEvent?.Invoke();
            Destroy(gameObject);
        }
        
        public void TakeDamage()
        {
            OnAsteroidTookDamageEvent?.Invoke(this);
            Die();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            OnAsteroidCollisionEvent?.Invoke(this, other.gameObject);
        }
    }
}
