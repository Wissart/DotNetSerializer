using DotNetSerializer.Base.CollectionHandlers;
using DotNetSerializer.Base.Storages;

namespace DotNetSerializer.Base
{
    /// <summary>
    /// Provides options that control the serialization behavior.
    /// </summary>
    public abstract class SerializerOptions
    {
        /// <summary></summary>
        public TypeInfoStorage TypeInfoStorage { get; }

        /// <summary>Gets the default attribute storage.</summary>
        public DefaultAttributeStorage DefaultAttributes { get; }
        /// <summary>Gets the collection handler storage.</summary>
        public CollectionHandlerStorage CollectionHandlers { get; }
        /// <summary>Gets a value indication whether the desrialization stream must be read to end.</summary>
        public bool RequireEnd { get; }

        /// <summary>
        /// Initializes a new options instance and registers default converters.
        /// </summary>
        public SerializerOptions()
        {
            TypeInfoStorage = new TypeInfoStorage();
            DefaultAttributes = new DefaultAttributeStorage();
            CollectionHandlers = new CollectionHandlerStorage();

            RequireEnd = true;

            RegisterCollectionHandlers();
        }

        private void RegisterCollectionHandlers()
        {
            CollectionHandlers.Add<ArrayHandler>();
            CollectionHandlers.Add<ListHandler>();
        }
    }
}
