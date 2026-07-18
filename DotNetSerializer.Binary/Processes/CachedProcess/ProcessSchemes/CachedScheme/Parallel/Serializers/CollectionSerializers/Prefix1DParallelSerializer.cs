using DotNetSerializer.Base.CollectionHandlers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Utilities.ProcessPool;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Serializers.CollectionSerializers
{
    internal class Prefix1DParallelSerializer : PrefixParallelSerializer
    {
        protected override int Rank => 1;

        public Prefix1DParallelSerializer(PropertyInfo property,
            ICollectionHandler collectionHandler, 
            Type[] elementTypes, 
            IElementSerializer elementSerializer,
            int elementSize) 
            : base(property, collectionHandler, elementTypes, elementSerializer, elementSize)
        {
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            var shape = ReadShape(reader);

            var partStreamSize = _elementSize * shape[0];

            object collection;
            if (partStreamSize >= ParallelProcessPool.PARALLELABLE_MIN_DATA_SIZE)
            {
                collection = _collectionHandler.CreateCollection(_elementTypes, shape);

                var cloneData = context.Clone();

                var partStream = new MemoryStream(reader.ReadBytes(partStreamSize));
                BinaryReader partReader = new BinaryReader(partStream);

                ParallelProcessPool.AddTask(context.ProcessID, () =>
                {
                    using (ElementContextScope(cloneData, out BinaryContext prepMetaData))
                    {
                        for (int i = 0; i < shape[0]; i++)
                        {
                            var value = _elementSerializer.DeserializeElement(partReader, prepMetaData);
                            _collectionHandler.AddItem(collection, value, i);
                        }
                    }
                    partReader.Close();
                    partStream.Close();
                });
            }
            else
            {
                var items = Deserialize1D(reader, shape, context);
                collection = _collectionHandler.CreateCollectionWithItems(_elementTypes, items);
            }

            return collection;
        }
    }
}
