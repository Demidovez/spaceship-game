using System;
using UnityEngine;

namespace GameManagementSpace
{
    public class LiveBoundary : MonoBehaviour
    {
        public delegate void OnLiveBoundaryCollision(GameObject other);
        public static event OnLiveBoundaryCollision OnLiveBoundaryCollisionEvent;
        
        private void OnTriggerExit(Collider other)
        {
            OnLiveBoundaryCollisionEvent?.Invoke(other.gameObject);
        }
    }
}
