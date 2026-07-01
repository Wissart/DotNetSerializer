using DotNetSerializer.Base.Attributes;
using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Base.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;

namespace DotNetSerializer.Base.Tests.Unit.Reflection
{
    [TestClass]
    public class SerializableTypeInfoTests
    {
        #region Test Data Setup

        private class CommonClass
        {
            public int ID { get; set; }
            public int Name { get; set; }
        }

        [Versionable]
        private class VersionableClass
        {
            public int Sign { get; set; }
            public uint Version { get; set; }
            [RequireVersion(3)]
            public int Param1 { get; set; }
            [RequireVersion(1, 2)]
            public int Param2 { get; set; }
            [RequireVersion(1, 5)]
            public int Param3 { get; set; }
            public int Param4 { get; set; }
        }

        [Versionable("Ver", 5)]
        public class CustomVersionableClass
        {
            public int Sign { get; set; }
            public uint Ver { get; set; }
            [RequireVersion(3)]
            public int Param1 { get; set; }
            [RequireVersion(1, 2)]
            public int Param2 { get; set; }
            [RequireVersion(1, 5)]
            public int Param3 { get; set; }
            public int Param4 { get; set; }
        }

        [Versionable]
        public class VersionableClassWithoutProperty
        {
            public int Sign { get; set; }
            [RequireVersion(3)]
            public int Param1 { get; set; }
            [RequireVersion(1, 2)]
            public int Param2 { get; set; }
            [RequireVersion(1, 5)]
            public int Param3 { get; set; }
            public int Param4 { get; set; }
        }

        #endregion


        [TestMethod]
        public void Constructor_WithValidParameters_ShouldInitilizeProperties()
        {
            var type = typeof(CommonClass);
            var properties = type.GetProperties();

            var typeInfo = new SerializableTypeInfo(type, properties);

            Assert.AreEqual(type, typeInfo.Type);
            CollectionAssert.AreEqual(properties, typeInfo.Properties);
        }

        [TestMethod]
        public void GetVersionProperty_WithDefaultPropertyName_ShouldReturnProperty()
        {
            var type = typeof(VersionableClass);
            var properties = type.GetProperties();
            var typeInfo = new SerializableTypeInfo(type, properties);

            var versionProperty = typeInfo.GetVersionProperty();

            Assert.IsNotNull(versionProperty);
            Assert.AreEqual("Version", versionProperty.Name);
            Assert.AreEqual(typeof(uint), versionProperty.PropertyType);
        }

        [TestMethod]
        public void GetVersionProperty_WithCustomPropertyName_ShouldReturnProperty()
        {
            var type = typeof(CustomVersionableClass);
            var properties = type.GetProperties();
            var typeInfo = new SerializableTypeInfo(type, properties);

            var versionProperty = typeInfo.GetVersionProperty();

            Assert.IsNotNull(versionProperty);
            Assert.AreEqual("Ver", versionProperty.Name);
            Assert.AreEqual(typeof(uint), versionProperty.PropertyType);
        }

        [TestMethod]
        public void GetVersionProperty_WhenCalledTwice_ShouldReturnCachedProperty()
        {
            var type = typeof(VersionableClass);
            var properties = type.GetProperties();
            var typeInfo = new SerializableTypeInfo(type, properties);

            var firstCall = typeInfo.GetVersionProperty();
            var secondCall = typeInfo.GetVersionProperty();

            Assert.AreSame(firstCall, secondCall);
        }

        [TestMethod]
        public void GetVersionProperty_WhenNoVersionProperty_ShouldThrowVersionPropertyNotFoundException()
        {
            var type = typeof(VersionableClassWithoutProperty);
            var properties = type.GetProperties();
            var typeInfo = new SerializableTypeInfo(type, properties);

            var exception = Assert.ThrowsException<VersionPropertyNotFoundException>(
                () => typeInfo.GetVersionProperty());
            Assert.AreEqual("Version", exception.PropertyName);
            Assert.AreEqual(typeof(VersionableClassWithoutProperty), exception.ObjectType);
        }

