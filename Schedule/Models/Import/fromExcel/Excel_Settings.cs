namespace Schedule.Models.Import.fromExcel
{
    public readonly struct Excel_Settings
    {
        public readonly int LIST_NUMBER;
        public readonly int DATE_COLUMN;
        public readonly int GROUP_ROW;

        public Excel_Settings()
        {
            this.LIST_NUMBER = 0;
            this.DATE_COLUMN = 2;
            this.GROUP_ROW = 13;
        }

        public Excel_Settings(int list_number, int date_column, int group_row)
        {
            this.LIST_NUMBER = list_number;
            this.DATE_COLUMN = date_column;
            this.GROUP_ROW = group_row;
        }
    }
}
