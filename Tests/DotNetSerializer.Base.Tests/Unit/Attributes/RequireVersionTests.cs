using DotNetSerializer.Base.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DotNetSerializer.Base.Tests.Unit.Attributes
{
    [TestClass]
    public class RequireVersionTests
    {
        [TestMethod]
        public void Constructor_WithoutMaxVersion_MaxVersionDefaultValue()
        {
            uint minVersion = 145;

            var attribute = new RequireVersionAttribute(minVersion);

            Assert.AreEqual(uint.MaxValue, attribute.Max);
        }

        [TestMethod]
        public void Constructor_WhenMaxLessThanMinVersion_ThrowArgumentException()
        {
            uint minVersion = 156;
            uint maxVersion = 145;

            var exception = Assert.ThrowsException<ArgumentException>(
                () => new RequireVersionAttribute(minVersion, maxVersion));
            Assert.AreEqual("max", exception.ParamName);
        }

        [TestMethod]
        public void IsSupportVersion_WithValidVersion_ReturnTrue()
        {
            uint minVersion = 145;
            uint maxVersion = 156;
            uint supportversion = 150;
            var attribute = new RequireVersionAttribute(minVersion, maxVersion);

            var result = attribute.IsSupportVersion(supportversion);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsSupportVersion_WithMinRangeVersion_ReturnTrue()
        {
            uint minVersion = 145;
            uint maxVersion = 156;
            var attribute = new RequireVersionAttribute(minVersion, maxVersion);

            var result = attribute.IsSupportVersion(minVersion);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsSupportVersion_WithMaxRangeVersion_ReturnTrue()
        {
            uint minVersion = 145;
            uint maxVersion = 156;
            var attribute = new RequireVersionAttribute(minVersion, maxVersion);

            var result = attribute.IsSupportVersion(maxVersion);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsSupportVersion_WithVersionOutOfRange_ReturnFalse()
        {
            uint minVersion = 145;
            uint maxVersion = 156;
            uint unsupportVersion = 160;
            var attribute = new RequireVersionAttribute(minVersion, maxVersion);

            var result = attribute.IsSupportVersion(unsupportVersion);

            Assert.IsFalse(result);
        }
    }
}
