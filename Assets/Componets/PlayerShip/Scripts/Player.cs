using System;
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
            _rb.velocity = movement * _speed;

            _rb.position = new Vector3
            (
                Mathf.Clamp(_rb.position.x, GameManagement.Instance.MovingBoundary.xMin,  GameManagement.Instance.MovingBoundary.xMax),
                0,
                Mathf.Clamp(_rb.position.z,  GameManagement.Instance.MovingBoundary.zMin,  GameManagement.Instance.MovingBoundary.zMax)
            );
            
            _rb.rotation = Quaternion.Euler(Mathf.Min(_rb.velocity.z, 0) * _tilt,0,_rb.velocity.x * -_tilt);
        }
    }
}