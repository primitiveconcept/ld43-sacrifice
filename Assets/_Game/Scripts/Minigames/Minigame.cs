namespace LetsStartAKittyCult.Minigames
{
    using UnityEngine;


    public abstract class Minigame<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T _instance;

        
        #region Properties
        protected static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<T>();
                return _instance;
            }
        }
        #endregion


        public abstract void Initialize();


        public void Hide()
        {
            this.gameObject.SetActive(false);
            GameWorld.Show();
        }


        public void Show()
        {
            Initialize();
            GameWorld.Hide();
            this.gameObject.SetActive(true);
        }
    }
}