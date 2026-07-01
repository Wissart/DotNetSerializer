using System;

namespace DotNetSerializer.Base.CollectionHandlers
{
    /// <summary>
    /// Class for handling array collection creation and manipulation during processing.
    /// </summary>
    public class ArrayHandler : CollectionHandler
    {
        /// <summary>Gets the array type.</summary>
        public override Type CollectionType => typeof(Array);

        /// <inheritdoc/>
        public override object CreateCollection(Type[] elementTypes, int[] sizes)
        {
            return Array.CreateInstance(elementTypes[0], sizes);
        }

        /// <inheritdoc/>
        public override object CreateCollectionWithItems(Type[] elementTypes, object[] items)
        {
            var array = Array.CreateInstance(elementTypes[0], items.Length);
            items.CopyTo(array, 0);
            return array;
        }

        /// <inheritdoc/>
        public override object CreateCollectionWithItems(Type[] elementTypes, object items, int[] sizes)
        {
            var array = Array.CreateInstance(elementTypes[0], sizes);
            Array.Copy((Array)items, array, array.Length);
            return array;
        }

        /// <inheritdoc/>
        public override int GetRank(Type declareCollectionType)
        {
            return declareCollectionType.GetArrayRank();
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

        /// <inheritdoc/>
        public override int[] GetCapacity(object collection)
        {
            return GetArrayDimensions(collection);
        }

        /// <inheritdoc/>
        public override int[] GetItemsCount(object collection)
        {
            return GetArrayDimensions(collection);
        }

        /// <inheritdoc/>
        public override void AddItem(object collection, object item, params int[] indices)
        {
            ((Array)collection).SetValue(item, indices);
        }

        private int[] GetArrayDimensions(object collection)
        {
            var rank = GetRank(collection.GetType());
            int[] capacities = new int[rank];

            var array = collection as Array;
            for (int i = 0; i < rank; i++)
            {
                capacities[i] = array.GetLength(i);
            }
            return capacities;
        }
    }
}
