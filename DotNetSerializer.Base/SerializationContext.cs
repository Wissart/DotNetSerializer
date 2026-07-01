using DotNetSerializer.Base.Exceptions;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetSerializer.Base
{
    /// <summary>
    /// Represents serialization context data for processing the object and it's property.
    /// </summary>
    public class SerialiationObjectContext
    {
        /// <summary>Gets the prvious data in the chain.</summary>
        public SerialiationObjectContext Prev { get; }
        /// <summary>Gets or sets the processing object.</summary>
        public object Object { get; set; }
        /// <summary>Gets or sets the processing property.</summary>
        public PropertyInfo Property { get; set; }

        /// <summary>
        /// Initializes a new instance with specified previous data.
        /// </summary>
        /// <param name="prev">Previous data in the chain.</param>
        public SerialiationObjectContext(SerialiationObjectContext prev)
        {
            Prev = prev;
        }

        /// <summary>
        /// Gets the current value of the property.
        /// </summary>
        /// <returns>The property value from the processed object.</returns>
        public object GetValue()
        {
            return Property.GetValue(Object);
        }

        /// <summary>
        /// Sets the value of the property in the processed object.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(object value)
        {
            Property.SetValue(Object, value);
        }
    }


    /// <summary>
    /// Base context for serialization operations.
    /// </summary>
    public abstract class SerializationContext
    {
        protected Dictionary<string, object> _transientValues;

        /// <summary>Gets or sets the version of the serialization objects.</summary>
        public uint Version { get; set; }
        /// <summary>Gets read-only access to transient values.</summary>
        public IReadOnlyDictionary<string, object> TransientValues => _transientValues;
        /// <summary>Gets or sets the current object context.</summary>
        public SerialiationObjectContext ObjectContext { get; set; }

        /// <summary>
        /// Initializes a new instance, optionally copying from previous context.
        /// </summary>
        /// <param name="prevMetaData">Previous context to copy from, or null.</param>
        public SerializationContext(SerializationContext prevMetaData)
        {
            if (prevMetaData != null)
            {
                ObjectContext = prevMetaData.ObjectContext;
                _transientValues = prevMetaData._transientValues;
            }
            else
            {
                ObjectContext = null;
                _transientValues = new Dictionary<string, object>();
            }
        }

        /// <summary>
        /// Adds a transient value. Throws if key already exists.
        /// </summary>
        /// <param name="name">Key name.</param>
        /// <param name="value">Value to store.</param>
        public void AddTransientValue(string name, object value)
        {
            _transientValues.Add(name, value);
        }

        /// <summary>
        /// Sets or replace a transient value.
        /// </summary>
        /// <param name="name">Key name.</param>
        /// <param name="value">Value to store.</param>
        public void SetTransientValue(string name, object value)
        {
            _transientValues[name] = value;
        }

        /// <summary>
        /// Gets a transient value by key.
        /// </summary>
        /// <typeparam name="TValue">Expected type of the value.</typeparam>
        /// <param name="name">Key name.</param>
        /// <returns>The stored value cast to TValue.</returns>
        /// <exception cref="TransientValueNotFoundException">Thrown when key doesn't exist.</exception>
        public TValue GetTransientValue<TValue>(string name)
        {
            if (!_transientValues.ContainsKey(name))
                throw new TransientValueNotFoundException(name);

            return (TValue)_transientValues[name];
        }
    }

    /// <summary>
    /// Generic serialization context with type-safe previous context chaining.
    /// </summary>
    /// <typeparam name="T">The concrete context type derived from this case.</typeparam>
    public abstract class SerializationContext<T> : SerializationContext where T : SerializationContext<T>
    {
        /// <summary>Gets the previous context in the chain.</summary>
        public T Prev { get; private set; }


        /// <summary>
        /// Initializes a new context with the specified previous context.
        /// </summary>
        /// <param name="prev">The previous context in the chain.</param>
        public SerializationContext(T prev) : base(prev)
        {
            Prev = prev;
        }
    }
}
