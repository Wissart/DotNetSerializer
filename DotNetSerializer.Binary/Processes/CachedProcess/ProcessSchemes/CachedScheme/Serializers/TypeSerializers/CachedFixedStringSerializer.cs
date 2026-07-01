using DotNetSerializer.Binary.Converters.Default;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.TypeSerializers
{
    internal class CachedFixedStringSerializer : StringSerializer
    {
        private readonly int _size;
        public CachedFixedStringSerializer(PropertyInfo property, StringConverter converter, string encodingName, int size) : base(property, converter, encodingName)
        {
            _size = size;
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            return StringConverter.ReadFixedSizeString(reader, _encoding, _size);
        }

        public override object DeserializeElement(BinaryReader reader, BinaryContext context)
        {
            return StringConverter.ReadFixedSizeString(reader, _encoding, _size);
        }

        public override void Serialize(BinaryWriter writer, object value, BinaryContext context)
        {
            StringConverter.WriteFixedString(writer, (string)value, _encoding, _size);
        }

        public override void SerializeElement(BinaryWriter writer, object element, BinaryContext context)
        {
            StringConverter.WriteFixedString(writer, (string)element, _encoding, _size);
        }

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = _size;
            return true;
        }
    }
}
