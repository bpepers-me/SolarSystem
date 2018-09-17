using System;
using Improbable;
using Improbable.Core;
using Improbable.Ship;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Vector3d = UnityEngine.Vector3d;

namespace Assets.Gamelogic.Player
{
    // Add this MonoBehaviour on both client and server-side workers
    public class ShipMovement : MonoBehaviour
    {
        /*
         * An entity with this MonoBehaviour will have it enabled only for the single worker (whether client or server)
         * which has write-access for its Position and Rotation components.
         */
        [Require] private Position.Writer PositionWriter;
        [Require] private TransformInfo.Writer TransformInfoWriter;
        [Require] protected ShipControls.Reader ShipControlsReader;

        private float targetSpeed; // [-1..1]
        private float currentSpeed; // [-1..1]
        private float targetSteering; // [-1..1]
        private float currentSteering; // [-1..1]

        [SerializeField] private Rigidbody myRigidbody;
        [SerializeField] private float MovementSpeed = 10000.0f;
        [SerializeField] private float TurningSpeed = 1.0f;
        [SerializeField] private AudioSource boatMovementAudio;

        private void OnEnable()
        {
            // Initialize entity's gameobject transform from Position and Rotation component values
            var position = TransformInfoWriter.Data.position.FromImprobable();
            var rotation = TransformInfoWriter.Data.rotation.FromImprobable();

            transform.localPosition = (Vector3)(position / Scales.unityFactor);
            transform.localRotation = rotation;

            myRigidbody.inertiaTensorRotation = UnityEngine.Quaternion.identity;
            ShipControlsReader.WarpTriggered.Add(OnWarp);
        }

        // Calculate speed and steer values ready from input for next physics actions in FixedUpdate
        private void Update()
        {
            var inputSpeed = ShipControlsReader.Data.targetSpeed;
            var inputSteering = ShipControlsReader.Data.targetSteering;

            var delta = Time.deltaTime;

            // Slowly decay the speed and steering values over time and make sharp turns slow down the ship.
            targetSpeed = Mathf.Lerp(targetSpeed, 0f, delta * (0.5f + Mathf.Abs(targetSteering)));
            targetSteering = Mathf.Lerp(targetSteering, 0f, delta * 3f);

            // Calculate the input-modified speed
            targetSpeed = targetSpeed + delta * inputSpeed;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Mathf.Clamp01(delta * 5f));

            // Steering is affected by speed -- the slower the ship moves, the less maneuverable it becomes
            targetSteering = Mathf.Clamp(targetSteering + delta * 6 * inputSteering * (0.1f + 0.9f * (currentSpeed + 0.1f)), -1f, 1f);
            currentSteering = Mathf.Lerp(currentSteering, targetSteering, delta * 5f);

            // Update sailing sounds volume based on speed and turning - fast movement and turning causes louder sounds
            if (boatMovementAudio)
            {
                float newVolume = currentSpeed*0.1f + Mathf.Abs(currentSteering)*0.3f;
                boatMovementAudio.volume = newVolume;
            }
        }

        // Move ship using local speed and steer value
        public void FixedUpdate()
        {
            var deltaTime = Time.deltaTime;
            ApplyPhysicsToShip(deltaTime);
            SendPositionAndRotationUpdates();
        }

        private void ApplyPhysicsToShip(double deltaTime)
        {
            var velocityChange = CalculateVelocityChange(deltaTime);
            var torqueToApply = CalculateTorqueToApply(deltaTime);

            myRigidbody.AddTorque(torqueToApply);
            myRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        private Vector3 CalculateVelocityChange(double deltaTime)
        {
            var currentVelocity = myRigidbody.velocity;
            var targetVelocity = transform.localRotation * Vector3.forward * (float)(currentSpeed * deltaTime * MovementSpeed);
            return targetVelocity - currentVelocity;
        }

        private Vector3 CalculateTorqueToApply(double deltaTime)
        {
            return new Vector3(0f, currentSteering * (float)(deltaTime * TurningSpeed), 0f);
        }

        private void SendPositionAndRotationUpdates()
        {
            var unityPosition = transform.localPosition;
            var position = new Vector3d(unityPosition.x * Scales.unityFactor, unityPosition.y * Scales.unityFactor, unityPosition.z * Scales.unityFactor);
            var spatialPosition = (Vector3)(position / Scales.spatialFactor);

            PositionWriter.Send(new Position.Update().SetCoords(spatialPosition.ToImprobableCoordinates()));
            TransformInfoWriter.Send(new TransformInfo.Update().SetPosition(position.ToImprobable()).SetRotation(transform.rotation.ToImprobable()));
        }

		private void OnWarp(Warp warp)
		{
			myRigidbody.position = new Vector3(0, 0, 0);
		}
    }
}
