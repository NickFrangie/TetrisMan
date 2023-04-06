using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Blocks
{
    public class Spawner : MonoBehaviour
    {
        public GameObject[] TetrisBlocks;
        
        // Start is called before the first frame update
        void Start()
        {
            SpawnBlock();
        }

        public void SpawnBlock()
        {
            Instantiate(TetrisBlocks[Random.Range(0, TetrisBlocks.Length)], transform.position, Quaternion.identity);
        }
    }
}