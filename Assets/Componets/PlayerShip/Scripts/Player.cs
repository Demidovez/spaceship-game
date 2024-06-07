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
                Mathf.Clamp(_rb.position.x, GameManagement.Instance.Boundary.xMin,  GameManagement.Instance.Boundary.xMax),
                0,
                Mathf.Clamp(_rb.position.z,  GameManagement.Instance.Boundary.zMin,  GameManagement.Instance.Boundary.zMax)
            );
            
            _rb.rotation = Quaternion.Euler(Mathf.Min(_rb.velocity.z, 0) * _tilt,0,_rb.velocity.x * -_tilt);
        }
    }
}