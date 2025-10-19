namespace Hisabwala.Application.Shared
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input[0].ToString().Trim().ToUpper() + input.Substring(1).Trim().ToLower()
            };
    }
}
