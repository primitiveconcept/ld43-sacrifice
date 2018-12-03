namespace LetsStartAKittyCult
{
    using TMPro;
    using UnityEngine;


    public class UIHumanStats : MonoBehaviour
    {
        [SerializeField]
        private UIMeter affectionMeter;

        [SerializeField]
        private TextMeshProUGUI identityLabel;
        
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

            string memberStatus = human.IsHappy
                                      ? "[IN YOUR CULT]"
                                      : "[NOT A CULT MEMBER]";
            
            this.identityLabel.text = $"{human.HumanName} ({human.HumanGender}) {memberStatus}";
            
            
            
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