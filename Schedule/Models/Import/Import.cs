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
                // Читаем содержимое файла
                var jsonContent = File.ReadAllText(filepath);

                // Десериализуем JSON-данные в список строк
                var filesList = JsonConvert.DeserializeObject<List<string>>(jsonContent);

                // Преобразуем список в HashSet и возвращаем
                return new HashSet<string>(filesList);
            }
            catch (Exception ex)
            {
                // Обработка ошибок (например, файл не найден, ошибки формата JSON и т.д)
                return new HashSet<string>(); // Возвращаем пустой HashSet в случае ошибки
            }
        }
    }
}
