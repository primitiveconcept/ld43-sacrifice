namespace LetsStartAKittyCult
{
    using TMPro;
    using UnityEngine;


    public class CatGod : MonoBehaviour
    {
        [SerializeField]
        private string[] tutorialScript;

        [SerializeField]
        private string[] randomConversation;

        [SerializeField]
        private float showTextDuration = 5f;

        [SerializeField]
        private GameObject textPanel;

        [SerializeField]
        private TextMeshPro textMesh;

        [SerializeField]
        private GameObject barrier;

        private Interactable interactable;
        private int tutorialScriptIndex;
        private int randomScriptIndex;
        private float showTextTimer;


        public void Awake()
        {
            this.interactable = GetComponent<Interactable>();
            this.barrier.SetActive(true);
            HideText();
        }


        public void HideText()
        {
            this.textPanel.SetActive(false);
        }


        public void ShowText(string text)
        {
            this.textMesh.text = text;
            this.textPanel.SetActive(true);
        }


        public void Speak(GameObject interactor)
        {
            // Tutorial
            if (this.tutorialScriptIndex < this.tutorialScript.Length)
            {
                PlayerInput playerInput = interactor.GetComponent<PlayerInput>();
                playerInput.LockMovement();

                this.interactable.ShowCaption("E: CONTINUE", Color.yellow);
                ShowText(this.tutorialScript[this.tutorialScriptIndex]);
                this.tutorialScriptIndex++;

                if (this.tutorialScriptIndex == this.tutorialScript.Length)
                {
                    this.showTextTimer = this.showTextDuration;
                    this.interactable.HideCaption();
                    playerInput.UnlockMovement();
                    GameHub.PlayerHud.gameObject.SetActive(true);
                    this.barrier.SetActive(false);
                }
            }

            // Random
            else
            {
                ShowText(this.randomConversation[this.randomScriptIndex]);
                this.randomScriptIndex++;
                if (this.randomScriptIndex == this.randomConversation.Length)
                    this.randomScriptIndex = 0;
                this.showTextTimer = this.showTextDuration;
            }
        }


        public void CancelSpeak(GameObject interactor)
        {
            if (this.tutorialScriptIndex >= this.tutorialScript.Length)
                return;
            
            PlayerInput playerInput = interactor.GetComponent<PlayerInput>();
            
            ShowText("OH, AM I BORING YOU? FINE, FIGURE IT OUT YOURSELF!");
            this.showTextTimer = this.showTextDuration;
            this.interactable.HideCaption();
            playerInput.UnlockMovement();
            GameHub.PlayerHud.gameObject.SetActive(true);
            this.barrier.SetActive(false);
        }


        public void Update()
        {
            if (this.showTextTimer > 0)
            {
                this.showTextTimer -= GameTime.DeltaTime;
                if (this.showTextTimer <= 0)
                {
                    this.showTextTimer = 0;
                    HideText();
                }
            }
        }
    }
}