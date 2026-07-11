using DotNetSerializer.Base.CollectionHandlers;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Utilities.ProcessPool;
using DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers;
using System;
using System.IO;
using System.Reflection;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Serializers.CollectionSerializers
{
    internal class Fixed2DParallelSerializer : FixedParallelSerializer
    {
        protected override int Rank => 2;

        public Fixed2DParallelSerializer(PropertyInfo property,
            ICollectionHandler collectionHandler, 
            Type[] elementTypes, 
            IElementSerializer elementSerializer, 
            int[] shape, 
            int elementSize) 
            : base(property, collectionHandler, elementTypes, elementSerializer, shape, elementSize)
        {
        }

        public override object Deserialize(BinaryReader reader, BinaryContext context)
        {
            var collection = _collectionHandler.CreateCollection(_elementTypes, _shape);

            var cloneData = context.Clone();
            var partStream = new MemoryStream(reader.ReadBytes(_elementSize * _shape[0] * _shape[1]));
            BinaryReader partReader = new BinaryReader(partStream);

            ParallelProcessPool.AddTask(context.ProcessID, () =>
            {
                using (PrepContextScope(cloneData, out BinaryContext prepMetaData))
                {
                    for (int i = 0; i < _shape[0]; i++)
                    {
                        for (int j = 0; j < _shape[1]; j++)
                        {
                            var value = _elementSerializer.DeserializeElement(partReader, prepMetaData);
                            _collectionHandler.AddItem(collection, value, i, j);
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
