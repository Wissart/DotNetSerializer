using DotNetSerializer.Base.Storages;

namespace DotNetSerializer.Binary
{
    internal class BinaryConfiguration
    {
        public TypeInfoStorage TypeInfoStorage { get; }
        public BinaryOptions Options { get; }

        public BinaryConfiguration(BinaryOptions options)
        {
            TypeInfoStorage = new TypeInfoStorage();
            Options = new BinaryOptions(options);
        }
    }
}
