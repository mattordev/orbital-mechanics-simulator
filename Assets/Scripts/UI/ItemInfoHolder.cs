using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mattordev.Universe;
using Mattordev.Spaceship;
using UnityEngine.UI;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.UI
{
    /// <summary>
    /// A class to hold some basic information for the table entries, populates them when called for by passing in an attractor.
    /// 
    /// also handles the updating of each entires speed value - this is fine for a smaller number of bodies but might become unperformant when there's
    /// lots of bodies in a scene.
    /// </summary>
    public class ItemInfoHolder : MonoBehaviour
    {
        [Header("Text Objects")]
        public TMP_Text nameText;   // Name of the body
        public TMP_Text massText;   // Mass of the body
        public TMP_Text speedText;  // Speed of the body

        [Header("Image")]
        public Image image; // Image used to display the body

        // Needed for focusing
        [Header("Other")]
        public Attractor attractor; // The attractor focused on
        public SpaceshipController satalite;
        public Button button;  // Button used for clicking on the table to focus on the body 
        public TableGenerator tableGenerator; // The table generator script

        Rigidbody2D rb2D; // Ref to the planets rigidbody, used for accessing the mass.

        /// <summary>
        /// Start finds the ref to the table gen and adds a listener to the button to call the focus func.
        /// </summary>
        private void Start()
        {
            tableGenerator = FindObjectOfType<TableGenerator>();
            button.onClick.AddListener(() => tableGenerator.TableClickToFocus(button.gameObject));
        }


        /// <summary>
        /// Populates the item fields with the correct parameters.
        /// </summary>
        /// <param name="attractor"> Attractor object, all other variables are gotten from this</param>
        public void SetInfo(Attractor attractor)
        {
            this.attractor = attractor;
            rb2D = attractor.GetComponent<Rigidbody2D>();

            SpriteRenderer spriteRenderer = attractor.GetComponent<SpriteRenderer>();
            image.sprite = spriteRenderer.sprite;



            nameText.text = attractor.gameObject.name;
            massText.text = rb2D.mass.ToString();
        }

        /// <summary>
        /// Populates the item fields with the correct parameters.
        /// </summary>
        /// <param name="satalite"> Attractor object, all other variables are gotten from this</param>
        public void SetInfo(SpaceshipController satalite)
        {
            this.satalite = satalite;
            rb2D = satalite.GetComponent<Rigidbody2D>();

            SpriteRenderer spriteRenderer = satalite.GetComponentInChildren<SpriteRenderer>();
            image.sprite = spriteRenderer.sprite;

            nameText.text = satalite.gameObject.name;
            massText.text = rb2D.mass.ToString();
        }

        /// <summary>
        /// Continuosly update the speed text based on the RBs vel.magnitude
        /// </summary>
        private void Update()
        {
            // Update the speed val
            speedText.text = rb2D.velocity.magnitude.ToString("F2");
        }
    }
}
