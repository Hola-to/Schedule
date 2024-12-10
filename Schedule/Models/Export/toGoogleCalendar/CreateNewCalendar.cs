using Google.Apis.Calendar.v3.Data;

namespace Schedule.Models.Export.toGoogleCalendar
{
    public partial class Export_Data
    {
        public static void Create_NewCalendar(string calendarId)
        {
            // Указываем идентификатор календаря
            if (calendarId == null || calendarId == "") calendarId = "Расписание ВГУ";

            // Получаем список всех календарей
            var calendarList = service.CalendarList.List().Execute();

            // Проверяем, существует ли календарь с таким именем
            var existingCalendar = calendarList.Items.FirstOrDefault(c => c.Summary == calendarId);

            if (existingCalendar != null)
            {
                // Если календарь с таким именем уже существует, используем его
                ScheduleCalendar = service.Calendars.Get(existingCalendar.Id).Execute();
                return;
            }

            // Если календаря с таким именем нет, создаем новый
            var primaryCalendar = calendarList.Items.FirstOrDefault(); // предполагается, что первый элемент - это основной календарь
            string timeZone = primaryCalendar?.TimeZone ?? "Europe/Moscow"; // Используем временную зону основного календаря или зону по умолчанию

            // Создание нового календаря с использованием временной зоны
            Calendar newCalendar = new Calendar()
            {
                Summary = calendarId,
                TimeZone = timeZone
            };

            // Вызов API для создания календаря
            var createdCalendar = service.Calendars.Insert(newCalendar).Execute();

            ScheduleCalendar = createdCalendar;
            Console.WriteLine($"Создан новый календарь: {createdCalendar.Summary} (ID: {createdCalendar.Id})");
        }
    }
}
