using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Object
{
    /// <summary>
    /// Utility component for Game Objects that allows for spawning objects.
    /// </summary>
    public class ObjectSpawner : MonoBehaviour
    {
        // Inspector
        [SerializeField] private Transform objectToSpawn;
        [SerializeField] private Transform spawnPoint; 


        /// <summary>
        /// Exposed method to spawn the desired object.
        /// </summary>
        public void SpawnObject()
        {
            Transform spawned = Instantiate(objectToSpawn, transform.root);
            spawned.position = spawnPoint.position;
            spawned.SetParent(null);
        }

        #region Debug
            private void OnDrawGizmos() 
            {
                if (spawnPoint) {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(spawnPoint.position, .05f);
                }
            }
        #endregion
    }
}
