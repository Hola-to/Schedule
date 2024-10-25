using System.Collections;

namespace Schedule.Models.Database
{
    public class Data_List : IEnumerable<Data>
    {
        public static List<Data> All_Data { get; set; } = new List<Data>();

        public bool this[int row, int column]
        {
            get
            {
                foreach (Data data in All_Data)
                {
                    if (data.Rows.Contains(row) && data.Column == column) return true;
                }
                return false;
            }
        }

        public IEnumerator<Data> GetEnumerator()
        {
            return ((IEnumerable<Data>)All_Data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)All_Data).GetEnumerator();
        }
    }
}
