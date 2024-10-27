using Schedule.Models.Import;
using Schedule.Models.Import.fromExcel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;

namespace Schedule.Controllers
{
    public class ImportController : Controller
    {
        private readonly Importer _importer;

        public ImportController(Importer importer)
        {
            _importer = importer;
        }

        private readonly string importedFilesPath = "importedFiles.json";

        [HttpPost]
        public IActionResult SaveFiles([FromBody] List<string> files)
        {
            // Здесь вы можете разместить код для записи данных о файлах в JSON файл.
            System.IO.File.WriteAllText(importedFilesPath, JsonConvert.SerializeObject(files));

            return Ok();
        }

        // GET: Import/Excel
        public IActionResult Excel()
        {
            return View();
        }

        // POST: Import/Excel
        [HttpPost]
        public IActionResult Excel(int mode, string param)
        {
            var type = new Import_all_data();

            if (ModelState.IsValid)
            {
                var Files = _importer.GetFiles(importedFilesPath);

                // Выполнение процесса импорта
                foreach (var FilePath in Files)
                {
                    _importer.ImportObject(type, FilePath, mode, param);
                }

                // Возвращаем что-то после успешного импорта
                return RedirectToAction("Success"); // например, страница успеха
            }

            // Если данные не валидны, возвращаем ту же страницу с ошибками
            return View();
        }
    }
}
