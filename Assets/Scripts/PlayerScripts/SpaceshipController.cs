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
        public float rotationSpeed = 10f;
        public float thrusterForce = 5f;

        [Header("Ship Options")]
        public bool inertialDampers = false;
        public float intertialDamperForce;

        [Header("Other")]
        public Sprite spaceShipSprite;
        public Rigidbody2D rb2D;
        public KeyCode thrustForward = KeyCode.Space;
        public KeyCode rotateLeft = KeyCode.Q;
        public KeyCode rotateRight = KeyCode.E;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            GetInput();
        }

        private void GetInput()
        {
            if (Input.GetKeyDown(thrustForward))
            {
                Vector2 newForce = new Vector2();
                rb2D.AddForce(newForce);
            }

            if (Input.GetKeyDown(rotateLeft))
            {

            }

            if (Input.GetKeyDown(rotateRight))
            {

            }
        }
    }
}
