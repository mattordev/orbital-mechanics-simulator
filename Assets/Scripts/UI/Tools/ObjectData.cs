using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Mattordev.Utils;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev
{
    public class ObjectData : MonoBehaviour, IPointerClickHandler
    {
        public int objNumber;
        // Don't like doing it this way, feels too spaghetti code like. Events might be a better way to handle it
        // or however you invoke a method easily without cross referencing scripts like this.
        public AddObject addObject;

        //Detect if a click occurs
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            addObject.SelectedObject(this.gameObject);
        }
    }
}