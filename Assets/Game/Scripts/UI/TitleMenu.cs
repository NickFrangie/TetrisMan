using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        }

        private void OnDestroy() 
        {
            PlayerManager.instance.playerJoinEvent -= PlayerSelectJoin;
            PlayerManager.instance.playerLeaveEvent -= PlayerSelectLeft;
        }

        #region PlayerSelect
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
