using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// Component for handling UI Player Select Prefab functionality.
    /// </summary>
    public class PlayerSelect : MonoBehaviour
    {
        // Inspector
        [Header("Previews")]
        [SerializeField] private GameObject[] previewPrefabs; 
        [SerializeField] private Transform previewSpawnPoint;


        /// <summary>
        /// Spawns the preview prefab corresponding to the specified index.
        /// </summary>
        /// <param name="index">Designates which preview prefab to spawn.</param>
        public void SpawnPreviewPrefab(int index)
        {
            GameObject previewPrefabObj = Instantiate(previewPrefabs[index], Vector3.zero, Quaternion.identity, previewSpawnPoint);
            previewPrefabObj.transform.localPosition = Vector3.zero;
        }
    }
}
