namespace LetsStartAKittyCult
{
    using UnityEngine;


    public partial class ActorAnimator : MonoBehaviour
    {
        public const string HorizontalSpeed = "HorizontalSpeed";
        public const string VerticalSpeed = "VerticalSpeed";

        private Actor actor;
        private IMovable movable;

        private Animator _animator;


        private Animator Animator
        {
            get
            {
                if (this._animator == null)
                    this._animator = this.actor.Animator;
                return this._animator;
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
            if (this.movable.CurrentSpeed.x == 0)
            {
                Debug.Log("ZERO");
            }
            
            this.Animator.SetFloat(HorizontalSpeed, this.movable.CurrentSpeed.x);
            this.Animator.SetFloat(VerticalSpeed, this.movable.CurrentSpeed.y);
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
                    AddFloatParameter(controller, VerticalSpeed);
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