using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattordev.Universe;
using TMPro;

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
        public string closestBody;
        public float orbitalPeriod;
        public float bodySpeed;
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


        // Start is called before the first frame update
        void Start()
        {
            universeParameters = FindObjectOfType<UniverseParameters>();
            GetBodies();
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
            selectedClosestBodyText.text = closestBody;
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
            // Get the Closest body - TODO
            // Get the orbital period - TODO
            Attractor selectedAttractor = body.GetComponent<Attractor>();
            bodySpeed = int.Parse(selectedAttractor.currentVelocity.y.ToString());
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

            GameObject body = cameraController.transform.parent.gameObject;
            return body;

        }

        #endregion
    }
}
