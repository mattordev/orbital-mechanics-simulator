using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattordev.Universe;

/// <summary>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact me directly
/// </summary>
namespace Mattordev
{
    [ExecuteInEditMode]
    public class OrbitDisplay : MonoBehaviour
    {
        public int numberOfSteps = 1000;    // the number of points to display on the path
        public float timeStep = 0.1f;   //
        public bool usePhysicsTimeStep; // if the script should use the physics timestep or a locally set version

        public bool relativeToBody; // Defines whether it will calculate relative to a body or not
        public Attractor centralBody; // The center of the solar system
        public float width = 100f; // The width of the line displayed
        public bool useThickLines; // Defines whether the lines will be thicker when displayed

        public bool hideOrbitPathsOnPlay; // Defines whether the orbit paths will be shown when the game plays
        public bool updateOrbitsLive; // Will the oribits upate live? WIP

        public UniverseParameters universeParameters;
        public Color pathColor;


        // Start is called before the first frame update
        void Start()
        {
            if (Application.isPlaying && hideOrbitPathsOnPlay)
            {
                HideOrbits();
            }
        }

        void Update()
        {

            if (!Application.isPlaying && universeParameters != null)
            {
                DrawOrbits();
            }

            if (updateOrbitsLive)
            {
                numberOfSteps = 100;
                DrawOrbits();
            }
        }

        public void ToggleLiveOrbits(bool state)
        {
            // make it remember the old orbits
            updateOrbitsLive = state;
        }

        private void OnEnable()
        {
            // Make an array of all the attractors in the scene
            Attractor[] bodies = FindObjectsOfType<Attractor>();

            foreach (Attractor attractor in bodies)
            {
                var lr = attractor.gameObject.GetComponent<LineRenderer>();
                lr.enabled = true;
            }
        }

        private void OnDisable()
        {
            // Make an array of all the attractors in the scene
            Attractor[] bodies = FindObjectsOfType<Attractor>();

            foreach (Attractor attractor in bodies)
            {
                var lr = attractor.gameObject.GetComponent<LineRenderer>();
                lr.enabled = false;
            }
        }

        void DrawOrbits()
        {
            // Make an array of all the attractors in the scene
            Attractor[] bodies = FindObjectsOfType<Attractor>();
            // Create an array of virtual bodies based on the original length
            var virtualBodies = new VirtualBody[bodies.Length];
            // Create an array of draw points based on the original length
            var drawPoints = new Vector3[bodies.Length][];
            int referenceFrameIndex = 0; // Ref index
            Vector3 referenceBodyInitialPosition = Vector3.zero;

            // Initialized the virtual bodies, we don't want to move the actual bodies though
            for (int i = 0; i < virtualBodies.Length; i++)
            {
                // Create a new virtual body, and set the number of points to the number of steps
                virtualBodies[i] = new VirtualBody(bodies[i]);
                drawPoints[i] = new Vector3[numberOfSteps];

                // Check to see if the current iteration is the central body
                if (bodies[i] == centralBody && relativeToBody)
                {
                    referenceFrameIndex = i;
                    referenceBodyInitialPosition = virtualBodies[i].position;
                }
            }

            // Simulate the orbit a number of times based on the step amount
            for (int step = 0; step < numberOfSteps; step++)
            {
                // Magick lambda expression. Checks to see if the script should be relative and then sets the value proplerly.
                Vector3 referenceBodyPosition = (relativeToBody) ? virtualBodies[referenceFrameIndex].position : Vector3.zero;
                // Update velocities
                for (int i = 0; i < virtualBodies.Length; i++)
                {
                    // Calculate the correct acceleration
                    virtualBodies[i].velocity += CalculateAcceleration(i, virtualBodies) * timeStep;
                }
                // Update the positions
                for (int i = 0; i < virtualBodies.Length; i++)
                {
                    // Calcutlate the new position, by adding the position and vel together, and multiplying by the timestep.
                    Vector3 newPos = virtualBodies[i].position + virtualBodies[i].velocity * timeStep;
                    // Set the iteration to the new pos
                    virtualBodies[i].position = newPos;

                    // If the script is acting rel to a body
                    if (relativeToBody)
                    {
                        // Subtract positions
                        var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                        // Reset new pos to the initial
                        newPos -= referenceFrameOffset;
                    }
                    if (relativeToBody && i == referenceFrameIndex)
                    {
                        newPos = referenceBodyInitialPosition;
                    }

                    // Populate the array with points.
                    drawPoints[i][step] = newPos;
                }
            }

            for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++)
            {
                // If we're using thick lines..
                if (useThickLines)
                {
                    var lr = bodies[bodyIndex].gameObject.GetComponent<LineRenderer>();
                    lr.enabled = true;
                    lr.positionCount = drawPoints[bodyIndex].Length;
                    lr.SetPositions(drawPoints[bodyIndex]);
                    lr.startColor = pathColor;
                    lr.endColor = pathColor;
                    lr.widthMultiplier = width;
                }
                else
                {
                    // Draw the line based on the number of points
                    for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++)
                    {
                        Debug.DrawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], pathColor);
                    }

                    // Now that we're drawing the line, we hide the lr if we're in editor
                    if (!Application.isPlaying)
                    {
                        var lr = bodies[bodyIndex].gameObject.GetComponent<LineRenderer>();
                        if (lr)
                        {
                            lr.enabled = false;
                        }
                    }
                }
            }
        }

        Vector3 CalculateAcceleration(int i, VirtualBody[] virtualBodies)
        {
            // Base acceleration vector
            Vector3 acceleration = Vector3.zero;
            for (int j = 0; j < virtualBodies.Length; j++)
            {
                if (i == j)
                {
                    continue;
                }
                // Calculate the force direction
                Vector3 forceDir = (virtualBodies[j].position - virtualBodies[i].position).normalized;
                // Calculate the squared distance
                float sqrDistance = (virtualBodies[j].position - virtualBodies[i].position).sqrMagnitude;
                // Calculate the acceleration - multiply the force direction by G, times that result by the mass and then divded by the sqr distance.
                acceleration += forceDir * universeParameters.gravitationalConstant * virtualBodies[j].mass / sqrDistance;
            }
            return acceleration;
        }

        void HideOrbits()
        {
            Attractor[] bodies = FindObjectsOfType<Attractor>();

            // Draw the paths
            for (int bodyIndex = 0; bodyIndex < bodies.Length; bodyIndex++)
            {
                // Get ref to the lr
                var lr = bodies[bodyIndex].gameObject.GetComponent<LineRenderer>();
                // set the count to 0, to *hide* the lines
                lr.positionCount = 0;
            }
        }

        private void OnValidate()
        {
            if (usePhysicsTimeStep && universeParameters != null)
            {
                timeStep = universeParameters.physicsTimeStep;
            }
        }

        /// <summary>
        /// Stores information about each body.
        /// </summary>
        class VirtualBody
        {
            public Vector3 position; // The position of the body
            public Vector3 velocity; // The velocity of the body
            public float mass; // The Mass of the body

            public VirtualBody(Attractor body)
            {
                position = body.transform.position;
                velocity = body.initialVelocity;
                mass = body.rb.mass;
            }
        }
    }
}
