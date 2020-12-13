using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DataLayer.Repository
{
    public abstract class FileRepository<T> : IFileRepository<T>
        where T : class, new()
    {

        private static readonly string folderPath = Directory.GetCurrentDirectory() + @"\";
        private string fileName = "";
        public FileRepository()
        {
            fileName = $"{typeof(T).Name}.json";
            CheckIfNotExistsCreateFile();
        }
        public FileRepository(string name)
        {
            fileName = name + ".json";
            CheckIfNotExistsCreateFile();
        }
        public void Save(List<T> entity)
        {
            string data = JsonConvert.SerializeObject(entity, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            Write(data);
        }
        private void Write(string data)
        {
            File.WriteAllText(FullPath(), data);
        }
        public List<T> Get()
        {
            string content = GetFileContent();
            if (string.IsNullOrEmpty(content))
            {
                return new List<T>();
            }

            return JsonConvert.DeserializeObject<List<T>>(content);
        }
        private void CheckIfNotExistsCreateFile()
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (!File.Exists(FullPath()))
            {
                using (File.Create(FullPath()))
                {
                    ;
                }
            }
        }
        private string GetFileContent()
        {
            return File.ReadAllText(FullPath());
        }
        public string FullPath()
        {
            return folderPath + fileName;
        }

        public void Insert(T entity)
        {
            List<T> exist = Get();
            exist.Add(entity);
            Save(exist);
        }
        public void Recover(string jsonText)
        {
            Write(jsonText);
        }
        public void Flush()
        {
            File.Delete(FullPath());
            CheckIfNotExistsCreateFile();
        }
    }
}
