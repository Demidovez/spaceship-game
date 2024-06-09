using UnityEngine;

namespace BoltSpace
{
    public class Bolt : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private ESourceType _sourceType;
        
        private Rigidbody _rigidBody;
        private bool _isMoved;
        internal ESourceType SourceType => _sourceType;

        public enum ESourceType
        {
            None,
            Enemy,
            Player
        }
        
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
                float direction = transform.rotation.y < 0 ? -1 : 1;
                
                _rigidBody.velocity = Vector3.forward * (direction * _speed);
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

