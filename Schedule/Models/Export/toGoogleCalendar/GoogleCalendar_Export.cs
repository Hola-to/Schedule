using Schedule.Models.Export;

namespace Schedule.Models.Export.toGoogleCalendar
{
    public class GoogleCalendar_Export : Export_Data, SExport
    {
        public void Execute(string calendarId)
        {
            Authorization();

            var calendarList = service.CalendarList.List().Execute();

            foreach (var calendar in calendarList.Items)
            {
                if (calendar.Id == calendarId)
                {
                    ScheduleCalendar = service.Calendars.Get(calendar.Id).Execute();

                    Export_Data.GoogleCalendar_Export();
                    return;
                }
            }

            Create_NewCalendar(calendarId);

            Export_Data.GoogleCalendar_Export();
        }
    }
}
