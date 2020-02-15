namespace ProjectCaitlin.Methods
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class GetCalendarsMethod
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("nextSyncToken")]
        public string NextSyncToken { get; set; }

        [JsonProperty("items")]
        public CalendarsItem[] Items { get; set; }
    }

    public partial class CalendarsItem
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("colorId")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long ColorId { get; set; }

        [JsonProperty("backgroundColor")]
        public string BackgroundColor { get; set; }

        [JsonProperty("foregroundColor")]
        public string ForegroundColor { get; set; }

        [JsonProperty("selected")]
        public bool Selected { get; set; }

        [JsonProperty("accessRole")]
        public string AccessRole { get; set; }

        [JsonProperty("defaultReminders")]
        public EventsDefaultReminder[] DefaultReminders { get; set; }

        [JsonProperty("conferenceProperties")]
        public CalendarsConferenceProperties ConferenceProperties { get; set; }

        [JsonProperty("notificationSettings", NullValueHandling = NullValueHandling.Ignore)]
        public CalendarsNotificationSettings NotificationSettings { get; set; }

        [JsonProperty("primary", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Primary { get; set; }

        [JsonProperty("summaryOverride", NullValueHandling = NullValueHandling.Ignore)]
        public string SummaryOverride { get; set; }
    }

    public partial class CalendarsConferenceProperties
    {
        [JsonProperty("allowedConferenceSolutionTypes")]
        public string[] AllowedConferenceSolutionTypes { get; set; }
    }

    public partial class CalendarsDefaultReminder
    {
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("minutes")]
        public long Minutes { get; set; }
    }

    public partial class CalendarsNotificationSettings
    {
        [JsonProperty("notifications")]
        public CalendarsNotification[] Notifications { get; set; }
    }

    public partial class CalendarsNotification
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
    }

    public partial class GetCalendarsMethod
    {
        public static GetCalendarsMethod FromJson(string json) => JsonConvert.DeserializeObject<GetCalendarsMethod>(json, ProjectCaitlin.Methods.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this GetCalendarsMethod self) => JsonConvert.SerializeObject(self, ProjectCaitlin.Methods.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}


//{
// "kind": "calendar#calendarList",
// "etag": "\"p33cf3s40v3duc0g\"",
// "nextSyncToken": "CNjx8ID42-YCEhRraG9lZmVyMTAwQGdtYWlsLmNvbQ==",
// "items": [
//  {
//   "kind": "calendar#calendarListEntry",
//   "etag": "\"1577567351544000\"",
//   "id": "s0dr1r8crir0ub65e50n1vdl34@group.calendar.google.com",
//   "summary": "Rowan",
//   "description": "Rowan's Test Calendar",
//   "timeZone": "America/Los_Angeles",
//   "colorId": "15",
//   "backgroundColor": "#9fc6e7",
//   "foregroundColor": "#000000",
//   "selected": true,
//   "accessRole": "writer",
//   "defaultReminders": [],
//   "conferenceProperties": {
//    "allowedConferenceSolutionTypes": [
//     "eventHangout"
//    ]
//   }
//  },
//  {
//   "kind": "calendar#calendarListEntry",
//   "etag": "\"1565289970655000\"",
//   "id": "khoefer100@gmail.com",
//   "summary": "khoefer100@gmail.com",
//   "timeZone": "America/Los_Angeles",
//   "colorId": "14",
//   "backgroundColor": "#9fe1e7",
//   "foregroundColor": "#000000",
//   "selected": true,
//   "accessRole": "owner",
//   "defaultReminders": [
//    {
//     "method": "popup",
//     "minutes": 30
//    }
//   ],
//   "notificationSettings": {
//    "notifications": [
//     {
//      "type": "eventCreation",
//      "method": "email"
//     },
//     {
//      "type": "eventChange",
//      "method": "email"
//     },
//     {
//      "type": "eventCancellation",
//      "method": "email"
//     },
//     {
//      "type": "eventResponse",
//      "method": "email"
//     }
//    ]
//   },
//   "primary": true,
//   "conferenceProperties": {
//    "allowedConferenceSolutionTypes": [
//     "eventHangout"
//    ]
//   }
//  },
//  {
//   "kind": "calendar#calendarListEntry",
//   "etag": "\"1577580465534000\"",
//   "id": "iodevcalendar@gmail.com",
//   "summary": "iodevcalendar@gmail.com",
//   "timeZone": "America/Los_Angeles",
//   "colorId": "24",
//   "backgroundColor": "#a47ae2",
//   "foregroundColor": "#000000",
//   "selected": true,
//   "accessRole": "owner",
//   "defaultReminders": [],
//   "conferenceProperties": {
//    "allowedConferenceSolutionTypes": [
//     "eventHangout"
//    ]
//   }
//  },
//  {
//   "kind": "calendar#calendarListEntry",
//   "etag": "\"1571514383132000\"",
//   "id": "addressbook#contacts@group.v.calendar.google.com",
//   "summary": "Contacts",
//   "timeZone": "America/Los_Angeles",
//   "summaryOverride": "Contacts",
//   "colorId": "13",
//   "backgroundColor": "#92e1c0",
//   "foregroundColor": "#000000",
//   "selected": true,
//   "accessRole": "reader",
//   "defaultReminders": [],
//   "conferenceProperties": {
//    "allowedConferenceSolutionTypes": [
//     "eventHangout"
//    ]
//   }
//  },
//  {
//   "kind": "calendar#calendarListEntry",
//   "etag": "\"1565289971685000\"",
//   "id": "en.usa#holiday@group.v.calendar.google.com",
//   "summary": "Holidays in United States",
//   "timeZone": "America/Los_Angeles",
//   "colorId": "8",
//   "backgroundColor": "#16a765",
//   "foregroundColor": "#000000",
//   "selected": true,
//   "accessRole": "reader",
//   "defaultReminders": [],
//   "conferenceProperties": {
//    "allowedConferenceSolutionTypes": [
//     "eventHangout"
//    ]
//   }
//  }
// ]
//}
