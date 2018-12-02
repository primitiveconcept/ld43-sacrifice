namespace LetsStartAKittyCult.Minigames
{
    using System.Collections;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityStandardAssets.CrossPlatformInput;


    public class CapturePaws : MonoBehaviour
    {
        [SerializeField]
        private float pawMoveSpeed = 0.1f;
        
        [SerializeField]
        private float pawResetDuration;

        [SerializeField]
        private float maxYPosition = -0.33f;
        
        [SerializeField]
        private GameObject leftPaw;

        [SerializeField]
        private float leftOffset = -0.25f;

        [SerializeField]
        private GameObject rightPaw;

        [SerializeField]
        private float rightOffset = 0.25f;

        private Vector3 leftPawRestPosition;
        private Vector3 rightPawRestPosition;
        private float pawResetTimer;
        private Vector3 attackPosition;
        private bool attacking;
        

        public void Awake()
        {
            this.leftPawRestPosition = this.leftPaw.transform.position;
            this.rightPawRestPosition = this.rightPaw.transform.position;
        }


        public void Update()
        {
            if (this.attacking)
            {
                MovePawsToTarget();
                return;
            }
            
            if (this.pawResetTimer <= 0)
                return;

            this.pawResetTimer -= GameTime.DeltaTime;

            if (this.pawResetTimer <= 0)
            {
                this.leftPaw.transform.position = new Vector3(
                    this.leftPawRestPosition.x,
                    this.leftPawRestPosition.y,
                    this.leftPawRestPosition.z);
                this.rightPaw.transform.position = new Vector3(
                    this.rightPawRestPosition.x,
                    this.rightPawRestPosition.y,
                    this.rightPawRestPosition.z);
            }
                
        }
        
        
        public void Attack(Vector3 position)
        {
            if (this.pawResetTimer > 0
                || this.attacking)
            {
                return;
            }

            if (position.y > this.maxYPosition)
                position.y = this.maxYPosition;
            
            this.attackPosition = position;
            this.attacking = true;
        }


        private void MovePawsToTarget()
        {
            Vector3 leftTarget = new Vector3(
                this.attackPosition.x + this.leftOffset,
                this.attackPosition.y,
                this.attackPosition.z);
            Vector3 rightTarget = new Vector3(
                this.attackPosition.x + this.rightOffset,
                this.attackPosition.y,
                this.attackPosition.z);
            
            float leftX = this.leftPaw.transform.position.x;
            float leftY = this.leftPaw.transform.position.y;
            float rightX = this.rightPaw.transform.position.x;
            float rightY = this.rightPaw.transform.position.y;

            if (leftTarget.x < leftX)
                leftX -= this.pawMoveSpeed;
            else if (leftTarget.x > leftX)
                leftX += this.pawMoveSpeed;
            if (leftTarget.y < leftY)
                leftY -= this.pawMoveSpeed;
            else if (leftTarget.y > leftY)
                leftY += this.pawMoveSpeed;
            
            if (rightTarget.x < rightX)
                rightX -= this.pawMoveSpeed;
            else if (rightTarget.x > rightX)
                rightX += this.pawMoveSpeed;
            if (rightTarget.y < rightY)
                rightY -= this.pawMoveSpeed;
            else if (rightTarget.y > rightY)
                rightY += this.pawMoveSpeed;
            
            this.leftPaw.transform.position = new Vector3(
                leftX,
                leftY,
                this.attackPosition.z);
            this.rightPaw.transform.position = new Vector3(
                rightX,
                rightY,
                this.attackPosition.z);

            float leftDistance = Vector3.Distance(leftTarget, this.leftPaw.transform.position);
            float rightDistance = Vector3.Distance(rightTarget, this.rightPaw.transform.position);

            if (leftDistance <= this.pawMoveSpeed
                || rightDistance <= this.pawMoveSpeed)
            {
                this.attacking = false;
                this.pawResetTimer = this.pawResetDuration;
            }
        }
    }
}