using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Object
{
/// <summary>
    /// Utility component for Game Objects that provides functionality 
    /// for switching between preselected colors.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class ColorSwitcher : MonoBehaviour
    {
        // Inspector
        [Header("Colors")]
        [SerializeField] private Color[] colors;

        // References
        private SpriteRenderer spriteRenderer;

        
        private void Awake() 
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetColor(int index)
        {
            spriteRenderer.color = colors[index];
        }
    }
}
