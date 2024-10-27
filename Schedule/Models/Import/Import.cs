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
    }
}
