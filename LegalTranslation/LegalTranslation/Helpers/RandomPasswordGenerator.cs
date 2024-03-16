using System.Text;

namespace LegalTranslation.Helpers
{
    public class RandomPasswordGenerator
    {
        private const string SpecialSymbols = "!@#$%^&*()-_=+[]{}|;:'\",.<>?/";

        public static string GenerateRandomPassword()
        {
            // Define character sets
            string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string digitChars = "0123456789";

            // Create a random number generator
            Random random = new Random();

            // Initialize a StringBuilder to build the password
            StringBuilder password = new StringBuilder();

            // Add at least one lowercase character
            password.Append(lowercaseChars[random.Next(lowercaseChars.Length)]);

            // Add at least one uppercase character
            password.Append(uppercaseChars[random.Next(uppercaseChars.Length)]);

            // Add at least one digit
            password.Append(digitChars[random.Next(digitChars.Length)]);

            // Add at least one special symbol
            password.Append(SpecialSymbols[random.Next(SpecialSymbols.Length)]);

            // Fill the rest of the password with random characters
            for (int i = password.Length; i < 10; i++)
            {
                string allChars = lowercaseChars + uppercaseChars + digitChars + SpecialSymbols;
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            // Shuffle the characters in the password to make it more random
            string shuffledPassword = new string(password.ToString().ToCharArray().OrderBy(c => random.Next()).ToArray());

            shuffledPassword += '@';

            return shuffledPassword;
        }
    }
}
