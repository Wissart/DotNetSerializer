using DotNetSerializer.Base;
using DotNetSerializer.Base.Processes;
using DotNetSerializer.Binary.Processes.CachedProcess;
using DotNetSerializer.Binary.Processes.DefaultProcess;
using System;
using DotNetSerializer.Binary.Processes;

namespace DotNetSerializer.Binary
{
    /// <summary>
    /// Serializer for binary format serialization and deserialization
    /// </summary>
    public class BinarySerializer : Serializer
    {
        private readonly ProcessProvider _processProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySerializer"/> class with the default binary options.
        /// </summary>
        public BinarySerializer()
        {
            var configuration = new BinaryConfiguration(new BinaryOptions());
            _processProvider = GetProcessProvider(configuration);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySerializer"/> class with the specified binary options.
        /// </summary>
        /// <param name="options">The configuration options for binary serialization.</param>
        public BinarySerializer(BinaryOptions options)
        {
            var configuration = new BinaryConfiguration(options);
            _processProvider = GetProcessProvider(configuration);
        }

        protected override IDeserializationProcess GetDeserializationProcess()
        {
            return _processProvider.GetDeserializationProcess();
        }

        protected override ISerializationProcess GetSerializationProcess()
        {
            return _processProvider.GetSerializationProcess();
        }

        private ProcessProvider GetProcessProvider(BinaryConfiguration configuration)
        {
            switch (configuration.Options.ProcessType)
            {
                case ProcessType.Default:
                    return new DefaultProcessProvider(configuration);
                case ProcessType.Cached:
                    return new CachedProcessProvider(configuration);
                default:
                    throw new ArgumentException(nameof(configuration.Options.ProcessType));
            }
        }
    }
}
