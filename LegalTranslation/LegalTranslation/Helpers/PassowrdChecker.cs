namespace LegalTranslation.Helpers
{
    public static class PassowrdChecker
    {
        public static bool IsPasswordValid(string password)
        {
            // Check if the password has at least 6 characters
            if (password.Length < 6)
            {
                //Console.WriteLine("Password must have at least 6 characters.");
                return false;
            }

            // Check if the password contains at least 1 digit
            if (!password.Any(char.IsDigit))
            {
                //Console.WriteLine("Password must contain at least 1 digit.");
                return false;
            }

            // Check if the password contains at least 1 special symbol
            if (!password.Any(IsSpecialSymbol))
            {
                //Console.WriteLine("Password must contain at least 1 special symbol.");
                return false;
            }

            if (!password.Any(char.IsLower))
            {
                //Console.WriteLine("Password must contain at least 1 lowercase letter.");
                return false;
            }

            // Check if the password contains at least 1 uppercase letter
            if (!password.Any(char.IsUpper))
            {
                //Console.WriteLine("Password must contain at least 1 uppercase letter.");
                return false;
            }

            // Password meets all the requirements
            //Console.WriteLine("Password is valid.");
            return true;
        }

        private static bool IsSpecialSymbol(char c)
        {
            // Define your set of special symbols here
            char[] specialSymbols = { '@', '$', '!', '%', '*', '#', '?', '&' };

            // Check if the character is in the set of special symbols
            return specialSymbols.Contains(c);
        }
    }
}
