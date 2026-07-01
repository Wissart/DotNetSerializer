using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using System;
using System.IO;
using System.Reflection;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Utilities.ProcessPool;
using DotNetSerializer.Binary.Processes.Common.Base;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Serializers.TypeSerializers
{
    internal class ClassParallelSerializer : PropertySerializer
    {
        protected readonly int _objectSize;

        protected readonly IProcessScheme _serializationScheme;

        public ClassParallelSerializer(PropertyInfo property, IProcessScheme serializationScheme, int objectSize) : base(property)
        {
            _objectSize = objectSize;
            _serializationScheme = serializationScheme;
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            var obj = Activator.CreateInstance(_serializationScheme.TypeInfo.Type);

            var cloneData = context.Clone();
            var partStream = new MemoryStream(reader.ReadBytes(_objectSize));
            BinaryReader partReader = new BinaryReader(partStream);

            ParallelProcessPool.AddTask(context.ProcessID, () =>
            {
                using (cloneData.NewObjectContextScope(obj))
                    DeserializeClassObject(partReader, cloneData);

                partReader.Close();
                partStream.Close();
            });

            return obj;
        }

        protected void DeserializeClassObject(BinaryReader reader, BinaryContext context)
        {
            foreach (var serializer in _serializationScheme.GetSerializers(context.Version))
            {
                context.ObjectContext.Property = serializer.Property;
                var value = serializer.Deserialize(reader, context);
                context.ObjectContext.SetValue(value);
            }
        }


        public override void Serialize(BinaryWriter writer, object value, BinaryContext context)
        {
            var obj = Property.GetValue(context.ObjectContext.Object);

            ClassSerializer.Serialize(writer, obj, context, SerializeClassObject);
        }

        protected void SerializeClassObject(BinaryWriter writer, BinaryContext context)
        {
            foreach (var serializer in _serializationScheme.GetSerializers(context.Version))
            {
                context.ObjectContext.Property = serializer.Property;
                var value = context.ObjectContext.GetValue();
                serializer.Serialize(writer, value, context);
            }
        }

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            return _serializationScheme.TryGetSize(context, out size);
        }
    }
}
