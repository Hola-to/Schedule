using Schedule.Models.Database;

namespace Schedule.Models.Import.fromExcel
{
    public class Import_Groups : Main_Params
    {
        public static void GetData(object obj)
        {
            if (obj == null) throw new ArgumentNullException("Чёт пусто");

            int row = 0;

            if (obj is int k)
            {
                row = k;
            }
            else throw new ArgumentException("Это не число");

            Groups_List.Groups.Add(null);

            for (int i = 1; i < colCount; i++)
            {
                var cell = Worksheet.Cells[row, i].Value;

                string? tmp = null;
                if (cell != null) tmp = cell.ToString();

                Groups_List.Groups.Add(tmp);
            }

            new SubGroups_List();
        }
    }
}
