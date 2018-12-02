namespace LetsStartAKittyCult
{
    using UnityEngine;


    public abstract class UsableItem : MonoBehaviour
    {
        [SerializeField]
        private AudioClip useClip;

        public virtual void Use(Cat origin, Vector2 direction)
        {
            if (this.useClip != null)
                AudioPlayer.Play(this.useClip);
        }
    }
}
