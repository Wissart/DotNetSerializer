using DotNetSerializer.Base.Storages;
using System;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes
{
    internal class SchemeStorage<TProcessScheme> : DictionaryWrapper<Type, TProcessScheme>
        where TProcessScheme : IProcessScheme
    {
        protected readonly BinaryOptions _options;
        protected readonly ISchemeMaker _schemeMaker;

        public SchemeStorage(BinaryOptions options, ISchemeMaker schemeMaker)
        {
            _options = options;
            _schemeMaker = schemeMaker;
        }

        public override TProcessScheme Get(Type key)
        {
            if (_storage.ContainsKey(key))
                return _storage[key];

            var typeInfo = _options.TypeInfoStorage.Get(key);
            _storage[key] = (TProcessScheme)Activator.CreateInstance(typeof(TProcessScheme), typeInfo, _schemeMaker);

            return _storage[key];
        }
    }
}
