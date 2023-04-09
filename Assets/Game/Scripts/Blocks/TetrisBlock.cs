using UnityEngine.Tilemaps;
using UnityEngine;

namespace Game.Blocks
{
    public class TetrisBlock : MonoBehaviour
    {
        // Contants
        private const float FALL_TIME_BUFFER = .8f;
        
        // Inspector
        public Vector3 RotationPoint;
        public static int height = 20;
        public static int width = 10;

        // Internal
        private float fallTimer;

        // References
        internal Spawner spawner;
        private static Transform[,] grid = new Transform[width, height];

        
        private void Start() 
        {
            fallTimer = FALL_TIME_BUFFER;    
        }

        private void Update()
        {
            // Timers
            fallTimer -= Time.deltaTime;

            // Falling
            if (fallTimer < 0) {
                FallBlock();
            }
        }

        void AddToGrid()
        {
            foreach (Transform child in transform)
            {
                var position = child.transform.position;
                int roundedX = Mathf.RoundToInt(position.x );
                int roundedY = Mathf.RoundToInt(position.y);
                grid[roundedX, roundedY] = child;
            }
        }

        #region Validation
           private bool ValidMove(Vector2 position)
            {
                foreach (Transform child in transform)
                {
                    Vector2 tempPos = (Vector2) child.transform.position + position;
                    int roundedX = Mathf.RoundToInt(tempPos.x );
                    int roundedY = Mathf.RoundToInt(tempPos.y);

                    if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height || grid[roundedX,roundedY] != null) return false;
                }
                return true;
            }

            private bool ValidRotation()
            {
                foreach (Transform child in transform)
                {
                    Vector3 position = child.transform.position;
                    int roundedX = Mathf.RoundToInt(position.x);
                    int roundedY = Mathf.RoundToInt(position.y);

                    if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height || grid[roundedX,roundedY] != null) return false;
                }
                return true;
            }
        #endregion

        #region Transformation 
            /// <summary>
            /// Move the Tetris Block in the specified direction, if the movement is valid.
            /// </summary>
            /// <param name="direction">Direction of movement.</param>
            /// <returns>Whether the move succeeded.</returns>
            public bool MoveBlock(Vector2 direction)
            {
                if (ValidMove(direction)) {
                    transform.position += (Vector3) direction;
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Move the Tetris Block down, or into a locked position if movement is invalid.
            /// </summary>
            /// <returns>Returns true if the movement occurred, false if the block was locked.</returns>
            public bool FallBlock()
            {
                if (MoveBlock(Vector2.down)) {
                    // Move Down
                    fallTimer = FALL_TIME_BUFFER;
                    return true;
                } else {
                    // Lock Block
                    AddToGrid();
                    spawner.SpawnBlock();
                    this.enabled = false;
                }
                return false;
            }

            /// <summary>
            /// Rotate the Tetris Block.
            /// </summary>
            /// <returns>Whether the rotation succeeded.</returns>
            public bool RotateBlock()
            {
                transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0,0,1), 90);

                if (!ValidRotation()) {
                    transform.RotateAround(transform.TransformPoint(RotationPoint),new Vector3(0,0,1),-90);
                    return false;
                }
                return true;
            }
        #endregion
    }
}

