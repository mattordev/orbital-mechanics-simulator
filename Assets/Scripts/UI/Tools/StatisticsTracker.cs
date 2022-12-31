using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattordev.Universe;
using TMPro;
using System.Linq;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.Utils.Stats
{
    public class StatisticsTracker : MonoBehaviour
    {
        public bool updateStats = true;
        public GameObject sun;

        // Used for orbital period
        // Track the minimum and maximum distances between the two objects
        public float minDistance = float.MaxValue;
        public float maxDistance = 0;
        public float periapsis = float.MaxValue; // Periapsis of the orbit in meters
        public float apoapsis = float.MinValue; // Apoapsis of the orbit in meters

        // General Stats
        [Header("General Statistics")]
        public List<Attractor> attractors;
        public int numberOfBodies;
        public int totalMassOfBodies;
        public int simulationRuntime;
        public float simulationSpeed;

        // Universe Stats
        [Header("Universe Parameters")]
        public UniverseParameters universeParameters;
        public float gravitationalConstant;
        public float physicsTimestep;

        // Selected object stats
        [Header("Selected body Statistics")]
        public float mass;
        public float orbitalPeriod;
        public float bodySpeed;
        public string closestbody;
        public GameObject selectedBody;

        [Space]
        [Header("Text Objects")]
        public TMP_Text numberOfBodiesText;
        public TMP_Text totalMassOfBodiesText;
        public TMP_Text simulationRuntimeText;
        public TMP_Text simulationSpeedText;
        [Space]
        public TMP_Text gravitationalConstantText;
        public TMP_Text physicsTimestepText;
        [Space]
        public TMP_Text selectedMassText;
        public TMP_Text selectedClosestBodyText;
        public TMP_Text selectedOrbitalPeriodText;
        public TMP_Text selectedBodySpeedText;

        Dictionary<string, float> objectDistances = new Dictionary<string, float>();


        // Start is called before the first frame update
        void Start()
        {
            universeParameters = FindObjectOfType<UniverseParameters>();
            GetBodies();
            GetClosestBody();
        }

        // Update is called once per frame
        void Update()
        {
            if (updateStats)
            {
                GetStats();
                UpdateStats();
            }
        }

        public void UpdateStats()
        {
            // General stats
            numberOfBodiesText.text = numberOfBodies.ToString();
            totalMassOfBodiesText.text = totalMassOfBodies.ToString();
            simulationRuntimeText.text = simulationRuntime.ToString();
            simulationSpeedText.text = simulationSpeed.ToString();

            // Universe params
            gravitationalConstantText.text = gravitationalConstant.ToString();
            physicsTimestepText.text = physicsTimestep.ToString();

            // Selected stats
            selectedMassText.text = mass.ToString();
            selectedClosestBodyText.text = closestbody;
            // This will need to be fixed (see the xml summary of the function)
            selectedOrbitalPeriodText.text = "WIP";
            selectedBodySpeedText.text = bodySpeed.ToString();
        }

        public void GetStats()
        {
            #region General
            numberOfBodies = attractors.Count;
            // Total mass is gotten at the start
            simulationRuntime = (int)Time.timeSinceLevelLoad;
            simulationSpeed = Time.deltaTime;
            #endregion

            #region Universe
            gravitationalConstant = universeParameters.gravitationalConstant;
            physicsTimestep = universeParameters.physicsTimeStep;
            #endregion

            #region Selected
            GameObject body = GetSelectedBody();
            // If there's nothing selected
            if (!body)
            {
                mass = 0;
            }

            if (body == this.gameObject)
            {
                return;
            }
            Rigidbody2D selectedRb = body.GetComponent<Rigidbody2D>();
            mass = selectedRb.mass;
            closestbody = GetClosestBody();
            //orbitalPeriod = OrbitalPeriod();
            Attractor selectedAttractor = body.GetComponent<Attractor>();
            bodySpeed = selectedAttractor.rb.velocity.y;
            #endregion
        }

        #region Get Statistic functions
        /// <summary>
        /// Called for getting the amount of bodies/attractors in the scene
        /// </summary>
        public void GetBodies()
        {
            attractors.Clear();
            foreach (Attractor body in FindObjectsOfType<Attractor>())
            {
                attractors.Add(body);
            }
            Debug.Log($"Sucessfully found {attractors.Count} bodies");
            totalMassOfBodies = GetTotalMass();
        }

        private int GetTotalMass()
        {
            foreach (Attractor body in attractors)
            {
                for (int i = 0; i < attractors.Count; i++)
                {
                    totalMassOfBodies += (int)body.rb.mass;
                }
            }

            return totalMassOfBodies;
        }

        private float GetSimRuntime()
        {
            return Time.timeSinceLevelLoad;
        }

        private float GetSimSpeed()
        {
            return Time.deltaTime;
        }

        // Universe Params
        private float GetGravitationalConstant()
        {
            return universeParameters.gravitationalConstant;
        }

        private float GetPhysicsTimestep()
        {
            return universeParameters.physicsTimeStep;
        }

        // Selected Body
        public GameObject GetSelectedBody()
        {
            CameraController cameraController = FindObjectOfType<CameraController>();

            if (!cameraController.focusing)
            {
                return this.gameObject;
            }

            if (cameraController.transform.parent != null)
            {
                GameObject body = cameraController.transform.parent.gameObject;
                return body;
            }
            return null;
        }

        private string GetClosestBody()
        {
            // Get the selected body
            GameObject selectedBody = GetSelectedBody();

            // Initialize a dictionary to store the name and distance of each Attractor
            Dictionary<string, float> distances = new Dictionary<string, float>();

            // Iterate through all Attractors in the list
            foreach (Attractor attractor in attractors)
            {
                // Skip the distance calculation and comparison if the current Attractor is the selected body
                if (attractor.gameObject == selectedBody)
                {
                    continue;
                }

                // Calculate the distance between the selected body and the current Attractor
                float distance = Vector3.Distance(selectedBody.transform.position, attractor.transform.position);

                // Add the Attractor's name and distance to the dictionary
                distances.Add(attractor.name, distance);
            }

            // Find the name and distance of the closest Attractor
            string closestObjectName = "";
            float closestDistance = Mathf.Infinity;
            foreach (KeyValuePair<string, float> distance in distances)
            {
                if (distance.Value < closestDistance)
                {
                    closestDistance = distance.Value;
                    closestObjectName = distance.Key;
                }
            }

            // Draws a line between the selected obj and the closest one.
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                // Get the closest Attractor game object
                GameObject closestAttractor = GameObject.Find(closestObjectName);

                // Draw a 2D debug line between the selected body and the closest Attractor
                Debug.DrawLine(selectedBody.transform.position, closestAttractor.transform.position, Color.red, .1f, false);
            }

            // Return the name and distance of the closest Attractor as a string
            return $"{closestObjectName} ({closestDistance:F2})";
        }

        #region Calculate Oribtal Parameters

        // The commented out code is the actual math that I should be using
        // I just couldn't get it to work..

        // private float CalculateEccentricity()
        // {
        //     selectedBody = GetSelectedBody();
        //     Vector3 r = selectedBody.transform.position;
        //     Vector3 velocity = selectedBody.GetComponent<Rigidbody2D>().velocity;

        //     Vector3 e = (Vector3.Cross(velocity, CalculateSpecificAngularMomentumVector()) / CalculateSpecificOrbitalEnergy() - r.normalized);
        //     return e.magnitude;
        // }

        // private Vector3 CalculateSpecificAngularMomentumVector()
        // {
        //     Vector3 velocity = selectedBody.GetComponent<Rigidbody2D>().velocity;
        //     return Vector3.Cross(selectedBody.transform.position, velocity);
        // }

        // private float CalculateSemiMajorAxis()
        // {
        //     return CalculateStandardGravitationalParameter() / Mathf.Pow(CalculateSpecificOrbitalEnergy(), 2);
        // }

        // private float CalculateStandardGravitationalParameter()
        // {
        //     return universeParameters.gravitationalConstant * mass;
        // }

        // private float CalculateSpecificOrbitalEnergy()
        // {
        //     float radius = selectedBody.GetComponent<CircleCollider2D>().radius;
        //     return (bodySpeed * bodySpeed) / 2 - (CalculateStandardGravitationalParameter() / radius);
        // }


        #endregion


        public void ResetSelectedToDefault()
        {
            selectedBody = null;
            selectedBodySpeedText.text = string.Empty;
            selectedMassText.text = string.Empty;
            selectedClosestBodyText.text = string.Empty;
            selectedOrbitalPeriodText.text = string.Empty;
        }

        #endregion
    }
}
