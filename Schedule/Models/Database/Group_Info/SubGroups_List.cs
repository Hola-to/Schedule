using Schedule.Models.Database;
using System.Collections;
using System.Collections.Generic;

namespace Schedule.Models.Database
{
    public class SubGroups_List: IEnumerable<string>
    {
        public static List<string> SubGroups = new List<string>();

        public string this[int index]
        {
            get { return SubGroups[index]; }
            set { SubGroups[index] = value; }
        }
        public static int GetIndex(string group)
        {
            int index = SubGroups.FindIndex(g => g != null && g.Equals(group, StringComparison.OrdinalIgnoreCase));
            if (index == -1) throw new ArgumentException("Группа не найдена");
            else return index;
        }

        new public IEnumerator<string> GetEnumerator()
        {
            return (IEnumerator<string>)(IEnumerator)SubGroups.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)SubGroups.GetEnumerator();
        }
    }
}
