using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers
{
    internal abstract class TypeSerializer : PropertySerializer, IElementSerializer
    {
        public TypeSerializer(PropertyInfo property) : base(property)
        {
        }

        public virtual BinaryContext ElementContext(BinaryContext context) => context;
        public virtual void RemoveLastObjectContext(BinaryContext context) { }

        public abstract object DeserializeElement(BinaryReader reader, BinaryContext context);
        public abstract void SerializeElement(BinaryWriter writer, object element, BinaryContext context);
    }
}
