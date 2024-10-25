using Schedule.Models.Database;

namespace Schedule.Models.Import.fromExcel
{
    public class Import_Date : Main_Params
    {
        public static void GetData(object obj)
        {
            if (obj == null) throw new ArgumentNullException("Чёт пусто");

            int column = 0;

            if (obj is int k)
            {
                column = k;
            }
            else throw new ArgumentException("Это не число");

            for (int i = 1; i <= rowCount; i++)
            {
                var cell = Worksheet.Cells[i, column].Value;

                if (cell != null && cell.ToString() != "")
                {
                    Database.DayOfWeek one_Day = new Database.DayOfWeek(cell.ToString(), i);
                    Date_List.All_Date.Add(one_Day);
                }
            }

            Date_List.ToWeek();
        }
    }
}
