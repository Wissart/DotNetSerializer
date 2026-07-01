using DotNetSerializer.Base.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetSerializer.Base.CollectionHandlers
{
    /// <summary>
    /// Class for handling List collection creation and manipulation during processing.
    /// </summary>
    public class ListHandler : CollectionHandler
    {
        /// <summary>Gets the List<> type.</summary>
        public override Type CollectionType => typeof(List<>);

        /// <inheritdoc/>
        public override object CreateCollection(Type[] elementTypes, int[] sizes)
        {
            var collectionType = CollectionType.MakeGenericType(elementTypes[0]);
            return Activator.CreateInstance(collectionType, sizes[0]);
        }

        /// <inheritdoc/>
        public override object CreateCollectionWithItems(Type[] elementTypes, object[] items)
        {
            var collectionType = CollectionType.MakeGenericType(elementTypes[0]);
            var array = Array.CreateInstance(elementTypes[0], items.Length);
            items.CopyTo(array, 0);
            return Activator.CreateInstance(collectionType, array);
        }

        /// <summary>
        /// Creates a collection with populated items.
        /// </summary>
        /// <param name="elementTypes">Type of the collection elements.</param>
        /// <param name="items">Items to populate the collection with.</param>
        /// <param name="sizes">Dimension sizes for the collection.</param>
        /// <returns>The created List<> instance.</returns>
        /// <exception cref="DotNetSerializerException">Thrown when <paramref name="sizes"> lengths > 1.</exception>
        public override object CreateCollectionWithItems(Type[] elementTypes, object items, int[] sizes)
        {
            if (sizes.Length > 1)
                throw new DotNetSerializerException("Dimension error. The List dimension cannot be greater than 1.");

            var collectionType = CollectionType.MakeGenericType(elementTypes[0]);
            var array = Array.CreateInstance(elementTypes[0], sizes[0]);
            Array.Copy((Array)items, array, sizes[0]);
            return Activator.CreateInstance(collectionType, array);
        }

        /// <summary>
        /// Gets the type formed from the specified types.
        /// </summary>
        /// <param name="elementTypes">Type of the collection elements.</param>
        /// <returns>The first type from the types array.</returns>
        public override Type GetElementType(Type[] elementTypes)
        {
            return elementTypes[0];
        }

        /// <summary>
        /// Gets the number of dimensions (rank) of the declared collection type.
        /// </summary>
        /// <param name="declareCollectionType">The declared collection type.</param>
        /// <returns>1</returns>
        public override int GetRank(Type declareCollectionType) => 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public override int[] GetCapacity(object collection)
        {
            var size = (int)collection.GetType().GetProperty("Capacity").GetValue(collection);
            return new int[] { size };
        }

        /// <inheritdoc/>
        public override int[] GetItemsCount(object collection)
        {
            var list = collection as IList;
            return new int[] { list.Count };
        }

        /// <inheritdoc/>
        public override void AddItem(object collection, object item, params int[] indices)
        {
            ((IList)collection).Insert(indices[0], item);
        }
    }
}
