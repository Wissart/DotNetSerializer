using DotNetSerializer.Base;
using DotNetSerializer.Base.Infrastructure;
using System;

namespace DotNetSerializer.Binary
{
    /// <summary>
    /// The class container for type and processing information.
    /// </summary>
    public class BinaryContext  : SerializationContext<BinaryContext>
    {
        /// <summary>Gets the value that specifies which serialization process owns the context.</summary>
        internal int ProcessID { get; }


        internal BinaryContext(BinaryContext prev) : base(prev)
        {
            if (prev != null)
                ProcessID = prev.ProcessID;
        }

        internal BinaryContext(BinaryContext prev, int processID) : base(prev)
        {
            ProcessID = processID;
        }

        internal BinaryContext(BinaryContext prev, object obj) : this(prev)
        {
            CreateMetaData(obj);
        }


        internal IDisposable CreateMetaDataScope(object obj)
        {
            CreateMetaData(obj);
            return DisposableScope.Create(() =>
            {
                RemoveMetaData();
            });
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SerialiationMetaData"/> class.
        /// </summary>
        /// <param name="obj">The object for which the meta data is created.</param>
        internal void CreateMetaData(object obj)
        {
            MetaData = new SerialiationMetaData(MetaData)
            {
                Object = obj,
            };
        }

        /// <summary>Removes the last meta data in the object context chain.</summary>
        internal void RemoveMetaData()
        {
            MetaData = MetaData.Prev;
        }

        internal BinaryContext Clone()
        {
            return new BinaryContext(this.Prev, ProcessID)
            {
                Version = this.Version,
                MetaData = this.MetaData.Clone(),
                _transientValues = new System.Collections.Generic.Dictionary<string, object>(_transientValues),
            };
        }
    }
}
