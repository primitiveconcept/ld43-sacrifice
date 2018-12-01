namespace LetsStartAKittyCult
{
    using System;
    using UnityEngine;


    public interface IMovable
    {
        float MovementSpeed { get; set; }
        Vector2 MoveDirection { get; set; }
        Vector2 AdditionalVelocity { get; set; }
        Vector2 CurrentVelocity { get; }
        Vector2 CurrentSpeed { get; }

        Rigidbody2D Rigidbody2D { get; }

        event Action StartedMoving;
        event Action StoppedMoving;

        void Move();
    }


    public static class MovableExtensions
    {
        public static Vector2 ClampToIntegerDirection(this Vector2 vector2)
        {
            return new Vector2(
                Mathf.Clamp((int)vector2.x, -1, 1),
                Mathf.Clamp((int)vector2.y, -1, 1));
        }

        public static Vector2 ClampToDirection(this Vector2 vector2)
        {
            return new Vector2(
                Mathf.Clamp(vector2.x, -1, 1),
                Mathf.Clamp(vector2.y, -1, 1));
        }
    }
}
