using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Schedule.Models.Database;
using Schedule.Models.Export;
using Schedule.Models.Export.toGoogleCalendar;

namespace Schedule.Controllers
{
    [Route("Export")]
    public class ExportController : Controller
    {
        private readonly Exporter _exporter;

        public ExportController(Exporter exporter)
        {
            _exporter = exporter;
        }


        [HttpGet("ExportPreview")]
        public IActionResult ExportPreview()
        {
            List<Linker> model = Linker_List.linkers;

            if (model == null || model.Count == 0)
            {
                model = new List<Linker>(); // Создайте пустую коллекцию, чтобы избежать null
            }

            return View(model);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Google")]
        public async Task<IActionResult> Google()
        {
            try
            {
                var type = new GoogleCalendar_Export();

                _exporter.ExportObject(type);

                return Ok();
            }
            catch (Exception ex)
            {
                // Логируем ошибку и возвращаем сообщение об ошибке
                return StatusCode(600, "Ошибка при выполнении экспорта: " + ex.Message);
            }
        }
    }
}
