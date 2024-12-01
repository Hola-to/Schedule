using Newtonsoft.Json;

namespace Schedule.Models.Import
{
    public interface IImport
    {
        public FileInfo? Some_File { get; set; }
        public void Execute(string filePath, int mode, string param);
    }

    public delegate void ImportDelegate<T>(T obj) where T : IImport;

    public class Importer
    {
        public event ImportDelegate<IImport>? OnImport;

        public void ImportObject(IImport importable, string filePath, int mode, string param)
        {
            OnImport?.Invoke(importable);
            importable.Execute(filePath, mode, param);
        }

        public HashSet<string> GetFiles(string filepath)
        {
            try
            {
                var jsonContent = File.ReadAllText(filepath);
                var jsonObject = JsonConvert.DeserializeObject<ImportedFiles>(jsonContent);
                var filesList = jsonObject?.Files ?? new List<string>();
                return new HashSet<string>(filesList);
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                return new HashSet<string>(); // Возвращаем пустой HashSet в случае ошибки
            }
        }
    }

    public class ImportedFiles
    {
        public List<string> Files { get; set; }
    }
}
