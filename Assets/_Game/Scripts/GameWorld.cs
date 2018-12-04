namespace LetsStartAKittyCult
{
    using UnityEngine;


    public class GameWorld : MonoBehaviour
    {
        private static GameWorld _instance;


        #region Properties
        private static GameWorld Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<GameWorld>();
                return _instance;
            }
        }
        #endregion


        public static void Hide()
        {
            Instance.gameObject.SetActive(false);
        }


        public static void Show()
        {
            Instance.gameObject.SetActive(true);
        }


        private void OnEnable()
        {
            if (Player.Get() != null
                && Player.Get().Cult.Glory == 9)
            {
                GameHub.PlayCutscene(GameHub.EndCutscene);
            }
        }
    }
}