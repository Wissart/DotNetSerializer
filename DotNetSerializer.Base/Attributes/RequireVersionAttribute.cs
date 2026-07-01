using System;

namespace DotNetSerializer.Base.Attributes
{
    /// <summary>
    /// Defines which versions are available for processing th property.</summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RequireVersionAttribute : DotNetSerializerAttribute
    {
        /// <summary>Gets the minimum value of the version range.</summary>
        public uint Min { get; }
        /// <summary>Gets the maximum value of the version range.</summary>
        public uint Max { get; }

        /// <summary>
        /// Initializes version requirement with min/max bound.
        /// </summary>
        /// <param name="min">Minimum version.</param>
        /// <param name="max">Maximum version. Default: <see cref="uint.MaxValue"/>.</param>
        /// <exception cref="ArgumentException">Throw if <paramref name="min"> > <paramref name="max">.</exception>
        public RequireVersionAttribute(uint min, uint max = uint.MaxValue)
        {
            if (min > max)
                throw new ArgumentException($"Max must be >= Min. Min: {min}, Max: {max}", nameof(max));

            Min = min;
            Max = max;
        }

        public bool IsSupportVersion(uint version) => ((version >= Min) && (version <= Max));
    }
}
