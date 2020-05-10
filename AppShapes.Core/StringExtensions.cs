using System.Text.RegularExpressions;

namespace AppShapes.Core
{
    public static class StringExtensions
    {
        public static string[] WordsFromCamelCase(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? new string[] { } : Regex.Replace(value, @"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])", " ").Split(" ");
        }
    }
}