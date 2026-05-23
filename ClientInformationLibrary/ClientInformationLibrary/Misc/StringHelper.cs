namespace ClientInformation.Library.Core.Misc
{
    public static class StringHelper
    {
        public static string Reverse(string input)
        {
            char[] characters = input.ToCharArray();
            System.Array.Reverse(characters);
            return new string(characters);
        }

        public static bool IsNullOrWhiteSpace(string? input)
        {
            return string.IsNullOrWhiteSpace(input);
        }
    }
}
