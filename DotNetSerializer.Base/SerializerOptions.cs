using DotNetSerializer.Base.CollectionHandlers;
using DotNetSerializer.Base.Storages;
using System;

namespace DotNetSerializer.Base
{
    /// <summary>
    /// Provides options that control the serialization behavior.
    /// </summary>
    public abstract class SerializerOptions
    {
        // TODO: refactor: replace to process
        ///// <summary></summary>
        //public TypeInfoStorage TypeInfoStorage { get; }

        /// <summary>Gets the default attribute storage.</summary>
        public DefaultAttributeStorage DefaultAttributes { get; }
        /// <summary>Gets the collection handler storage.</summary>
        public CollectionHandlerStorage CollectionHandlers { get; }
        /// <summary>Gets a value indication whether the desrialization stream must be read to end.</summary>
        public bool RequireEnd { get; }

        /// <summary>
        /// Initializes a new options instance with default parameters.
        /// </summary>
        public SerializerOptions()
        {
            DefaultAttributes = new DefaultAttributeStorage();
            CollectionHandlers = new CollectionHandlerStorage();

            RequireEnd = true;

            RegisterCollectionHandlers();
        }

        /// <summary>
        /// Initializes a new options instance by copying parameters from the specified source options.
        /// </summary>
        /// <param name="options">The source options to copy from.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <see langword="null"/>.</exception>
        public SerializerOptions(SerializerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));


            DefaultAttributes = new DefaultAttributeStorage(options.DefaultAttributes);
            CollectionHandlers = new CollectionHandlerStorage(options.CollectionHandlers);

            RequireEnd = options.RequireEnd;
        }

        private void RegisterCollectionHandlers()
        {
            CollectionHandlers.Add<ArrayHandler>();
            CollectionHandlers.Add<ListHandler>();
        }
    }
}
