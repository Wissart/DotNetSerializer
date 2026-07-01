using DotNetSerializer.Binary.Converters;
using System;
using System.IO;

namespace DotNetSerializer.Binary.Processes.Common.Base
{
    internal static class VersionableSerializer
    {
        public static object Deserialize(BinaryReader reader, 
            Type type, 
            BinaryContext context,
            Action<BinaryReader, BinaryContext> deserializeVersionableObject)
        {
            var obj = Activator.CreateInstance(type);

            var newContext = new BinaryContext(context, obj);
            deserializeVersionableObject(reader, newContext);

            return obj;
        }

        public static object DeserializeWithConverter(BinaryReader reader,
            BinaryConverter converter,
            BinaryContext context,
            Action<BinaryReader, BinaryContext> deserializeClassObject)
        {
            var obj = converter.Read(reader, context);

            if (!converter.IsComplete)
            {
                var newContext = new BinaryContext(context, obj);
                deserializeClassObject(reader, newContext);
            }

            return obj;
        }

        public static void Serialize(BinaryWriter writer, 
            object obj, 
            BinaryContext context,
            Action<BinaryWriter, BinaryContext> serializeVersionableClass)
        {
            var newContext = new BinaryContext(context, obj);
            serializeVersionableClass(writer, newContext);
        }

        public static void SerializeWithConverter(BinaryWriter writer,
            BinaryConverter converter,
            object obj,
            BinaryContext context,
            Action<BinaryWriter, BinaryContext> serializeClassObject)
        {
            if (converter.CanWrite)
                converter.Write(writer, obj, context);

            if (!converter.IsComplete)
            {
                var newContext = new BinaryContext(context, obj);
                serializeClassObject(writer, newContext);
            }
        }
    }
}
