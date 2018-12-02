namespace LetsStartAKittyCult
{
    using System;
    using LetsStartAKittyCult.Exensions.Physics;
    using UnityEngine;


    public class SimpleMovement : MonoBehaviour,
                                  IMovable
    {
        [SerializeField]
        private float speed;

        [SerializeField]
        private Vector2 moveDirection;

        [SerializeField]
        private Vector2 additionalVelocity;

        [SerializeField]
        private bool autoMove;

        private new Rigidbody2D rigidbody2D;
        private bool shouldMove;
        private bool wasMoving;

        public event Action StartedMoving;
        public event Action StoppedMoving;


        #region Properties
        public Vector2 AdditionalVelocity
        {
            get { return this.additionalVelocity; }
            set { this.additionalVelocity = value; }
        }


        public bool AutoMove
        {
            get { return this.autoMove; }
            set { this.autoMove = value; }
        }


        public Vector2 MoveDirection
        {
            get { return this.moveDirection; }
            set { this.moveDirection = value; }
        }


        public Rigidbody2D Rigidbody2D
        {
            get { return this.rigidbody2D; }
        }


        public float MovementSpeed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }


        public Vector2 CurrentVelocity
        {
            get { return this.rigidbody2D.velocity; }
        }


        public Vector2 CurrentSpeed
        {
            get 
            { return new Vector2(
                Mathf.Abs(this.rigidbody2D.velocity.x),
                Mathf.Abs(this.rigidbody2D.velocity.y)); 
            }
        }
        #endregion


        public void Awake()
        {
            this.rigidbody2D = this.gameObject.SetupRigidbody();
        }


        public void FixedUpdate()
        {
            if (this.moveDirection == Vector2.zero)
                this.rigidbody2D.velocity = Vector2.zero;

            if (GameTime.IsPaused)
                return;

            if (!this.shouldMove)
                return;

            this.shouldMove = false;

            Vector2 velocity = new Vector2(
                                   this.moveDirection.x,
                                   this.moveDirection.y)
                               * this.speed
                               + this.additionalVelocity;
            
            if (this.wasMoving
                && velocity == Vector2.zero
                && this.StoppedMoving != null)
            {
                StoppedMoving();
            }

            if (!this.wasMoving
                && velocity != Vector2.zero
                && StartedMoving != null)
            {
                StartedMoving();
            }

            this.wasMoving = velocity != Vector2.zero;
            
            this.rigidbody2D.velocity = velocity;
        }


        public void Move()
        {
            this.shouldMove = true;
        }


        public void OnValidate()
        {
            this.moveDirection = this.moveDirection.ClampToDirection();
        }


        public void Update()
        {
            if (this.autoMove)
                Move();
        }
    }
}