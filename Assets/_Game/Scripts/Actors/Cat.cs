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

        [SerializeField]
        private RivalGod deity;

        [Header("Blessing")]
        [SerializeField]
        private float blessAmount = 0.35f;

        [SerializeField]
        private float blessingCooldown = 1f;

        private SpriteSorter spriteSorter;
        private WalkAI walkAI;
        private AttackAI attackAI;
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


        public void Awake()
        {
            this.spriteSorter = GetComponent<SpriteSorter>();
            this.walkAI = GetComponent<WalkAI>();
            this.attackAI = GetComponent<AttackAI>();
            this.health = GetComponent<Health>();
            
            if (this.deity != null)
                this.deity.AddCat(this);
        }


        public void Respawn()
        {
            if (this.walkAI != null)
                this.walkAI.Unlock();

            this.health.SetToMax();
            this.transform.position = this.respawnTarget.position;

            if (GetComponent<Player>() != null)
            {
                Vector3 cameraTarget = this.respawnTarget.position;
                cameraTarget.z = Camera.main.transform.position.z;
                Camera.main.transform.position = cameraTarget;
            }
        }


        public void TriggerRespawnTimer()
        {
            if (this.walkAI != null)
                this.walkAI.Lock();

            if (this.attackAI != null)
                this.attackAI.Lock();

            this.health.EnableTemporaryInvulnerability(this.respawnTime);
            this.respawnTimer = this.respawnTime;
            this.transform.position = this.respawnTarget.position;
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


        public void OnDestroy()
        {
            if (this.deity != null)
                this.deity.RemoveCat(this);
        }
    }
}