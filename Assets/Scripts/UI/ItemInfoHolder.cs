using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mattordev.Universe;
using UnityEngine.UI;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.UI
{
    public class ItemInfoHolder : MonoBehaviour
    {
        [Header("Text Objects")]
        public TMP_Text nameText;
        public TMP_Text massText;
        public TMP_Text speedText;

        [Header("Image")]
        public Image image;

        // Needed for focusing
        [Header("Other")]
        public Attractor attractor;
        public Button button;
        public TableGenerator tableGenerator;

        Rigidbody2D rb2D;

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

        private void Update()
        {
            // Update the speed val
            speedText.text = rb2D.velocity.magnitude.ToString();
        }
    }
}
