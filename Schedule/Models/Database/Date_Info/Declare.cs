namespace Schedule.Models.Database
{
    public partial class Pair
    {
        public int start_hour;
        public int start_minute;
        public int end_hour;
        public int end_minute;

        public int[] Pair_Rows { get; set; } = new int[3]; 
    }


    public partial class Time
    {
        public Pair[] pairs = new Pair[10];

        public readonly int Count = 8;
    }


    public partial class DayOfWeek : Time
    {
        private Time? List_Pairs { get; set; } = new Time();

        public int Day { get; set; }
        public string? Month { get; set; }
        public int Year { get; set; }

        public int Row { get; set; }
        public int RowEnd { get; set; }
        public string? Day_Of_Week { get; set; }

        public int Month_index { get; set; }

        public enum Days_Week
        {
            Понедельник = 0, 
            Вторник = 1, 
            Среда = 2, 
            Четверг = 3, 
            Пятница = 4, 
            Суббота = 5, 
            Воскресенье = 6
        }

        public enum Month_List
        {
            Янв = 1,
            Фев = 2,
            Мар = 3,
            Апр = 4,
            Май = 5,
            Мая = 5,
            Маи = 5,
            Мае = 5,
            Маю = 5,
            Июн = 6,
            Июл = 7,
            Авг = 8,
            Сен = 9,
            Окт = 10,
            Ноя = 11,
            Дек = 12
        }
    }
}
