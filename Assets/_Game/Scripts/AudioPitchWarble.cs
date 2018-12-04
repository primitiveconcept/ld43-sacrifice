namespace LetsStartAKittyCult
{
    using UnityEngine;
    public class AudioPitchWarble : MonoBehaviour
    {
        [SerializeField]
        private float warbleAmount = 0.5f;

        [SerializeField]
        private float warbleRate = 0.01f;

        [SerializeField]
        private float pitchChangeFrequency = 0.2f;
        
        private AudioSource audio;
        private float originalPitch;
        private bool risingPitch;
        

        public void Awake()
        {
            this.audio = GetComponent<AudioSource>();
            this.originalPitch = this.audio.pitch;
        }


        public void Update()
        {
            if (!this.audio.isPlaying)
                return;

            if (this.risingPitch)
            {
                this.audio.pitch += this.warbleRate;
                if (this.audio.pitch > this.originalPitch + this.warbleAmount)
                {
                    this.audio.pitch = this.originalPitch + this.warbleAmount;
                    this.risingPitch = false;
                }
                    
            }
            else
            {
                this.audio.pitch -= this.warbleRate;
                if (this.audio.pitch < this.originalPitch - this.warbleAmount)
                {
                    this.audio.pitch = this.originalPitch - this.warbleAmount;
                    this.risingPitch = true;
                }
            }
        }
    }
}