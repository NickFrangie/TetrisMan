using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Blocks
{
    public class Spawner : MonoBehaviour
    {
        // Inspector
        [Header("Spawning")]
        public Tetromino[] tetrominos;
        
        // Internal
        internal BlockController blockController;
        

        public void SpawnBlock()
        {
            GameObject block = Instantiate(tetrominos[Random.Range(0, tetrominos.Length)].gameObject, transform.position, Quaternion.identity);
            
            Tetromino tetromino = block.GetComponent<Tetromino>();
            blockController.activeBlock = tetromino; 
        }

        public void SpawnEnd()
        {
            blockController.activeBlock = null;
        }
    }
}