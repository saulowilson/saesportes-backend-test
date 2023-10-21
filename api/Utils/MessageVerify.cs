using System.Text.RegularExpressions;

namespace api.Utils
{
    public static class MessageVerify
    {
        public static bool validateMessage(string message)
        {
            if (message.Length < 15 || message.Length > 20)
            {
                return false;
            }

            if (!Regex.IsMatch(message, "^[a-zA-Z0-9@#$%&_]*$"))
            {
                return false;
            }

            int lowercaseCharacters = Regex.Matches(message, "[a-z]").Count;
            if (lowercaseCharacters < 2)
            {
                return false;
            }

            int upperCaseCharacters = Regex.Matches(message, "[A-Z]").Count;
            if (upperCaseCharacters < 5)
            {
                return false;
            }

            int repeatedCharacters = 0;
            for (int i = 0; i < message.Length - 1; i++)
            {
                if (message[i] == message[i + 1])
                {
                    repeatedCharacters++;
                    if (repeatedCharacters >= 4)
                    {
                        break;
                    }
                }
            }
            if (repeatedCharacters < 4)
            {
                return false;
            }

            int specialCharacters = Regex.Matches(message, "[@#$%&_]").Count;
            if (specialCharacters < 2)
            {
                return false;
            }

            return true;
        }
    }
}
