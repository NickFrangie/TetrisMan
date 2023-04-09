using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Blocks
{
    public class Spawner : MonoBehaviour
    {
        // Inspector
        [Header("Spawning")]
        public GameObject[] TetrisBlocks;
        
        // Temporary (TODO: Remove !!!)
        public BlockController blockController;
        

        void Start()
        {
            SpawnBlock();
        }

        public void SpawnBlock()
        {
            GameObject block = Instantiate(TetrisBlocks[Random.Range(0, TetrisBlocks.Length)], transform.position, Quaternion.identity) as GameObject;
            
            TetrisBlock tetrisBlock = block.GetComponent<TetrisBlock>();
            tetrisBlock.spawner = this;
            blockController.activeBlock = tetrisBlock; 
        }
    }
}