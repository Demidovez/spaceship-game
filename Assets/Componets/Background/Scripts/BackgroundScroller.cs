using System;
using UnityEngine;

namespace BackgroundSpace
{
    public class BackgroundScroller : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _height;

        private Vector3 _startPosition;

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            float newPosition = Mathf.Repeat(Time.time * _speed, _height);
            transform.position = _startPosition + Vector3.forward * newPosition;
        }
    }
}

