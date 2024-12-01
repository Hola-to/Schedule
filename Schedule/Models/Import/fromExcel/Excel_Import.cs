using Newtonsoft.Json;
using OfficeOpenXml;
using Schedule.Models.Database;
using Schedule.Models.Import;
using System.Threading;
using System.Collections.Generic;

namespace Schedule.Models.Import.fromExcel
{
    public class Import_all_data : IImport
    {
        public FileInfo? Some_File { get; set; }
        public void Execute(string filePath, int mode, string param)
        {
            if (!File.Exists(filePath)) throw new Exception("Файл не найден");

            Some_File = new FileInfo(filePath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(Some_File))
            {
                string SettingsPath = "Models\\Import\\fromExcel\\Excel_Settings.json";

                try
                {
                    string json = File.ReadAllText(SettingsPath);

                    Dictionary<string, object>? data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                    int listNumber = Convert.ToInt32(data["LIST_NUMBER"]);
                    int dateColumn = Convert.ToInt32(data["DATE_COLUMN"]);
                    int groupRow = Convert.ToInt32(data["GROUP_ROW"]);

                    Excel_Settings settings = new Excel_Settings(listNumber, dateColumn, groupRow);
                    new GetFromExcel(package, settings);
                }
                catch (Exception ex) { throw new Exception("Ошибка при чтении файла ", ex); }

                Thread imp_groups = new Thread(Import_Groups.GetData);
                imp_groups.Start(Main_Params.Settings.GROUP_ROW);

                Thread imp_date = new Thread(Import_Date.GetData);
                imp_date.Start(Main_Params.Settings.DATE_COLUMN);

                imp_groups.Join();
                imp_date.Join();


                if (mode == 0)
                    Import_Data.GetData_Group(param);
                else if (mode == 1)
                    Import_Data.GetData_Teacher(param);
                else throw new ArgumentOutOfRangeException(nameof(mode));

                new Linker_List();

            }
        }
    }
}
