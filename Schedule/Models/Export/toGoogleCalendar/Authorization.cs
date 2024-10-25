using Google.Apis.Calendar.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Schedule.Models.Export.toGoogleCalendar
{
    public partial class Export_Data
    {
        public static void Authorization()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("..\\..\\..\\..\\Data\\credentials.json", FileMode.Open, FileAccess.Read))
            {
                // Временное хранилище для токенов доступа
                string credPath = "..\\..\\..\\..\\Data\\Tokens";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Создаем сервис для работы с API
            service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
    }
}
