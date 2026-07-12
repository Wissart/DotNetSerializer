using DotNetSerializer.Base.Processes;

namespace DotNetSerializer.Binary.Processes
{
    internal abstract class ProcessProvider
    {
        protected readonly BinaryConfiguration _configuration;

        public ProcessProvider(BinaryConfiguration configuration)
        {
            _configuration = configuration;
        }

        public abstract IDeserializationProcess GetDeserializationProcess();
        public abstract ISerializationProcess GetSerializationProcess();
    }
}
