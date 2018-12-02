namespace LetsStartAKittyCult
{
    using UnityEngine;


    public static class Tags
    {
        public const string Player = "Player";
        public const string Enemy = "Enemy";
        public const string Crop = "Crop";
        public const string AutoScroll = "AutoScroll";


        public static bool HasTag(this GameObject gameObject, params string[] tags)
        {
            if (tags == null
                || tags.Length < 1)
            {
                return true;
            }

            foreach (string tag in tags)
            {
                if (gameObject.CompareTag(tag))
                {
                    return true;
                }
					
            }

            return false;
        }
    }
}
