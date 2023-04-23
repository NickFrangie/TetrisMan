using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Game.Input;

namespace Game.UI
{
    /// <summary>
    /// Component for handling Player Select Menu functionality.
    /// </summary>
    public class TitleMenu : MonoBehaviour
    {
        // Inspector
        [SerializeField] private PlayerSelect[] playerSelects;


        private void Start() 
        {
            PlayerManager.instance.playerJoinEvent += PlayerSelectJoin;
            PlayerManager.instance.playerLeaveEvent += PlayerSelectLeft;

            PlayerManager.instance.playerJoinEvent += BindGameStart;
        }

        private void OnDestroy() 
        {
            PlayerManager.instance.playerJoinEvent -= PlayerSelectJoin;
            PlayerManager.instance.playerLeaveEvent -= PlayerSelectLeft;

            PlayerManager.instance.playerJoinEvent -= BindGameStart;
        }

        #region Game
        public void StartGame()
        {
            if (PlayerManager.instance.ValidPlayers()) {
                SceneManager.LoadScene("Game");
            }
        }

        public void BindGameStart(PlayerConfiguration player) 
        {
            player.confirmEvent += StartGame;
        }
        #endregion

        #region Connection
        private void PlayerSelectJoin(PlayerConfiguration player)
        {
            playerSelects[player.number].ActivatePlayer(player);
        }

        private void PlayerSelectLeft(PlayerConfiguration player)
        {
            playerSelects[player.number].DeactivatePlayer();
        }
        #endregion
    }
}
