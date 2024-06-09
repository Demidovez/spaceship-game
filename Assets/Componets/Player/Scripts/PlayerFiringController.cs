using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public class PlayerFiringController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Player.Instance.IsFiring = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Player.Instance.IsFiring = false;
            }
        }
    } 
}

