namespace LetsStartAKittyCult
{
    using UnityEngine;


    public partial class ActorAnimator : MonoBehaviour
    {
        public const string HorizontalVelocity = "HorizontalVelocity";
        public const string HorizontalSpeed = "HorizontalSpeed";
        public const string VerticalSpeed = "VerticalSpeed";
        public const string VerticalVelocity = "VerticalVelocity";

        private Actor actor;
        private IMovable movable;

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;


        private Animator Animator
        {
            get
            {
                if (this._animator == null)
                    this._animator = this.actor.Animator;
                return this._animator;
            }
        }


        private SpriteRenderer SpriteRenderer
        {
            get
            {
                if (this._spriteRenderer == null)
                    this._spriteRenderer = this.actor.SpriteRenderer;
                return this._spriteRenderer;
            }
        }
        

        public void Awake()
        {
            this.actor = GetComponent<Actor>();
            this.movable = GetComponent<IMovable>();
            this.movable.StartedMoving += Movable_OnStartedMoving;
            this.movable.StoppedMoving += Movable_OnStartedMovingOnStoppedMoving;
        }


        public void Update()
        {
            this.Animator.SetFloat(HorizontalSpeed, this.movable.CurrentSpeed.x);
            this.Animator.SetFloat(HorizontalVelocity, this.movable.CurrentVelocity.x);
            this.Animator.SetFloat(VerticalSpeed, this.movable.CurrentSpeed.y);
            this.Animator.SetFloat(VerticalVelocity, this.movable.CurrentVelocity.y);

            if (this.movable.CurrentVelocity.x < 0)
            {
                this.SpriteRenderer.flipX = true;
            }
            else if (this.movable.CurrentVelocity.x > 0)
            {
                this.SpriteRenderer.flipX = false;
            }
        }


        private void Movable_OnStartedMoving()
        {
            Debug.Log("Started moving");
        }


        private void Movable_OnStartedMovingOnStoppedMoving()
        {
            Debug.Log("Stopped moving");
        }
    }
}

#if UNITY_EDITOR
namespace LetsStartAKittyCult
{
    using System.Linq;
    using UnityEditor;
    using UnityEditor.Animations;
    using UnityEngine;


    public partial class ActorAnimator
    {
        [CustomEditor(typeof(ActorAnimator))]
        public class ActorAnimatorEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (GUILayout.Button("Populate Animation Parameters"))
                {
                    ActorAnimator actorAnimator = this.target as ActorAnimator;
                    AnimatorController controller = (AnimatorController)(actorAnimator.GetComponent<Animator>().runtimeAnimatorController);
                    AddFloatParameter(controller, HorizontalSpeed);
                    AddFloatParameter(controller, HorizontalVelocity);
                    AddFloatParameter(controller, VerticalSpeed);
                    AddFloatParameter(controller, VerticalVelocity);
                }
            }


            private void AddFloatParameter(AnimatorController controller, string parameterName)
            {
                if (HasParameter(controller, parameterName))
                    return;

                AnimatorControllerParameter parameter = new AnimatorControllerParameter();
                parameter.type = AnimatorControllerParameterType.Float;
                parameter.name = parameterName;
                controller.AddParameter(parameter);
            }


            private bool HasParameter(AnimatorController controller, string parameterName)
            {
                return controller.parameters.Any(parameter => parameter.name == parameterName);
            }
        }
    }
}
#endif