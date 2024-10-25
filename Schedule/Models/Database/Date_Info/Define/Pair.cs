namespace Schedule.Models.Database
{
    public partial class Pair
    {
        public int this[string per]
        {
            get
            {
                if (per == "start_hour") return this.start_hour;
                else if (per == "start_minute") return this.start_minute;
                else if (per == "end_hour") return this.end_hour;
                else if (per == "end_minute") return this.end_minute;
                else throw new ArgumentException("Аргумента не существует");
            }
            set { }
        }

        public Pair() { }

        public Pair(string per)
        {
            Convert_toInt(per);
        }

        private void Convert_toInt(string str)
        {
            string[] temp = new string[4];
            temp = str.Split(':', '-', ':');

            if (temp.Length > 4) throw new OutOfMemoryException("Недопустимое число");

            else
            {
                this.start_hour = int.Parse(temp[0]);
                this.start_minute = int.Parse(temp[1]);
                this.end_hour = int.Parse(temp[2]);
                this.end_minute = int.Parse(temp[3]);
            }
        }

        public string? Get_Pair()
        {
            return $"{start_hour}:{start_minute}-{end_hour}:{end_minute}";
        }
    }
}
