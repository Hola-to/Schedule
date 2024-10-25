using System.Collections;

namespace Schedule.Models.Database
{
    public class Groups_List : IEnumerable<string>
    {
        public static List<string> Groups { get; set; } = new List<string>();

        public virtual string this[int index]
        {
            get { return Groups[index]; }
            set { Groups[index] = value; }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return (IEnumerator<string>)(IEnumerator)Groups.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Groups.GetEnumerator();
        }
    }
}
