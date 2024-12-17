using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Schedule.Models
{
    public class JsonDataService
    {
        private readonly string _JsonFilePath;

        public JsonDataService(string JsonFilePath)
        {
            _JsonFilePath = JsonFilePath;
        }

        public List<string> SearchTeachers(string query)
        {
            var teachers = LoadJson<List<string>>(_JsonFilePath, "Teachers");
            if (teachers == null) return new List<string>();

            return teachers
                .Where(t => t.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<string> GetSubGroups()
        {
            var subGroups = LoadJson<List<string>>(_JsonFilePath, "SubGroups");
            if (subGroups == null) return new List<string>();

            return subGroups;
        }

        public List<string> GetTeachers()
        {
            var teachers = LoadJson<List<string>>(_JsonFilePath, "Teachers");
            if (teachers == null) return new List<string>();

            return teachers;
        }

        private T LoadJson<T>(string filePath, string key)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var jObject = JObject.Parse(json);

                if (jObject.ContainsKey(key))
                {
                    return jObject[key].ToObject<T>();
                }
            }
            return default;
        }
    }

    public class Teacher
    {
        public string TeacherFIO { get; set; }
    }

    public class SubGroup
    {
        public string SubGroupName { get; set; }
    }
}