using System.Collections;
using System.Collections.Generic;
using Mattordev.Utils.Stats;
using UnityEngine;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.Utils.Audio
{
    public static class AudioUtilities
    {
        public static float DecibelToLinear(float decibel)
        {
            return Mathf.Pow(10, decibel / 20f);
        }

        public static float LinearToDecibel(float linear)
        {
            if (linear > 0)
                return 20.0f * Mathf.Log10(linear);
            else
                return -80.0f; // Return a default volume level when linear value is zero
        }
    }
}
