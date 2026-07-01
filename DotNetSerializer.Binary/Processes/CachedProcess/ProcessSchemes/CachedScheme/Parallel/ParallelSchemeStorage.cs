using System;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel
{
    internal class ParallelSchemeStorage : SchemeStorage<ParallelProcessScheme>
    {
        private readonly SchemeStorage<CachedProcessScheme> _cachedSchemeStorage;

        public ParallelSchemeStorage(BinaryOptions options, 
            ISchemeMaker schemeMaker,
            SchemeStorage<CachedProcessScheme> cachedSchemeStorage) 
            : base(options, schemeMaker)
        {
            _cachedSchemeStorage = cachedSchemeStorage;
        }

        public override ParallelProcessScheme Get(Type key)
        {
            if (_storage.ContainsKey(key))
                return _storage[key];

            var cachedSheme = _cachedSchemeStorage.Get(key);
            _storage[key] = new ParallelProcessScheme(cachedSheme, _schemeMaker);

            return _storage[key];
        }
    }
}
