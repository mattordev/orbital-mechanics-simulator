using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact me directly
/// </summary>
namespace Mattordev.Universe
{
    /// <summary>
    /// Class used for setting the basic parameters of the universe.
    /// </summary>
    public class UniverseParameters : MonoBehaviour
    {
        public float physicsTimeStep;
        public float gravitationalConstant = 9.617f;

        private void Awake() {
            physicsTimeStep = Time.fixedDeltaTime;
        }
    }
}
