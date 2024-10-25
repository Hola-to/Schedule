using System.Collections;
using System.Collections.Generic;

namespace Schedule.Models.Database
{
    public partial class DayOfWeek : Time
    {
        public DayOfWeek(string obj, int row)
        {
            Convert_Date(obj);
            this.Row = row;

            int counter = 0;

            for (int i = 0; i < base.pairs.Length && base.pairs != null && base.pairs[i] != null; i++)
            {
                for (int j = 0; j < base.pairs[i].Pair_Rows.Length && base.pairs[i] != null; j++)
                {
                    if (i == 0) base.pairs[0].Pair_Rows[j] = 0;
                    else base.pairs[i].Pair_Rows[j] = row + counter++;
                }
            }

            this.RowEnd = base.pairs[base.Count].Pair_Rows[2];

            GetMonth_index();
        }

        public DayOfWeek() { }

        private void Convert_Date(string obj)
        {
            string[] tmp = new string[3];
            tmp = obj.Split(' ', ' ');

            this.Day = Convert.ToInt32(tmp[0]);
            this.Month = tmp[1];
            this.Year = Convert.ToInt32(tmp[2]);
        }

        public override string this[int index]
        {
            get
            {
                if (index > 6) throw new ArgumentOutOfRangeException(nameof(index));
                else return $"{Day} {Month} {Year}";
            }
            set
            {
                if (index > 6) throw new ArgumentOutOfRangeException(nameof(index));
                else
                {
                    var tmp = (Days_Week)index;
                    this.Day_Of_Week = tmp.ToString();
                }
            }
        }

        public string GetFormattedDate()
        {
            return $"{Day} {Month} {Year}";
        }

        public int CountPairs() { return this.pairs.Length; }

        public Time GetTime() 
        {
            if (this.List_Pairs == null) throw new ArgumentNullException("Пары не загрузились");
            return this.List_Pairs;
        }

        public void GetMonth_index()
        {
            if (this.Month != null && this.Month != "" && this.Month.Length >= 3)
            {
                string? tmp = this.Month;
                Month_List some_month = new Month_List();

                tmp = tmp.Substring(0, 3);

                Enum.TryParse(tmp, true, out some_month);

                this.Month_index = (int)some_month;
            }
        }

        public bool is_Null()
        {
            if (this.Day != 0 && this.Month_index != 0) return false;
            else return true;
        }
    }
}
