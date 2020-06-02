using System.IO;
using System.Runtime.Serialization;

namespace AppShapes.Core.Testing.Core
{
    public static class FormatterHelper
    {
        public static T Deserialize<T>(IFormatter formatter, Stream stream, bool resetPosition = true)
        {
            if (resetPosition)
                stream.Position = 0;
            return (T) formatter.Deserialize(stream);
        }

        public static Stream Serialize(IFormatter formatter, Stream stream, object value, bool resetPosition = true)
        {
            formatter.Serialize(stream, value);
            if (resetPosition)
                stream.Position = 0;
            return stream;
        }
    }
}