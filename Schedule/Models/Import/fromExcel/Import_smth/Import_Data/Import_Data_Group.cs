using Schedule.Models.Database;

namespace Schedule.Models.Import.fromExcel
{
    public partial class Import_Data : Main_Params
    {
        public static void GetData_Group(string s_group)
        {
            int temp_column = SubGroups_List.GetIndex(s_group);

            for (int column = Settings.DATE_COLUMN + 2; column <= temp_column; column++)
            {
                for (int row = Settings.GROUP_ROW + 3; row < rowCount;)
                {
                    Schedule.Models.Database.Data one_Data = new Schedule.Models.Database.Data();
                    one_Data.Rows = [row, row + 1, row + 2];
                    one_Data.Column = column;
                    string? str_tmp = null;

                    Everything[] smth = new Everything[3]
                    {
                        one_Data.Discipline_Set,
                        one_Data.Teacher_FIO_Set,
                        one_Data.Auditorium_Set
                    };

                    Schedule.Models.Database.Data_List tmp_l = new Schedule.Models.Database.Data_List();

                    if (Worksheet.Cells[row, column].Merge == false)
                    {

                        for (int i = 0; i < 3; i++)
                        {
                            object tmp = null;

                            tmp = Worksheet.Cells[row, column].Value;

                            if (tmp != null && tmp as string != "") str_tmp = tmp.ToString();
                            smth[i](str_tmp);
                            str_tmp = null;
                            row++;
                        }

                        Schedule.Models.Database.Data_List.All_Data.Add(one_Data);
                    }

                    else if (Worksheet.Cells[row, column].Merge == true && tmp_l[row, column] == false)
                    {
                        List<Schedule.Models.Database.Data> datas = new List<Schedule.Models.Database.Data>();

                        object tmp = null;
                        int i_col = 0;

                        while (Worksheet.Cells[row, column + i_col++].Merge == true && column < colCount)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                tmp = Worksheet.Cells[row, column].Value;
                                if (tmp != null && tmp as string != "") str_tmp = tmp.ToString();
                                smth[i](str_tmp);
                                str_tmp = null;
                                row++;
                            }

                            row -= 3;

                            datas.Add(one_Data);

                            if (Worksheet.Cells[row, column + i_col].Value != null
                                && Worksheet.Cells[row, column + i_col].Value as string != "") break;
                        }

                        for (int i = 0; i < datas.Count; i++)
                        {
                            one_Data = new Schedule.Models.Database.Data();
                            one_Data.Discipline = datas[i].Discipline;
                            one_Data.Teacher_FIO = datas[i].Teacher_FIO;
                            one_Data.Auditorium = datas[i].Auditorium;
                            one_Data.Rows = datas[i].Rows;
                            one_Data.Column = datas[i].Column + i;

                            Schedule.Models.Database.Data_List.All_Data.Add(one_Data);
                        }

                        row += 3;
                    }

                    else { row++; }

                    foreach (var date in Schedule.Models.Database.Date_List.All_Date)
                    {
                        if (one_Data.Rows[2] == date.RowEnd)
                        {
                            ++row;
                        }
                    }
                }
            }

            Filter_Group(s_group);
        }

        public static void Filter_Group(string s_group)
        {
            int temp_column = SubGroups_List.GetIndex(s_group);

            for (int i = 0; i < Database.Data_List.All_Data.Count; i++)
            {
                while (Database.Data_List.All_Data[i].Column != temp_column || Database.Data_List.All_Data[i] == null
                    && i < Database.Data_List.All_Data.Count - 1)
                {
                    Database.Data_List.All_Data.RemoveAt(i);
                }
            }
        }
    }
}
