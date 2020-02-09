namespace ProjectCaitlin.Methods
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class GetEventsListMethod
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("updated")]
        public DateTime Updated { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("accessRole")]
        public string AccessRole { get; set; }

        [JsonProperty("defaultReminders")]
        public EventsDefaultReminder[] DefaultReminders { get; set; }

        [JsonProperty("nextPageToken")]
        public string NextPageToken { get; set; }

        [JsonProperty("nextSyncToken")]
        public string NextSyncToken { get; set; }

        [JsonProperty("items")]
        public EventsItems[] Items { get; set; }
    }

    public partial class EventsDefaultReminder
    {
        [JsonProperty("method")]
        public string method { get; set; }

        [JsonProperty("minutes")]
        public int minutes { get; set; }
    }

    public partial class EventsItems
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("htmlLink", NullValueHandling = NullValueHandling.Ignore)]
        public string HtmlLink { get; set; }

        [JsonProperty("created", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("updated", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset Updated { get; set; }

        [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
        public string EventName { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Desciption { get; set; }

        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public string Location { get; set; }

        [JsonProperty("colorId", NullValueHandling = NullValueHandling.Ignore)]
        public string ColorID { get; set; }

        [JsonProperty("creator", NullValueHandling = NullValueHandling.Ignore)]
        public EventsCreator Creator { get; set; }

        [JsonProperty("organizer", NullValueHandling = NullValueHandling.Ignore)]
        public EventsCreator Organizer { get; set; }

        [JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
        public EventsEnd Start { get; set; }

        [JsonProperty("end", NullValueHandling = NullValueHandling.Ignore)]
        public EventsEnd End { get; set; }

        [JsonProperty("iCalUID", NullValueHandling = NullValueHandling.Ignore)]
        public string ICalUid { get; set; }

        [JsonProperty("sequence", NullValueHandling = NullValueHandling.Ignore)]
        public long? Sequence { get; set; }

        [JsonProperty("attendees", NullValueHandling = NullValueHandling.Ignore)]
        public EventsAttendee[] Attendees { get; set; }

        [JsonProperty("hangoutLink", NullValueHandling = NullValueHandling.Ignore)]
        public Uri HangoutLink { get; set; }

        [JsonProperty("conferenceData", NullValueHandling = NullValueHandling.Ignore)]
        public EventsConferenceData ConferenceData { get; set; }

        [JsonProperty("guestsCanModify", NullValueHandling = NullValueHandling.Ignore)]
        public bool? GuestsCanModify { get; set; }

        [JsonProperty("reminders", NullValueHandling = NullValueHandling.Ignore)]
        public EventsReminders Reminders { get; set; }

        [JsonProperty("recurrence", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Recurrence { get; set; }

        [JsonProperty("recurringEventId", NullValueHandling = NullValueHandling.Ignore)]
        public string RecurringEventId { get; set; }

        [JsonProperty("originalStartTime", NullValueHandling = NullValueHandling.Ignore)]
        public EventsOriginalStartTime OriginalStartTime { get; set; }

    }

    public partial class EventsAttendee
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("displayName", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [JsonProperty("organizer", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Organizer { get; set; }

        [JsonProperty("responseStatus")]
        public string ResponseStatus { get; set; }

        [JsonProperty("self", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Self { get; set; }

        [JsonProperty("resource", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Resource { get; set; }

        [JsonProperty("optional", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Optional { get; set; }

        [JsonProperty("comment", NullValueHandling = NullValueHandling.Ignore)]
        public string Comment { get; set; }
    }

    public partial class EventsConferenceData
    {
        [JsonProperty("entryPoints")]
        public EventsEntryPoint[] EntryPoints { get; set; }

        [JsonProperty("conferenceSolution")]
        public EventsConferenceSolution ConferenceSolution { get; set; }

        [JsonProperty("conferenceId", NullValueHandling = NullValueHandling.Ignore)]
        public string ConferenceId { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("createRequest", NullValueHandling = NullValueHandling.Ignore)]
        public EventsCreateRequest CreateRequest { get; set; }
    }

    public partial class EventsConferenceSolution
    {
        [JsonProperty("key")]
        public EventsKey Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("iconUri")]
        public string IconUri { get; set; }
    }

    public partial class EventsKey
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class EventsCreateRequest
    {
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("conferenceSolutionKey")]
        public EventsKey ConferenceSolutionKey { get; set; }

        [JsonProperty("status")]
        public EventsStatusClass Status { get; set; }
    }

    public partial class EventsStatusClass
    {
        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }
    }

    public partial class EventsEntryPoint
    {
        [JsonProperty("entryPointType")]
        public string EntryPointType { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }

        [JsonProperty("pin", NullValueHandling = NullValueHandling.Ignore)]
        public string Pin { get; set; }

        [JsonProperty("regionCode", NullValueHandling = NullValueHandling.Ignore)]
        public string RegionCode { get; set; }
    }

    public partial class EventsCreator
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("displayName", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [JsonProperty("self", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Self { get; set; }
    }

    public partial class EventsEnd
    {
        [JsonProperty("dateTime")]
        public DateTimeOffset DateTime { get; set; }

        [JsonProperty("timeZone", NullValueHandling = NullValueHandling.Ignore)]
        public string TimeZone { get; set; }
    }

    public partial class EventsOriginalStartTime
    {
        [JsonProperty("dateTime")]
        public DateTimeOffset DateTime { get; set; }
    }

    public partial class EventsReminders
    {
        [JsonProperty("useDefault")]
        public bool UseDefault { get; set; }
    }

    public partial class GetEventsListMethod
    {
        public static GetEventsListMethod FromJson(string json) => JsonConvert.DeserializeObject<GetEventsListMethod>(json, ProjectCaitlin.Methods.Converter.Settings);
    }

    public static class Serialize1
    {
        public static string ToJson(this GetEventsListMethod self) => JsonConvert.SerializeObject(self, ProjectCaitlin.Methods.Converter.Settings);
    }
}






//{
// "kind": "calendar#events",
// "etag": "\"p32sd9vc8h7kuc0g\"",
// "summary": "iodevcalendar@gmail.com",
// "updated": "2020-01-04T04:07:38.259Z",
// "timeZone": "America/Los_Angeles",
// "accessRole": "owner",
// "defaultReminders": [
//  {
//   "method": "popup",
//   "minutes": 30
//  }
// ],
// "nextSyncToken": "CLjU_YiJ6eYCELjU_YiJ6eYCGAU=",
// "items": [
//  {
//   "kind": "calendar#event",
//   "etag": "\"3156221715226000\"",
//   "id": "4p300620rs9k4u48lv99u9psm6",
//   "status": "confirmed",
//   "htmlLink": "https://www.google.com/calendar/event?eid=NHAzMDA2MjByczlrNHU0OGx2OTl1OXBzbTYgaW9kZXZjYWxlbmRhckBt",
//   "created": "2020-01-04T04:07:37.000Z",
//   "updated": "2020-01-04T04:07:37.613Z",
//   "summary": "Test Event",
//   "creator": {
//    "email": "iodevcalendar@gmail.com",
//    "self": true
//   },
//   "organizer": {
//    "email": "iodevcalendar@gmail.com",
//    "self": true
//   },
//   "start": {
//    "dateTime": "2019-12-31T07:30:00-08:00"
//   },
//   "end": {
//    "dateTime": "2019-12-31T08:30:00-08:00"
//   },
//   "iCalUID": "4p300620rs9k4u48lv99u9psm6@google.com",
//   "sequence": 0,
//   "reminders": {
//    "useDefault": true
//   }
//  } 
// ]
//}