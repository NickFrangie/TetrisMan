using System;
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

        // Inspector
        [Header("Prefabs")]
        [SerializeField] private PlayerInput[] playerPrefabs;

        // Internal
        internal PlayerConfiguration[] players { get; private set; }
        internal Action<PlayerConfiguration> playerJoinEvent, playerLeaveEvent;

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

            PlayerConfiguration player;
            if (playerInput.TryGetComponent<PlayerConfiguration>(out player)) {
                // New Player Configuration joined
                for (int i = 0; i < players.Length; i++) {
                    if (!players[i]) {
                        players[i] = player;
                        player.number = i;
                        playerJoinEvent?.Invoke(player);
                        break;
                    }
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
            if (player) {
                // Player Configuration left
                players[player.number] = null;
                playerLeaveEvent?.Invoke(player);
            }
        }
        #endregion

        #region Instancing
        public void SpawnPlayers()
        {
            playerInputManager.DisableJoining();
            foreach (PlayerConfiguration player in players) {
                if (player != null) {
                    GameObject playerPrefab = playerPrefabs[player.number].gameObject;
                    player.SpawnGameInput(playerPrefab);
                }
            }
        }

        public bool ValidPlayers() 
        {
            return players[0] != null;
        }
        #endregion
    }
}
