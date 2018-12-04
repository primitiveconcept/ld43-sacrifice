namespace LetsStartAKittyCult
{
    using UnityEngine;


    public class WalkAI : MonoBehaviour
    {
        [SerializeField]
        private bool followPlayer = false;

        [SerializeField]
        private float minimumFollowDistance = 0.6f;

        [SerializeField]
        private float followStopThreshold = 0.1f;

        [SerializeField]
        private float directionChangeFrequency = 0.01f;

        private WalkDirection currentWalkDirection;
        private IMovable movable;
        private bool isLocked;


        public enum WalkDirection
        {
            Idle,
            Up,
            Down,
            Left,
            Right
        }


        #region Properties
        public WalkDirection CurrentWalkDirection
        {
            get { return this.currentWalkDirection; }
            set { this.currentWalkDirection = value; }
        }


        public bool FollowPlayer
        {
            get { return this.followPlayer; }
            set { this.followPlayer = value; }
        }
        #endregion


        public void Awake()
        {
            this.movable = GetComponent<IMovable>();
        }


        public void Lock()
        {
            this.isLocked = true;
        }


        public void Unlock()
        {
            this.isLocked = false;
        }


        public void Update()
        {
            if (this.isLocked)
                return;

            if (this.followPlayer)
                ProcessFollowWalkDirection();
            else
                ProcessRandomWalkDirection();

            switch (this.currentWalkDirection)
            {
                case WalkDirection.Idle:
                    this.movable.MoveDirection = Vector2.zero;
                    break;
                case WalkDirection.Up:
                    this.movable.MoveDirection = Vector2.up;
                    break;
                case WalkDirection.Down:
                    this.movable.MoveDirection = Vector2.down;
                    break;
                case WalkDirection.Left:
                    this.movable.MoveDirection = Vector2.left;
                    break;
                case WalkDirection.Right:
                    this.movable.MoveDirection = Vector2.right;
                    break;
            }

            //if (this.currentWalkDirection != WalkDirection.Idle)
                this.movable.Move();
        }


        private void PickRandomWalkDirection()
        {
            int randomDirection = Random.Range(0, 5);
            WalkDirection result = (WalkDirection)randomDirection;
            this.currentWalkDirection = result;
        }


        private void ProcessFollowWalkDirection()
        {
            float distance = Vector2.Distance(this.transform.position, Player.Get().transform.position);
            if (distance > this.minimumFollowDistance)
            {
                ProcessRandomWalkDirection();
                return;
            }

            if (distance < this.followStopThreshold)
            {
                this.currentWalkDirection = WalkDirection.Idle;
                return;
            }

            Vector2 distances = Player.Get().transform.position - this.transform.position;
            if (Mathf.Abs(distances.x) > Mathf.Abs(distances.y))
            {
                if (distances.x < 0)
                    this.currentWalkDirection = WalkDirection.Left;
                else if (distances.x > 0)
                    this.currentWalkDirection = WalkDirection.Right;
                else
                    this.currentWalkDirection = WalkDirection.Idle;
            }
            else
            {
                if (distances.y < 0)
                    this.currentWalkDirection = WalkDirection.Down;
                else if (distances.y > 0)
                    this.currentWalkDirection = WalkDirection.Up;
                else
                    this.currentWalkDirection = WalkDirection.Idle;
            }
        }


        private void ProcessRandomWalkDirection()
        {
            float changeDirection = Random.RandomRange(0f, 1f);

            if (changeDirection * 2 < this.directionChangeFrequency)
                this.currentWalkDirection = WalkDirection.Idle;
            else if (changeDirection < this.directionChangeFrequency)
                PickRandomWalkDirection();
        }
    }
}