        [TestMethod]
        public void GetVersionPropertyID_WithDefaultPropertyName_ShouldReturn2()
        {
            var type = typeof(VersionableClass);
            var properties = type.GetProperties();
            var typeInfo = new SerializableTypeInfo(type, properties);

            int result = typeInfo.GetVersionPropertyID();

            // Sign (0), Version (1), Param1 (2) ...
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void GetVersionPropertyID_WithCustomPropertyName_ShouldReturn2()
        {
            var type = typeof(CustomVersionableClass);
            var properties = type.GetProperties();
            var typeInfo = new SerializableTypeInfo(type, properties);

            int result = typeInfo.GetVersionPropertyID();

            // Sign (0), Ver (1), Param1 (2) ...
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void GetVersionPropertyID_WhenNoVersionProperty_ShouldThrowVersionPropertyNotFoundException()
        {
            var type = typeof(VersionableClassWithoutProperty);
            var properties = type.GetProperties();
            var typeInfo = new SerializableTypeInfo(type, properties);

            var exception = Assert.ThrowsException<VersionPropertyNotFoundException>(
                () => typeInfo.GetVersionPropertyID());
            Assert.AreEqual("Version", exception.PropertyName);
            Assert.AreEqual(typeof(VersionableClassWithoutProperty), exception.ObjectType);
        }

        [TestMethod]
        public void GetPropertiesByVersion_WithVersionableClass_VersionIs1_ShouldReturnCorrectPropertiesCollection()
        {
            var type = typeof(VersionableClass);
            var properties = type.GetProperties();
            var typeInfo = new SerializableTypeInfo(type, properties);

            var result = typeInfo.GetPropertiesByVersion(1);

            Assert.AreEqual(5, result.Length);
            Assert.IsTrue(result.Any(p => p.Name == "Sign"));
            Assert.IsTrue(result.Any(p => p.Name == "Version"));
            Assert.IsTrue(result.Any(p => p.Name == "Param2"));
            Assert.IsTrue(result.Any(p => p.Name == "Param3"));
            Assert.IsTrue(result.Any(p => p.Name == "Param4"));
        }

        [TestMethod]
        public void GetPropertiesByVersion_WithVersionableClass_VersionIs3_ShouldReturnCorrectProperties()
        {
            var type = typeof(VersionableClass);
            var properties = type.GetProperties();
            var typeInfo = new SerializableTypeInfo(type, properties);

            var result = typeInfo.GetPropertiesByVersion(3);

            Assert.AreEqual(5, result.Length);
            Assert.IsTrue(result.Any(p => p.Name == "Sign"));
            Assert.IsTrue(result.Any(p => p.Name == "Version"));
            Assert.IsTrue(result.Any(p => p.Name == "Param1"));
            Assert.IsTrue(result.Any(p => p.Name == "Param3"));
            Assert.IsTrue(result.Any(p => p.Name == "Param4"));
        }

        [TestMethod]
        public void GetPropertiesByVersion_WithVersionableClass_VersionIs6_ShouldReturnCorrectProperties()
        {
            var type = typeof(VersionableClass);
            var properties = type.GetProperties();
            var typeInfo = new SerializableTypeInfo(type, properties);

            var result = typeInfo.GetPropertiesByVersion(6);

            Assert.AreEqual(4, result.Length);
            Assert.IsTrue(result.Any(p => p.Name == "Sign"));
            Assert.IsTrue(result.Any(p => p.Name == "Version"));
            Assert.IsTrue(result.Any(p => p.Name == "Param1"));
            Assert.IsTrue(result.Any(p => p.Name == "Param4"));
        }

        [TestMethod]
        public void GetPropertiesByVersion_WhenCalledTwice_ShouldReturnCachedArray()
        {
            var type = typeof(VersionableClass);
            var properties = type.GetProperties();
            var typeInfo = new SerializableTypeInfo(type, properties);

            var firstCall = typeInfo.GetPropertiesByVersion(3);
            var secondCall = typeInfo.GetPropertiesByVersion(3);

            Assert.AreSame(firstCall, secondCall);
            CollectionAssert.AreEqual(firstCall, secondCall);
        }

        [TestMethod]
        public void GetPropertiesByVersion_WithUnsupportedVersion_ShouldThrowUnsupportedVersionException()
        {
            var type = typeof(CustomVersionableClass);
            var properties = type.GetProperties();
            var typeInfo = new SerializableTypeInfo(type, properties);

            var exception = Assert.ThrowsException<UnsupportedVersionException>(
                () => typeInfo.GetPropertiesByVersion(6));

            Assert.AreEqual(typeof(CustomVersionableClass), exception.Type);
            Assert.AreEqual<uint>(6, exception.Version);

        }
    }
}
