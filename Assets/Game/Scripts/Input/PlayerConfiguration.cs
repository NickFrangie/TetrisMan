using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    /// <summary>
    /// Contains information and cofigured a single player's connection.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerConfiguration : MonoBehaviour
    {
        // Internal
        internal int number = 0;
        internal int displayed { get { return number + 1; } }
        internal Action confirmEvent;

        // References
        internal PlayerInput playerConfigInput { get; private set; }
        internal PlayerInput playerGameInput { get; private set; }


        private void Awake() 
        {
            playerConfigInput = GetComponent<PlayerInput>();
        }

        #region Connection
        /// <summary>
        /// Disconnects this User Configuration and its related PlayerInput.
        /// </summary>
        public void Disconnect()
        {
            foreach (InputDevice device in playerConfigInput.devices) {
                if (device != null) playerConfigInput.user.UnpairDevice(device);
            }
            playerConfigInput.actions = null;
            Destroy(transform.gameObject);
        }

        /// <summary>
        /// Spawn a new game object with a player input for this player configuration.
        /// </summary>
        /// <param name="gameObject">The game object to spawn.</param>
        /// <param name="controlScheme">The action map control scheme.</param>
        internal void SpawnGameInput(GameObject gameObject, string controlScheme)
        {
            playerGameInput = PlayerInput.Instantiate(gameObject, playerConfigInput.playerIndex, controlScheme, -1, playerConfigInput.devices[0]);
            playerConfigInput.enabled = false;
        }
        #endregion

        #region Input
        public void Confirm(InputAction.CallbackContext context)
        {
            confirmEvent?.Invoke();
        }
        #endregion
    }
}
