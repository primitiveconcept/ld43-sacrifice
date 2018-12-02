namespace LetsStartAKittyCult
{
    using LetsStartAKittyCult.Minigames;
    using UnityEngine;


    public class UIHub : MonoBehaviour
    {
        private static UIHub _instance;

        [SerializeField]
        private UIPlayerHud playerHud;

        [SerializeField]
        private GameObject startScreen;

        [SerializeField]
        private GameObject endScreen;

        [SerializeField]
        private CaptureMinigame captureMinigame;

        [SerializeField]
        private UIHumanStats humanStats;


        #region Properties
        public static CaptureMinigame CaptureMinigame
        {
            get { return Instance.captureMinigame; }
        }


        public static UIHumanStats HumanStats
        {
            get { return Instance.humanStats; }
        }


        public static UIPlayerHud PlayerHud
        {
            get { return Instance.playerHud; }
        }


        private static UIHub Instance
        {
            get { return _instance; }
        }
        #endregion


        public static void HideUI(GameObject ui)
        {
            if (ui != null)
                ui.SetActive(false);
        }


        public static void ShowUI(GameObject ui)
        {
            if (ui != null)
                ui.SetActive(true);
        }


        public void Awake()
        {
            _instance = this;
        }


        public void Start()
        {
            HideUI(this.captureMinigame.gameObject);
            //HideUI(this.endScreen.gameObject);
            //HideUI(this.playerHud.gameObject);
            //ShowUI(this.startScreen.gameObject);
            HideUI(this.humanStats.gameObject);
            //ShowUI(this.playerHud.gameObject);
        }
    }
}