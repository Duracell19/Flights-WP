namespace Flights.Infrastructure.Interfaces
{
    public interface IFileStore
    {
        T Load<T>(string fileName);
        void Save(string fileName, object obj);
    }
}
