namespace LetsStartAKittyCult
{
    using UnityEngine;


    public class GameTime : MonoBehaviour
    {
        private static GameTime _instance;

        [SerializeField]
        private bool isPaused;

        [SerializeField]
        private double timeElapsed;


        #region Properties
        public static float DeltaTime
        {
            get
            {
                if (Instance.isPaused)
                    return 0f;

                return Time.deltaTime;
            }
        }


        public static float FixedDeltaTime
        {
            get
            {
                if (Instance.isPaused)
                    return 0f;

                return Time.fixedDeltaTime;
            }
        }


        public static bool IsPaused
        {
            get { return Instance.isPaused; }
        }


        public static double TimeElapsed
        {
            get { return Instance.timeElapsed; }
            set { Instance.timeElapsed = value; }
        }


        private static GameTime Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject instanceObject = new GameObject("GameTime");
                    _instance = instanceObject.AddComponent<GameTime>();
                }

                return _instance;
            }
        }
        #endregion


        public static void Pause()
        {
            Instance.isPaused = true;
        }


        public static void Unpause()
        {
            Instance.isPaused = false;
        }


        public void Update()
        {
            if (Instance.isPaused)
                return;

            Instance.timeElapsed += Time.deltaTime;
        }
    }
}