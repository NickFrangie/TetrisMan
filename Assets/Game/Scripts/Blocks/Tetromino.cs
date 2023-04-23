using UnityEngine.Tilemaps;
using UnityEngine;

namespace Game.Blocks
{
    public class Tetromino : MonoBehaviour
    {
        // Inspector
        public Vector3 RotationPoint;

        // Internal
        private float fallTimer;

        
        private void Start() 
        {
            fallTimer = BlockManager.instance.FallTimeBuffer;    
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
        

        #region Validation
            private bool ValidPosition()
            {
                foreach (Transform child in transform) {
                    Vector2Int blockPosition = Vector2Int.RoundToInt((Vector2) child.transform.position);
                    if (!BlockManager.instance.PositionInBounds(blockPosition) || !BlockManager.instance.PositionIsFree(blockPosition)) return false;
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
                transform.position += (Vector3) direction;

                if (!ValidPosition()) {
                    transform.position -= (Vector3) direction;
                    return false;
                }
                return true;
            }

            /// <summary>
            /// Move the Tetris Block down, or into a locked position if movement is invalid.
            /// </summary>
            /// <returns>Returns true if the movement occurred, false if the block was locked.</returns>
            public void FallBlock()
            {
                if (MoveBlock(Vector2.down)) {
                    // Move Down
                    fallTimer = BlockManager.instance.FallTimeBuffer;
                } else {
                    // Lock Block
                    this.enabled = false;
                    BlockManager.instance.AddToGrid(this);
                }
            }

            /// <summary>
            /// Rotate the Tetris Block.
            /// </summary>
            /// <returns>Whether the rotation succeeded.</returns>
            public bool RotateBlock()
            {
                transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0,0,1), 90);

                if (!ValidPosition()) {
                    transform.RotateAround(transform.TransformPoint(RotationPoint),new Vector3(0,0,1),-90);
                    return false;
                }
                return true;
            }
        #endregion

        #region Debug
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(RotationPoint, .1f);
        }
        #endregion
    }
}

