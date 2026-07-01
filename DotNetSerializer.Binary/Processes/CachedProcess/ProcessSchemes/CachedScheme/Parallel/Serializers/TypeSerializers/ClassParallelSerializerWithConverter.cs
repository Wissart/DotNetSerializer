using DotNetSerializer.Binary.Converters;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Utilities.ProcessPool;
using DotNetSerializer.Binary.Processes.Common.Base;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Serializers.TypeSerializers
{
    internal class ClassParallelSerializerWithConverter : ClassParallelSerializer
    {
        private readonly BinaryConverter _converter;
        public ClassParallelSerializerWithConverter(PropertyInfo property,
            IProcessScheme serializationScheme, 
            int objectSize, 
            BinaryConverter converter)
            : base(property, serializationScheme, objectSize)
        {
            _converter = converter;
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            var obj = _converter.Read(reader, context);

            if (!_converter.IsComplete)
            {
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
            }

            return obj;
        }

        public override void Serialize(BinaryWriter writer, object value, BinaryContext context)
        {
            ClassSerializer.SerializeWithConverter(writer, _converter, value, context, SerializeClassObject);
        }

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            if (_converter.IsComplete)
                return _converter.TryGetSize(context, out size);
            else
                return _serializationScheme.TryGetSize(context, out size);
        }
    }
}
