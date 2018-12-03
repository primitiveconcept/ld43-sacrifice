namespace LetsStartAKittyCult
{
    using UnityEngine;
    using UnityStandardAssets.CrossPlatformInput;


    public partial class PlayerInput : MonoBehaviour
    {
        [SerializeField]
        private bool isLocked;

        [SerializeField]
        private bool isMovementLocked;

        private Pounce pounce;
        private IMovable movement;
        private Cat cat;


        #region Properties
        public bool IsLocked
        {
            get { return this.isLocked; }
        }
        #endregion


        public void Awake()
        {
            this.pounce = GetComponent<Pounce>();
            this.movement = GetComponent<IMovable>();
            this.cat = GetComponent<Cat>();
        }


        public void Lock()
        {
            this.isLocked = true;
        }


        public void LockMovement()
        {
            this.isMovementLocked = true;
        }


        public void Move(Vector2 direction)
        {
            this.movement.MoveDirection = direction;
            this.movement.Move();
        }


        public void Unlock()
        {
            this.isLocked = false;
        }


        public void UnlockMovement()
        {
            this.isMovementLocked = false;
        }


        public void Update()
        {
            if (this.isLocked
                || GameTime.IsPaused)
                return;

            if (CrossPlatformInputManager.GetButtonUp(Controls.Interact))
            {
                Player.Get().Interact(this.gameObject);
            }
                
            
            if (CrossPlatformInputManager.GetButtonDown(Controls.Cancel))
            {
                Player.Get().CancelInteract(this.gameObject);
            }

            if (CrossPlatformInputManager.GetButtonDown(Controls.ActivatePowerup))
            {
                // TODO
            }

            if (this.isMovementLocked)
                return;

            float horizontalInput = Input.GetAxisRaw(Controls.HorizontalAxis);
            float verticalInput = Input.GetAxisRaw(Controls.VerticalAxis);
            float mouseWheelInput = Input.GetAxisRaw(Controls.MouseWheelAxis);

            Move(new Vector2(horizontalInput, verticalInput));

            if (mouseWheelInput > 0
                || CrossPlatformInputManager.GetButtonDown(Controls.NextItem))
            {
                // TODO
            }

            else if (mouseWheelInput < 0
                     || CrossPlatformInputManager.GetButtonDown(Controls.PreviousItem))
            {
                // TODO
            }

            if (CrossPlatformInputManager.GetButtonDown(Controls.Primary))
            {
                this.pounce.Activate();
            }
        }
    }
}