namespace Schedule.Models.Import
{
    public interface SImport
    {
        public FileInfo? Some_File { get; set; }
        public void Execute(string filePath, int mode, string param);
    }
}
