using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Schedule.Models.Database;
using System;
using System.Collections.Generic;
using System.IO;

namespace Schedule.Models.Import.fromExcel
{
    public class Import_Groups : Main_Params
    {
        public static void GetData(object obj, string JsonFilePath)
        {
            if (obj == null) throw new ArgumentNullException("Чёт пусто");

            int row = 0;

            if (obj is int k)
            {
                row = k;
            }
            else throw new ArgumentException("Это не число");

            Groups_List.Groups.Add(null);

            for (int i = 1; i < colCount; i++)
            {
                var cell = Worksheet.Cells[row, i].Value;

                string? tmp = null;
                if (cell != null) tmp = cell.ToString();

                Groups_List.Groups.Add(tmp);

                // Добавляем группы в JSON-файл
                if (!string.IsNullOrEmpty(tmp))
                {
                    AddGroupsToJson(tmp, JsonFilePath);
                }
            }

            SubGroups_List.SubGroups.Add(null);

            for (int i = 1; i < colCount; i++)
            {
                var cell = Worksheet.Cells[row + 1, i].Value;

                string? tmp = null;
                if (cell != null) tmp = cell.ToString();

                SubGroups_List.SubGroups.Add(tmp);

                // Добавляем подгруппы в JSON-файл только если они уникальны
                if (!string.IsNullOrEmpty(tmp))
                {
                    string baseGroupName = GetBaseGroupName(tmp);
                    AddSubGroupsToJson(tmp, baseGroupName, JsonFilePath);
                }
            }
        }

        private static void AddGroupsToJson(string groupName, string JsonFilePath)
        {
            // Проверяем, существует ли файл
            if (File.Exists(JsonFilePath))
            {
                // Читаем существующие данные из файла
                var existingJson = File.ReadAllText(JsonFilePath);
                var existingData = JObject.Parse(existingJson);

                // Проверяем, есть ли уже ключ "Groups"
                if (existingData.ContainsKey("Groups"))
                {
                    // Если ключ "Groups" существует, объединяем данные
                    var existingGroups = existingData["Groups"].ToObject<HashSet<string>>();
                    existingGroups.Add(groupName); // Добавляем только уникальные значения
                    existingData["Groups"] = JArray.FromObject(existingGroups.ToList()); // Обновляем данные
                }
                else
                {
                    // Если ключа "Groups" нет, добавляем его
                    existingData["Groups"] = JArray.FromObject(new List<string> { groupName });
                }

                // Сериализуем обновленные данные
                var updatedJson = existingData.ToString(Formatting.Indented);
                File.WriteAllText(JsonFilePath, updatedJson);
            }
            else
            {
                // Если файл не существует, создаем новый файл с новыми данными
                var newData = new
                {
                    Groups = new List<string> { groupName }
                };
                var json = JsonConvert.SerializeObject(newData, Formatting.Indented);
                File.WriteAllText(JsonFilePath, json);
            }
        }

        private static void AddSubGroupsToJson(string subGroupName, string baseGroupName, string JsonFilePath)
        {
            // Проверяем, существует ли файл
            if (File.Exists(JsonFilePath))
            {
                // Читаем существующие данные из файла
                var existingJson = File.ReadAllText(JsonFilePath);
                var existingData = JObject.Parse(existingJson);

                // Проверяем, есть ли уже ключ "SubGroups"
                if (existingData.ContainsKey("SubGroups"))
                {
                    // Если ключ "SubGroups" существует, объединяем данные
                    var existingSubGroups = existingData["SubGroups"].ToObject<HashSet<string>>();
                    existingSubGroups.Add(subGroupName); // Добавляем только уникальные значения
                    existingData["SubGroups"] = JArray.FromObject(existingSubGroups.ToList()); // Обновляем данные
                }
                else
                {
                    // Если ключа "SubGroups" нет, добавляем его
                    existingData["SubGroups"] = JArray.FromObject(new List<string> { subGroupName });
                }

                // Сериализуем обновленные данные
                var updatedJson = existingData.ToString(Formatting.Indented);
                File.WriteAllText(JsonFilePath, updatedJson);
            }
            else
            {
                // Если файл не существует, создаем новый файл с новыми данными
                var newData = new
                {
                    SubGroups = new List<string> { subGroupName }
                };
                var json = JsonConvert.SerializeObject(newData, Formatting.Indented);
                File.WriteAllText(JsonFilePath, json);
            }
        }

        private static string GetBaseGroupName(string subGroupName)
        {
            // Удаляем последнее вхождение "_цифра"
            return System.Text.RegularExpressions.Regex.Replace(subGroupName, @"_\d+$", "");
        }
    }

    public class Group
    {
        public string GroupName { get; set; }
    }

    public class SubGroup
    {
        public string SubGroupName { get; set; }
        public string BaseGroupName { get; set; }
    }
}