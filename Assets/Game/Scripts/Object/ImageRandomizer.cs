using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Object
{
    /// <summary>
    /// Utility component for Game Objects that randomizes its sprite from an array.
    /// </summary>
    public class ImageRandomizer : MonoBehaviour
    {
        // Inspector
        [SerializeField] private Image image;
        [SerializeField] private Sprite[] sprites; 

        
        /// <summary>
        /// Exposed method to randomize the sprite from an array.
        /// </summary>
        public void RandomizeColor()
        {
            Sprite sprite = sprites[Random.Range(0, sprites.Length)];
            image.sprite = sprite;
        }
    }
}