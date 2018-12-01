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


        public void UpdateMeter(ObservableRangeInt observableRangeInt)
        {
            float totalRange = observableRangeInt.Max - observableRangeInt.Min;
            float percent = observableRangeInt.Current / totalRange;
            this.image.fillAmount = percent;
        }
    }
}