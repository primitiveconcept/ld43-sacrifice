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
        
        public static string[] HumanIdlePhrases =
            new[]
                {
                    "teh earf is flat, dur",
                    "i want my first daughter to be a girl",
                    "i have good immune systum, i shouldn't get pregnat",
                    "go packers!",
                    "wtf is obama's last name?",
                    "my gf take me for granite",
                    "wtf is a afterdavid?",
                    "is stevie wonder still blind?",
                    "is there a non winter olympic or?"
                };
        
        public static string[] HumanIdleHappyPhrases =
            new[]
                {
                    "i lub mah cat",
                    "mah cat lub me",
                    "cat better than dog",
                    "i do anything for cat",
                    "is cat liquid?",
                    "toe beans",
                    "special cat look like potato",
                    "me smoke catnip, so high now, ha ha!",
                    "my cat have asterik butt, ha ha!",
                    "Ph'nglui mglw'nafh Cathulhu Cheez'burger wgah'nagl fhtagn",
                    "i shuld let mah kid play wif cat",
                    "cats clean, no need clean litter"
                };


        public static string GetRandom(this string[] stringArray)
        {
            int index = UnityEngine.Random.Range(0, stringArray.Length);
            return stringArray[index];
        }
    }
}