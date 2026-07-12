using DotNetSerializer.Base.Storages;
using System;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes
{
    internal class SchemeStorage<TProcessScheme> : DictionaryWrapper<Type, TProcessScheme>
        where TProcessScheme : IProcessScheme
    {
        protected readonly TypeInfoStorage _typeInfoStorage;
        protected readonly ISchemeMaker _schemeMaker;

        public SchemeStorage(TypeInfoStorage typeInfoStorage, ISchemeMaker schemeMaker)
        {
            _typeInfoStorage = typeInfoStorage;
            _schemeMaker = schemeMaker;
        }

        public override TProcessScheme Get(Type key)
        {
            if (_storage.ContainsKey(key))
                return _storage[key];

            var typeInfo = _typeInfoStorage.Get(key);
            _storage[key] = (TProcessScheme)Activator.CreateInstance(typeof(TProcessScheme), typeInfo, _schemeMaker);

            return _storage[key];
        }
    }
}
