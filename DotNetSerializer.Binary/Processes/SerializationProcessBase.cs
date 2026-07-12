using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Base.Processes;
using DotNetSerializer.Base.Utilities;
using DotNetSerializer.Binary.Converters;
using DotNetSerializer.Binary.Processes.Common.Base;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes
{
    internal abstract class SerializationProcessBase : ProcessBase, ISerializationProcess
    {
        protected SerializationProcessBase(BinaryConfiguration configuration) 
            : base(configuration)
        {
        }

        public void Serialize<T>(Stream stream, T obj)
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                Serialize(writer, obj, new BinaryContext(null));
            }
        }

        private void Serialize(BinaryWriter writer, object value, BinaryContext context)
        {
            if (value == null)
                throw new SerializationException("Passed value can not be null");

            if (CollectionUtilities.TryGetElementTypes(value.GetType(), out Type[] elementTypes))
            {
                SerializeCollection(writer, value, elementTypes, context);
            }
            else
            {
                SerializeValue(writer, value, value.GetType(), context);
            }
        }

        protected abstract void SerializeCollection(BinaryWriter writer, object value, Type[] elementTypes, BinaryContext context);

        private void SerializeValue(BinaryWriter writer, object value, Type converterType, BinaryContext context)
        {
            Type valueType = value.GetType();

            if (SerializationUtilities.IsClass(valueType))
            {
                if (TypeInfoStorage.Get(valueType).IsVersionable)
                {
                    if (Options.Converters.Items.TryGetValue(converterType, out BinaryConverter converter))
                        VersionableSerializer.SerializeWithConverter(writer, converter, value, context, SerializeVersionableObject);
                    else
                        VersionableSerializer.Serialize(writer, value, context, SerializeVersionableObject);
                }
                else
                {
                    if (Options.Converters.Items.TryGetValue(converterType, out BinaryConverter converter))
                        ClassSerializer.SerializeWithConverter(writer, converter, value, context, SerializeClassObject);
                    else
                        ClassSerializer.Serialize(writer, value, context, SerializeClassObject);
                }
            }
            else
            {
                var converter = Options.Converters.Get(converterType);
                NonClassSerializer.Serialize(writer, converter, value, context);
            }
        }

        protected void SerializeVersionableObject(BinaryWriter writer, BinaryContext context)
        {
            var typeInfo = TypeInfoStorage.Get(context.ObjectContext.Object.GetType());

            SerializeVersion(writer, context);

            var properties = typeInfo.GetPropertiesByVersion(context.Version);
            SerializeProperties(writer, properties, properties.Length, context);
        }

        private void SerializeVersion(BinaryWriter writer, BinaryContext context)
        {
            var typeInfo = TypeInfoStorage.Get(context.ObjectContext.Object.GetType());
            var properties = typeInfo.Properties;

            var versionPropPos = typeInfo.GetVersionPropertyID();

            SerializeProperties(writer, properties, versionPropPos, context);

            var version = typeInfo.GetVersionProperty().GetValue(context.ObjectContext.Object);
            context.Version = (uint)version;
        }

        protected void SerializeClassObject(BinaryWriter writer, BinaryContext context)
        {
            var typeInfo = TypeInfoStorage.Get(context.ObjectContext.Object.GetType());

            var properties = typeInfo.GetPropertiesByVersion(context.Version);
            SerializeProperties(writer, properties, properties.Length, context);
        }

        private void SerializeProperties(BinaryWriter writer, PropertyInfo[] properties, int toPropPos, BinaryContext context)
        {
            for (int i = 0; i < toPropPos; i++)
            {
                context.ObjectContext.Property = properties[i];
                var value = context.ObjectContext.GetValue();
                SerializeProperty(writer, value, context);
            }
        }

        private void SerializeProperty(BinaryWriter writer, object value, BinaryContext context)
        {
            if (CollectionUtilities.TryGetElementTypes(value.GetType(), out Type[] elementTypes))
            {
                SerializePropertyCollection(writer, value, elementTypes, context);
            }
            else
            {
                SerializePropertyValue(writer, value, context);
            }
        }

        protected abstract void SerializePropertyCollection(BinaryWriter writer, object collection, Type[] elementTypes, BinaryContext context);

        private void SerializePropertyValue(BinaryWriter writer, object value, BinaryContext context)
        {
            Type converterType = value.GetType();
            var converterAttribute = AttributeUtilities.GetConverterAttribute(context.ObjectContext.Property);
            if (converterAttribute != null)
            {
                converterType = converterAttribute.ConverterType;
                if (!Options.Converters.Contains(converterType))
                    throw new ConverterNotFoundException(converterType);
            }

            SerializeValue(writer, value, converterType, context);
        }
    }
}
