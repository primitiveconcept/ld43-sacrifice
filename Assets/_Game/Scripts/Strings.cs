namespace LetsStartAKittyCult
{
    public static class Strings
    {
        public static string[] HumanAdjectives =
            new[]
                {
                    "sadistic",
                    "stupid",
                    "pathetic",
                    "insipid",
                    "dumb",
                    "worthless"
                };
        
        public static string[] HumanNouns =
            new[]
                {
                    "meat bag",
                    "hyoomun",
                    "sack of flesh"
                };


        public static string GetRandom(this string[] stringArray)
        {
            int index = UnityEngine.Random.Range(0, stringArray.Length);
            return stringArray[index];
        }
    }
}