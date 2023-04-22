using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private List<GameObject> BlockParents = new List<GameObject>();
        public int LinesClearedThisRound = 0;

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

        private void CheckForLines()
        {
            for (int i = HEIGHT-1; i >= 0; i--){
                if(HasLine(i))
                {
                    LinesClearedThisRound++;
                    ClearLine(i);
                    MoveDown(i);
                    DestroyEmptyParents();
                }
            }
            ScoreManager.Instance.LineCleared(LinesClearedThisRound);
            LinesClearedThisRound = 0;
        }

        private bool HasLine(int index)
        {
            for (int j = 0; j<WIDTH; j++)
            {
                if (grid[j,index] == null) return false;
            }
            return true;
        }

        private void ClearLine(int index)
        {
            for (int j = 0; j<WIDTH; j++)
            {
                GameObject parent = grid[j, index].transform.parent.gameObject;
                if(!BlockParents.Contains(parent)) BlockParents.Add(parent);
                Destroy(grid[j, index].gameObject);
                grid[j, index] = null;
            }
        }

        private void DestroyEmptyParents()
        {
            foreach (GameObject parent in BlockParents)
            {
                if (parent.transform.childCount == 0)
                {
                    Destroy(parent);
                }
            }
        }

        private void MoveDown(int index)
        {
            for (int y = index; y < HEIGHT; y++){
                for (int j = 0; j < WIDTH; j++){
                    if(grid[j,y] != null)
                    {
                        grid[j, y - 1] = grid[j, y];
                        grid[j, y] = null;
                        grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                    }
                }
            }
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
            ScoreManager.Instance.BlockAddedToBoard();
            CheckForLines();
        }
    }
}
