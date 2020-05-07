using System;
using System.Collections.Generic;

namespace AppShapes.Core.Testing.Core
{
    public class Assertions
    {
        public static TValue Contains<TKey, TValue>(TKey expected, IDictionary<TKey, TValue> collection)
        {
            if (!collection.TryGetValue(expected, out TValue value))
                throw new ArgumentException("Collection does not contain expected value.");
            return value;
        }

        public static object NotEqual(object expected, object actual)
        {
            return !Equals(expected, actual) ? actual : throw new ArgumentException($"{actual} equals {expected}");
        }

        public static object NotNull(object actual)
        {
            return actual ?? throw new ArgumentNullException(nameof(actual));
        }
    }
}