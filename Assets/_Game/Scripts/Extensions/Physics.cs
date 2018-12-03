namespace LetsStartAKittyCult.Exensions.Physics
{
    using UnityEngine;


    public static class PhysicsExtensions
    {
        public static Rigidbody2D SetupRigidbody(this GameObject gameObject, bool overwrite = false)
        {
            Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            bool lackedRigidbody = rigidbody2D == null;

            if (lackedRigidbody)
                rigidbody2D = gameObject.AddComponent<Rigidbody2D>();

            if (lackedRigidbody
                || overwrite)
            {
                rigidbody2D.gravityScale = 0;
                rigidbody2D.isKinematic = false;
                rigidbody2D.mass = 5;
                rigidbody2D.angularDrag = 0;
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
			
            return rigidbody2D;
        }
    }
}