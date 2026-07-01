using DotNetSerializer.Base.Processes;

namespace DotNetSerializer.Binary.Processes.DefaultProcess
{
    internal class DefaultProcessProvider : ProcessProvider
    {
        private IDeserializationProcess _deserializationProcess;
        private ISerializationProcess _serializationProcess;

        public DefaultProcessProvider(BinaryOptions options) 
            : base(options)
        {
        }

        public override IDeserializationProcess GetDeserializationProcess()
        {
            if(_deserializationProcess == null)
                _deserializationProcess = new DefaultDeserializationProcess(_options);

            return _deserializationProcess;
        }

        public override ISerializationProcess GetSerializationProcess()
        {
            if (_serializationProcess == null)
                _serializationProcess = new DefaultSerializationProcess(_options);

            return _serializationProcess;
        }
    }
}
