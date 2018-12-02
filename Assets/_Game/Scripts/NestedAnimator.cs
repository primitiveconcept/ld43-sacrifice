namespace LetsStartAKittyCult
{
    using UnityEngine;


    public interface INestedAnimator
    {
        Animator Animator { get; set; }
    }


    public static class NestedAnimatorExtensions
    {
        public static void NestAnimator<T>(this T gameObject)
            where T : MonoBehaviour, INestedAnimator
        {
            Animator animator = gameObject.GetComponent<Animator>();
            if (animator == null)
                return;

            Transform nestedSpriteObject = gameObject.transform.Find("SpriteRenderer");
            if (nestedSpriteObject == null)
                nestedSpriteObject = new GameObject("Animator").transform;
            
            nestedSpriteObject.SetParent(gameObject.transform);
            nestedSpriteObject.localPosition = Vector3.zero;

            Animator nestedAnimator = nestedSpriteObject.gameObject.AddComponent<Animator>();
            nestedAnimator.runtimeAnimatorController = animator.runtimeAnimatorController;
            nestedAnimator.avatar = animator.avatar;
            nestedAnimator.applyRootMotion = animator.applyRootMotion;
            nestedAnimator.updateMode = animator.updateMode;
            nestedAnimator.cullingMode = animator.cullingMode;
            GameObject.Destroy(animator);

            gameObject.Animator = nestedAnimator;
        }
    }
}