using System;

namespace DotNetSerializer.Base.CollectionHandlers
{
    /// <summary>
    /// Base class for handling collection creation and manipulation during processing.
    /// </summary>
    public interface ICollectionHandler
    {
        /// <summary>Gets the type of collection this handler supports.</summary>
        Type CollectionType { get; }

        /// <summary>
        /// Creates a collection with populated items.
        /// </summary>
        /// <param name="elementTypes">Type of the collection elements.</param>
        /// <param name="items">Items to populate the collection with.</param>
        /// <returns>The created collection instance.</returns>
        object CreateCollectionWithItems(Type[] elementTypes, object[] items);
        
        /// <summary>
        /// Create a multidimensional collection with populated items.
        /// </summary>
        /// <param name="elementTypes">Type of the collection elements.</param>
        /// <param name="items">Items to populate the collection with.</param>
        /// <param name="sizes">Dimension sizes for the collection.</param>
        /// <returns>The created collection instance.</returns>
        object CreateCollectionWithItems(Type[] elementTypes, object items, int[] sizes);
        
        /// <summary>
        /// Creates an empty collection with specified dimensions.
        /// </summary>
        /// <param name="elementTypes">Type of the collection elements.</param>
        /// <param name="sizes">Dimension sizes fore the collection.</param>
        /// <returns>The created collection instance.</returns>
        object CreateCollection(Type[] elementTypes, int[] sizes);
        
        /// <summary>
        /// Gets the number of dimensions (rank) of the declared collection type.
        /// </summary>
        /// <param name="declareCollectionType">The declared collection type.</param>
        /// <returns>The rank (dimmensions count).</returns>
        int GetRank(Type declareCollectionType);

        /// <summary>
        /// Gets the type formed from the specified types.
        /// </summary>
        /// <param name="elementTypes">Type of the collection elements.</param>
        /// <returns>The formed type.</returns>
        Type GetElementType(Type[] elementTypes);
        
        /// <summary>
        /// Gets the capacity of the specified collection.
        /// </summary>
        /// <param name="collection">The collection instance.</param>
        /// <returns>Array of capacities values per dimensions.</returns>
        int[] GetCapacity(object collection);
        
        /// <summary>
        /// Gets the item count on of the specified collection.
        /// </summary>
        /// <param name="collection">The collection instance.</param>
        /// <returns>Array of item counts per dimension.</returns>
        int[] GetItemsCount(object collection);
        
        /// <summary>
        /// Adds an item to the collection at the specified indices. 
        /// </summary>
        /// <param name="collection">The collection instance.</param>
        /// <param name="item">The item to add.</param>
        /// <param name="indices">Indices indicating where to place the item.</param>
        void AddItem(object collection, object item, params int[] indices);
    }
}
