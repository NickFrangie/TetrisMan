using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractionZone : MonoBehaviour
    {
        // Inspector
        [Header("Focus")]
        [SerializeField] private Transform sightOriginPoint;
        [SerializeField] LayerMask sightCheckLayer = 0;

        // Internal
        private List<Interactable> inZone = new List<Interactable>();
        private Interactable focused;


        private void Update() 
        {
            UpdateFocus();
        }

        #region Focus
        private void UpdateFocus()
        {
            focused?.Unfocus();
            
            if (inZone.Count == 0) {
                focused = null;
            } else if (inZone.Count == 1) {
                focused = inZone[0];
            } else {
                focused = DetermineFocus();
            }

            focused?.Focus();
        }

        private Interactable DetermineFocus()
        {
            float distance = Vector2.Distance(transform.position, sightOriginPoint.position) * 2;
            Vector2 direction = transform.position - sightOriginPoint.position;
            RaycastHit2D hit = Physics2D.Raycast(sightOriginPoint.position, direction, distance, sightCheckLayer);
            
            if (hit) {
                foreach (Interactable interactable in inZone) {
                    if (hit.transform.gameObject == interactable.gameObject) {
                        return interactable;
                    }
                }
            }
            return null;
        }
        #endregion

        #region Trigger Events
        private void OnTriggerEnter2D(Collider2D other) 
        {
            Interactable interactable;
            if (other.TryGetComponent<Interactable>(out interactable)) {
                inZone.Add(interactable);
            }
        }

        private void OnTriggerExit2D(Collider2D other) 
        {
            Interactable interactable;
            if (other.TryGetComponent<Interactable>(out interactable)) {
                interactable.Unfocus();
                inZone.Remove(interactable);
            }
        }
        #endregion

        #region Gizmos
        private void OnDrawGizmos() {
            if (sightOriginPoint) {
                Gizmos.color = Color.yellow;

                float distance = Vector2.Distance(transform.position, sightOriginPoint.position) * 2;
                Vector2 direction = transform.position - sightOriginPoint.position;
                Gizmos.DrawLine(sightOriginPoint.position, sightOriginPoint.position + (Vector3) direction * distance);
            }
        }
        #endregion
    }
}
