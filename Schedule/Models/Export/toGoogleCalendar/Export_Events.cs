using Schedule.Models.Database;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;

namespace Schedule.Models.Export.toGoogleCalendar
{
    public partial class Export_Data
    {
        private static string[] Scopes = { CalendarService.Scope.Calendar };
        public static string ApplicationName = "VSU Schedule Exporter to Google Calendar";
        protected static Calendar? ScheduleCalendar {  get; set; } = new Calendar();

        public static CalendarService service = new CalendarService();

        public static void GoogleCalendar_Export()
        {
            if (ScheduleCalendar == null)
            {
                throw new Exception("Календаря не существует");
            }

            var calendarId = ScheduleCalendar.Id;

            List<Event> events = new List<Event>();

            foreach (var date in Linker_List.linkers)
            {
                if (date.Date.is_Null()) continue;

                foreach (var pair in date)
                {

                    string? groupOrTeacher = "";

                    if (pair.ManyData.Count > 1)
                    {
                        foreach (var item in pair)
                        {
                            groupOrTeacher += $"{item.Teacher_FIO} ";
                        }
                    }

                    else if (pair.ManyData.Count == 1) 
                    {
                        groupOrTeacher = pair.ManyData[0].Teacher_FIO;
                    }

                    if (pair.ManyData.Count != 0)
                    {
                        var data = pair.ManyData[0];
                        if (data.Discipline == null || data.Discipline == "" &&
                           data.Auditorium == null || data.Auditorium == "" &&
                           data.Teacher_FIO == null || data.Teacher_FIO == "")
                        {
                            continue;
                        }

                        Event newEvent = new Event()
                        {
                            Summary = $"{data.Discipline}  {data.Auditorium}",
                            Description = $"{data.Discipline}\n{data.Auditorium}\n{groupOrTeacher}",
                            Start = new EventDateTime()
                            {
                                DateTimeDateTimeOffset =
                                            new DateTime(date.Date.Year, date.Date.Month_index, date.Date.Day,
                                            pair.One_Pair.start_hour, pair.One_Pair.start_minute, 0) // Укажите дату и время начала
                            },
                            End = new EventDateTime()
                            {
                                DateTimeDateTimeOffset =
                                            new DateTime(date.Date.Year, date.Date.Month_index, date.Date.Day,
                                            pair.One_Pair.end_hour, pair.One_Pair.end_minute, 0)// Укажите дату и время окончания
                            },
                        };

                        events.Add(newEvent);
                    }
                }
            }

            foreach (var someEvent in events)
            {
                EventsResource.InsertRequest request = service.Events.Insert(someEvent, calendarId);
                request.Execute();
            }
        }
    }
}
