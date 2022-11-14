using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// for external use, please contact me directly
/// </summary>
namespace Mattordev.Universe
{
    public class Attractor : MonoBehaviour
    {
        public static List<Attractor> attractors;
        public Rigidbody2D rb;
        public Vector2 initialVelocity;
        private Vector2 currentVelocity;
        public float _currentVelocity;

        UniverseParameters universeParameters;

        private void Awake() {
            universeParameters = FindObjectOfType<UniverseParameters>();
            currentVelocity = initialVelocity;
        }

        private void FixedUpdate() {
            foreach(Attractor attractor in attractors)
            {
                if (attractor != this)
                {
                    Attract(attractor);
                }
            }

            _currentVelocity = rb.velocity.y;
        }

        private void OnEnable() {
            if (attractors == null)
            {
                attractors = new List<Attractor>();
            }
            attractors.Add(this);
        }

        private void OnDisable() {
            attractors.Remove(this);
        }

        void Attract (Attractor objToAttract)
        {
            Rigidbody2D rbToAttract = objToAttract.rb;

            Vector2 direction = rb.position - rbToAttract.position;
            float distance = direction.magnitude;

            if(distance == 0f)
            {
                return;
            }

            float forceMagnitude = universeParameters.gravitationalConstant * (rb.mass * rbToAttract.mass) / Mathf.Pow (distance, 2);
            Vector2 force = direction.normalized * forceMagnitude;
            Vector2 acceleration = force / rb.mass;

            rbToAttract.AddForce(force);
        }

        public void UpdatePosition(float timeStep)
        {
            rb.position += currentVelocity * timeStep;
        }
    }
}
