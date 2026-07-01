using DotNetSerializer.Base.Reflection;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.TypeSerializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel
{
    internal class ParallelProcessScheme : IProcessScheme
    {
        private readonly CachedProcessScheme _cachedScheme;
        private readonly ISchemeMaker _schemeMaker;
        private readonly Dictionary<string, PropertySerializer> _serializers;
        private readonly Dictionary<uint, PropertySerializer[]> _serializersByVersion;

        public SerializableTypeInfo TypeInfo => _cachedScheme.TypeInfo;

        public ParallelProcessScheme(CachedProcessScheme cachedScheme,
            ISchemeMaker schemeMaker)
        {
            _cachedScheme = cachedScheme;
            _schemeMaker = schemeMaker;

            _serializers = new Dictionary<string, PropertySerializer>();
            _serializersByVersion = new Dictionary<uint, PropertySerializer[]>();
        }


        public object Deserialize(BinaryReader reader, BinaryContext context)
        {
            if (TypeInfo.IsVersionable)
                return new CachedVersionableSerializer(null, this).Deserialize(reader, context);
            else
                return new CachedClassSerializer(null, this).Deserialize(reader, context);
        }

        public PropertySerializer GetCollectionSerializer(Type[] elementTypes, BinaryContext context)
        {
            var propertyName = context.ObjectContext.Property.Name;
            if (!_serializers.ContainsKey(propertyName))
                _serializers[propertyName] = _schemeMaker.GetCollectionSerializerByProperty(elementTypes, context);

            return _serializers[propertyName];
        }

        public IEnumerable<PropertySerializer> GetVersionSerializers()
        {
            return _cachedScheme.GetVersionSerializers();
        }

        public IEnumerable<PropertySerializer> GetSerializers(uint version = 0)
        {
            if (_serializersByVersion.ContainsKey(version))
                return _serializersByVersion[version];

            var properties = TypeInfo.GetPropertiesByVersion(version);

            var context = new BinaryContext(null, null)
            {
                Version = version,
            };

            var serializers = new PropertySerializer[properties.Length];
            for (int i = 0; i < serializers.Length; i++)
            {
                context.ObjectContext.Property = (properties[i]);
                serializers[i] = GetSerializer(properties[i], context);
            }

            _serializersByVersion[version] = serializers;

            return _serializersByVersion[version];
        }

        private PropertySerializer GetSerializer(PropertyInfo property, BinaryContext context)
        {
            var propertyName = property.Name;
            if (!_serializers.ContainsKey(propertyName))
                _serializers[propertyName] = _schemeMaker.GetSerializerByProperty(property, context);

            return _serializers[propertyName];
        }

        public bool TryGetSize(BinaryContext context, out int size)
        {
            size = -1;
            return false;
        }
    }
}
