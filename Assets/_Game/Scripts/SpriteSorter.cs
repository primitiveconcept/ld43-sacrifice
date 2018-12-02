namespace LetsStartAKittyCult
{
    using UnityEngine;


    public class SpriteSorter : MonoBehaviour,
                                INestedAnimator,
                                INestedSprite
    {
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        private INestedSprite nestedSprite;


        #region Properties
        public Animator Animator
        {
            get { return this.animator; }
            set { this.animator = value; }
        }


        public SpriteRenderer SpriteRenderer
        {
            get { return this.spriteRenderer; }
            set { this.spriteRenderer = value; }
        }
        #endregion


        public void Awake()
        {
            this.NestSprite();
            this.NestAnimator();
        }


        private void LateUpdate()
        {
            this.spriteRenderer.sortingOrder = -(int)(this.transform.position.y * 100);
        }
    }
}