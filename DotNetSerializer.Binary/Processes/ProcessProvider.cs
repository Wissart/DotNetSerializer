using DotNetSerializer.Base.Processes;

namespace DotNetSerializer.Binary.Processes
{
    internal abstract class ProcessProvider
    {
        protected readonly BinaryOptions _options;

        public ProcessProvider(BinaryOptions options)
        {
            _options = options;
        }

        public abstract IDeserializationProcess GetDeserializationProcess();
        public abstract ISerializationProcess GetSerializationProcess();
    }
}
