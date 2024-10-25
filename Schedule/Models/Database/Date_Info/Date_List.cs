using System.Collections;

namespace Schedule.Models.Database
{
    public class Date_List : DayOfWeek, IEnumerable<DayOfWeek>
    {
        public static List<DayOfWeek> All_Date { get; set; } = new List<DayOfWeek>();
        
        public static void ToWeek()
        {
            for (int index = 0; index < All_Date.Count; index++)
            {
                if (index > All_Date.Count) throw new ArgumentOutOfRangeException(nameof(index));
                else
                {
                    var tmp = (Days_Week)index;
                    All_Date[index].Day_Of_Week = tmp.ToString();
                }
            }
            
        }

        public IEnumerator<DayOfWeek> GetEnumerator()
        {
            return ((IEnumerable<DayOfWeek>)All_Date).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)All_Date).GetEnumerator();
        }
    }
}
