using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.Spaceship
{
    /// <summary>
    /// A simple class for controlling a spaceship in the simulation.
    /// </summary>
    public class SpaceshipController : MonoBehaviour
    {
        [Header("Force Variables")]
        public float rotationSpeed = 100f;
        public float thrusterForce = 1000f;

        [Header("Ship Options")]
        public bool inertialDampers = false;
        public float intertialDamperForce = 100f;

        [Header("Other")]
        public SpriteRenderer spaceShipSprite;
        public Rigidbody2D rb2D;
        public KeyCode thrustForward = KeyCode.Space;
        public KeyCode rotateLeft = KeyCode.Q;
        public KeyCode rotateRight = KeyCode.E;
        private bool isColliding = false;

        // Update is called once per frame
        void Update()
        {
            GetInput();
        }

        /// <summary>
        /// Gets the input for thrust and rotation
        /// </summary>
        private void GetInput()
        {
            if (Input.GetKey(thrustForward))
            {
                ApplyThrust(thrusterForce);
            }
            else
            {
                // Apply inertial dampeners here when no thrust input
                ApplyInertialDampeners(intertialDamperForce);
            }

            if (Input.GetKey(rotateLeft))
            {
                RotateObject(rotationSpeed);
            }

            if (Input.GetKey(rotateRight))
            {
                RotateObject(-rotationSpeed);
            }
        }

        // Function to smoothly rotate the object
        // Function to smoothly rotate the object
        private void RotateObject(float rotationAmount)
        {
            // Apply torque to the Rigidbody2D for rotation
            rb2D.AddTorque(rotationAmount * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        private void ApplyThrust(float thrusterForce)
        {
            float desiredAcceleration = thrusterForce / rb2D.mass;
            // Calculate the force needed based on mass and desired acceleration
            float force = rb2D.mass * desiredAcceleration;

            // Apply the force in the forward direction of the object
            rb2D.AddForce(transform.up * force, ForceMode2D.Force);
        }

        private void ApplyInertialDampeners(float dampeningFactor)
        {
            // Calculate the dampening force based on the velocity
            Vector2 velocity = rb2D.velocity;
            Vector2 dampeningForce = -velocity * dampeningFactor;

            // Calculate the angular velocity
            float angularVelocity = rb2D.angularVelocity;

            // Apply the dampening force to linear motion
            rb2D.AddForce(dampeningForce, ForceMode2D.Force);

            // Apply torque in the opposite direction to stop rotation
            rb2D.AddTorque(-angularVelocity * dampeningFactor, ForceMode2D.Force);
        }
    }
}
