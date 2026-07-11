using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetSerializer.Base.Attributes
{

    /// <summary>
    /// Defines the serializer to use for processing the type. Also defines version compatibility rules for marked types
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class VersionableAttribute : DotNetSerializerAttribute
    {
        private readonly uint? _maxVersion;
        private readonly HashSet<uint> _exactVersions;
        private readonly HashSet<uint> _excludedVersions;

        /// <summary>Property name storing version. Default: "Version".</summary>
        public string VersionPropertyName { get; set; } = "Version";

        /// <summary>
        /// Initializes with no version restriction (all versions supported).
        /// </summary>
        public VersionableAttribute()
        {
            _maxVersion = null;
            _exactVersions = new HashSet<uint>();
            _excludedVersions = new HashSet<uint>();
        }

        /// <summary>
        /// Initializes with a maximum version (all versions ≤ <paramref name="maxVersion"/> supported).
        /// </summary>
        /// <param name="maxVersion">Maximum allow version (inclusive).</param>
        public VersionableAttribute(uint maxVersion) : this()
        {
            _maxVersion = maxVersion;
            _exactVersions = new HashSet<uint>();
            _excludedVersions = new HashSet<uint>();
        }

        /// <summary>
        /// Initializes with only the specified exact versions supported.
        /// </summary>
        /// <param name="exactVersions">List of versions that are explicitly supported.</param>
        public VersionableAttribute(uint[] exactVersions)
        {
            _maxVersion = null;
            _exactVersions = new HashSet<uint>(exactVersions);
            _excludedVersions = new HashSet<uint>();
        }


        /// <summary>
        /// Initializes with a maximum version, excluding specific versions.
        /// </summary>
        /// <param name="maxVersion">Maximum allowed version (inclusive).</param>
        /// <param name="excludedVersions">Versions to exclude from support.</param>
        public VersionableAttribute(uint maxVersion, uint[] excludedVersions)
        {
            _maxVersion = maxVersion;
            _exactVersions = new HashSet<uint>();
            _excludedVersions = new HashSet<uint>(excludedVersions);
        }

        /// <summary>
        /// Initializes with no version restrictions and custom property name.
        /// </summary>
        /// <param name="versionPropertyName">Name of the property storing the version.</param>
        public VersionableAttribute(string versionPropertyName) : this()
        {
            VersionPropertyName = versionPropertyName;
        }

        /// <summary>
        /// Initializes with a maximum version and custom property name.
        /// </summary>
        /// <param name="versionPropertyName">Name of the property storing the version.</param>
        /// <param name="maxVersion">Maximum allowed version (inclusive).</param>
        public VersionableAttribute(string versionPropertyName, uint maxVersion) : this()
        {
            _maxVersion = maxVersion;
            _exactVersions = new HashSet<uint>();
            _excludedVersions = new HashSet<uint>();

            VersionPropertyName = versionPropertyName;
        }

        /// <summary>
        /// Initializes with exact versions and custom property name.
        /// </summary>
        /// <param name="versionPropertyName">Name of the property storing the version.</param>
        /// <param name="exactVersions">List of versions that are explicitly supported.</param>
        public VersionableAttribute(string versionPropertyName, uint[] exactVersions)
        {
            _maxVersion = null;
            _exactVersions = new HashSet<uint>(exactVersions);
            _excludedVersions = new HashSet<uint>();

            VersionPropertyName = versionPropertyName;
        }

        /// <summary>
        /// Initializes with a maximum version, excluded versions, and custom property name.
        /// </summary>
        /// <param name="versionPropertyName">Name of the property storing the version.</param>
        /// <param name="maxVersion">Maximum allowed version (inclusive).</param>
        /// <param name="excludedVersions">Versions to exclude from support.</param>
        public VersionableAttribute(string versionPropertyName, uint maxVersion, uint[] excludedVersions)
        {
            _maxVersion = maxVersion;
            _exactVersions = new HashSet<uint>();
            _excludedVersions = new HashSet<uint>(excludedVersions);

            VersionPropertyName = versionPropertyName;
        }

        /// <summary>
        /// Checks if the specified version is supported
        /// </summary>
        /// <param name="version">Version to check</param>
        /// <returns>True if supported; otherwise false</returns>
        public bool IsSupported(uint version)
        {
            if (_excludedVersions.Contains(version))
                return false;

            if (_exactVersions.Any())
                return _exactVersions.Contains(version);

            if (_maxVersion != null)
                return version <= _maxVersion;

            return true;
        }
    }
}
