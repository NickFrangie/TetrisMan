using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Blocks
{
    public class BlockManager : MonoBehaviour
    {
        // Contants
        public const int HEIGHT = 20; 
        public const int WIDTH = 10;
        public static IndividualBlock[,] grid = new IndividualBlock[WIDTH, HEIGHT];
        public static BlockManager Instance;
        
        public float FallTimeBuffer = .8f;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //CheckForLines();
        }

        private void CheckForLines()
        {
            for (int i = HEIGHT-1; i >= 0; i--){
                if(HasLine(i)){
                    ClearLine(i);
                    MoveDown(i);
                }
            }
        }

        private bool HasLine(int index)
        {
            for (int i = WIDTH - 1; i >= 0; i--)
            {
                if (grid[index,i] == null) return false;
            }
            return true;
        }

        private void ClearLine(int index)
        {
            print($"Clear line {index}");
        }

        private void MoveDown(int index)
        {
            
        }

        /// <summary>
        /// Adds the tetromino blocks to their corresponding grid locations
        /// </summary>
        public void AddToGrid(Tetromino tetromino)
        {
            foreach (IndividualBlock block in tetromino.GetComponentsInChildren<IndividualBlock>())
            {
                Vector3 position = block.transform.position;
                int roundedX = Mathf.RoundToInt(position.x );
                int roundedY = Mathf.RoundToInt(position.y);
                grid[roundedX, roundedY] = block;
            }
        }
    }
}
