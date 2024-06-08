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
        
        public void SetInactive()
        {
            _isMoved = false;
            gameObject.SetActive(false);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            OnAsteroidCollisionEvent?.Invoke(this, other.gameObject);
        }
    }
}
