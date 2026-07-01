using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers
{
    internal abstract class ContainerSerializer : TypeSerializer
    {
        protected IProcessScheme _serializationScheme;
        public IProcessScheme SerializationScheme
        {
            get => _serializationScheme;
            set => _serializationScheme = value;
        }

        public ContainerSerializer(PropertyInfo property, IProcessScheme serializationScheme)
            : base(property)
        {
            _serializationScheme = serializationScheme;
        }
    }
}
