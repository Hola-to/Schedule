using System.Collections;

namespace Schedule.Models.Database
{
    public class Linker : IEnumerable<LinkPairsAndData>
    {
        public DayOfWeek? Date { get; set; }
    
        public List<LinkPairsAndData> DataAndPair { get; set; } = new List<LinkPairsAndData>();

        public IEnumerator<LinkPairsAndData> GetEnumerator()
        {
            return ((IEnumerable<LinkPairsAndData>)DataAndPair).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)DataAndPair).GetEnumerator();
        }
    }

    public class LinkPairsAndData : IEnumerable<Data>
    {
        public Pair One_Pair { get; set; } = new Pair();

        public List<Data> ManyData { get; set; } = new List<Data>();

        public IEnumerator<Data> GetEnumerator()
        {
            return ((IEnumerable<Data>)ManyData).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)ManyData).GetEnumerator();
        }
    }
}
