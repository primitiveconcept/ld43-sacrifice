namespace LetsStartAKittyCult
{
    using LetsStartAKittyCult.Minigames;
    using UnityEngine;


    public class Human : MonoBehaviour
    {
        [SerializeField]
        private float blessingLossRate = 0.01f;

        [SerializeField]
        private int toyIndex = 0;

        private Interactable interactable;
        private float currentBlessAmount;
        private bool incapacitated;


        #region Properties
        public float CurrentBlessAmount
        {
            get { return this.currentBlessAmount; }
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


        public void ResetBlessing()
        {
            this.currentBlessAmount = 0;
        }


        public void HideStats()
        {
            UIHub.HumanStats.Hide(this);
        }


        public void ReceiveBlessing(GameObject other)
        {
            Cat cat = other.GetComponent<Cat>();
            if (cat == null)
                return;

            cat.ActivateBlessing(this);
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
        }
    }
}