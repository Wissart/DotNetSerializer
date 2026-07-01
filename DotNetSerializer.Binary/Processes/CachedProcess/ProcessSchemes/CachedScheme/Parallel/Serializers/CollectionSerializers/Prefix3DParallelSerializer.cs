using DotNetSerializer.Base;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Utilities.ProcessPool;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Serializers.CollectionSerializers
{
    internal class Prefix3DParallelSerializer : PrefixParallelSerializer
    {
        protected override int Rank => 3;

        public Prefix3DParallelSerializer(PropertyInfo property,
            CollectionHandler collectionHandler,
            Type[] elementTypes,
            IElementSerializer elementSerializer,
            int elementSize)
            : base(property, collectionHandler, elementTypes, elementSerializer, elementSize)
        {
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            var shape = ReadShape(reader);
            var collection = _collectionHandler.CreateCollection(_elementTypes, shape);

            var cloneData = context.Clone();
            var partStream = new MemoryStream(reader.ReadBytes(_elementSize * shape[0] * shape[1] * shape[2]));
            BinaryReader partReader = new BinaryReader(partStream);

            ParallelProcessPool.AddTask(context.ProcessID, () =>
            {
                using (PrepContextScope(cloneData, out BinaryContext prepMetaData))
                {
                    for (int i = 0; i < shape[0]; i++)
                    {
                        for (int j = 0; j < shape[1]; j++)
                        {
                            for (int k = 0; k < shape[2]; k++)
                            {
                                var value = _elementSerializer.DeserializeElement(partReader, prepMetaData);
                                _collectionHandler.AddItem(collection, value, i, j, k);
                            }
                        }
                    }
                }
                partReader.Close();
                partStream.Close();
            });

            return collection;
        }
    }
}
