using System;

namespace DotNetSerializer.Base.Attributes
{
    /// <summary>
    /// Determines how collection size is processed
    /// </summary>
    public enum CollectionSizeType
    {
        /// <summary>Fixed size defined by <see cref="CollectionAttribute.Shape"/>.</summary>
        Fixed,
        /// <summary> Size is prefixed in the serialized data.</summary>
        Prefix,
    }

    /// <summary>
    /// Defines how the collection property is processed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CollectionAttribute : DotNetSerializerAttribute
    {
        /// <summary>Gets the size determination strategy.</summary>
        public CollectionSizeType SizeType { get; }

        /// <summary>Gets the fixed dimension lengths when <see cref="SizeType"/> is <see cref="CollectionSizeType.Fixed"/>.</summary>
        public int[] Shape { get; }

        /// <summary>
        /// Initializes with prefix-based size handling.
        /// </summary>
        public CollectionAttribute() => SizeType = CollectionSizeType.Prefix;

        /// <summary>
        /// Initializes with fixed size determination strategy.
        /// </summary>
        /// <param name="shape">Fixed collection dimension lengths. Cannot be empty.</param>
        public CollectionAttribute(params int[] shape)
        {
            if (shape.Length <= 0)
                throw new ArgumentException("Collection rank cannot be 0", nameof(shape));

            SizeType = CollectionSizeType.Fixed;
            Shape = shape;
        }
    }
}
