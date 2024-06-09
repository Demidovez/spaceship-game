using System;
using System.Collections;
using GameManagementSpace;
using UnityEngine;

namespace PlayerSpace
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _tilt = 7f;

        private Rigidbody _rb;
        public static Player Instance { get; private set; }
        public bool IsFiring { get; set; }
        private bool _isDead;
        
        public delegate void OnPlayerTookDamage(Player player);
        public static event OnPlayerTookDamage OnPlayerTookDamageEvent;
        
        public delegate void OnPlayerDead(Player player);
        public static event OnPlayerDead OnPlayerDeadEvent;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void Move(Vector3 movement)
        {
            var boundary = GameManagement.Instance.MovingPlayerBoundary;
            
            _rb.velocity = movement * _speed;

            _rb.position = new Vector3
            (
                Mathf.Clamp(_rb.position.x, boundary.xMin,  boundary.xMax),
                0,
                Mathf.Clamp(_rb.position.z,  boundary.zMin,  boundary.zMax)
            );
            
            _rb.rotation = Quaternion.Euler(Mathf.Min(_rb.velocity.z, 0) * _tilt,0,_rb.velocity.x * -_tilt);
        }
        
        private void Die()
        {
            OnPlayerDeadEvent?.Invoke(this);
        }

        public void TakeDamage()
        {
            if (_isDead)
            {
                return;
            }
            
            OnPlayerTookDamageEvent?.Invoke(this);
            gameObject.SetActive(false);
            _isDead = true;

            Invoke(nameof(Die), 1.5f);
        }
    }
}