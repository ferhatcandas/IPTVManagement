using System.Collections.Generic;

namespace DataLayer.Repository
{
    public interface IFileRepository<T>
        where T : class, new()
    {
        void Insert(T entity);
        void Save(List<T> entity);
        List<T> Get();
        void Flush();
        string FullPath();
        void Recover(string jsonText);
    }
}
