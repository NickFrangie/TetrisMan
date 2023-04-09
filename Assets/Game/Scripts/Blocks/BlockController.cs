using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Blocks
{
    /// <summary>
    /// Tetris Block Control for the player controlling Tetris Blocks.
    /// </summary>
    public class BlockController : MonoBehaviour
    {
        // Inspector
        [Header("Movement")]
        [SerializeField] private float xMoveTimeBuffer = .8f;
        [SerializeField] private float yMoveTimeBuffer = .4f;

        // Internal
        private float xMoveTimer, yMoveTimer;
        private Vector2 moveInput;
        internal TetrisBlock activeBlock;


        private void Update()
        {
            // Timers
            xMoveTimer -= Time.deltaTime;
            yMoveTimer -= Time.deltaTime;

            if (activeBlock) {
                // Horizontal Movement
                if (xMoveTimer < 0 && moveInput.x != 0) {
                    Vector2 moveDirection = new Vector2(Mathf.Sign(moveInput.x), 0);

                    if (activeBlock.MoveBlock(moveDirection)) {
                        xMoveTimer = xMoveTimeBuffer;
                    }
                }

                // Vertical Movement
                if (yMoveTimer < 0 && moveInput.y < 0) {
                    Vector2 moveDirection = Vector2.down;

                    activeBlock.FallBlock();
                    yMoveTimer = yMoveTimeBuffer;
                }
            }
        }

        #region Player Input
            public void Move(InputAction.CallbackContext context) 
            {
                moveInput = context.ReadValue<Vector2>();
                xMoveTimer = 0;
                yMoveTimer = 0;
            }

            public void Rotate(InputAction.CallbackContext context)
            {
                if (context.performed) activeBlock.RotateBlock();
            }
        #endregion
    }
} 