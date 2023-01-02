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
    /// <summary>
    /// Class used for setting the basic parameters of the universe.
    /// </summary>
    public class UniverseParameters : MonoBehaviour
    {
        public float physicsTimeStep;
        public float gravitationalConstant = 9.617f;

        private void Awake()
        {
            physicsTimeStep = Time.fixedDeltaTime;
            // gravitationalConstant = CalculateG();
        }


        private float CalculateG()
        {
            return 6.7f * Mathf.Pow(10, -11);
        }
    }
}
