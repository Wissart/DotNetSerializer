using DotNetSerializer.Base.Reflection;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers.TypeSerializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme
{
    internal class CachedProcessScheme : IProcessScheme
    {
        private readonly ISchemeMaker _schemeMaker;
        private readonly Dictionary<string, PropertySerializer> _serializers;
        private readonly Dictionary<uint, PropertySerializer[]> _serializersByVersion;

        private readonly Dictionary<uint, int> _sizeByVersion;

        private PropertySerializer[] _versionSerialzers;

        public SerializableTypeInfo TypeInfo { get; }

        public CachedProcessScheme(SerializableTypeInfo typeInfo, ISchemeMaker schemeMaker)
        {
            TypeInfo = typeInfo;
            _schemeMaker = schemeMaker;

            _serializers = new Dictionary<string, PropertySerializer>();
            _serializersByVersion = new Dictionary<uint, PropertySerializer[]>();
            _sizeByVersion = new Dictionary<uint, int>();
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
            var propertyName = context.MetaData.Property.Name;
            if (!_serializers.ContainsKey(propertyName))
                _serializers[propertyName] = _schemeMaker.GetCollectionSerializerByProperty(elementTypes, context);

            return _serializers[propertyName];
        }

        public IEnumerable<PropertySerializer> GetVersionSerializers()
        {
            if (_versionSerialzers != null)
                return _versionSerialzers;

            var versionPos = TypeInfo.GetVersionPropertyID();
            var properties = TypeInfo.Properties;

            _versionSerialzers = new PropertySerializer[versionPos];
            for (int i = 0; i < versionPos; i++)
            {
                _versionSerialzers[i] = _schemeMaker.GetSerializerByProperty(properties[i]);
            }

            return _versionSerialzers;
        }

        public IEnumerable<PropertySerializer> GetSerializers(uint version = 0)
        {
            if (_serializersByVersion.ContainsKey(version))
                return _serializersByVersion[version];

            var properties = TypeInfo.GetPropertiesByVersion(version);

            var serializers = new PropertySerializer[properties.Length];
            for (int i = 0; i < serializers.Length; i++)
            {
                serializers[i] = GetSerializer(properties[i]);
            }

            _serializersByVersion[version] = serializers;

            return _serializersByVersion[version];
        }

        public bool TryGetSize(BinaryContext context, out int size)
        {
            if (!_sizeByVersion.TryGetValue(context.Version, out size))
            {
                size = 0;
                using (context.CreateMetaDataScope(null))
                {
                    var metaData = context.MetaData;
                    foreach (var serializer in GetSerializers(context.Version))
                    {
                        metaData.Property = serializer.Property;
                        if (!serializer.TryGetSize(context, out int serializerSize))
                        {
                            size = -1;
                            break;
                        }

                        size += serializerSize;
                    }
                }

                _sizeByVersion[context.Version] = size;
            }

            return size > 0;
        }

        private PropertySerializer GetSerializer(PropertyInfo property)
        {
            if (!_serializers.ContainsKey(property.Name))
                _serializers[property.Name] = _schemeMaker.GetSerializerByProperty(property);

            return _serializers[property.Name];
        }
    }
}
