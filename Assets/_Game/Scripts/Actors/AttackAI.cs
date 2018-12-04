namespace LetsStartAKittyCult
{
    using UnityEngine;
    
    
    public class AttackAI : MonoBehaviour
    {
        [SerializeField]
        private float attackDistance = 0.3f;

        [SerializeField]
        private float axisThreshold = 0.1f;

        [SerializeField]
        private float attackCooldown = 1f;
        
        private bool isLocked;
        private Pounce pounce;
        private IMovable movable;
        private float attackCooldownTimer;


        public void Awake()
        {
            this.pounce = GetComponent<Pounce>();
            this.movable = GetComponent<IMovable>();
        }
        
        public void Update()
        {
            this.attackCooldownTimer -= GameTime.DeltaTime;
            if (this.attackCooldownTimer <= 0)
            {
                this.attackCooldownTimer = 0;
                ProcessAttackOpportunities();    
            }
        }


        public void Lock()
        {
            this.isLocked = true;
        }


        public void Unlock()
        {
            this.isLocked = false;
        }

        public void ProcessAttackOpportunities()
        {
            if (this.isLocked
                || this.attackCooldownTimer > 0)
            {
                return;
            }
            
            Player player = Player.Get();
            float playerDistance = Vector2.Distance(player.transform.position, this.transform.position);
            
            if (playerDistance > this.attackDistance)
                return;
            
            Vector2 playerDirection = (player.transform.position - this.transform.position);

            float xDistance = Mathf.Abs(playerDirection.x);
            float yDistance = Mathf.Abs(playerDirection.y);

            if (xDistance > yDistance)
            {
                playerDirection.y = 0;
                if (playerDirection.x < 0)
                {
                    playerDirection.x = -1;
                }
                else if (playerDirection.x > 0)
                {
                    playerDirection.x = 1;
                }
            }
            
            else if (yDistance > xDistance)
            {
                playerDirection.x = 0;
                if (playerDirection.y < 0)
                {
                    playerDirection.y = -1;
                }
                else if (playerDirection.y > 0)
                {
                    playerDirection.y = 1;
                }    
            }

            this.movable.MoveDirection = playerDirection;
            
            this.movable.Move();
            this.pounce.Activate();
            this.attackCooldownTimer = this.attackCooldown;
        }
    }
}