using Schedule.Models.Import;
using Schedule.Models.Import.fromExcel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using Schedule.Models;

namespace Schedule.Controllers
{
    [Route("Import")]
    public class ImportController : Controller
    {
        private readonly Importer _importer;
        private readonly ILogger<HomeController> _logger;
        private readonly JsonDataService _jsonFileReader;

        private readonly string importedFilesPath = "importedFiles.json";
        private readonly string DirectoryPath = "UploadedFiles";
        private static List<string> fullPaths = new List<string>();
        private static string JsonFilePath = "data.json";

        public ImportController(Importer importer, ILogger<HomeController> logger)
        {
            _importer = importer;
            _logger = logger;
            _jsonFileReader = new JsonDataService(JsonFilePath);
        }


        [HttpGet("GetGroups")]
        public IActionResult GetGroups(string query)
        {
            var groups = _jsonFileReader.GetSubGroups();
            var result = groups.Where(g => g.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(result);
        }

        [HttpGet("GetTeachers")]
        public IActionResult GetTeachers(string query)
        {
            var teachers = _jsonFileReader.GetTeachers();
            var result = teachers.Where(t => t.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(result);
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFiles(List<IFormFile> _files)
        {
            try
            {
                // Убедитесь, что директория UploadedFiles существует
                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), DirectoryPath);
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory); // Создаём директорию, если её нет
                }

                // Проверка создания директории
                if (!Directory.Exists(uploadDirectory))
                {
                    return StatusCode(500, "Не удалось создать директорию " + DirectoryPath);
                }

                var fileNames = new List<string>();

                if (_files == null || !_files.Any())
                {
                    return BadRequest("Нет файлов для загрузки.");
                }

                foreach (var file in _files)
                {
                    if (file.Length > 0)
                    {
                        if (!file.FileName.EndsWith(".xls") && !file.FileName.EndsWith(".xlsx"))
                        {
                            return BadRequest("Неверный формат файла.");
                        }

                        var filePath = Path.Combine(uploadDirectory, file.FileName);

                        // Удаляем файл, если он уже существует
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                        // Сохраняем файл
                        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Сохраняем полный путь для внутреннего использования
                        fullPaths.Add(filePath);

                        JsonFilePath = Path.Combine(DirectoryPath, JsonFilePath);

                        // Сохраняем относительный путь для доступа через веб
                        string relativePath = Path.Combine(DirectoryPath, file.FileName);
                        fileNames.Add(relativePath);
                    }
                }

                var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), importedFilesPath);

                // Удаляем файл, если он уже существует
                if (System.IO.File.Exists(jsonPath))
                {
                    System.IO.File.Delete(jsonPath);
                }

                var jsonData = new
                {
                    files = fileNames,          // относительные пути для доступа через веб
                    fullPaths = fullPaths       // полные пути для внутреннего использования
                };

                // Сериализация данных
                var jsonString = System.Text.Json.JsonSerializer.Serialize(jsonData, new JsonSerializerOptions { WriteIndented = true });

                // Сохраняем JSON-файл
                using (var stream = new FileStream(jsonPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonString);
                    await stream.WriteAsync(jsonBytes, 0, jsonBytes.Length);
                }

                if (System.IO.File.Exists(JsonFilePath))
                {
                    System.IO.File.Delete(JsonFilePath);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при загрузке файла: {ex.Message}");
                return StatusCode(500, "Произошла ошибка при обработке запроса.");
            }
        }

        [HttpPost("ExcelUpload")]
        public async Task<IActionResult> ExcelUpload()
        {
            try
            {
                if(fullPaths.Count == 0) return StatusCode(500, "Произошла ошибка при импорте");

                foreach (var item in fullPaths)
                {
                    Import_Data.GetData(item, JsonFilePath);
                }

                return Ok();
            }
            catch
            {
                return StatusCode(500, "Произошла ошибка при импорте");
            }
        }

        [HttpPost("ExcelFind")]
        public async Task<IActionResult> ExcelFind([FromBody] ExcelRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Param) || request.Mode < 0)
            {
                return BadRequest("Недостающие или неправильные параметры.");
            }

            var type = new Import_all_data();

            try
            {
                foreach (var item in fullPaths)
                {
                    if (!item.Any())
                    {
                        return BadRequest("Нет файлов для импорта.");
                    }

                    _importer.ImportObject(type, request.Mode, request.Param);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                return StatusCode(500, "Ошибка при выполнении импорта: " + ex.Message);
            }
        }

        // Класс для запроса
        public class ExcelRequest
        {
            public int Mode { get; set; }
            public string Param { get; set; }
        }

        [HttpPost("DeleteUploadedFiles")]
        public IActionResult DeleteUploadedFiles()
        {
            try
            {
                string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), DirectoryPath);

                if (Directory.Exists(uploadDirectory))
                {
                    Directory.Delete(uploadDirectory, recursive: true);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при удалении папки UploadedFiles: {ex.Message}");
                return StatusCode(500, "Произошла ошибка при удалении папки " + DirectoryPath);
            }
        }
    }
}
