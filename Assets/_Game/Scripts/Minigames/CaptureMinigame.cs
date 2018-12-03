namespace LetsStartAKittyCult.Minigames
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;
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
        private GameObject winPanel;

        [SerializeField]
        private UIMeter timeLimitMeter;

        [SerializeField]
        private CapturePaws paws;

        [SerializeField]
        private TMP_InputField humanNameInput;

        [SerializeField]
        private Button humanNameInputButton;

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


        public static void IncrementScore()
        {
            Instance.currentScore++;

            if (Instance.currentScore != Instance.targetScore)
                return;

            Instance.Win();
        }


        public void Win()
        {
            Debug.Log("WON CAPTURE MINIGAME, YAY!!!");
            this.humanNameInput.text = "";
            this.humanNameInputButton.interactable = false;
            this.winPanel.SetActive(true);
            this.hasStarted = false;
            this.onWin.Invoke();
        }
        
        
        public void Begin()
        {
            this.hasStarted = true;
            HideMainText();
        }


        public override void Initialize()
        {
            this.hasStarted = false;
            this.currentScore = 0;
            this.timeLimitTimer = this.timeLimit;
            this.winPanel.SetActive(false);
        }


        public void Lose()
        {
            this.hasStarted = false;
            this.targetHuman.ResetBlessing();
            this.onLose.Invoke();
        }


        public void CheckName()
        {
            if (string.IsNullOrEmpty(this.humanNameInput.text))
                this.humanNameInputButton.interactable = false;
            else
                this.humanNameInputButton.interactable = true;
        }
        
        
        public void Reward()
        {
            if (string.IsNullOrEmpty(this.humanNameInput.text))
                return;
            
            this.targetHuman.Adopt(this.humanNameInput.text);
            Hide();
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

            string introText = $"{human.GivenName} wants to play with the {toy.ToyName}";
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