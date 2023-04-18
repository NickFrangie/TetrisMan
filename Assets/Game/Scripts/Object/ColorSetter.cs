using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Object
{
    /// <summary>
    /// Utility component for Game Objects to set their sprite renderer to a specified color.
    /// </summary>
    public class ColorSetter : MonoBehaviour
    {
        // Inspector
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color[] colors; 


        /// <summary>
        /// Exposed method to set the color according to the specified index.
        /// </summary>
        /// <param name="index">The index corresponding to the desired color.</param>
        public void SetColor(int index)
        {
            spriteRenderer.color = colors[index];
        }
    }
}
