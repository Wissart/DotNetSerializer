using DotNetSerializer.Base.Attributes;
using DotNetSerializer.Base.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace DotNetSerializer.Base.Tests.Unit.Utilities
{
    [TestClass]
    public class AttributeUtilitiesTests
    {
        private class ConverterTypeStub { }
        private class NonVersionableClass { }

        [Versionable]
        private class VersionableClass
        {
            public int NonAttributedProperty { get; set; }

            public uint Version { get; set; }

            [Converter(typeof(ConverterTypeStub))]
            public float ConvertableProperty { get; set; }

            [Collection()]
            public int[] CollectionProperty { get; set; }

            [Converter(typeof(ConverterTypeStub))]
            [Collection()]
            public int[] MultipleAttributesProperty { get; set; }
            [StringFormat(StringSizeType.Prefix)]
            public string StringFormatProperty { get; set; }
        }




        [TestMethod]
        public void ContainConverterAttribute_WhenConverterAttributeExists_ShouldReturnTrue()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.ConvertableProperty));

            var result = AttributeUtilities.ContainConverterAttribute(property);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainConverterAttribute_WhenConverterAttributeNoExist_ShouldReturnFalse()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.NonAttributedProperty));

            var result = AttributeUtilities.ContainConverterAttribute(property);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetConverterAttribute_WhenConverterAttributeExists_ShouldReturnCorrectConverterAttribute()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.ConvertableProperty));

            var attribute = AttributeUtilities.GetConverterAttribute(property);

            Assert.IsNotNull(attribute);
            Assert.IsInstanceOfType(attribute, typeof(ConverterAttribute));
            Assert.AreEqual(typeof(ConverterTypeStub), attribute.ConverterType);
        }

        [TestMethod]
        public void GetConverterAttribute_WhenNoConverterAttribute_ShouldReturnNull()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.NonAttributedProperty));

            var attribute = AttributeUtilities.GetConverterAttribute(property);

            Assert.IsNull(attribute);
        }

        [TestMethod]
        public void GetCollectionAttribute_WhenCollectionAttributeExists_ShouldReturnCorrectCollectionAttribute()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.CollectionProperty));

            var attribute = AttributeUtilities.GetCollectionAttribute(property);

            Assert.IsNotNull(attribute);
            Assert.IsInstanceOfType(attribute, typeof(CollectionAttribute));
            Assert.AreEqual(CollectionSizeType.Prefix, attribute.SizeType);
        }

        [TestMethod]
        public void GetCollectionAttribute_WhenNoCollectionAttribute_ShouldReturnNull()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.NonAttributedProperty));

            var attribute = AttributeUtilities.GetCollectionAttribute(property);

            Assert.IsNull(attribute);
        }

        [TestMethod]
        public void GetStringFormatAttribute_WhenStringFormatAttributeExists_ShouldReturnCorrectStringFormatAttribute()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.StringFormatProperty));

            var attribute = AttributeUtilities.GetStringFormatAttribute(property);

            Assert.IsNotNull(attribute);
            Assert.IsInstanceOfType(attribute, typeof(StringFormatAttribute));
            Assert.AreEqual(StringSizeType.Prefix, attribute.SizeType);
        }

        [TestMethod]
        public void GetStringFormatAttribute_WhenNoStringFormatAttribute_ShouldReturnNull()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.NonAttributedProperty));

            var attribute = AttributeUtilities.GetStringFormatAttribute(property);

            Assert.IsNull(attribute);
        }

        [TestMethod]
        public void IsTypeContainAttribute_Generic_WhenTypeHasAttribute_ShouldReturnTrue()
        {
            var type = typeof(VersionableClass);

            var result = AttributeUtilities.IsTypeContainAttribute<VersionableAttribute>(type);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsTypeContainAttribute_Generic_WhenTypeDoesNotHaveAttribute_ShouldReturnFalse()
        {
            var type = typeof(NonVersionableClass);

            var result = AttributeUtilities.IsTypeContainAttribute<VersionableAttribute>(type);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsTypeContainAttribute_NonGeneric_WhenTypeHasAttribute_ShouldReturnTrue()
        {
            var type = typeof(VersionableClass);

            var result = AttributeUtilities.IsTypeContainAttribute(type, typeof(VersionableAttribute));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsTypeContainAttribute_NonGeneric_WhenTypeDoesNotHaveAttribute_ShouldReturnFalse()
        {
            var type = typeof(NonVersionableClass);

            var result = AttributeUtilities.IsTypeContainAttribute(type, typeof(VersionableAttribute));

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsTypeContainAttribute_WhenTypeIsNull_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => AttributeUtilities.IsTypeContainAttribute<VersionableAttribute>(null));
        }

        [TestMethod]
        public void IsPropertyContainsAttribute_Generic_WhenPropertyHasAttribute_ShouldReturnTrue()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.CollectionProperty));

            var result = AttributeUtilities.IsPropertyContainAttribute<CollectionAttribute>(property);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPropertyContainsAttribute_Generic_WhenPropertyDoesNotHaveAttribute_ShouldReturnFalse()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.NonAttributedProperty));

            var result = AttributeUtilities.IsPropertyContainAttribute<CollectionAttribute>(property);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsPropertyContainsAttribute_NonGeneric_WhenPropertyHasAttribute_ShouldReturnTrue()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.CollectionProperty));

            var result = AttributeUtilities.IsPropertyContainAttribute(property, typeof(CollectionAttribute));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPropertyContainsAttribute_NonGeneric_WhenPropertyDoesNotHaveAttribute_ShouldReturnFalse()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.NonAttributedProperty));

            var result = AttributeUtilities.IsPropertyContainAttribute(property, typeof(CollectionAttribute));

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsPropertyContainsAttribute_WhenPropertyIsNull_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => AttributeUtilities.IsPropertyContainAttribute<CollectionAttribute>(null));
        }

        [TestMethod]
        public void GetAttributes_WhenPropertyHasMultipleAttributes_ShouldReturnAllAttributes()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.MultipleAttributesProperty));

            var attributes = AttributeUtilities.GetAttributes(property).ToList();

            Assert.AreEqual(2, attributes.Count);
            Assert.IsTrue(attributes.Any(a => a is ConverterAttribute));
            Assert.IsTrue(attributes.Any(a => a is CollectionAttribute));
        }

        [TestMethod]
        public void GetAttributes_WhenPropertyHasSingleAttributes_ShouldReturnOneAttribute()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.CollectionProperty));

            var attributes = AttributeUtilities.GetAttributes(property).ToList();

            Assert.AreEqual(1, attributes.Count);
            Assert.IsInstanceOfType(attributes[0], typeof(CollectionAttribute));
        }

        [TestMethod]
        public void GetAttributes_WhenPropertyHasNoAttribute_ShouldReturnEmptyCollection()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.NonAttributedProperty));

            var attributes = AttributeUtilities.GetAttributes(property).ToList();

            Assert.IsNotNull(attributes);
            Assert.AreEqual(0, attributes.Count);
        }

        [TestMethod]
        public void GetAttributes_WhenPropertyIsNull_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => AttributeUtilities.GetAttributes(null));
        }

        [TestMethod]
        public void TryGetAttribute_WhenPropertyHasAttribute_ShouldReturnTrueAndAttribute()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.CollectionProperty));

            var result = AttributeUtilities.TryGetPropertyAttribute<CollectionAttribute>(property, out var attribute);

            Assert.IsTrue(result);
            Assert.IsNotNull(attribute);
            Assert.IsInstanceOfType(attribute, typeof(CollectionAttribute));
        }

        [TestMethod]
        public void TryGetAttribute_WhenPropertyHasNoAttribute_ShouldReturnFalseAndNull()
        {
            var property = typeof(VersionableClass).GetProperty(nameof(VersionableClass.NonAttributedProperty));

            var result = AttributeUtilities.TryGetPropertyAttribute<CollectionAttribute>(property, out CollectionAttribute attribute);

            Assert.IsFalse(result);
            Assert.IsNull(attribute);
        }

        [TestMethod]
        public void TryGetAttribute_WhenPropertyIsNull_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => AttributeUtilities.TryGetPropertyAttribute<CollectionAttribute>(null, out var attribute));
        }
    }
}
