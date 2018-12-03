namespace LetsStartAKittyCult
{
    using System.Collections;
    using System.Runtime.InteropServices.WindowsRuntime;
    using UnityEngine;
    using UnityEngine.Events;


    public class Health : ObservableRangeInt
    {
        [SerializeField]
        private int lives = 9;

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

        [SerializeField]
        private IntegerEvent onLastLifeLost;

        private bool isInvulnerable;
        private int originalHealth;


        #region Properties
        public bool IsInvulnerable
        {
            get { return this.isInvulnerable; }
        }


        public int Lives
        {
            get { return this.lives; }
        }


        public UnityEvent OnDepleted
        {
            get { return this.onDepleted; }
        }
        #endregion


        public void ConsumeLife()
        {
            if (this.lives == 0)
            {
                return;
            }
            else if (this.lives < 0)
            {
                this.onDepleted.Invoke();
                return;
            }

            this.lives--;

            if (this.lives == 0)
                this.onLastLifeLost.Invoke(this.lives);
            else
                this.onDepleted.Invoke();
        }


        public void EnableTemporaryInvulnerability(float duration)
        {
            StartCoroutine(InvulnerabilityCoroutine(duration));
        }


        public void EnableTemporaryInvulnerability()
        {
            if (this.invulnerabilityDuration <= 0)
                return;

            StartCoroutine(InvulnerabilityCoroutine(this.invulnerabilityDuration));
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
                ConsumeLife();
            else if (this.invulnerabilityDuration > 0)
                EnableTemporaryInvulnerability();
        }


        public override void Start()
        {
            if (this.setToMaxOnStart)
                this.current = this.max;

            this.originalHealth = this.Current;
        }


        private IEnumerator InvulnerabilityCoroutine(float duration)
        {
            this.isInvulnerable = true;
            float invulnerabilityCounter = 0;
            yield return null; // Hack, sprite renderer may not be available on this frame.

            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            Color originalColor = spriteRenderer.color;
            while (invulnerabilityCounter < duration)
            {
                spriteRenderer.color = spriteRenderer.color == originalColor
                                           ? this.invulnerabilityFlickerColor
                                           : originalColor;
                invulnerabilityCounter += GameTime.DeltaTime;
                yield return new WaitForSeconds(this.flickerInterval);
            }

            spriteRenderer.color = originalColor;
            this.isInvulnerable = false;
        }
    }
}

#if UNITY_EDITOR
namespace LetsStartAKittyCult
{
    using UnityEditor;
    using UnityEngine;


    [CustomEditor(typeof(Health))]
    public class HealthEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (Application.isPlaying
                && GUILayout.Button("KILL INSTANTLY"))
            {
                Health health = this.target as Health;
                health.Reduce(health.Max);
            }
        }
    }
}
#endif