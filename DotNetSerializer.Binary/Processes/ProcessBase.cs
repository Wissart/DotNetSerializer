namespace DotNetSerializer.Binary.Processes
{
    internal abstract class ProcessBase
    {
        public BinaryOptions Options { get; }

        public ProcessBase(BinaryOptions options)
        {
            Options = options;
        }
    }
}
