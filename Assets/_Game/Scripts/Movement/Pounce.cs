namespace LetsStartAKittyCult
{
    using UnityEngine;
    using UnityEngine.Experimental.UIElements;


    public class Pounce : MonoBehaviour
    {
        [SerializeField]
        private float moveMultiplier = 3.5f;
        
        [SerializeField]
        private float pounceDuration = 0.35f;

        [SerializeField]
        private float pounceCooldown = 0.2f;

        [SerializeField]
        private GameObject attackObject;
        
        
        private IMovable movable;
        private float pounceDurationTimer;
        private float pounceCooldownTimer;
        private float previousMovableSpeed;
        private Vector2 pounceDirection;


        public void Awake()
        {
            this.movable = GetComponent<IMovable>();
            ToggleAttackObject(false);
        }


        private void ToggleAttackObject(bool active)
        {
            if (this.attackObject == null)
                return;
            this.attackObject.SetActive(active);
        }
        
        
        public void Activate()
        {
            if (this.pounceDurationTimer > 0
                || this.pounceCooldownTimer > 0
                || this.movable.MoveDirection == Vector2.zero)
            {
                return;
            }

            this.pounceDirection = this.movable.MoveDirection;
            this.previousMovableSpeed = this.movable.MovementSpeed;
            this.movable.MovementSpeed *= this.moveMultiplier;
            this.pounceDurationTimer = this.pounceDuration;
            ToggleAttackObject(true);
        }


        public void FixedUpdate()
        {
            if (this.pounceDurationTimer <= 0
                && this.pounceCooldownTimer <= 0)
            {
                return;
            }
            
            if (this.pounceDurationTimer > 0)
            {
                this.pounceDurationTimer -= GameTime.DeltaTime;
                this.movable.MoveDirection = this.pounceDirection;
                this.movable.Move();

                if (this.pounceDurationTimer <= 0)
                {
                    //this.movable.MovementSpeed = 0;
                    //this.pounceCooldownTimer = this.pounceDuration;
                    this.movable.MovementSpeed = this.previousMovableSpeed;
                    ToggleAttackObject(false);
                }

                return;
            }

            /*
            if (this.pounceCooldownTimer > 0)
            {
                this.pounceCooldownTimer -= GameTime.DeltaTime;

                if (this.pounceCooldownTimer <= 0)
                {
                    this.movable.MovementSpeed = this.previousMovableSpeed;
                }
            }
            */
        }
    }
}