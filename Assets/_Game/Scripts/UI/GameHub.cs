namespace LetsStartAKittyCult
{
    using LetsStartAKittyCult.Minigames;
    using UnityEngine;
    using UnityEngine.Playables;
    using UnityEngine.SceneManagement;


    public class GameHub : MonoBehaviour
    {
        private static GameHub _instance;

        [SerializeField]
        private UIPlayerHud playerHud;

        [SerializeField]
        private CaptureMinigame captureMinigame;

        [SerializeField]
        private UIHumanStats humanStats;

        [SerializeField]
        private GameWorld gameWorld;

        [Header("Cutscenes")]
        [SerializeField]
        private PlayableDirector introCutscene;
        
        [SerializeField]
        private PlayableDirector sacrificeCutscene;

        [SerializeField]
        private PlayableDirector endCutscene;


        #region Properties
        public static PlayableDirector IntroCutscene
        {
            get { return Instance.introCutscene; }
        }


        public static CaptureMinigame CaptureMinigame
        {
            get { return Instance.captureMinigame; }
        }


        public static GameWorld GameWorld
        {
            get { return Instance.gameWorld; }
        }


        public static UIHumanStats HumanStats
        {
            get { return Instance.humanStats; }
        }


        public static UIPlayerHud PlayerHud
        {
            get { return Instance.playerHud; }
        }


        public static PlayableDirector SacrificeCutscene
        {
            get { return Instance.sacrificeCutscene; }
        }


        public static PlayableDirector EndCutscene
        {
            get { return Instance.endCutscene; }
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
            
            Hide(this.sacrificeCutscene.gameObject);
            Hide(this.endCutscene.gameObject);
            
            PlayCutscene(this.introCutscene);       
        }
        

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }


        public static void PlayCutscene(PlayableDirector cutscene)
        {
            GameWorld.gameObject.SetActive(false);
            cutscene.gameObject.SetActive(true);
            cutscene.Play();
        }
    }
}


#if UNITY_EDITOR
namespace LetsStartAKittyCult
{
    using UnityEditor;
    using UnityEngine;


    [CustomEditor(typeof(GameHub))]
    public class GameHubEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying
                && GUILayout.Button("WIN GAME"))
            {
                GameHub.PlayCutscene(GameHub.EndCutscene);
            }
        }
    }

}
#endif