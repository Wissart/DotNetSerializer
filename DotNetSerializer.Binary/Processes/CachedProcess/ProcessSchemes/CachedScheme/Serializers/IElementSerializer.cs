using System.IO;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers
{
    internal interface IElementSerializer
    {
        BinaryContext PrepareContext(BinaryContext context);
        void FreeObjectContext(BinaryContext context);

        object DeserializeElement(BinaryReader reader, BinaryContext context);
        void SerializeElement(BinaryWriter writer, object element, BinaryContext context);

        bool TryGetSize(BinaryContext context, out int size);
    }
}
