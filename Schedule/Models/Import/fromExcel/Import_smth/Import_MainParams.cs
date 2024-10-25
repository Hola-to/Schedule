using OfficeOpenXml;

namespace Schedule.Models.Import.fromExcel
{
    public abstract class Main_Params
    {
        protected static ExcelWorksheet? Worksheet { get; set; }

        protected static int rowCount;
        protected static int colCount;
        public static Excel_Settings Settings { get; set; }
    }

    public class GetFromExcel : Main_Params
    {
        public GetFromExcel(ExcelPackage package, Excel_Settings settings)
        {
            Settings = settings;

            Worksheet = package.Workbook.Worksheets[settings.LIST_NUMBER];

            rowCount = Worksheet.Dimension.Rows;
            colCount = Worksheet.Dimension.Columns;
        }
    }
}
