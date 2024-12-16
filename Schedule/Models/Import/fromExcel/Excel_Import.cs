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
        public void Execute(int mode, string param)
        {
            if (mode == 0)
                Import_Data.GetData_Group(param);
            else if (mode == 1)
                Import_Data.GetData_Teacher(param);
            else throw new ArgumentOutOfRangeException(nameof(mode));

            new Linker_List();
        }
    }
}
