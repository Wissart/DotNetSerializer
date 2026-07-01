using System;

namespace DotNetSerializer.Base.Utilities
{
    /// <summary>
    /// Utility methods for use during processing
    /// </summary>
    public static class SerializationUtilities
    {
        /// <summary>
        /// Determines if the type is a reference type (class) excluding string.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is a class and not a string; otherwise, false.</returns>
        public static bool IsClass(Type type)
        {
            return type.IsClass && (type != typeof(string));
        }
    }
}
