using DotNetSerializer.Base.Attributes;
using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Base.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetSerializer.Base.Reflection
{
    /// <summary>
    /// Contains information for a type that is required for serialization.
    /// </summary>
    public class SerializableTypeInfo
    {
        private readonly VersionableAttribute _versionableAttribute;
        private readonly Dictionary<uint, PropertyInfo[]> _propertiesByVersion;

        private int _versionPropertyID;
        private PropertyInfo _versionProperty;

        /// <summary>
        /// Gets the described type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the properties of the described type.
        /// </summary>
        public PropertyInfo[] Properties { get; }

        /// <summary>
        /// Gets a value that indicates is the type marked with the versionable attribute.
        /// </summary>
        public bool IsVersionable { get; }

        /// <summary>
        /// Initializes a new instance with type, and properties.
        /// </summary>
        /// <param name="type">The described type.</param>
        /// <param name="properties">The properties of the described type.</param>
        public SerializableTypeInfo(Type type, PropertyInfo[] properties)
        {
            Type = type;
            Properties = properties;
            IsVersionable = AttributeUtilities.TryGetTypeAttribute<VersionableAttribute>(type, out _versionableAttribute);

            _propertiesByVersion = new Dictionary<uint, PropertyInfo[]>();
        }

        /// <summary>
        /// Provides PropertyInfo for the property that contains the version value
        /// </summary>
        /// <returns>The property what contains version value</returns>
        /// <exception cref="VersionPropertyNotFoundException">Thrown when object not contains property.</exception> 
        public PropertyInfo GetVersionProperty()
        {
            if (_versionProperty == null)
                FindVersionProperty();

            return _versionProperty;
        }

        /// <summary>
        /// Provides the ID for a version property
        /// </summary>
        /// <returns>The ID for a version property</returns>
        /// <exception cref="VersionPropertyNotFoundException">Thrown when an object does not contain the property.</exception> 
        public int GetVersionPropertyID()
        {
            if (_versionPropertyID == 0)
                FindVersionProperty();

            return _versionPropertyID;
        }

        /// <summary>
        /// Provides the property collection of the type sorted by version 
        /// </summary>
        /// <param name="version">Object version</param>
        /// <returns>The property collection of the type sorted by version </returns>
        /// /// <exception cref="UnsupportedVersionException">Thrown when the type does not support a version.</exception> 
        public PropertyInfo[] GetPropertiesByVersion(uint version)
        {
            if (!_propertiesByVersion.ContainsKey(version))
            {
                if (IsVersionable && !_versionableAttribute.IsSupported(version))
                    throw new UnsupportedVersionException(version, Type);

                _propertiesByVersion[version] = Properties.Skip(_versionPropertyID).Where(prop => !AttributeUtilities.TryGetPropertyAttribute(prop, out RequireVersionAttribute supportVersion)
                                                                || supportVersion.IsSupportVersion(version)).ToArray();
            }
            return _propertiesByVersion[version];
        }

        private void FindVersionProperty()
        {
            var ID = 0;
            if(IsVersionable)
            {
                var versionPropertyName = _versionableAttribute.VersionPropertyName;
                foreach (var property in Properties)
                {
                    ID++;
                    if (property.Name == versionPropertyName)
                    {
                        _versionPropertyID = ID;
                        _versionProperty = property;
                        return;
                    }
                }
                throw new VersionPropertyNotFoundException(versionPropertyName, Type);
            }
        }
    }
}
