namespace Schedule.Models.Export
{
    public interface IExport
    {
        public void Execute(string obj);
    }

    public delegate void ExportDelegate<T>(T obj) where T : IExport;

    public class Exporter
    {
        public event ExportDelegate<IExport>? OnExport;

        public void ExportObject(IExport exportable)
        {
            OnExport?.Invoke(exportable);
            exportable.Execute("Расписание ВГУ");
        }
    }
}
