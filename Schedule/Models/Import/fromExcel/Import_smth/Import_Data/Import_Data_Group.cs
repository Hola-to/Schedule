using Schedule.Models.Database;

namespace Schedule.Models.Import.fromExcel
{
    public partial class Import_Data : Main_Params
    {
        public static void GetData_Group(string s_group)
        {
            Filter_Group(s_group);
        }

        public static void Filter_Group(string s_group)
        {
            int temp_column = SubGroups_List.GetIndex(s_group);

            for (int i = Database.Data_List.All_Data.Count - 1; i >= 0; i--)
            {
                if (Database.Data_List.All_Data[i] == null || Database.Data_List.All_Data[i].Column != temp_column)
                {
                    Database.Data_List.All_Data.RemoveAt(i);
                }
            }
        }
    }
}
