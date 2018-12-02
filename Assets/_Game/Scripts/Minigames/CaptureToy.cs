namespace LetsStartAKittyCult.Minigames
{
    using UnityEngine;


    public class CaptureToy : MonoBehaviour
    {
        [Header("Objective")]
        [SerializeField]
        private int targetScore = 3;

        [Header("Movement")]
        [SerializeField]
        private float switchDirectionRate = 0.01f;

        [SerializeField]
        private float verticalMoveRange = 1f;

        [SerializeField]
        private float horizontalMoveRange = 2f;

        [SerializeField]
        private float verticalMoveSpeed = 0.05f;

        [SerializeField]
        private float horizontalMoveSpeed = 0.08f;

        [Header("Morph")]
        [SerializeField]
        private float verticalMorphRange = 0.1f;

        [SerializeField]
        private float morphRate = 0.01f;

        private SpriteRenderer spriteRenderer;
        private Vector3 originalScale;
        private Vector3 originalPosition;
        private MorphType currentMorphType = MorphType.Squish;
        private MoveDirection currentMoveDirection = MoveDirection.Down;


        private enum MorphType
        {
            Squish,
            Stretch
        }


        private enum MoveDirection
        {
            Up,
            Down,
            Left,
            Right
        }


        #region Properties
        public int TargetScore
        {
            get { return this.targetScore; }
        }
        #endregion


        public void Awake()
        {
            this.spriteRenderer = GetComponent<SpriteRenderer>();
            this.originalScale = this.transform.localScale;
            this.originalPosition = this.transform.localPosition;
            PickNewMoveDirection();
        }


        public void MoveDown()
        {
            if (this.transform.localPosition.y <= this.originalPosition.y - this.verticalMoveRange)
            {
                PickNewMoveDirection();
                return;
            }

            this.transform.localPosition = new Vector3(
                this.transform.localPosition.x,
                this.transform.localPosition.y - this.verticalMoveSpeed,
                this.transform.localPosition.z);
        }


        public void MoveLeft()
        {
            if (this.transform.localPosition.x <= this.originalPosition.x - this.horizontalMoveRange)
            {
                PickNewMoveDirection();
                return;
            }

            this.transform.localPosition = new Vector3(
                this.transform.localPosition.x - this.horizontalMoveSpeed,
                this.transform.localPosition.y,
                this.transform.localPosition.z);
        }


        public void MoveRight()
        {
            if (this.transform.localPosition.x >= this.originalPosition.x + this.horizontalMoveRange)
            {
                PickNewMoveDirection();
                return;
            }

            this.transform.localPosition = new Vector3(
                this.transform.localPosition.x + this.horizontalMoveSpeed,
                this.transform.localPosition.y,
                this.transform.localPosition.z);
        }


        public void MoveUp()
        {
            if (this.transform.localPosition.y >= this.originalPosition.y + this.verticalMoveRange)
            {
                PickNewMoveDirection();
                return;
            }

            this.transform.localPosition = new Vector3(
                this.transform.localPosition.x,
                this.transform.localPosition.y + this.verticalMoveSpeed,
                this.transform.localPosition.z);
        }


        public void Squish()
        {
            if (this.transform.localScale.y <= this.originalScale.y - this.verticalMorphRange)
            {
                this.currentMorphType = MorphType.Stretch;
                return;
            }

            this.transform.localScale = new Vector3(
                this.transform.localScale.x,
                this.transform.localScale.y - this.morphRate,
                this.transform.localScale.z);
        }


        public void Stretch()
        {
            if (this.transform.localScale.y >= this.originalScale.y + this.verticalMorphRange)
            {
                this.currentMorphType = MorphType.Squish;
                return;
            }

            this.transform.localScale = new Vector3(
                this.transform.localScale.x,
                this.transform.localScale.y + this.morphRate,
                this.transform.localScale.z);
        }


        public void Update()
        {
            switch (this.currentMorphType)
            {
                case MorphType.Squish:
                    Squish();
                    break;
                case MorphType.Stretch:
                    Stretch();
                    break;
            }

            switch (this.currentMoveDirection)
            {
                case MoveDirection.Left:
                    MoveLeft();
                    break;
                case MoveDirection.Right:
                    MoveRight();
                    break;
                case MoveDirection.Up:
                    MoveUp();
                    break;
                case MoveDirection.Down:
                    MoveDown();
                    break;
            }

            float switchDirectionCheck = Random.Range(0f, 1f);
            if (switchDirectionCheck < this.switchDirectionRate)
                PickNewMoveDirection();

            switch (this.currentMoveDirection)
            {
                case MoveDirection.Up:
                    break;
                case MoveDirection.Down:
                    break;
            }
        }


        private void PickNewMoveDirection()
        {
            int randomDirection = Random.Range(0, 4);
            MoveDirection result = (MoveDirection)randomDirection;
            if (result == this.currentMoveDirection)
                PickNewMoveDirection();
            else
                this.currentMoveDirection = result;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("HIT!");
        }
    }
}