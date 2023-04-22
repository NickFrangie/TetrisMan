using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Object
{
    /// <summary>
    /// Utility component for Game Objects that randomizes its colors within a range.
    /// </summary>
    public class ColorRandomizer : MonoBehaviour
    {
        // Inspector
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color color1, color2; 


        /// <summary>
        /// Exposed method to randomize the color between the specified range.
        /// </summary>
        public void RandomizeColor()
        {
            Color randColor = Color.Lerp(color1, color2, Random.Range(0f, 1f));
            spriteRenderer.color = randColor;
        }
    }
}
