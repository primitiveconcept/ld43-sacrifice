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

        private SpriteSorter spriteSorter;


        
        public void Awake()
        {
            this.actor = GetComponent<Actor>();
            this.movable = GetComponent<IMovable>();
            this.spriteSorter = this.actor.SpriteSorter;
            this.movable.StartedMoving += Movable_OnStartedMoving;
            this.movable.StoppedMoving += Movable_OnStartedMovingOnStoppedMoving;
        }


        public void Update()
        {
            this.spriteSorter.Animator.SetFloat(HorizontalSpeed, this.movable.CurrentSpeed.x);
            this.spriteSorter.Animator.SetFloat(HorizontalVelocity, this.movable.CurrentVelocity.x);
            this.spriteSorter.Animator.SetFloat(VerticalSpeed, this.movable.CurrentSpeed.y);
            this.spriteSorter.Animator.SetFloat(VerticalVelocity, this.movable.CurrentVelocity.y);

            if (this.movable.CurrentVelocity.x < 0)
            {
                this.spriteSorter.SpriteRenderer.flipX = true;
            }
            else if (this.movable.CurrentVelocity.x > 0)
            {
                this.spriteSorter.SpriteRenderer.flipX = false;
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