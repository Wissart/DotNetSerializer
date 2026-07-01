using DotNetSerializer.Binary.Converters;
using DotNetSerializer.Binary.Processes.Common.Base;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.TypeSerializers
{
    internal class CachedNonClassSerializer : TypeSerializer
    {
        private readonly BinaryConverter _converter;

        public CachedNonClassSerializer(PropertyInfo property, BinaryConverter converter) : base(property)
        {
            _converter = converter;
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            return NonClassSerializer.Deserialize(reader, _converter, context);
        }

        public override object DeserializeElement(BinaryReader reader, BinaryContext context)
        {
            var value = _converter.Read(reader, context);
            return value;
        }

        public override void Serialize(BinaryWriter writer, object value, BinaryContext context)
        {
            NonClassSerializer.Serialize(writer, _converter, value, context);
        }

        public override void SerializeElement(BinaryWriter writer, object element, BinaryContext context)
        {
            _converter.Write(writer, element, context);
        }

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            return _converter.TryGetSize(context, out size);
        }
    }
}
