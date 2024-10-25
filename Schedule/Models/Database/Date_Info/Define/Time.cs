namespace Schedule.Models.Database
{
    public partial class Time
    {
        public Time()
        {
            this.pairs[0] = new Pair("06:25-07:50");
            this.pairs[1] = new Pair("08:00-09:25");
            this.pairs[2] = new Pair("09:35-11:00");
            this.pairs[3] = new Pair("11:30-12:55");
            this.pairs[4] = new Pair("13:05-14:30");
            this.pairs[5] = new Pair("14:40-16:05");
            this.pairs[6] = new Pair("16:35-18:00");
            this.pairs[7] = new Pair("18:10-19:35");
            this.pairs[8] = new Pair("19:45-21:10");
        }

        public virtual string this[int index]
        {
            get {
                return $"{pairs[index]["start_hour"]}:{pairs[index]["start_minute"]}-{pairs[index]["end_hour"]}:{pairs[index]["end_minute"]}"; 
            }
            set { this.pairs[index] = new Pair(value); }
        }

        
    }
}
