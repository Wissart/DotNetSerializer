using DotNetSerializer.Base.CollectionHandlers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Utilities.ProcessPool;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Serializers
{
    internal abstract class ParallelCollectionSerializer : CollectionSerializer
    {
        protected readonly int _elementSize;

        protected ParallelCollectionSerializer(PropertyInfo property,
            ICollectionHandler collectionHandler,
            int rank,
            Type[] elementTypes,
            IElementSerializer elementSerializer,
            int elementSize)
            : base(property, collectionHandler, rank, elementTypes, elementSerializer)
        {
            _elementSize = elementSize;
        }

        protected override object DeserializeCollection(BinaryReader reader, int[] shape, BinaryContext context)
        {
            var partStreamSize = _elementSize;
            for (int i = 0; i < shape.Length; i++)
                partStreamSize *= shape[i];

            if (partStreamSize >= ParallelProcessPool.PARALLELABLE_MIN_DATA_SIZE)
            {
                var cloneContext = context.Clone();

                var partStream = new MemoryStream(reader.ReadBytes(partStreamSize));
                BinaryReader partReader = new BinaryReader(partStream);

                ParallelProcessPool.AddTask(context.ProcessID, () =>
                {
                    var collection = base.DeserializeCollection(partReader, shape, cloneContext);
                    cloneContext.MetaData.SetValue(collection);

                    partReader.Close();
                    partStream.Close();
                });

                return null;
            }

            return base.DeserializeCollection(reader, shape, context);
        }


    }
}
