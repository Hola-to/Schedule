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
            var fileNames = new List<string>();

            try
            {
                if (files == null) return StatusCode(500, "Ошибка при загрузке файлов");

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        // Проверка на формат файла
                        if (!file.FileName.EndsWith(".xls") && !file.FileName.EndsWith(".xlsx"))
                        {
                            return BadRequest("Неверный формат файла.");
                        }

                        fileNames.Add(file.FileName);
                        // Логика сохранения файла, если требуется
                    }
                }

                // Логика сохранения имен файлов в JSON
                var jsonPath = Path.Combine("", importedFilesPath);
                var jsonData = new { files = fileNames };
                var jsonString = System.Text.Json.JsonSerializer.Serialize(jsonData);

                await System.IO.File.WriteAllTextAsync(jsonPath, jsonString);

                return Ok();
            }
            catch (Exception ex)
            {
                // Логируем ошибку и возвращаем сообщение об ошибке
                return StatusCode(500, "Ошибка при загрузке файла: " + ex.Message);
            }
        }

        [HttpPost("Index")]
        public IActionResult Index(string selectedAction)
        {
            if (!string.IsNullOrEmpty(selectedAction))
            {
                return RedirectToAction(selectedAction);
            }

            ViewData["Message"] = "Неверный выбор";
            return View();
        }

        [HttpPost("Excel")]
        public async Task<IActionResult> Excel([FromBody] ExcelRequest request)
        {
            // Проверяем, переданы ли необходимые параметры
            if (request == null || string.IsNullOrEmpty(request.Param))
            {
                return BadRequest("Недостающие параметры."); // Возврат ошибки 400 с сообщением
            }

            var type = new Import_all_data();

            // Логика импорта
            try
            {
                var Files = _importer.GetFiles(importedFilesPath);

                // Выполнение процесса импорта
                foreach (var FilePath in Files)
                {
                    _importer.ImportObject(type, FilePath, request.Mode, request.Param);
                }

                return Ok(); 
            }
            catch (Exception ex)
            {
                // Логируем ошибку и возвращаем сообщение об ошибке
                return StatusCode(600, "Ошибка при выполнении импорта: " + ex.Message);
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
