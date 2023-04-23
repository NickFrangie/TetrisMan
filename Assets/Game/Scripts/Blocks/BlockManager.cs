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
        public static IndividualBlock[,] grid;
        public static BlockManager instance;
        
        // Inspector
        [Header("Dimensions")]
        [SerializeField] private Vector2Int bottomLeft;
        [SerializeField] private Vector2Int size;
        
        [Header("Spawner")]
        [SerializeField] private Spawner spawner;

        public float FallTimeBuffer = .8f;
        private List<GameObject> BlockParents = new List<GameObject>();
        public int LinesClearedThisRound = 0;


        private void Awake()
        {
            if (instance != null) Destroy(this);
            else instance = this;
        }

        private void Start() 
        {
            grid = new IndividualBlock[size.x, size.y];
        }

        private void OnDestroy() 
        {
            if (instance == this) instance = null; 
        }

        private void CheckForLines()
        {
            for (int i = size.y-1; i >= 0; i--){
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
            for (int j = 0; j<size.x; j++)
            {
                if (grid[j,index] == null) return false;
            }
            return true;
        }

        private void ClearLine(int index)
        {
            for (int j = 0; j<size.x; j++)
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
            for (int y = index; y < size.y; y++){
                for (int j = 0; j < size.x; j++){
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
        /// Adds the tetromino blocks to their corresponding grid locations.
        /// </summary>
        public void AddToGrid(Tetromino tetromino)
        {
            foreach (IndividualBlock block in tetromino.GetComponentsInChildren<IndividualBlock>())
            {
                Vector2Int position = Vector2Int.RoundToInt(block.transform.position);
                Vector2Int gridCoors = PositionToGrid(position);

                if (AboveGrid(gridCoors)) {
                    // TODO: Game Over Handling !!!
                    Debug.Log("Game Over!");
                    spawner.SpawnEnd();
                    return;
                }
                
                grid[gridCoors.x, gridCoors.y] = block;
            }

            ScoreManager.Instance.BlockAddedToBoard();
            CheckForLines();
            spawner.SpawnBlock();
        }

        #region Conversion
        /// <summary>
        /// Converts the specified position to a grid coordinate pair.
        /// </summary>
        /// <param name="position">World position.</param>
        /// <returns>Grid coordinate pairing.</returns>
        public Vector2Int PositionToGrid(Vector2Int position)
        {
            Vector2Int offset = position - bottomLeft;
            return offset;
        }
        #endregion

        #region Validation
        /// <summary>
        /// Determine whether the specifed grid position is above the grid.
        /// </summary>
        /// <param name="gridCoors">Specified grid position.</param>
        /// <returns>Whether it is above the grid.</returns>
        public bool AboveGrid(Vector2Int gridCoors)
        {
            return (gridCoors.y >= size.y);
        }

        /// <summary>
        /// Determine whether the specified Vector2Int position is in bounds.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>Whether the position is in bounds.</returns>
        public bool PositionInBounds(Vector2Int position)
        {
            return (position.x >= bottomLeft.x && position.x < bottomLeft.x + size.x
                && position.y >= bottomLeft.y);
        }

        /// <summary>
        /// Determine whether the specified Vector2Int position is free. 
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>Whether this position on the grid is free.</returns>
        public bool PositionIsFree(Vector2Int position)
        {
            Vector2Int gridCoors = PositionToGrid(position);

            if (AboveGrid(gridCoors)) return true;
            else return grid[gridCoors.x, gridCoors.y] == null;
        }
        #endregion

        #region Gizmos
        private void OnDrawGizmos() 
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube((Vector2) bottomLeft + ((Vector2) size - Vector2.one) / 2, (Vector2) size);    
        }
        #endregion
    }
}
