namespace Hisabwala.Application.Shared
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            if (input is null) 
                throw new ArgumentNullException(nameof(input));
            
            string trimmed = input.Trim();
            
            if (trimmed == "") 
                throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
            
            return char.ToUpper(trimmed[0]) + trimmed.Substring(1).ToLower();
        }
    }
}
