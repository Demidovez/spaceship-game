using UnityEngine;

namespace BoltSpace
{
    public class Bolt : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        
        private Rigidbody _rigidBody;
        private bool _isMoved;
        
        public delegate void OnBoltCollision(Bolt bolt, GameObject other);
        public static event OnBoltCollision OnBoltCollisionEvent;
        
        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }
        
        private void FixedUpdate()
        {
            if (!_isMoved)
            {
                _rigidBody.velocity = Vector3.forward * _speed;
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
            OnBoltCollisionEvent?.Invoke(this, other.gameObject);
        }
    }
}

