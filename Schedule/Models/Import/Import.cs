using Newtonsoft.Json;

namespace Schedule.Models.Import
{
    public interface IImport
    {
        public void Execute(int mode, string param);
    }

    public delegate void ImportDelegate<T>(T obj) where T : IImport;

    public class Importer
    {
        public event ImportDelegate<IImport>? OnImport;

        public void ImportObject(IImport importable, int mode, string param)
        {
            OnImport?.Invoke(importable);
            importable.Execute(mode, param);
        }
    }
}
