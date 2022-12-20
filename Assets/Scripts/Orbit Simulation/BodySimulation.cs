using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.Universe
{
    public class BodySimulation : MonoBehaviour
    {
        Attractor[] bodies;
        UniverseParameters universeParameters;

        private void Awake()
        {
            universeParameters = FindObjectOfType<UniverseParameters>();
            bodies = FindObjectsOfType<Attractor>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            for (int i = 0; i < bodies.Length; i++)
            {
                // Update the position of the planets based on the physics timestep.
                bodies[i].UpdatePosition(universeParameters.physicsTimeStep);
            }
        }

        public void GetBodies()
        {
            Attractor[] newBodies = FindObjectsOfType<Attractor>();
            bodies = newBodies;
            foreach (Attractor attractor in newBodies)
            {
                attractor.UpdateAttractors();
            }
        }
    }
}
