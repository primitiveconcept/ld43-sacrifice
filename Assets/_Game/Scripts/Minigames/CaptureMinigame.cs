namespace LetsStartAKittyCult.Minigames
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityStandardAssets.CrossPlatformInput;


    public class CaptureMinigame : Minigame<CaptureMinigame>
    {
        [SerializeField]
        private float timeLimit;

        [Header("Assets")]
        [SerializeField]
        private CaptureToy[] toys;

        [SerializeField]
        private GameObject mainTextPanel;

        [SerializeField]
        private UIMeter timeLimitMeter;

        [SerializeField]
        private CapturePaws paws;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onWin;

        [SerializeField]
        private UnityEvent onLose;

        private bool hasStarted;
        private float timeLimitTimer;
        private int targetScore;
        private int currentScore;
        private Human targetHuman;


        #region Properties
        public static bool HasStarted
        {
            get { return Instance.hasStarted; }
        }


        public static Human TargetHuman
        {
            get { return Instance.targetHuman; }
            set { Instance.targetHuman = value; }
        }
        #endregion


        public void Begin()
        {
            this.hasStarted = true;
            HideMainText();
        }


        public void IncrementScore()
        {
            this.currentScore++;

            if (this.currentScore != this.targetScore)
                return;

            Debug.Log("WON CAPTURE MINIGAME, YAY!!!");
            this.onWin.Invoke();
        }


        public override void Initialize()
        {
            this.hasStarted = false;
            this.currentScore = 0;
            this.timeLimitTimer = this.timeLimit;
        }


        public void Lose()
        {
            this.hasStarted = false;
            this.targetHuman.ResetBlessing();
            this.onLose.Invoke();
        }


        public void StartGame(Human human)
        {
            Show();
            this.targetHuman = human;
            this.hasStarted = false;

            foreach (CaptureToy availableToy in Instance.toys)
                availableToy.gameObject.SetActive(false);

            CaptureToy toy = Instance.toys[human.ToyIndex];
            this.targetScore = toy.TargetScore;
            toy.gameObject.SetActive(true);

            string adjective = Strings.HumanAdjectives.GetRandom();
            string noun = Strings.HumanNouns.GetRandom();
            string introText = $"{adjective} {noun} wants to play with the {toy.ToyName}";
            ShowMainText(introText);
        }


        public void Update()
        {
            if (!this.hasStarted)
                return;

            if (this.timeLimitTimer > 0)
            {
                this.timeLimitTimer -= GameTime.DeltaTime;
                if (this.timeLimitTimer <= 0)
                {
                    this.timeLimitTimer = 0;
                    Lose();
                }

                this.timeLimitMeter.UpdateMeter(this.timeLimitTimer / this.timeLimit);
            }

            if (CrossPlatformInputManager.GetButtonDown(Controls.Primary))
            {
                Vector3 mousePosition = Input.mousePosition;
                //mousePosition.z = 10;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                mousePosition.z = 0;
                this.paws.Attack(mousePosition);
            }
        }


        private void HideMainText()
        {
            this.mainTextPanel.SetActive(false);
        }


        private void ShowMainText(string text)
        {
            this.mainTextPanel.gameObject.SetActive(true);
            TextMeshProUGUI textMesh = this.mainTextPanel.GetComponentInChildren<TextMeshProUGUI>();
            textMesh.text = text;
        }
    }
}