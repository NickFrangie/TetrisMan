using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interaction
{
    public class Interactable : MonoBehaviour
    {
        [Header("Focus")]
        [SerializeField] private UnityEvent onFocus, offFocus;


        public void Focus()
        {
            onFocus?.Invoke();
        }

        public void Unfocus()
        {
            offFocus?.Invoke();
        }
    }
}
