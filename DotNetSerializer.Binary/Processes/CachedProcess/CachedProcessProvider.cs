using DotNetSerializer.Base.Exceptions;
using DotNetSerializer.Base.Processes;
using DotNetSerializer.Binary.Processes.CachedProcess.Parallel;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel;
using System.Collections.Generic;

namespace DotNetSerializer.Binary.Processes.CachedProcess
{
    internal class CachedProcessProvider : ProcessProvider
    {
        private readonly Dictionary<CachedProcessType, IDeserializationProcess> _deserializationProcesses;
        private readonly Dictionary<CachedProcessType, ISerializationProcess> _serializationProcesses;


        private readonly CachedSerializerProvider _cachedSerializerProvider;
        private ParallelSerializerProvider _parallelSerializerProvider;

        public CachedProcessProvider(BinaryConfiguration configuration) 
            : base(configuration)
        {
            _deserializationProcesses = new Dictionary<CachedProcessType, IDeserializationProcess>();
            _serializationProcesses = new Dictionary<CachedProcessType, ISerializationProcess>();

            _cachedSerializerProvider = new CachedSerializerProvider(configuration);
        }

        public override IDeserializationProcess GetDeserializationProcess()
        {
            var cachedProcessType = _configuration.Options.CachedProcessSettings.ProcessType;

            if (!_deserializationProcesses.ContainsKey(cachedProcessType))
                _deserializationProcesses[cachedProcessType] = CreateDeserializationProcess(cachedProcessType);

            return _deserializationProcesses[cachedProcessType];
        }

        public override ISerializationProcess GetSerializationProcess()
        {
            var cachedProcessType = _configuration.Options.CachedProcessSettings.ProcessType;

            if (!_serializationProcesses.ContainsKey(cachedProcessType))
                _serializationProcesses[cachedProcessType] = CreateSerializationProcess(cachedProcessType);

            return _serializationProcesses[cachedProcessType];
        }


        private IDeserializationProcess CreateDeserializationProcess(CachedProcessType cachedProcessType)
        {
            switch (cachedProcessType)
            {
                case CachedProcessType.Single:
                    return new CachedDeserializationProcess(_configuration, _cachedSerializerProvider);
                case CachedProcessType.Parallel:
                    if (_parallelSerializerProvider == null)
                        _parallelSerializerProvider = new ParallelSerializerProvider(_configuration, _cachedSerializerProvider);

                    return new ParallelDeserializationProcess(_configuration, _parallelSerializerProvider);
                default:
                    throw new DotNetSerializerException("Unknown cached process type");
            }
        }

        private ISerializationProcess CreateSerializationProcess(CachedProcessType cachedProcessType)
        {
            switch (cachedProcessType)
            {
                case CachedProcessType.Single:
                    return new CachedSerializationProcess(_configuration, _cachedSerializerProvider);
                case CachedProcessType.Parallel:
                    if (_parallelSerializerProvider == null)
                        _parallelSerializerProvider = new ParallelSerializerProvider(_configuration, _cachedSerializerProvider);

                    return new ParallelSerializationProcess(_configuration, _parallelSerializerProvider);
                default:
                    throw new DotNetSerializerException("Unknown cached process type");
            }
        }
    }
}
