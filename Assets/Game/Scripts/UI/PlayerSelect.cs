using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.Input;

namespace Game.UI
{
    /// <summary>
    /// Component for handling UI Player Select Prefab functionality.
    /// </summary>
    public class PlayerSelect : MonoBehaviour
    {
        // Inspector
        [Header("Text")]
        [SerializeField] private TMP_Text playerText;
        [SerializeField] private TMP_Text statusText;

        [Header("Previews")]
        [SerializeField] private Transform previewSpawnPoint;
        [SerializeField] private GameObject[] previewPrefabs;

        // Internal
        private Transform preview;


        /// <summary>
        /// Activates the Player Select according to the given player configuration.
        /// </summary>
        /// <param name="player"></param>
        public void ActivatePlayer(PlayerConfiguration player)
        {
            playerText.SetText($"Player {player.displayed}");
            statusText.SetText("Connected");
            preview = SpawnPreviewPrefab(player.number);
        }

        /// <summary>
        /// Deactivates the Player Select.
        /// </summary>
        public void DeactivatePlayer()
        {
            playerText.SetText("No Player");
            statusText.SetText("Press Any Button");
            Destroy(preview.gameObject);
        }

        /// <summary>
        /// Spawns the preview prefab corresponding to the specified index.
        /// </summary>
        /// <param name="index">Designates which preview prefab to spawn.</param>
        public Transform SpawnPreviewPrefab(int index)
        {
            GameObject previewPrefabObj = Instantiate(previewPrefabs[index], Vector3.zero, Quaternion.identity, previewSpawnPoint);
            previewPrefabObj.transform.localPosition = Vector3.zero;
            return previewPrefabObj.transform;
        }
    }
}
