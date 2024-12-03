using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Schedule.Models;
using System.Diagnostics;

namespace Schedule.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Authors()
        {
            return View();
        }

        public IActionResult Tables()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Метод для выхода пользователя
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Логика выхода пользователя
            await HttpContext.SignOutAsync();

            // Здесь вы можете указать путь к папке, которую хотите удалить
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

            // Вызов функции для удаления папки
            RemoveDirectory(folderPath);

            // Перенаправление на главную страницу или куда-то еще
            return RedirectToAction("Index", "Home");
        }

        // Метод для удаления папки
        private void RemoveDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true); // true - чтобы удалить также вложенные папки
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при удалении папки: {ex.Message}");
            }
        }
    }
}
