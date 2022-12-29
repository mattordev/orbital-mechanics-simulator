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
        float minDistance = float.MaxValue;
        float maxDistance = 0;

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
        private GameObject selectedBody;

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

        public Dictionary<string, float> objectDistances = new Dictionary<string, float>();


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
            selectedOrbitalPeriodText.text = orbitalPeriod.ToString();
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
            orbitalPeriod = OrbitalPeriod();
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
        private GameObject GetSelectedBody()
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mass1"></param>
        /// <param name="mass2"></param>
        /// <param name="distance"></param>
        /// <param name="eccentricity"></param>
        /// <returns></returns>
        public float CalculateOrbitPeriod(float mass1, float mass2, float distance, float eccentricity)
        {
            // Calculate the semi-major axis of the elliptical orbit
            float a = distance / (1 - eccentricity);

            // Keplerian formula - T = 2 * pi * sqrt(a^3 / (G * (m1 + m2)
            // Calculate the orbital period using the formula above
            float T = 2 * Mathf.PI * Mathf.Sqrt((Mathf.Pow(a, 3)) / (universeParameters.gravitationalConstant * (mass1 + mass2)));

            // Convert the orbital period to seconds
            float orbitalPeriodInSeconds = T / 60;


            return orbitalPeriodInSeconds;
        }

        /// <summary>
        /// TODO:
        /// Calculate all orbits at the start of the game - save them to a dictionary with a name and a value for the period
        /// Set the UI Text to whichever is selected instead of calculating it every time a planet is clicked, every frame.
        /// 
        /// This will increase initial overhead, but be more performant in the longrun.
        /// I also need to take into account that for more complex simulations, this wont work (Keplerian formula - T = 2 * pi * sqrt(a^3 / (G * (m1 + m2))))). As the orbital period formula I've chosen
        /// is only made to be used in 2-body systems, not more than that. To solve for this, I can start using numerical integration or, solve using
        /// this formula in pairs - This will still work, but will become innacurate over time.
        /// 
        /// As another note, I might need to make that dictionary a 4D dictionary to hold the name, period, apoapsis and periapsis.
        /// </summary>
        /// <returns>The orbital period in seconds</returns>
        public float OrbitalPeriod()
        {
            selectedBody = GetSelectedBody();

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

                minDistance = Mathf.Min(minDistance, distance);
                maxDistance = Mathf.Max(maxDistance, distance);

                CalculateApoapsisPeriapsis();

                Rigidbody2D m1Rb = selectedBody.GetComponent<Rigidbody2D>();
                Rigidbody2D m2Rb = sun.GetComponent<Rigidbody2D>();

                orbitalPeriod = CalculateOrbitPeriod(m1Rb.mass, m2Rb.mass, distance, CalculateEccentricity(minDistance, maxDistance));

                return orbitalPeriod;
            }
            return 0;
        }

        /// <summary>
        /// Calculates the eccentricity based on the apoapsis and periapsis
        /// </summary>
        /// <param name="periapsis"></param>
        /// <param name="apoapsis"></param>
        /// <returns></returns>
        public float CalculateEccentricity(float periapsis, float apoapsis)
        {
            // e = (apoapsis - periapsis) / (apoapsis + periapsis)
            // Calculate the eccentricity using the formula above
            float eccentricity = (apoapsis - periapsis) / (apoapsis + periapsis);

            return eccentricity;
        }

        public void CalculateApoapsisPeriapsis()
        {
            // The apoapsis is the maximum distance between the two objects
            float apoapsis = maxDistance;

            // The periapsis is the minimum distance between the two objects
            float periapsis = minDistance;
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

                minDistance = Mathf.Min(minDistance, distance);
                maxDistance = Mathf.Max(maxDistance, distance);
                CalculateApoapsisPeriapsis();

                // We set the orbital period here for ease

                Rigidbody2D m1Rb = selectedBody.GetComponent<Rigidbody2D>();
                Rigidbody2D m2Rb = sun.GetComponent<Rigidbody2D>();

                orbitalPeriod = CalculateOrbitPeriod(m1Rb.velocity.y, m2Rb.velocity.y, distance, CalculateEccentricity(minDistance, maxDistance));

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
