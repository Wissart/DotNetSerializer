using DotNetSerializer.Binary.Converters;
using System;
using System.IO;

namespace DotNetSerializer.Binary.Processes.Common.Base
{
    internal static class ClassSerializer
    {
        public static object Deserialize(BinaryReader reader,
            Type type, 
            BinaryContext context, 
            Action<BinaryReader, BinaryContext> deserializeClassObject)
        {
            var obj = Activator.CreateInstance(type);

            using (context.CreateMetaDataScope(obj))
                deserializeClassObject(reader, context);

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
                using (context.CreateMetaDataScope(obj))
                    deserializeClassObject(reader, context);
            }

            return obj;
        }

        public static void Serialize(BinaryWriter writer,
            object obj,
            BinaryContext context,
            Action<BinaryWriter, BinaryContext> serializeClassObject)
        {
            using (context.CreateMetaDataScope(obj))
                serializeClassObject(writer, context);
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
                using (context.CreateMetaDataScope(obj))
                    serializeClassObject(writer, context);
            }
        }
    }
}
