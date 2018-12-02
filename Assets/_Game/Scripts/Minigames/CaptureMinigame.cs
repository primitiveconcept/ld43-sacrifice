namespace LetsStartAKittyCult.Minigames
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityStandardAssets.CrossPlatformInput;


    public class CaptureMinigame : Minigame<CaptureMinigame>
    {
        [SerializeField]
        private CaptureToy[] toys;

        [SerializeField]
        private UnityEvent onWin;

        [SerializeField]
        private CapturePaws paws;
        
        private int targetScore;
        private int currentScore;


        public void Update()
        {
            if (CrossPlatformInputManager.GetButtonDown(Controls.Primary))
            {
                Vector3 mousePosition = Input.mousePosition;
                //mousePosition.z = 10;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                mousePosition.z = 0;
                this.paws.Attack(mousePosition);
            }
        }
        

        public static void Start(int toyIndex)
        {
            Instance.currentScore = 0;
            Instance.Show();
            foreach (CaptureToy availableToy in Instance.toys)
                availableToy.gameObject.SetActive(false);

            CaptureToy toy = Instance.toys[toyIndex];
            Instance.targetScore = toy.TargetScore;
            toy.gameObject.SetActive(true);
        }


        public void IncrementScore()
        {
            this.currentScore++;

            if (this.currentScore != this.targetScore)
                return;
            
            Debug.Log("WON CAPTURE MINIGAME, YAY!!!");
            this.onWin.Invoke();
        }
    }
}