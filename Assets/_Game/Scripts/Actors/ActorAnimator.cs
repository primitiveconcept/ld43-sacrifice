namespace LetsStartAKittyCult
{
    using UnityEngine;


    public partial class ActorAnimator : MonoBehaviour
    {
        public const string IsBlessing = "IsBlessing";

        public const string IsHappy = "IsHappy";
        public const string IsIncapacitated = "IsIncapacitated";

        public const string HorizontalVelocity = "HorizontalVelocity";
        public const string HorizontalSpeed = "HorizontalSpeed";
        public const string VerticalSpeed = "VerticalSpeed";
        public const string VerticalVelocity = "VerticalVelocity";

        private IMovable movable;

        private SpriteSorter spriteSorter;
        private Cat cat;
        private Human human;


        #region Properties
        private Animator Animator
        {
            get { return this.spriteSorter.Animator; }
        }


        private SpriteRenderer SpriteRenderer
        {
            get { return this.spriteSorter.SpriteRenderer; }
        }
        #endregion


        public void Awake()
        {
            this.movable = GetComponent<IMovable>();
            this.spriteSorter = GetComponent<SpriteSorter>();
            this.cat = GetComponent<Cat>();
            this.human = GetComponent<Human>();
            this.movable.StartedMoving += Movable_OnStartedMoving;
            this.movable.StoppedMoving += Movable_OnStartedMovingOnStoppedMoving;
        }


        public void Update()
        {
            // Movement
            UpdateMovementParameters();

            // Cat
            if (this.cat != null)
                UpdateCatParameters();

            // Human
            if (this.human != null)
                UpdateHumanParameters();
        }


        private void Movable_OnStartedMoving()
        {
            // TODO
        }


        private void Movable_OnStartedMovingOnStoppedMoving()
        {
            // TODO
        }


        private void UpdateCatParameters()
        {
            this.Animator.SetBool(IsBlessing, this.cat.IsBlessing);
        }


        private void UpdateHumanParameters()
        {
            this.Animator.SetBool(IsHappy, this.human.IsHappy);
            this.Animator.SetBool(IsIncapacitated, this.human.IsIncapacitated);
        }


        private void UpdateMovementParameters()
        {
            this.Animator.SetFloat(HorizontalSpeed, this.movable.CurrentSpeed.x);
            this.Animator.SetFloat(HorizontalVelocity, this.movable.CurrentVelocity.x);
            this.Animator.SetFloat(VerticalSpeed, this.movable.CurrentSpeed.y);
            this.Animator.SetFloat(VerticalVelocity, this.movable.CurrentVelocity.y);

            if (this.movable.CurrentVelocity.x < 0)
                this.SpriteRenderer.flipX = true;
            else if (this.movable.CurrentVelocity.x > 0)
                this.SpriteRenderer.flipX = false;
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
                    AnimatorController controller =
                        (AnimatorController)actorAnimator.GetComponent<Animator>().runtimeAnimatorController;

                    // Movement
                    AddFloatParameter(controller, HorizontalSpeed);
                    AddFloatParameter(controller, HorizontalVelocity);
                    AddFloatParameter(controller, VerticalSpeed);
                    AddFloatParameter(controller, VerticalVelocity);

                    // Cat
                    Cat cat = actorAnimator.GetComponent<Cat>();
                    if (cat != null)
                        AddBoolParameter(controller, IsBlessing);

                    // Human
                    Human human = actorAnimator.GetComponent<Human>();
                    if (human != null)
                    {
                        AddBoolParameter(controller, IsHappy);
                        AddBoolParameter(controller, IsIncapacitated);
                    }
                }
            }


            private void AddBoolParameter(AnimatorController controller, string parameterName)
            {
                if (HasParameter(controller, parameterName))
                    return;

                AnimatorControllerParameter parameter = new AnimatorControllerParameter();
                parameter.type = AnimatorControllerParameterType.Bool;
                parameter.name = parameterName;
                controller.AddParameter(parameter);
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