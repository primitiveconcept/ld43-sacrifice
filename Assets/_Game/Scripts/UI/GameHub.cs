namespace LetsStartAKittyCult
{
    using LetsStartAKittyCult.Minigames;
    using UnityEngine;


    public class GameHub : MonoBehaviour
    {
        private static GameHub _instance;

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

        [SerializeField]
        private GameWorld gameWorld;


        #region Properties
        public static GameWorld GameWorld
        {
            get { return Instance.gameWorld; }
        }
        
        
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


        private static GameHub Instance
        {
            get { return _instance; }
        }
        #endregion


        public static void Hide(GameObject ui)
        {
            if (ui != null)
                ui.SetActive(false);
        }


        public static void Show(GameObject ui)
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
            Hide(this.captureMinigame.gameObject);
            //HideUI(this.endScreen.gameObject);
            //HideUI(this.playerHud.gameObject);
            //ShowUI(this.startScreen.gameObject);
            Hide(this.humanStats.gameObject);
            //ShowUI(this.playerHud.gameObject);
            Hide(this.playerHud.gameObject);
            Show(this.gameWorld.gameObject);
        }
    }
}