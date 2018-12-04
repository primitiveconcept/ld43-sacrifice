namespace LetsStartAKittyCult
{
    using System.Collections.Generic;
    using UnityEngine;


    public class RivalGod : MonoBehaviour
    {
        [SerializeField]
        private float attackFrequency = 1f;

        [SerializeField]
        private float projectileSpeed = 1f;
        
        [SerializeField]
        private Projectile projectilePrefab;

        private List<Cat> cats = new List<Cat>();
        private float attackCounter;

        public void AddCat(Cat cat)
        {
            this.cats.Add(cat);
        }


        public void Awake()
        {
        }


        public void RemoveCat(Cat cat)
        {
            this.cats.Remove(cat);
        }


        public void Update()
        {
            if (this.attackCounter <= 0)
            {
                Projectile projectile = Instantiate(this.projectilePrefab);
                projectile.transform.position = this.transform.position;
                
                Vector2 attackDirection = Player.Get().transform.position - this.transform.position;
                attackDirection.Normalize();
                attackDirection *= this.projectileSpeed;
                projectile.attackVector = attackDirection;
                this.attackCounter = this.attackFrequency;
            }
            else
            {
                this.attackCounter -= GameTime.DeltaTime;
            }
        }


        private void OnDestroy()
        {
            foreach (Cat cat in this.cats)
                if (cat != null)
                    cat.GetComponent<Health>().Lives = 1;
        }
    }
}