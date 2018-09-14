using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Gamelogic.Planets
{
    public class PlanetRotate : MonoBehaviour
    {
        public float initialRotation;
        public float rotationPeriod;

        void Start()
        {
            transform.rotation = Quaternion.Euler(initialRotation, 0, 0);
        }

        void FixedUpdate()
        {
            transform.Rotate(0, 360f / rotationPeriod * Time.fixedDeltaTime, 0, Space.Self);
        }
    }
}
