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
            CreateObjectContext(obj);
        }


        internal IDisposable NewObjectContextScope(object obj)
        {
            CreateObjectContext(obj);
            return DisposableScope.Create(() =>
            {
                RemoveObjectContext();
            });
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SerialiationObjectContext"/> class.
        /// </summary>
        /// <param name="obj">The object for which the context is created.</param>
        internal void CreateObjectContext(object obj)
        {
            ObjectContext = new SerialiationObjectContext(ObjectContext)
            {
                Object = obj,
            };
        }

        /// <summary>Removes the last context in the  object context chain. </summary>
        internal void RemoveObjectContext()
        {
            ObjectContext = ObjectContext.Prev;
        }

        internal BinaryContext Clone()
        {
            return new BinaryContext(this.Prev, ProcessID)
            {
                Version = this.Version,
                ObjectContext = this.ObjectContext,
                _transientValues = this._transientValues
            };
        }
    }
}
