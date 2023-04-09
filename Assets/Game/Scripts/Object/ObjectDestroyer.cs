using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Object
{
    /// <summary>
    /// Utility component for Game Objects that allows destroying itself.
    /// </summary>
    public class ObjectDestroyer : MonoBehaviour
    {
        /// <summary>
        /// Exposed method to destroy this object.
        /// </summary>
        public void DestroyObject()
        {
            Destroy(this.gameObject);
        }

    }
}
