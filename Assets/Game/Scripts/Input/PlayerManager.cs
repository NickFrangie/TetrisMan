using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    /// <summary>
    /// Singleton Manager of Player Connection and Multiplayer.
    /// </summary>
    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerManager : MonoBehaviour
    {
        // Static
        public static PlayerManager instance;

        // Internal
        private PlayerConfiguration[] players;

        // References
        internal PlayerInputManager playerInputManager { get; private set; }


        private void Awake() 
        {
            if (instance != null) {
                Destroy(this);
                return;
            } else {
                instance = this;
                DontDestroyOnLoad(instance);
            }

            playerInputManager = GetComponent<PlayerInputManager>();
        }

        private void Start() 
        {
            players = new PlayerConfiguration[playerInputManager.maxPlayerCount];
        }

        #region Connection
        /// <summary>
        /// Player join event.
        /// </summary>
        /// <param name="playerInput">The joined player.</param>
        public void PlayerJoined(PlayerInput playerInput) 
        {
            playerInput.transform.SetParent(transform);

            PlayerConfiguration player = playerInput.GetComponent<PlayerConfiguration>();
            for (int i = 0; i < players.Length; i++) {
                if (!players[i]) {
                    players[i] = player;
                    player.number = i + 1;
                    Debug.Log($"Player {player.number} joined.");
                    break;
                }
            }
        }

        /// <summary>
        /// Player leave event.
        /// </summary>
        /// <param name="playerInput">The leaving player.</param>
        public void PlayerLeft(PlayerInput playerInput) 
        {
            PlayerConfiguration player = playerInput.GetComponent<PlayerConfiguration>();
            players[player.number - 1] = null;

            Debug.Log($"Player {player.number} left");
        }
        #endregion
    }
}
