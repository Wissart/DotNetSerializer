using DotNetSerializer.Base.Processes;

namespace DotNetSerializer.Binary.Processes.DefaultProcess
{
    internal class DefaultProcessProvider : ProcessProvider
    {
        private IDeserializationProcess _deserializationProcess;
        private ISerializationProcess _serializationProcess;

        public DefaultProcessProvider(BinaryConfiguration configuration) 
            : base(configuration)
        {
        }

        public override IDeserializationProcess GetDeserializationProcess()
        {
            if(_deserializationProcess == null)
                _deserializationProcess = new DefaultDeserializationProcess(_configuration);

            return _deserializationProcess;
        }

        public override ISerializationProcess GetSerializationProcess()
        {
            if (_serializationProcess == null)
                _serializationProcess = new DefaultSerializationProcess(_configuration);

            return _serializationProcess;
        }
    }
}
