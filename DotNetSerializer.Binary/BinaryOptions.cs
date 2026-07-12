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
    /// Specified which targets are eligible for caching
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
    public struct CachedProcessSettings
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
        private static readonly BinaryConverterStorage _defaultConverters;


        /// <summary>Gets the converter registry for custom serialization.</summary>
        public BinaryConverterStorage Converters { get; }

        /// <summary>Gets or sets the processing mode for serialization.</summary>
        public ProcessType ProcessType { get; }

        /// <summary>Gets the setting for cahced processing behavior.</summary>
        public CachedProcessSettings CachedProcessSettings { get; }

        /// <summary>
        /// Initializes the static configuration parameters.
        /// </summary>
        static BinaryOptions()
        {
            _defaultConverters = new BinaryConverterStorage();
            RegisterDefaultConverters();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOptions"/> class with default parameters.
        /// </summary>
        public BinaryOptions()
        {
            ProcessType = ProcessType.Default;
            Converters = new BinaryConverterStorage(_defaultConverters);
            RegisterConverters();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOptions"/> class with the specified cached process settings.
        /// </summary>
        /// <param name="cachedProcessType">The execution mode for cached processing.</param>
        /// <param name="targets">The type of targets are eligible for caching.</param>
        public BinaryOptions(CachedProcessType cachedProcessType, CachingTargets targets)
            : this()
        {
            ProcessType = ProcessType.Cached;
            CachedProcessSettings = new CachedProcessSettings
            {
                ProcessType = cachedProcessType,
                CachingTargets = targets
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOptions"/> class by copying parameters from specified source options.
        /// </summary>
        /// <param name="options">The source <see cref="BinaryOptions"/> class to copy from.</param>
        public BinaryOptions(BinaryOptions options) 
            : base(options)
        {
            Converters = new BinaryConverterStorage(options.Converters);
            ProcessType = options.ProcessType;
            CachedProcessSettings = options.CachedProcessSettings;
        }

        private void RegisterConverters()
        {
            Converters.Add(new StringConverter(DefaultAttributes));
        }

        private static void RegisterDefaultConverters()
        {
            _defaultConverters.Add<BoolConverter>();
            _defaultConverters.Add<ByteConverter>();
            _defaultConverters.Add<SByteConverter>();
            _defaultConverters.Add<Int16Converter>();
            _defaultConverters.Add<Int32Converter>();
            _defaultConverters.Add<Int64Converter>();
            _defaultConverters.Add<UInt16Converter>();
            _defaultConverters.Add<UInt32Converter>();
            _defaultConverters.Add<UInt64Converter>();
            _defaultConverters.Add<SingleConverter>();
            _defaultConverters.Add<DoubleConverter>();
            _defaultConverters.Add<DecimalConverter>();
            _defaultConverters.Add<CharConverter>();
        }
    }
}
