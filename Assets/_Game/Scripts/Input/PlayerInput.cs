namespace LetsStartAKittyCult
{
    using UnityEngine;
    using UnityStandardAssets.CrossPlatformInput;


    public partial class PlayerInput : MonoBehaviour
    {
        [SerializeField]
        private bool locked;

        private IMovable movement;
        private Cat cat;
        

        public bool Locked
        {
            get { return this.locked; }
            set { this.locked = value; }
        }


        public void Awake()
        {
            this.movement = GetComponent<IMovable>();
            this.cat = GetComponent<Cat>();
        
        }


        public void Move(Vector2 direction)
        {
            this.movement.MoveDirection = direction;
            this.movement.Move();
        }


        public void Update()
        {
            if (this.locked
                || GameTime.IsPaused)
            {
                return;
            }

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
                this.GetComponent<Pounce>().Activate();
            }
                

            if (CrossPlatformInputManager.GetButtonDown(Controls.Secondary))
            {
                // TODO
            }


            if (CrossPlatformInputManager.GetButtonUp(Controls.Interact))
            {
                Player.Get(0).Interact(this.gameObject);
            }
            

            if (CrossPlatformInputManager.GetButtonDown(Controls.ActivatePowerup))
            {
                // TODO
            }

			
        }
    }
}