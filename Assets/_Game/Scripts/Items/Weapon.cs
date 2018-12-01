namespace LetsStartAKittyCult
{
    using UnityEngine;

    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField]
        private AudioClip fireClip;


        public virtual void Attack(Vector2 direction)
        {
            if (this.fireClip != null)
                AudioPlayer.Play(this.fireClip);
        }
    }
}
