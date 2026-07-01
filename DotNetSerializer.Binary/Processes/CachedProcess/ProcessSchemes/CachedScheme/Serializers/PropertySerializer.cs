using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers
{
    internal abstract class PropertySerializer
    {
        public PropertyInfo Property { get; }

        public PropertySerializer(PropertyInfo property)
        {
            Property = property;
        }

        public abstract object Deserialize(BinaryReader reader, BinaryContext context);
        public abstract void Serialize(BinaryWriter writer, object value, BinaryContext context);

        public abstract bool TryGetSize(BinaryContext context, out int size);
    }
}
