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

        public ImportController(Importer importer)
        {
            _importer = importer;
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

                        using (var stream = new FileStream(filePath, FileMode.Create))
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

                var jsonPath = Path.Combine("", "importedFiles.json");
                var jsonData = new
                {
                    files = fileNames,          // относительные пути для доступа через веб
                    fullPaths = fullPaths       // полные пути для внутреннего использования
                };

                // Сериализация с учетом полных путей
                var jsonString = System.Text.Json.JsonSerializer.Serialize(jsonData, new JsonSerializerOptions { WriteIndented = true });
                await System.IO.File.WriteAllTextAsync(jsonPath, jsonString);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ошибка при загрузке файла: " + ex.Message);
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
    }
}
