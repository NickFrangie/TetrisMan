using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Blocks
{
    /// <summary>
    /// Script for individual Tetris Blocks.
    /// </summary>
    [ExecuteAlways]
    public class IndividualBlock : MonoBehaviour
    {
        // Inspector
        [SerializeField] private UnityEvent onFocusEvents;
        [SerializeField] private UnityEvent offFocusEvents;


        private void Update() 
        {
            transform.position = Vector3Int.RoundToInt(transform.position);
        }

        #region Focus Events
        public void OnFocus()
        {
            onFocusEvents?.Invoke();
        }

        public void OffFocus()
        {
            offFocusEvents?.Invoke();
        }
        #endregion
    }
}
