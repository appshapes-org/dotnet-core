using System;

namespace AppShapes.Core
{
    public class EnvironmentHelper
    {
        public static string GetEnvironmentVariable(string variable)
        {
            string value = Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Process);
            return string.IsNullOrWhiteSpace(value) ? Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Machine) : value;
        }
    }
}