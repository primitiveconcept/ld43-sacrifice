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
        public InteractableEvent onInteract;

        [SerializeField]
        public InteractableEvent onCancelInteract;
        
        [SerializeField]
        public UnityEvent onApproach;

        [SerializeField]
        public UnityEvent onLeave;

        private bool canInteract;


        #region Properties
        public string CaptionText
        {
            get { return this.captionText; }
            set { this.captionText = value; }
        }


        public bool CanInteract
        {
            get { return this.canInteract; }
        }


        public TextMeshPro CaptionField
        {
            get { return this.captionField; }
        }
        #endregion


        public void Awake()
        {
            HideCaption();
        }
        

        public void AllowInteraction(GameObject other)
        {
            Player.Get(0).SetInteractable(this);
            this.canInteract = true;
            this.onApproach.Invoke();

            ShowCaption(this.captionText, Color.yellow);
        }


        public void DisallowInteraction(GameObject other)
        {
            Player.Get(0).UnsetInteractable(this);
            this.canInteract = false;
            this.onLeave.Invoke();

            HideCaption();
        }


        public void HideCaption()
        {
            if (this.captionField == null)
                return;

            this.captionField.gameObject.SetActive(false);
        }


        public void Interact(GameObject other)
        {
            Debug.Log("Interacted with: " + this.gameObject.name);
            this.onInteract.Invoke(other);
        }


        public void CancelInteract(GameObject other)
        {
            Debug.Log("Cancel interaction with: " + this.gameObject.name);
            this.onCancelInteract.Invoke(other);
        }


        public void ShowCaption(string text, Color color)
        {
            ShowCaption(text);
            this.captionField.color = color;
        }
        
        
        public void ShowCaption(string text)
        {
            if (text == null)
                return;

            if (string.IsNullOrEmpty(text))
                return;

            this.captionField.color = Color.white;
            this.captionField.text = text;
            this.captionField.gameObject.SetActive(true);
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