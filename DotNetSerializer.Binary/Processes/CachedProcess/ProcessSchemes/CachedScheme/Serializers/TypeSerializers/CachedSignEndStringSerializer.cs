using DotNetSerializer.Binary.Converters.Default;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.TypeSerializers
{
    internal class CachedSignEndStringSerializer : StringSerializer
    {
        public CachedSignEndStringSerializer(PropertyInfo property, StringConverter converter, string encodingName) 
            : base(property, converter, encodingName)
        {
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            return StringConverter.ReadSignEndString(reader, _encoding);
        }

        public override object DeserializeElement(BinaryReader reader, BinaryContext context)
        {
            return StringConverter.ReadSignEndString(reader, _encoding);
        }

        public override void Serialize(BinaryWriter writer, object value, BinaryContext context)
        {
            StringConverter.WriteSignEndString(writer, (string)value, _encoding);
        }

        public override void SerializeElement(BinaryWriter writer, object element, BinaryContext context)
        {
            StringConverter.WriteSignEndString(writer, (string)element, _encoding);
        }

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            return _converter.TryGetSize(context, out size);
        }
    }
}
