using Schedule.Models.Import;
using Schedule.Models.Import.fromExcel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.Text.Json;

namespace Schedule.Controllers
{
    [Route("Import")]
    public class ImportController : Controller
    {
        private readonly Importer _importer;

        private readonly ILogger<HomeController> _logger;

        public ImportController(Importer importer, ILogger<HomeController> logger)
        {
            _importer = importer;
            _logger = logger;
        }

        private readonly string importedFilesPath = "importedFiles.json";

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            Directory.CreateDirectory(uploadDirectory);

            var fileNames = new List<string>();
            var fullPaths = new List<string>();

            try
            {
                if (files == null || !files.Any())
                {
                    return BadRequest("Нет файлов для загрузки.");
                }

                foreach (var file in files)
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

                        // Создаем поток с параметром FileShare.ReadWrite
                        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Сохранение полного пути для внутреннего использования
                        fullPaths.Add(filePath);
                        // Добавление относительного пути для доступа через веб
                        string relativePath = Path.Combine("UploadedFiles", file.FileName);
                        fileNames.Add(relativePath);
                    }
                }

                var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "importedFiles.json");

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

                // Сериализация с учетом полных путей
                var jsonString = System.Text.Json.JsonSerializer.Serialize(jsonData, new JsonSerializerOptions { WriteIndented = true });

                // Создаем поток с параметром FileShare.None
                using (var stream = new FileStream(jsonPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonString);
                    await stream.WriteAsync(jsonBytes, 0, jsonBytes.Length);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                _logger.LogError($"Ошибка при загрузке файла: {ex.Message}");

                // Возвращаем общую ошибку
                return StatusCode(500, "Произошла ошибка при обработке запроса.");
            }
        }

        [HttpPost("Excel")]
        public async Task<IActionResult> Excel([FromBody] ExcelRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Param) || request.Mode < 0)
            {
                return BadRequest("Недостающие или неправильные параметры.");
            }

            var type = new Import_all_data();

            try
            {
                var Files = _importer.GetFiles(importedFilesPath);

                if (!Files.Any())
                {
                    return BadRequest("Нет файлов для импорта.");
                }

                foreach (var FilePath in Files)
                {
                    _importer.ImportObject(type, FilePath, request.Mode, request.Param);
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
                string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

                if (Directory.Exists(uploadDirectory))
                {
                    Directory.Delete(uploadDirectory, recursive: true);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при удалении папки UploadedFiles: {ex.Message}");
                return StatusCode(500, "Произошла ошибка при удалении папки UploadedFiles.");
            }
        }
    }
}
