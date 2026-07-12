using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Base.Processes;
using DotNetSerializer.Base.Storages;
using DotNetSerializer.Base.Utilities;
using DotNetSerializer.Binary.Converters;
using DotNetSerializer.Binary.Processes.Common.Base;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes
{
    internal abstract class DeserializationProcessBase : ProcessBase, IDeserializationProcess
    {
        protected DeserializationProcessBase(BinaryConfiguration configuration) 
            : base(configuration)
        {
        }

        public T Deserialize<T>(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {

                var result = Deserialize<T>(reader);

                if (Options.RequireEnd && stream.Position != stream.Length)
                    throw new StreamEndNotReachedException(stream.Length - stream.Position);

                return result;
            }
        }

        protected virtual T Deserialize<T>(BinaryReader reader)
        {
            var context = new BinaryContext(null);
            return (T)Deserialize(reader, typeof(T), context);
        }

        protected virtual object Deserialize(BinaryReader reader, Type type, BinaryContext context)
        {
            if (CollectionUtilities.TryGetElementTypes(type, out Type[] elementTypes))
            {
                return DeserializeCollection(reader, type, elementTypes, context);
            }
            else
            {
                return DeserializeValue(reader, type, type, context);
            }
        }

        protected abstract object DeserializeCollection(BinaryReader reader, Type type, Type[] elementTypes, BinaryContext context);

        protected object DeserializeValue(BinaryReader reader, Type valueType, Type converterType, BinaryContext context)
        {
            if (SerializationUtilities.IsClass(valueType))
            {
                if (TypeInfoStorage.Get(valueType).IsVersionable)
                {
                    if (Options.Converters.Items.TryGetValue(converterType, out BinaryConverter converter))
                        return VersionableSerializer.DeserializeWithConverter(reader, converter, context, DeserializeVersionableObject);
                    else
                        return VersionableSerializer.Deserialize(reader, valueType, context, DeserializeVersionableObject);
                }
                else
                {
                    if (Options.Converters.Items.TryGetValue(converterType, out BinaryConverter converter))
                        return ClassSerializer.DeserializeWithConverter(reader, converter, context, DeserializeClassObject);
                    else
                        return ClassSerializer.Deserialize(reader, valueType, context, DeserializeClassObject);
                }
            }
            else
            {
                var converter = Options.Converters.Get(converterType);
                return NonClassSerializer.Deserialize(reader, converter, context);
            }
        }

        protected void DeserializeVersionableObject(BinaryReader reader, BinaryContext context)
        {
            var typeInfo = TypeInfoStorage.Get(context.ObjectContext.Object.GetType());

            DeserializeVersion(reader, context);

            var properties = typeInfo.GetPropertiesByVersion(context.Version);
            DeserializeProperties(reader, properties, properties.Length, context);
        }

        private void DeserializeVersion(BinaryReader reader, BinaryContext context)
        {
            var typeInfo = TypeInfoStorage.Get(context.ObjectContext.Object.GetType());
            var properties = typeInfo.Properties;

            var versionPropPos = typeInfo.GetVersionPropertyID();

            DeserializeProperties(reader, properties, versionPropPos, context);

            var version = typeInfo.GetVersionProperty().GetValue(context.ObjectContext.Object);
            context.Version = (uint)version;
        }

        protected void DeserializeClassObject(BinaryReader reader, BinaryContext context)
        {
            var typeInfo = TypeInfoStorage.Get(context.ObjectContext.Object.GetType());

            var properties = typeInfo.GetPropertiesByVersion(context.Version);
            DeserializeProperties(reader, properties, properties.Length, context);
        }

        private void DeserializeProperties(BinaryReader reader, PropertyInfo[] properties, int toPropPos, BinaryContext context)
        {
            for (int i = 0; i < toPropPos; i++)
            {
                context.ObjectContext.Property = properties[i];
                var value = DeserializeProperty(reader, context.ObjectContext.Property.PropertyType, context);
                context.ObjectContext.SetValue(value);
            }
        }

        private object DeserializeProperty(BinaryReader reader, Type propertyType, BinaryContext context)
        {
            if (CollectionUtilities.TryGetElementTypes(propertyType, out Type[] elementTypes))
            {
                return DeserializePropertyCollection(reader, propertyType, elementTypes, context);
            }
            else
            {
                return DeserializePropertyValue(reader, propertyType, context);
            }
        }

        protected abstract object DeserializePropertyCollection(BinaryReader reader, Type propertyType, Type[] elementTypes, BinaryContext context);

        private object DeserializePropertyValue(BinaryReader reader, Type valueType, BinaryContext context)
        {
            Type converterType = valueType;
            var converterAttribute = AttributeUtilities.GetConverterAttribute(context.ObjectContext.Property);
            if (converterAttribute != null)
            {
                converterType = converterAttribute.ConverterType;
                if (!Options.Converters.Contains(converterType))
                    throw new ConverterNotFoundException(converterType);
            }

            return DeserializeValue(reader, valueType, converterType, context);
        }
    }
}
