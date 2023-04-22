using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Blocks;

namespace Game.Character
{
    /// <summary>
    /// Character Control and Movement handling for the Platformer Character.
    /// Code adapted from tutorial: https://www.youtube.com/watch?v=KbtcEVCM7bw
    /// </summary>
    public class CharacterController : MonoBehaviour
    {
        // Constants
        private const float JUMP_BUFFER_TIME = .1f;

        // Inspector
        [Header("Running")]
        [SerializeField] private float maxSpeed = 10; 
        [SerializeField] private float accelerationRate = 0.5f;
        [SerializeField] private float deaccelerationRate = 1.0f;
        [SerializeField] private float velocityPower = 1.5f;
        [SerializeField] private float frictionAmount = 1.0f;

        [Header("Jumping")]
        [SerializeField] private float jumpForce = 5;
        [SerializeField] private float jumpCoyoteTime = .1f;
        [Range(0,1)] [SerializeField] private float jumpCutMultiplier = .2f;
        [SerializeField] private float gravityScale = 6f;
        [SerializeField] private float fallGravityMultiplier = 1.2f;
        [SerializeField] private Transform groundCheckPoint; 
        [SerializeField] private Vector2 groundCheckSize = Vector2.one;
        [SerializeField] LayerMask groundCheckLayer = 0;

        [Header("Interaction")]
        [SerializeField] private GameObject placementBlockPrefab;
        [SerializeField] private Transform interactionPoint;

        // Internal
        private Vector2 moveInput;
        private float jumpTimer = 0;
        private float coyoteTimer = 0;
        private bool isJumping = true;
        private bool stageJump = false;
        private bool stageJumpCancel = false;
        private IndividualBlock focusedBlock;
        private IndividualBlock heldBlock;
        private GameObject placementBlock;

        // References
        private Animator animator;
        private new Rigidbody2D rigidbody;


        private void Awake() 
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update() 
        {
            IndividualBlock currentFocus = CheckInteraction();
            if (heldBlock) {
                placementBlock.SetActive(!currentFocus);
                if (!currentFocus) {
                    placementBlock.transform.position = Vector3Int.RoundToInt(interactionPoint.transform.position);
                }
            } else {
                if (currentFocus != focusedBlock) {
                    if (currentFocus) currentFocus.OnFocus();
                    if (focusedBlock) focusedBlock.OffFocus();
                }    
            }
            focusedBlock = currentFocus;
        }

        private void FixedUpdate() 
        {
            // Timers
            jumpTimer -= Time.fixedDeltaTime;
            coyoteTimer -= Time.fixedDeltaTime;
            
            // Input
            float targSpeed = moveInput.x * maxSpeed;
            float velDiff = targSpeed - rigidbody.velocity.x;
            float acceleration = (Mathf.Abs(targSpeed) > 0.1f) ? accelerationRate : deaccelerationRate;
            float movement = Mathf.Pow(Mathf.Abs(velDiff) * acceleration, velocityPower) * Mathf.Sign(velDiff);
            rigidbody.AddForce(movement * Vector2.right);

            // Grounded Information
            if (isGrounded()) {
                coyoteTimer = jumpCoyoteTime;

                // Friction
                if (Mathf.Abs(moveInput.x) < 0.1f) {
                    float friction = Mathf.Min(Mathf.Abs(rigidbody.velocity.x), Mathf.Abs(frictionAmount));
                    friction *= Mathf.Sign(rigidbody.velocity.x);
                    rigidbody.AddForce(-friction * Vector2.right, ForceMode2D.Impulse);
                }

                // Landing
                if (isJumping && jumpTimer <= 0) {
                    animator.SetTrigger("Landed");
                    isJumping = false;
                }
            }

            // Jump Cancel
            if (isJumping && stageJumpCancel && jumpTimer <= 0) JumpCancel();

            // Gravity
            rigidbody.gravityScale = (rigidbody.velocity.y > 0) ? gravityScale : gravityScale * fallGravityMultiplier;
        }

        private void LateUpdate() 
        {
            // Animator Updates
            animator.SetFloat("Horizontal", Mathf.Abs(rigidbody.velocity.x));
            animator.SetFloat("Vertical", rigidbody.velocity.y);
            animator.SetBool("isGrounded", isGrounded());
        }

        #region Player Input
        public void Move(InputAction.CallbackContext context) 
        {
            moveInput = context.ReadValue<Vector2>();

            if (moveInput.x != 0) {
                transform.localScale = new Vector2(Mathf.Sign(moveInput.x), 1);
            }
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (!context.canceled) {
                // Jump Start
                if ((isGrounded() || isCoyoteTime()) && !isJumping) {
                    stageJump = true;
                    animator.SetTrigger("Jumped");
                }
            } else if (stageJump || isJumping) {
                // Jump Cancel
                stageJumpCancel = true;
            }
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (context.performed) {
                if (heldBlock) {
                    TryPlaceBlock();
                } else {
                    TryPickupBlock();
                }
            }
        }
        #endregion

        #region Character Movement
        /// <summary>
        /// Applies force of jump.
        /// </summary>
        public void JumpForce()
        {
            stageJump = false;
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = true;
            jumpTimer = JUMP_BUFFER_TIME;
        }

        /// <summary>
        /// Applies jump cut.
        /// </summary>
        public void JumpCancel()
        {
            stageJumpCancel = false;
            if (rigidbody.velocity.y > 0) {
                rigidbody.AddForce(Vector2.down * rigidbody.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
            }
        }
        #endregion

        #region Interaction
        private IndividualBlock CheckInteraction()
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector3) Vector3Int.RoundToInt(interactionPoint.position), 0.5f * Vector2.one, 0);
            foreach (Collider2D collider in colliders) {
                IndividualBlock block;
                if (collider.TryGetComponent<IndividualBlock>(out block)) {
                    return block;
                }
            }
            return null;
        }

        /// <summary>
        /// Attempt to pickup the block near the interaction point. 
        /// </summary>
        /// <returns>Whether a block was picked up.</returns>
        private bool TryPickupBlock()
        {
            if (focusedBlock) {
                heldBlock = focusedBlock;
                heldBlock.gameObject.SetActive(false);

                // Spawn Placement Block if it does not exist
                if (!placementBlock) placementBlock = Instantiate(placementBlockPrefab.gameObject);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempt to place the block near the interaction point. 
        /// </summary>
        /// <returns>Whether a block was placed down.</returns>
        private bool TryPlaceBlock()
        {
            if (!focusedBlock) {
                // Hide Placement Block
                placementBlock.gameObject.SetActive(false);

                // Spawn Held Block
                heldBlock.transform.position = Vector3Int.RoundToInt(interactionPoint.transform.position);
                heldBlock.gameObject.SetActive(true);
                heldBlock = null;
                return true;
            }
            return false;
        }
        #endregion

        #region Utility
        /// <summary>
        /// Checks whether the character is on the ground.
        /// </summary>
        /// <returns>Whether the character is on the ground.</returns>
        private bool isGrounded()
        {
            return Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundCheckLayer);
        }

        /// <summary>
        /// Checks whether coyote time is active, which determine whether a player is able to jump after falling off a platform.
        /// </summary>
        /// <returns>Whether coyote time is active.</returns>
        private bool isCoyoteTime()
        {
            return (coyoteTimer > 0);
        }
        #endregion

        #region Debug
        private void OnDrawGizmos() 
        {
            // Ground Check Box
            if (groundCheckPoint) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);    
            }

            // Interaction Point
            if (interactionPoint) {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube((Vector3) Vector3Int.RoundToInt(interactionPoint.position), Vector3.one); 
            }
        }
        #endregion
    }
}
