using Schedule.Models.Database;
using System.Collections;
using System.Collections.Generic;

namespace Schedule.Models.Database
{
    public class SubGroups_List : Groups_List, IEnumerable<string>
    {
        public static List<string> SubGroups = new List<string>();

        public override string this[int index]
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

        public SubGroups_List()
        {
            for (int i = 0; i < Groups_List.Groups.Count; i++)
            {
                var group = Groups_List.Groups[i];
                string? past = null;

                if (i != 0)
                {
                    past = Groups_List.Groups[i - 1];
                }

                if (group != null && group != "") SubGroups.Add(group + "_1");
                else if (group == null || group == "" && past != null && past != "") SubGroups.Add(past + "_2");

                else SubGroups.Add("");
            }
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
