using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using Schedule.Models.Database;
using System;
using System.Collections.Generic;
using System.IO;

namespace Schedule.Models.Import.fromExcel
{
    public partial class Import_Data : Main_Params
    {
        public static FileInfo? Some_File { get; set; }

        private delegate void Everything(string value);

        public static void GetData(string filePath, string JsonFilePath)
        {
            var setting = GetMainInfo(filePath, JsonFilePath);

            using (var package = new ExcelPackage(Some_File))
            {
                new GetFromExcel(package, setting);

                var teachers = new HashSet<string>(); // Используем HashSet для уникальных преподавателей

                for (int column = Settings.DATE_COLUMN + 2; column <= colCount; column++)
                {
                    for (int row = Settings.GROUP_ROW + 3; row < rowCount;)
                    {
                        Schedule.Models.Database.Data one_Data = new Schedule.Models.Database.Data();
                        one_Data.Rows = new[] { row, row + 1, row + 2 };
                        one_Data.Column = column;
                        string? str_tmp = null;

                        Everything[] smth = new Everything[3]
                        {
                            one_Data.Discipline_Set,
                            one_Data.Teacher_FIO_Set,
                            one_Data.Auditorium_Set
                        };

                        Schedule.Models.Database.Data_List tmp_l = new Schedule.Models.Database.Data_List();

                        if (Worksheet.Cells[row, column].Merge == false)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                object tmp = null;

                                tmp = Worksheet.Cells[row, column].Value;

                                if (tmp != null && tmp as string != "") str_tmp = tmp.ToString();
                                smth[i](str_tmp);
                                str_tmp = null;
                                row++;
                            }

                            Schedule.Models.Database.Data_List.All_Data.Add(one_Data);
                        }
                        else if (Worksheet.Cells[row, column].Merge == true && tmp_l[row, column] == false)
                        {
                            List<Schedule.Models.Database.Data> datas = new List<Schedule.Models.Database.Data>();

                            object tmp = null;
                            int i_col = 0;

                            while (Worksheet.Cells[row, column + i_col++].Merge == true && column < colCount)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    tmp = Worksheet.Cells[row, column].Value;
                                    if (tmp != null && tmp as string != "") str_tmp = tmp.ToString();
                                    smth[i](str_tmp);
                                    str_tmp = null;
                                    row++;
                                }

                                row -= 3;

                                datas.Add(one_Data);

                                if (Worksheet.Cells[row, column + i_col].Value != null
                                    && Worksheet.Cells[row, column + i_col].Value as string != "") break;
                            }

                            for (int i = 0; i < datas.Count; i++)
                            {
                                one_Data = new Schedule.Models.Database.Data();
                                one_Data.Discipline = datas[i].Discipline;
                                one_Data.Teacher_FIO = datas[i].Teacher_FIO;
                                one_Data.Auditorium = datas[i].Auditorium;
                                one_Data.Rows = datas[i].Rows;
                                one_Data.Column = datas[i].Column + i;

                                Schedule.Models.Database.Data_List.All_Data.Add(one_Data);
                            }

                            row += 3;
                        }
                        else { row++; }

                        foreach (var date in Schedule.Models.Database.Date_List.All_Date)
                        {
                            if (one_Data.Rows[2] == date.RowEnd)
                            {
                                ++row;
                            }
                        }

                        // Добавляем преподавателей в HashSet для уникальных значений
                        if (!string.IsNullOrEmpty(one_Data.Teacher_FIO))
                        {
                            teachers.Add(one_Data.Teacher_FIO);
                        }
                    }
                }

                // Сохраняем данные в JSON-файл
                SaveTeachersToJson(teachers, JsonFilePath);
            }
        }

        private static void SaveTeachersToJson(HashSet<string> teachers, string JsonFilePath)
        {
            // Создаем объект для новых данных
            var newData = new
            {
                Teachers = teachers.ToList() // Обернуть список преподавателей в объект с ключом "Teachers"
            };

            // Проверяем, существует ли файл
            if (File.Exists(JsonFilePath))
            {
                // Читаем существующие данные из файла
                var existingJson = File.ReadAllText(JsonFilePath);
                var existingData = JObject.Parse(existingJson);

                // Проверяем, есть ли уже ключ "Teachers"
                if (existingData.ContainsKey("Teachers"))
                {
                    // Если ключ "Teachers" существует, объединяем данные
                    var existingTeachers = existingData["Teachers"].ToObject<HashSet<string>>();
                    existingTeachers.UnionWith(teachers); // Объединяем уникальные значения
                    existingData["Teachers"] = JArray.FromObject(existingTeachers.ToList()); // Обновляем данные
                }
                else
                {
                    // Если ключа "Teachers" нет, добавляем его
                    existingData["Teachers"] = JArray.FromObject(teachers.ToList());
                }

                // Сериализуем обновленные данные
                var updatedJson = existingData.ToString(Formatting.Indented);
                File.WriteAllText(JsonFilePath, updatedJson);
            }
            else
            {
                // Если файл не существует, создаем новый файл с новыми данными
                var json = JsonConvert.SerializeObject(newData, Formatting.Indented);
                File.WriteAllText(JsonFilePath, json);
            }
        }

        public static Excel_Settings GetMainInfo(string filePath, string JsonFilePath)
        {
            if (!File.Exists(filePath)) throw new Exception("Файл не найден");

            Some_File = new FileInfo(filePath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(Some_File))
            {
                string SettingsPath = "Models\\Import\\fromExcel\\Excel_Settings.json";

                Excel_Settings temp_ = new Excel_Settings();
                try
                {
                    string json = File.ReadAllText(SettingsPath);

                    Dictionary<string, object>? data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                    int listNumber = Convert.ToInt32(data["LIST_NUMBER"]);
                    int dateColumn = Convert.ToInt32(data["DATE_COLUMN"]);
                    int groupRow = Convert.ToInt32(data["GROUP_ROW"]);

                    Excel_Settings settings = new Excel_Settings(listNumber, dateColumn, groupRow);
                    temp_ = settings;

                    new GetFromExcel(package, settings);
                }
                catch (Exception ex) { throw new Exception("Ошибка при чтении файла ", ex); }

                Import_Groups.GetData(Main_Params.Settings.GROUP_ROW, JsonFilePath);
                Import_Date.GetData(Main_Params.Settings.DATE_COLUMN);

                return temp_;
            }
        }
    }

    public class Teacher
    {
        public string TeacherFIO { get; set; }
    }
}