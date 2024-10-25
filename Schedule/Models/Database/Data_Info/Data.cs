using System.Collections;

namespace Schedule.Models.Database
{
    public class Data : IEquatable<string>
    {
        public string? Discipline { get; set; }
        public string? Teacher_FIO { get; set; }
        public string? Auditorium { get; set; }

        public int[] Rows { get; set; } = new int[3];
        public int Column { get; set; }

        public void Discipline_Set(string value)
        {
            Discipline = value;
        }

        public void Teacher_FIO_Set(string value)
        {
            Teacher_FIO = value;
        }

        public void Auditorium_Set(string value)
        {
            Auditorium = value;
        }

        public bool is_Empty()
        {
            if (Discipline == null) return true;
            else if (Teacher_FIO == null) return true;
            else if (Auditorium == null) return true;
            else return false;
        }

        public override string ToString()
        {
            return $"\t{this.Discipline}\n\t{this.Teacher_FIO}\n\t{this.Auditorium}";
        }

        public bool Equals(string? other)
        {
            if (Teacher_FIO != null && other != null && Teacher_FIO != "")
            {
                string[] tmp_array = Teacher_FIO.Split(" ");

                if (tmp_array.Length == 3 && other.Equals(Teacher_FIO, StringComparison.OrdinalIgnoreCase)) return true;
                else if (other.Trim().Equals(tmp_array[0].Trim(), StringComparison.OrdinalIgnoreCase)) return true;
                else return false;
            }
            else return false;
        }
    }
}
