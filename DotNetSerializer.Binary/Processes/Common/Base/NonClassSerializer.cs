using DotNetSerializer.Binary.Converters;
using System.IO;

namespace DotNetSerializer.Binary.Processes.Common.Base
{
    internal static class NonClassSerializer
    {
        public static object Deserialize(BinaryReader reader, 
            BinaryConverter converter, 
            BinaryContext context)
        {
            var value = converter.Read(reader, context);
            return value;
        }

        public static void Serialize(BinaryWriter writer, 
            BinaryConverter converter, 
            object value, 
            BinaryContext context)
        {
            converter.Write(writer, value, context);
        }
    }
}
