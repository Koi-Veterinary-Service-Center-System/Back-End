using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using KoiFishCare.Models;


namespace KoiFishCare.service
{
    public class GoogleCalendarService
    {
        protected GoogleCalendarService()
        {

        }
        public static async Task<Event> CreateGoogleCalendar(GoogleCalendar request, string vetCredentialFilePath, string vetEmail, List<string> attendeeEmails)
        {
            string[] Scopes = { "https://www.googleapis.com/auth/calendar" };
            string ApplicationName = "KoiNe";
            UserCredential credential;

            using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "Cre.json"), FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            var services = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            
            // Add the vet as the first attendee, then other participants
            var attendees = new List<EventAttendee>
            {
                new EventAttendee { Email = vetEmail, ResponseStatus = "accepted" }  // Vet's email is the main organizer
            };

            if (attendeeEmails != null && attendeeEmails.Count > 0)
            {
                attendees.AddRange(attendeeEmails.Select(email => new EventAttendee { Email = email }));
            }

            // Define the event with ConferenceData for Google Meet
            Event eventCalendar = new Event()
            {
                Summary = request.Summary,
                Location = request.Location,
                Start = new EventDateTime
                {
                    DateTime = request.Start,
                    TimeZone = "Asia/Ho_Chi_Minh"
                },
                End = new EventDateTime
                {
                    DateTime = request.End,
                    TimeZone = "Asia/Ho_Chi_Minh"
                },
                Description = request.Description,
                ConferenceData = new ConferenceData
                {
                    CreateRequest = new CreateConferenceRequest
                    {
                        ConferenceSolutionKey = new ConferenceSolutionKey
                        {
                            Type = "hangoutsMeet"  // This creates a Google Meet link
                        },
                        RequestId = Guid.NewGuid().ToString()  // Unique request ID
                    }
                },
                Attendees = attendees  // Add the list of attendees to the event
            };

            var eventRequest = services.Events.Insert(eventCalendar, "primary");
            eventRequest.ConferenceDataVersion = 1;  // Enable ConferenceData for Google Meet
            var requestCreate = await eventRequest.ExecuteAsync();

            return requestCreate;
        }
    }
}