namespace LetsStartAKittyCult
{
    using LetsStartAKittyCult.Minigames;
    using UnityEngine;


    public class Human : MonoBehaviour
    {
        public const string BlessCaption = "E: Bless";
        public const string SacrificeCaption = "E: Sacrifice";

        [SerializeField]
        private string humanName;

        [SerializeField]
        private Gender humanGender;

        [SerializeField]
        private float chattiness = 0.01f;

        [SerializeField]
        private float chatterDuration = 5f;

        [SerializeField]
        private float blessingLossRate = 0.01f;

        [SerializeField]
        private float timeToRecover = 7f;

        [SerializeField]
        private int toyIndex = 0;

        private Interactable interactable;
        private WalkAI walkAI;
        private float currentBlessAmount;
        private bool isIncapacitated;
        private bool isHappy;
        private float recoveryTimer;
        private float chatterTimer;


        public enum Gender
        {
            Nonbinary,
            Male,
            Female
        }


        private enum InteractionSetting
        {
            None,
            Bless,
            Sacrifice
        }


        #region Properties
        public float CurrentBlessAmount
        {
            get { return this.currentBlessAmount; }
        }


        public Gender HumanGender
        {
            get { return this.humanGender; }
        }


        public string HumanName
        {
            get { return this.humanName; }
            set { this.humanName = value; }
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
            this.humanName = newName;
            this.isHappy = true;
            Player.Get().Cult.AddCultMember(this);

            SetInteraction(InteractionSetting.None);
        }


        public void Awake()
        {
            this.interactable = GetComponent<Interactable>();
            this.walkAI = GetComponent<WalkAI>();

            string adjective = Strings.HumanAdjectives.GetRandom();
            string noun = Strings.HumanNouns.GetRandom();
            this.humanName = $"{adjective} {noun}";

            PickRandomGender();

            SetInteraction(InteractionSetting.Bless);
        }


        public void Chatter()
        {
            float chatterChance = Random.RandomRange(0f, 1f);
            if (chatterChance < this.chattiness
                && !GameTime.IsPaused)
                ShowChatter();
        }


        public void HideChatter()
        {
            if (this.interactable.CanInteract)
                return;

            this.chatterTimer = 0;
            this.interactable.HideCaption();
        }


        public void HideStats()
        {
            GameHub.HumanStats.Hide(this);
        }


        public void Incapacitate()
        {
            this.isIncapacitated = true;
            this.recoveryTimer = this.timeToRecover;
            this.walkAI.Lock();
            if (this.isHappy)
                SetInteraction(InteractionSetting.Sacrifice);
            else
                SetInteraction(InteractionSetting.None);
        }


        public void ReceiveBlessing(GameObject other)
        {
            Cat cat = other.GetComponent<Cat>();
            if (cat == null)
                return;

            cat.ActivateBlessing(this);
            this.walkAI.CurrentWalkDirection = WalkAI.WalkDirection.Idle;
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
            this.walkAI.Unlock();

            SetInteraction(InteractionSetting.Bless);
        }


        public void Sacrifice()
        {
            Player.Get().Cult.SacrificeHuman(this);
            this.interactable.onLeave.Invoke();
            this.gameObject.SetActive(false);
            GameHub.GameWorld.gameObject.SetActive(false);
            GameHub.SacrificeCutscene.gameObject.SetActive(true);
            GameHub.SacrificeCutscene.Play();
        }


        public void Seduce()
        {
            GameHub.CaptureMinigame.StartGame(this);
        }


        public void ShowChatter()
        {
            if (this.interactable.CanInteract)
                return;

            string idleChatter;

            if (this.isHappy)
                idleChatter = Strings.HumanIdleHappyPhrases.GetRandom();
            else
                idleChatter = Strings.HumanIdlePhrases.GetRandom();

            this.interactable.ShowCaption(idleChatter);
            this.chatterTimer = this.chatterDuration;
        }


        public void ShowStats()
        {
            GameHub.HumanStats.Show(this);
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

            if (this.chatterTimer > 0)
            {
                this.chatterTimer -= GameTime.DeltaTime;
                if (this.chatterTimer <= 0)
                    HideChatter();
            }
            else
            {
                Chatter();
            }
        }


        private void PickRandomGender(bool secondAttempt = false)
        {
            int genderIndex = Random.Range(0, 3);
            this.humanGender = (Gender)genderIndex;

            // "Other" less common, reroll one more time to reduce chance.
            if (this.humanGender == Gender.Nonbinary
                && !secondAttempt)
                PickRandomGender(true);
        }


        private void SetInteraction(InteractionSetting interaction)
        {
            this.interactable.onInteract.RemoveAllListeners();

            switch (interaction)
            {
                case InteractionSetting.None:
                    this.interactable.CaptionText = "";
                    break;
                case InteractionSetting.Bless:
                    this.interactable.CaptionText = $"{BlessCaption} {this.humanName}".ToUpper();
                    this.interactable.onInteract.AddListener(ReceiveBlessing);
                    break;
                case InteractionSetting.Sacrifice:
                    this.interactable.CaptionText = $"{SacrificeCaption} {this.humanName}".ToUpper();
                    this.interactable.onInteract.AddListener(target => Sacrifice());
                    break;
            }
        }
    }
}


#if UNITY_EDITOR
namespace LetsStartAKittyCult
{
    using UnityEditor;
    using UnityEngine;


    [CustomEditor(typeof(Human))]
    public class HumanEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying
                && GUILayout.Button("SACRIFICE"))
            {
                Human human = this.target as Human;
                human.Sacrifice();
            }
        }
    }
}
#endif