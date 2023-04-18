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

        // References
        internal PlayerInput playerInput { get; private set; }


        private void Awake() 
        {
            playerInput = GetComponent<PlayerInput>();
        }

        #region Connection
        /// <summary>
        /// Disconnects this User Configuration and its related PlayerInput.
        /// </summary>
        public void Disconnect()
        {
            foreach (InputDevice device in playerInput.devices) {
                if (device != null) playerInput.user.UnpairDevice(device);
            }
            playerInput.actions = null;
            Destroy(transform.gameObject);
        }
        #endregion
    }
}
