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

        // ����� ��� ������ ������������
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // ������ ������ ������������
            await HttpContext.SignOutAsync();

            // ����� �� ������ ������� ���� � �����, ������� ������ �������
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

            // ����� ������� ��� �������� �����
            RemoveDirectory(folderPath);

            // ��������������� �� ������� �������� ��� ����-�� ���
            return RedirectToAction("Index", "Home");
        }

        // ����� ��� �������� �����
        private void RemoveDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true); // true - ����� ������� ����� ��������� �����
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"������ ��� �������� �����: {ex.Message}");
            }
        }
    }
}
