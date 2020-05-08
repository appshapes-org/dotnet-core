using System;
using System.Text.RegularExpressions;

namespace AppShapes.Core.Domain
{
    public class SemanticVersion : NonEmptyString
    {
        public SemanticVersion(string value) : base(value)
        {
            if (!Regex.IsMatch(value, @"^v?([1-9]|[1-9]{1}[0-9]+)\.[0-9]+$"))
                throw new ArgumentException($"Invalid version: {value}");
        }
    }
}