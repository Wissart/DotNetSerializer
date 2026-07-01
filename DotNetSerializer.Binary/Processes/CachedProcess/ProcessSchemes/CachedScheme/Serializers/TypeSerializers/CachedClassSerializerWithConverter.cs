using DotNetSerializer.Binary.Converters;
using DotNetSerializer.Binary.Processes.Common.Base;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.TypeSerializers
{
    internal class CachedClassSerializerWithConverter : CachedClassSerializer
    {
        private readonly BinaryConverter _converter;

        public BinaryConverter Converter => _converter;
        public CachedClassSerializerWithConverter(PropertyInfo property, IProcessScheme serializationScheme, BinaryConverter converter) 
            : base(property, serializationScheme)
        {
            _converter = converter;
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            return ClassSerializer.DeserializeWithConverter(reader, _converter, context, DeserializeClassObject);
        }

        public override object DeserializeElement(BinaryReader reader, BinaryContext context)
        {
            var obj = _converter.Read(reader, context);

            if (!_converter.IsComplete)
            {
                context.ObjectContext.Object = obj;
                DeserializeClassObject(reader, context);
            }

            return obj;
        }

        public override void Serialize(BinaryWriter writer, object value, BinaryContext context)
        {
            ClassSerializer.SerializeWithConverter(writer, _converter, value, context, SerializeClassObject);
        }

        public override void SerializeElement(BinaryWriter writer, object element, BinaryContext context)
        {
            if (_converter.CanWrite)
                _converter.Write(writer, element, context);

            if (!_converter.IsComplete)
            {
                context.ObjectContext.Object = element;
                SerializeClassObject(writer, context);
            }
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
