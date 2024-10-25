using Google.Apis.Calendar.v3.Data;

namespace Schedule.Models.Export.toGoogleCalendar
{
    public partial class Export_Data
    {
        public static void Create_NewCalendar(string calendarId)
        {
            // Указываем идентификатор календаря
            if (calendarId == null || calendarId == "") calendarId = "Расписание ВГУ";

            var calendarList = service.CalendarList.List().Execute();
            var primaryCalendarId = calendarList.Items[0].Id; // предполагается, что первый элемент - это основной календарь

            var calendar = service.Calendars.Get(primaryCalendarId).Execute();
            string timeZone = calendar.TimeZone;

            // Создание нового календаря с использованием временной зоны из аккаунта
            Calendar schedule = new Calendar()
            {
                Summary = calendarId,
                TimeZone = timeZone
            };

            // Вызов API для создания календаря
            var createdCalendar = service.Calendars.Insert(schedule).Execute();

            ScheduleCalendar = createdCalendar;
        }
    }
}
