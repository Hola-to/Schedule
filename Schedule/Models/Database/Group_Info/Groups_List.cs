using System.Collections;

namespace Schedule.Models.Database
{
    public class Groups_List : IEnumerable<string>
    {
        public static List<string> Groups { get; set; } = new List<string>();

        public string this[int index]
        {
            get { return Groups[index]; }
            set { Groups[index] = value; }
        }

        public static int GetIndexGr(string group)
        {
            int index = Groups.FindIndex(g => g != null && g.Equals(group, StringComparison.OrdinalIgnoreCase));
            if (index == -1) throw new ArgumentException("Группа не найдена");
            else return index;
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
