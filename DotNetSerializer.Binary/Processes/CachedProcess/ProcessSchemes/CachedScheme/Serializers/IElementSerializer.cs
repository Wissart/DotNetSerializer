using System.IO;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers
{
    internal interface IElementSerializer
    {
        BinaryContext ElementContext(BinaryContext context);
        void RemoveLastObjectContext(BinaryContext context);

        object DeserializeElement(BinaryReader reader, BinaryContext context);
        void SerializeElement(BinaryWriter writer, object element, BinaryContext context);

        bool TryGetSize(BinaryContext context, out int size);
    }
}
