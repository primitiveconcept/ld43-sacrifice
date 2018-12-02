namespace LetsStartAKittyCult
{
    using UnityEngine;
    using UnityEngine.UI;


    public class UIMeter : MonoBehaviour
    {
        [SerializeField]
        private Image image;


        public void Awake()
        {
            if (this.image == null)
                this.image = GetComponent<Image>();
        }


        public void UpdateMeter(float percent)
        {
            this.image.fillAmount = percent;
        }
        
        
        public void UpdateMeter(ObservableRangeInt observableRangeInt)
        {
            this.image.fillAmount = observableRangeInt.GetPercent();
        }
        
        
        public void UpdateMeter(ObservableRangeFloat observableRangeFloat)
        {
            this.image.fillAmount = observableRangeFloat.GetPercent();
        }
    }
}