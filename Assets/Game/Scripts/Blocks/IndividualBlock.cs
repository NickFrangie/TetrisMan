using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Blocks
{
    /// <summary>
    /// Script for individual Tetris Blocks.
    /// </summary>
    [ExecuteAlways]
    public class IndividualBlock : MonoBehaviour
    {
        private void Update() 
        {
            transform.position = Vector3Int.RoundToInt(transform.position);
        }
    }
}
