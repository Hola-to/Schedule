using Schedule.Models.Database;

namespace Schedule.Models.Import.fromExcel
{
    public partial class Import_Data : Main_Params
    {
        public static void GetData_Teacher(string FIO)
        {
            GetData();

            Filter_Teacher(FIO);
        }

        public static void Filter_Teacher(string FIO)
        {
            Data_List.All_Data.RemoveAll(item => item.Teacher_FIO == null || !item.Equals(FIO));

            Data_List.All_Data.ForEach(item => item.Teacher_FIO = SubGroups_List.SubGroups[item.Column]);
        }
    }
}
