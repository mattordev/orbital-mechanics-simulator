using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact me directly
/// </summary>
namespace Mattordev
{
    [RequireComponent(typeof(LineRenderer))]
    public class Ellipse : MonoBehaviour
    {
        LineRenderer lr;

        [Range(3,36)]
        public int segments;
        public float xAxis;
        public float yAxis;
        
        private void Awake() {
            lr = GetComponent<LineRenderer>();
        }
    }
}
