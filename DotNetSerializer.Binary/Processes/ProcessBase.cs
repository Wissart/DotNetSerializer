using DotNetSerializer.Base.Storages;

namespace DotNetSerializer.Binary.Processes
{
    internal abstract class ProcessBase
    {
        public BinaryOptions Options { get; }
        public TypeInfoStorage TypeInfoStorage { get; }

        public ProcessBase(BinaryConfiguration configuration)
        {
            Options = configuration.Options;
            TypeInfoStorage = configuration.TypeInfoStorage;
        }
    }
}
