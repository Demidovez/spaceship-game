using System;
using UnityEngine;

namespace GameManagementSpace
{
	[Serializable]
    public class LevelBoundary
    {
        public float xMin = -6f, xMax = 6f, zMin = -7f, zMax = 8f;
    }

    public class GameManagement: MonoBehaviour
    {
        [SerializeField] private LevelBoundary _boundary;

        public static GameManagement Instance { get; private set; }
        public LevelBoundary Boundary => _boundary;

        private void Awake()
        {
            Instance = this;
        }
    }
}