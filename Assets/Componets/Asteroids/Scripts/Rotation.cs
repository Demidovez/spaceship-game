using UnityEngine;

namespace AsteroidSpace
{
    public class Rotation : MonoBehaviour
    {
        [SerializeField] private float _tumbleMin = 1f;
        [SerializeField] private float _tumbleMax = 4f;

        private Rigidbody _rb;
        private int _tumble;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            
            _tumble = (int) Random.Range(_tumbleMin, _tumbleMax);
            _rb.angularVelocity = Random.insideUnitSphere * _tumble;
        }
    }
}

