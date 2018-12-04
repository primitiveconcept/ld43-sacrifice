namespace LetsStartAKittyCult
{
    using LetsStartAKittyCult.Exensions.Physics;
    using UnityEngine;
    public class Projectile : MonoBehaviour
    {
        public float timeToLive;
        public Vector2 attackVector = Vector2.zero;

        private Rigidbody2D rigidbody2D;
        private ContactDamage contactDamage;
        private float timeToLiveCounter;

        public void Awake()
        {
            this.rigidbody2D = this.gameObject.SetupRigidbody();
            
            this.contactDamage = GetComponent<ContactDamage>();
            this.timeToLiveCounter = this.timeToLive;
        }
        
        public void Update()
        {
            this.rigidbody2D.velocity = this.attackVector;

            this.timeToLiveCounter -= GameTime.DeltaTime;
            if (this.timeToLiveCounter <= 0)
            {
                Destroy(this.gameObject);
            }
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!Tags.HasTag(other.gameObject, this.contactDamage.OnlyAffectsTags))
                return;
            
            Destroy(this.gameObject);
        }
    }
}