using DotNetSerializer.Base;
using DotNetSerializer.Base.Processes;
using DotNetSerializer.Binary.Processes.CachedProcess;
using DotNetSerializer.Binary.Processes.DefaultProcess;
using System;
using System.Collections.Generic;
using DotNetSerializer.Binary.Processes;

namespace DotNetSerializer.Binary
{
    /// <summary>
    /// Serializer for binary format serialization and deserialization
    /// </summary>
    public class BinarySerializer : Serializer
    {
        private readonly BinaryOptions _options;
        
        private readonly Dictionary<ProcessType, ProcessProvider> _processProviders;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySerializer"/> class with the specified binary options.
        /// </summary>
        /// <param name="options">The configuration options for binary serialization.</param>
        public BinarySerializer(BinaryOptions options)
        {
            _options = options;

            _processProviders = new Dictionary<ProcessType, ProcessProvider>();
        }

        protected override IDeserializationProcess GetDeserializationProcess()
        {
            if (!_processProviders.ContainsKey(_options.ProcessType))
                _processProviders[_options.ProcessType] = GetProcessProvider(_options.ProcessType);

            return _processProviders[_options.ProcessType].GetDeserializationProcess();
        }

        protected override ISerializationProcess GetSerializationProcess()
        {
            if (!_processProviders.ContainsKey(_options.ProcessType))
                _processProviders[_options.ProcessType] = GetProcessProvider(_options.ProcessType);

            return _processProviders[_options.ProcessType].GetSerializationProcess();
        }

        private ProcessProvider GetProcessProvider(ProcessType processType)
        {
            switch (processType)
            {
                case ProcessType.Default:
                    return new DefaultProcessProvider(_options);
                case ProcessType.Cached:
                    return new CachedProcessProvider(_options);
                default:
                    throw new ArgumentException(nameof(processType));
            }
        }
    }
}
