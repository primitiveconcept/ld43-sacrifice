namespace LetsStartAKittyCult
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;


    public class Health : ObservableRangeInt
    {
        [SerializeField]
        private float flickerInterval = 0.05f;

        [SerializeField]
        private Color damageFlickerColor = Color.red;

        [SerializeField]
        private Color invulnerabilityFlickerColor = Color.clear;

        [SerializeField]
        private float invulnerabilityDuration = 2f;

        [SerializeField]
        private bool invulerableOnSpawn = false;

        [SerializeField]
        private UnityEvent onDepleted;

        private bool isInvulnerable;
        private int originalHealth;


        #region Properties
        public bool IsInvulnerable
        {
            get { return this.isInvulnerable; }
        }


        public UnityEvent OnDepleted
        {
            get { return this.onDepleted; }
        }
        #endregion


        // TODO: Might want to pass duration value, for invulnerability powerups
        public void EnableTemporaryInvulnerability()
        {
            if (this.invulnerabilityDuration <= 0)
                return;

            StartCoroutine(InvulnerabilityCoroutine());
        }


        public void OnSpawn()
        {
            SetCurrent(this.originalHealth);
            if (this.invulerableOnSpawn)
                EnableTemporaryInvulnerability();
        }


        public override void Reduce(int amount, bool forceEvent = false)
        {
            if (this.isInvulnerable)
                return;

            base.Reduce(amount, forceEvent);

            if (this.Current == this.Min)
                this.onDepleted.Invoke();
        }


        public override void Start()
        {
            if (this.setToMaxOnStart)
                this.current = this.max;

            this.originalHealth = this.Current;
        }


        private IEnumerator InvulnerabilityCoroutine()
        {
            this.isInvulnerable = true;
            float invulnerabilityCounter = 0;
            yield return null; // Hack, sprite renderer may not be available on this frame.

            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            while (invulnerabilityCounter < this.invulnerabilityDuration)
            {
                spriteRenderer.color = spriteRenderer.color == Color.white
                                           ? this.invulnerabilityFlickerColor
                                           : Color.white;
                invulnerabilityCounter += GameTime.DeltaTime;
                yield return new WaitForSeconds(this.flickerInterval);
            }

            spriteRenderer.color = Color.white;
            this.isInvulnerable = false;
        }
    }
}