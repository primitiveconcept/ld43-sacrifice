namespace LetsStartAKittyCult
{
    using UnityEngine;


    public class Cat : MonoBehaviour
    {
        [Header("Rewspawn")]
        [SerializeField]
        private float respawnTime = 5f;

        [SerializeField]
        private Transform respawnTarget; 
        
        [Header("Blessing")]
        [SerializeField]
        private float blessAmount = 0.35f;

        [SerializeField]
        private float blessingCooldown = 1f;

        private SpriteSorter spriteSorter;
        private WalkAI walkAI;
        private Health health;

        private float blessingCooldownTimer;
        private float respawnTimer;
        private bool isBlessing;

        


        #region Properties
        public bool IsBlessing
        {
            get { return this.isBlessing; }
        }


        public SpriteSorter SpriteSorter
        {
            get { return this.spriteSorter; }
        }
        #endregion


        public void ActivateBlessing(Human human)
        {
            if (this.isBlessing)
                return;

            human.AddBlessing(this.blessAmount);
            this.isBlessing = true;
            this.blessingCooldownTimer = this.blessingCooldown;
        }


        public void TriggerRespawnTimer()
        {
            if (this.walkAI != null)
                this.walkAI.Lock();
            
            this.health.EnableTemporaryInvulnerability(this.respawnTime);
            this.respawnTimer = this.respawnTime;
            this.transform.position = this.respawnTarget.position;
        }
        

        public void Respawn()
        {
            if (this.walkAI != null)
                this.walkAI.Unlock();
            
            this.health.SetToMax();
        }
        

        public void Awake()
        {
            this.spriteSorter = GetComponent<SpriteSorter>();
            this.walkAI = GetComponent<WalkAI>();
            this.health = GetComponent<Health>();
        }     


        public void Update()
        {
            if (this.blessingCooldownTimer > 0)
            {
                this.blessingCooldownTimer -= GameTime.DeltaTime;
                if (this.blessingCooldownTimer <= 0)
                    this.isBlessing = false;
            }

            if (this.respawnTimer > 0)
            {
                this.respawnTimer -= GameTime.DeltaTime;
                if (this.respawnTimer <= 0)
                    Respawn();
            }
        }

    }
}