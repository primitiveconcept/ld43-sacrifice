namespace LetsStartAKittyCult
{
    using System.Collections.Generic;
    using UnityEngine;


    public class ContactDamage : MonoBehaviour
    {
        [SerializeField]
        private int damage;

        [SerializeField]
        private float damageInterval;

        [SerializeField]
        private bool destroyOnContact;

        [SerializeField]
        private string[] onlyAffectsTags;

        private List<GameObject> affectedObjects = new List<GameObject>();
        private float counter;


        #region Properties
        public string[] OnlyAffectsTags
        {
            get { return this.onlyAffectsTags; }
            set { this.onlyAffectsTags = value; }
        }
        #endregion


        public void OnCollisionEnter2D(Collision2D other)
        {
            if (!Tags.HasTag(other.gameObject, this.onlyAffectsTags))
                return;

            Health otherHealth = other.gameObject.GetComponent<Health>();
            DoDamage(otherHealth);
        }


        public void OnCollisionExit2D(Collision2D other)
        {
            if (!Tags.HasTag(other.gameObject, this.onlyAffectsTags))
                return;

            if (this.affectedObjects.Contains(other.gameObject))
                this.affectedObjects.Remove(other.gameObject);

            if (this.affectedObjects.Count < 1)
                this.counter = 0;
        }


        public void OnCollisionStay2D(Collision2D other)
        {
            if (!Tags.HasTag(other.gameObject, this.onlyAffectsTags))
                return;

            if (this.damageInterval <= 0)
                return;

            Health otherHealth = other.gameObject.GetComponent<Health>();
            DoDamageOverTime(otherHealth);
        }


        public void OnDespawn()
        {
            this.affectedObjects.Clear();
            this.counter = 0;
        }


        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!Tags.HasTag(other.gameObject, this.onlyAffectsTags))
                return;

            Health otherHealth = other.GetComponent<Health>();
            DoDamage(otherHealth);
        }


        public void OnTriggerExit2D(Collider2D other)
        {
            if (!Tags.HasTag(other.gameObject, this.onlyAffectsTags))
                return;

            if (this.affectedObjects.Contains(other.gameObject))
                this.affectedObjects.Remove(other.gameObject);

            if (this.affectedObjects.Count < 1)
                this.counter = 0;
        }


        public void OnTriggerStay2D(Collider2D other)
        {
            if (!Tags.HasTag(other.gameObject, this.onlyAffectsTags))
                return;

            if (this.damageInterval <= 0)
                return;

            Health otherHealth = other.GetComponent<Health>();
            if (otherHealth != null)
                DoDamageOverTime(otherHealth);
        }


        private void DoDamage(Health otherHealth)
        {
            if (this.destroyOnContact)
            {
                Destroyable destroyable = GetComponent<Destroyable>();
                if (destroyable != null)
                    destroyable.DestroyImmediately();
            }

            if (otherHealth == null)
                return;

            if (!this.affectedObjects.Contains(otherHealth.gameObject))
                this.affectedObjects.Add(otherHealth.gameObject);

            otherHealth.Reduce(this.damage);
        }


        private void DoDamageOverTime(Health otherHealth)
        {
            this.counter += GameTime.DeltaTime;

            if (this.counter < this.damageInterval)
                return;

            otherHealth.Reduce(this.damage);
            this.counter = 0;
        }
    }
}