using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Character
{
    public class CharacterController : MonoBehaviour
    {
        // References
        private new Rigidbody2D rigidbody;


        private void Awake() 
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Move(InputAction.CallbackContext context) 
        {
            Vector2 input = context.ReadValue<Vector2>();
            rigidbody.AddForce(input * 100);
        }
    }
}
