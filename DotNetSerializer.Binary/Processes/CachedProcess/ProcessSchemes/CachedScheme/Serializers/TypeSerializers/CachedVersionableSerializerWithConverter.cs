using DotNetSerializer.Binary.Converters;
using DotNetSerializer.Binary.Processes.Common.Base;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.TypeSerializers
{
    internal class CachedVersionableSerializerWithConverter : CachedVersionableSerializer
    {
        private readonly BinaryConverter _converter;

        public BinaryConverter Converter => _converter;
        public CachedVersionableSerializerWithConverter(PropertyInfo property, IProcessScheme serializationScheme, BinaryConverter converter) 
            : base(property, serializationScheme)
        {
            _converter = converter;
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            return VersionableSerializer.DeserializeWithConverter(reader, _converter, context, DeserializeVersionableObject);
        }

        public override object DeserializeElement(BinaryReader reader, BinaryContext context)
        {
            var obj = _converter.Read(reader, context.Prev);

            if (!_converter.IsComplete)
            {
                DeserializeElement(reader, obj, context);
            }

            return obj;
        }

        public override void Serialize(BinaryWriter writer, object value, BinaryContext context)
        {
            VersionableSerializer.SerializeWithConverter(writer, _converter, value, context, SerializeVersionableObject);
        }

        public override void SerializeElement(BinaryWriter writer, object element, BinaryContext context)
        {
            if (_converter.CanWrite)
                _converter.Write(writer, element, context.Prev);

            if (!_converter.IsComplete)
            {
                base.SerializeElement(writer, element, context);
            }
        }
    }
}
