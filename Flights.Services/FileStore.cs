using Flights.Infrastructure.Interfaces;
using MvvmCross.Plugins.File;

namespace Flights.Services
{
    public class FileStore : IFileStore
    {
        private readonly IMvxFileStore _fileStore;
        private readonly IJsonConverter _jsonConverter;

        public FileStore(IMvxFileStore fileStore, IJsonConverter jsonConverter)
        {
            _fileStore = fileStore;
            _jsonConverter = jsonConverter;
        }

        public T Load<T>(string fileName)
        {
            string txt;
            T result = default(T);
            if (_fileStore.TryReadTextFile(fileName, out txt))
            {
                return _jsonConverter.Deserialize<T>(txt);
            }
            return result;
        }

        public void Save(string fileName, object obj)
        {
            _fileStore.WriteFile(fileName, _jsonConverter.Serialize(obj));
        }
    }
}
