using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattordev.UI;

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
        public bool inertialDampeners = true;
        public float intertialDamperForce = 100f;

        [Header("Keybinds")]
        public KeyCode thrustForward = KeyCode.Space;
        public KeyCode rotateLeft = KeyCode.Q;
        public KeyCode rotateRight = KeyCode.E;
        public KeyCode inertialDampenersKey = KeyCode.V;

        [Header("Other")]
        public SpriteRenderer spaceShipSprite;
        public Rigidbody2D rb2D;
        public bool controlling = false; // Dictates whether the spaceship is being controlled
        public ParticleSystem[] particleSystems; // The Particle systems the ship uses for its thrusters.
        public GameObject thrusterLight;
        public ControllableUICanvasController controllableUICanvas;

        private void Start()
        {
            controllableUICanvas = FindObjectOfType<ControllableUICanvasController>();
        }

        // Update is called once per frame
        void Update()
        {
            // Update the velocity vector pointer on the UI regardless of whether the ship is being controlled or not.
            controllableUICanvas.UpdateVelocityVectorPointer(rb2D.velocity.magnitude);

            if (controlling)
            {
                GetInput();
            }
            else
            {
                // if the ship is not being controlled and interial dampeners are on, apply them.
                if (inertialDampeners)
                    ApplyInertialDampeners(intertialDamperForce);
            }
        }

        /// <summary>
        /// Gets the input for thrust and rotation
        /// </summary>
        private void GetInput()
        {
            if (Input.GetKey(thrustForward))
            {
                ApplyThrust(thrusterForce);

                // VFX
                particleSystems[0].Play();
                thrusterLight.SetActive(true);
            }
            else
            {
                if (Input.GetKeyDown(inertialDampenersKey))
                {
                    inertialDampeners = !inertialDampeners;
                }

                if (inertialDampeners)
                    // Apply inertial dampeners here when no thrust input
                    ApplyInertialDampeners(intertialDamperForce);

                // VFX
                thrusterLight.SetActive(false);
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

        /// <summary>
        /// Function to smoothly rotate the ship using Rigidbody Torque.
        /// </summary>
        /// <param name="rotationAmount">the amount to rotate by.</param>
        private void RotateObject(float rotationAmount)
        {
            // Apply torque to the Rigidbody2D for rotation
            rb2D.AddTorque(rotationAmount * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        /// <summary>
        /// Applies thrust via the Rigidbody whilst taking into account acceleration.
        /// </summary>
        /// <param name="thrusterForce">The force amount to apply</param>
        private void ApplyThrust(float thrusterForce)
        {
            float desiredAcceleration = thrusterForce / rb2D.mass;
            // Calculate the force needed based on mass and desired acceleration
            float force = rb2D.mass * desiredAcceleration;

            // Apply the force in the forward direction of the object
            rb2D.AddForce(transform.up * force, ForceMode2D.Force);
        }

        /// <summary>
        /// Used for slowing down the ship gradually. Cancels out velocity and Angular velocity.
        /// 
        /// Might be worth to split the dampening factor/amount for velocity and angular velocity.
        /// </summary>
        /// <param name="dampeningFactor">The amount of dampening to apply</param>
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
            rb2D.AddTorque(-angularVelocity * dampeningFactor * 2, ForceMode2D.Force);
        }
    }
}
