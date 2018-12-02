namespace LetsStartAKittyCult
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;


    public class Interactable : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro captionField;

        [SerializeField]
        private string captionText;

        [SerializeField]
        private InteractableEvent onInteract;


        public void Interact(GameObject other)
        {
            Debug.Log("Interacted with: " + this.gameObject.name);
            this.onInteract.Invoke(other);
        }
        

        public void AllowInteraction(GameObject other)
        {
            Player.Get(0).SetInteractable(this);

            if (this.captionField == null)
                return;

            if (string.IsNullOrEmpty(this.captionText))
                return;

            this.captionField.text = this.captionText;
            this.captionField.gameObject.SetActive(true);
        }


        public void DisallowInteraction(GameObject other)
        {
            Player.Get(0).UnsetInteractable(this);

            if (this.captionField == null)
                return;

            this.captionField.gameObject.SetActive(false);
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.HasTag(Tags.Player))
                AllowInteraction(other.gameObject);
        }


        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.HasTag(Tags.Player))
                DisallowInteraction(other.gameObject);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.HasTag(Tags.Player))
                AllowInteraction(other.gameObject);
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.HasTag(Tags.Player))
                DisallowInteraction(other.gameObject);
        }
    }


    [Serializable]
    public class InteractableEvent : UnityEvent<GameObject>
    {
    }
}