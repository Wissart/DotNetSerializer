using DotNetSerializer.Base;
using DotNetSerializer.Binary.Converters.Default;
using DotNetSerializer.Binary.Storages;

namespace DotNetSerializer.Binary
{
    
    /// <summary>
    /// Specifies the processing mode for serialization opertaions.
    /// </summary>
    public enum ProcessType
    {
        /// <summary>Standart processing without caching.</summary>
        Default,
        /// <summary>Processing with caching enabled.</summary>
        Cached,
    }

    /// <summary>
    /// Specified the execution mode for cached processing.
    /// </summary>
    public enum CachedProcessType
    {
        /// <summary>Single-threaded cached processing.</summary>
        Single,
        /// <summary>Multi-threaded cached processing.</summary>
        Parallel,
    }

    /// <summary>
    /// Specified which targets are eligible for cahcing
    /// </summary>
    public enum CachingTargets
    {
        /// <summary>Only collection types are cached.</summary>
        Collections,
        /// <summary>All eligible types are cached.</summary>
        All,
    }

    /// <summary>
    /// Configuration settings for cached processing behavior.
    /// </summary>
    public class CachedProcessSettings
    {
        /// <summary>Gets or sets the execution mode for caching.</summary>
        public CachedProcessType ProcessType { get; set; }
        /// <summary>Gets or sets which targets are eligible for caching.</summary>
        public CachingTargets CachingTargets { get; set; }
    }

    /// <summary>
    /// Configuration options for binary serialization.
    /// </summary>
    public class BinaryOptions : SerializerOptions
    {
        /// <summary>Gets the converter registry for custom serialization.</summary>
        public BinaryConverterStorage Converters { get; }

        /// <summary>Gets the setting for cahced processing behavior.</summary>
        public CachedProcessSettings CachedProcessSettings { get; }

        /// <summary>Gets or sets the processing mode for serialization.</summary>
        public ProcessType ProcessType { get; set; }


        /// <summary>
        /// Initializes the settings for cached processing behavior.
        /// </summary>
        public BinaryOptions()
        {
            Converters = new BinaryConverterStorage();
            CachedProcessSettings = new CachedProcessSettings();


            AddDefaultConverters();
        }

        private void AddDefaultConverters()
        {
            Converters.Add<BoolConverter>();
            Converters.Add<ByteConverter>();
            Converters.Add<SByteConverter>();
            Converters.Add<Int16Converter>();
            Converters.Add<Int32Converter>();
            Converters.Add<Int64Converter>();
            Converters.Add<UInt16Converter>();
            Converters.Add<UInt32Converter>();
            Converters.Add<UInt64Converter>();
            Converters.Add<SingleConverter>();
            Converters.Add<DoubleConverter>();
            Converters.Add<DecimalConverter>();
            Converters.Add<CharConverter>();

            Converters.Add(new StringConverter(DefaultAttributes));
        }
    }
}
