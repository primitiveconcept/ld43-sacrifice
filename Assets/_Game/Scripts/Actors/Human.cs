namespace LetsStartAKittyCult
{
    using LetsStartAKittyCult.Minigames;
    using UnityEngine;


    public class Human : MonoBehaviour
    {
        [SerializeField]
        private string givenName;

        [SerializeField]
        private float blessingLossRate = 0.01f;

        [SerializeField]
        private float timeToRecover = 7f;

        [SerializeField]
        private int toyIndex = 0;

        private Interactable interactable;
        private float currentBlessAmount;
        private bool isIncapacitated;
        private bool isHappy;
        private float recoveryTimer;


        #region Properties
        public float CurrentBlessAmount
        {
            get { return this.currentBlessAmount; }
        }


        public string GivenName
        {
            get { return this.givenName; }
            set { this.givenName = value; }
        }


        public bool IsHappy
        {
            get { return this.isHappy; }
            set { this.isHappy = value; }
        }


        public bool IsIncapacitated
        {
            get { return this.isIncapacitated; }
            set { this.isIncapacitated = value; }
        }


        public float RecoveryTimer
        {
            get { return this.recoveryTimer; }
        }


        public float TimeToRecover
        {
            get { return this.timeToRecover; }
        }


        public int ToyIndex
        {
            get { return this.toyIndex; }
            set { this.toyIndex = value; }
        }
        #endregion


        public void AddBlessing(float amount)
        {
            this.currentBlessAmount += amount;
            if (this.currentBlessAmount >= 1)
            {
                this.currentBlessAmount = 1;
                Seduce();
            }
        }


        public void Adopt(string newName)
        {
            this.givenName = newName;
            this.isHappy = true;
            Player.Get().Cult.Members.Add(this);
        }


        public void Awake()
        {
            string adjective = Strings.HumanAdjectives.GetRandom();
            string noun = Strings.HumanNouns.GetRandom();
            this.givenName = $"{adjective} {noun}";
        }


        public void HideStats()
        {
            UIHub.HumanStats.Hide(this);
        }


        public void Incapacitate()
        {
            this.isIncapacitated = true;
            this.recoveryTimer = this.timeToRecover;
        }


        public void ReceiveBlessing(GameObject other)
        {
            Cat cat = other.GetComponent<Cat>();
            if (cat == null)
                return;

            cat.ActivateBlessing(this);
        }


        public void ResetBlessing()
        {
            this.currentBlessAmount = 0;
        }


        public void Revive()
        {
            this.isIncapacitated = false;
            this.recoveryTimer = 0;
            this.isHappy = false;
            this.currentBlessAmount = 0;
            Health health = GetComponent<Health>();
            health.SetCurrent(health.Max);
        }


        public void Seduce()
        {
            UIHub.CaptureMinigame.StartGame(this);
        }


        public void ShowStats()
        {
            UIHub.HumanStats.Show(this);
        }


        public void Update()
        {
            if (this.currentBlessAmount > 0
                && this.currentBlessAmount < 1)
            {
                this.currentBlessAmount -= this.blessingLossRate;
                if (this.currentBlessAmount < 0)
                    this.currentBlessAmount = 0;
            }

            if (this.isIncapacitated
                && this.recoveryTimer > 0)
            {
                this.recoveryTimer -= GameTime.DeltaTime;
                if (this.recoveryTimer <= 0)
                    Revive();
            }
        }
    }
}