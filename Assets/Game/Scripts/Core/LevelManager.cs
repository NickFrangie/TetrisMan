using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Input;

namespace Game.Core
{
    /// <summary>
    /// Manages controls and functionality of a level.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        // Inspector
        [Header("Start Behavior")]
        [SerializeField] private bool spawnPlayers;


        private void Start() 
        {
            if (spawnPlayers) {
                if (PlayerManager.instance) PlayerManager.instance.SpawnPlayers();
                else Debug.LogWarning("Need a Player Manager class to spawn players from.");
            }    
        }
    }
}
