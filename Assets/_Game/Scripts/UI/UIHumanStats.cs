namespace LetsStartAKittyCult
{
    using UnityEngine;


    public class UIHumanStats : MonoBehaviour
    {
        [SerializeField]
        private UIMeter affectionMeter;

        private Human human;


        public void Hide(Human human)
        {
            if (this.human != human)
                return;

            this.gameObject.SetActive(false);
        }


        public void Show(Human human)
        {
            this.human = human;
            this.gameObject.SetActive(true);
        }


        public void Update()
        {
            if (this.human == null)
                return;

            this.affectionMeter.UpdateMeter(this.human.CurrentBlessAmount / 1f);
        }
    }
}