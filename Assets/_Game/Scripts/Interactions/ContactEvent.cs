namespace LetsStartAKittyCult
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;


    [RequireComponent(typeof(Collider2D))]
    public class ContactEvent : MonoBehaviour
    {
        [SerializeField]
        private string[] onlyAffectsTags;

        [SerializeField]
        private TriggerEvent onTriggerEnter;

        [SerializeField]
        private TriggerEvent onTriggerStay;

        [SerializeField]
        private TriggerEvent onTriggerExit;

        [SerializeField]
        private CollisionEvent onCollisionEnter;

        [SerializeField]
        private CollisionEvent onCollisionStay;

        [SerializeField]
        private CollisionEvent onCollisionExit;


        public void OnCollisionEnter2D(Collision2D other)
        {
            if (!Tags.HasTag(other.gameObject, this.onlyAffectsTags))
                return;

            if (this.onCollisionEnter != null)
                this.onCollisionEnter.Invoke(other);
        }


        public void OnCollisionExit2D(Collision2D other)
        {
            if (!Tags.HasTag(other.gameObject, this.onlyAffectsTags))
                return;

            if (this.onCollisionExit != null)
                this.onCollisionExit.Invoke(other);
        }


        public void OnCollisionStay2D(Collision2D other)
        {
            if (!Tags.HasTag(other.gameObject, this.onlyAffectsTags))
                return;

            if (this.onCollisionStay != null)
                this.onCollisionStay.Invoke(other);
        }


        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!Tags.HasTag(other.gameObject, this.onlyAffectsTags))
                return;

            if (this.onTriggerEnter != null)
                this.onTriggerEnter.Invoke(other);
        }


        public void OnTriggerExit2D(Collider2D other)
        {
            if (!Tags.HasTag(other.gameObject, this.onlyAffectsTags))
                return;

            if (this.onTriggerExit != null)
                this.onTriggerExit.Invoke(other);
        }


        public void OnTriggerStay2D(Collider2D other)
        {
            if (!Tags.HasTag(other.gameObject, this.onlyAffectsTags))
                return;

            if (this.onTriggerStay != null)
                this.onTriggerStay.Invoke(other);
        }


        [Serializable]
        public class CollisionEvent : UnityEvent<Collision2D>
        {
        }


        [Serializable]
        public class TriggerEvent : UnityEvent<Collider2D>
        {
        }
    }
}