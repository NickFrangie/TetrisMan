using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Character
{
    public class CharacterController : MonoBehaviour
    {
        public void Move(InputAction.CallbackContext context) 
        {
            Vector2 input = context.ReadValue<Vector2>();
            transform.position += (Vector3) input;
        }
    }
}
