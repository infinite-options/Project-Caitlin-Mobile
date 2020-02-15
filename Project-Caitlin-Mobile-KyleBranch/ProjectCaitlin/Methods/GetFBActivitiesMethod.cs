namespace ProjectCaitlin.Methods
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class GetFbActivitiesMethod
    {
        [JsonProperty("documents")]
        public Document[] Documents { get; set; }
    }

    public partial class Document
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fields")]
        public DocumentFields Fields { get; set; }

        [JsonProperty("createTime")]
        public DateTimeOffset CreateTime { get; set; }

        [JsonProperty("updateTime")]
        public DateTimeOffset UpdateTime { get; set; }

        [JsonProperty("stringValue")]
        public string StringValue { get; set; }

    }

    public partial class DocumentFields
    {
        [JsonProperty("tasks")]
        public Tasks Tasks { get; set; }
    }

    public partial class Tasks
    {
        [JsonProperty("arrayValue")]
        public ArrayValue ArrayValue { get; set; }
    }

    public partial class ArrayValue
    {
        [JsonProperty("values")]
        public Value[] Values { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("mapValue")]
        public MapValue MapValue { get; set; }
    }

    public partial class MapValue
    {
        [JsonProperty("fields")]
        public MapValueFields Fields { get; set; }
    }

    public partial class MapValueFields
    {
        [JsonProperty("title")]
        public Status Title { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("start_time", NullValueHandling = NullValueHandling.Ignore)]
        public StartTime StartTime { get; set; }
    }

    public partial class StartTime
    {
        [JsonProperty("timestampValue")]
        public DateTimeOffset TimestampValue { get; set; }
    }

    public partial class Status
    {
        [JsonProperty("stringValue")]
        public string StringValue { get; set; }
    }

    public partial class GetFbActivitiesMethod
    {
        public static GetFbActivitiesMethod FromJson(string json) => JsonConvert.DeserializeObject<GetFbActivitiesMethod>(json, ProjectCaitlin.Methods.Converter.Settings);
    }
}

//{
//  "documents": [
//    {
//      "name": "projects/project-caitlin-c71a9/databases/(default)/documents/users/7R6hAVmDrNutRkG3sVRy/Activities/Brush Teeth",
//      "fields": {
//        "steps": {
//          "arrayValue": {
//            "values": [
//              {
//                "mapValue": {
//                  "fields": {
//                    "title": {
//                      "stringValue": "Find Toothbrush"
//                    },
//                    "status": {
//                      "stringValue": "Completed"
//                    },
//                    "start_time": {
//                      "timestampValue": "2020-01-11T16:14:09Z"
//                    }
//                  }
//                }
//              },
//              {
//                "mapValue": {
//                  "fields": {
//                    "title": {
//                      "stringValue": "Find Toothpaste"
//                    },
//                    "status": {
//                      "stringValue": "Completed"
//                    },
//                    "start_time": {
//                      "timestampValue": "2020-01-11T16:14:11Z"
//                    }
//                  }
//                }
//              },
//              {
//                "mapValue": {
//                  "fields": {
//                    "start_time": {
//                      "timestampValue": "2020-01-11T16:14:56Z"
//                    },
//                    "title": {
//                      "stringValue": "Put Toothpaste on Toothbrush"
//                    },
//                    "status": {
//                      "stringValue": "Completed"
//                    }
//                  }
//                }
//              },
//              {
//                "mapValue": {
//                  "fields": {
//                    "title": {
//                      "stringValue": "Brush teeth with Toothbrush"
//                    },
//                    "status": {
//                      "stringValue": "In Progress"
//                    },
//                    "start_time": {
//                      "timestampValue": "2020-01-11T16:15:04Z"
//                    }
//                  }
//                }
//              },
//              {
//                "mapValue": {
//                  "fields": {
//                    "title": {
//                      "stringValue": "Rinse mouth"
//                    },
//                    "status": {
//                      "stringValue": "Planned"
//                    }
//                  }
//                }
//              },
//              {
//                "mapValue": {
//                  "fields": {
//                    "title": {
//                      "stringValue": "Rinse Toothbrush"
//                    },
//                    "status": {
//                      "stringValue": "Planned"
//                    }
//                  }
//                }
//              },
//              {
//                "mapValue": {
//                  "fields": {
//                    "title": {
//                      "stringValue": "Put Toothbrush in holder"
//                    },
//                    "status": {
//                      "stringValue": "Planned"
//                    }
//                  }
//                }
//              },
//              {
//                "mapValue": {
//                  "fields": {
//                    "title": {
//                      "stringValue": "Press Complete Button"
//                    },
//                    "status": {
//                      "stringValue": "Planned"
//                    }
//                  }
//                }
//              }
//            ]
//          }
//        }
//      },
//      "createTime": "2020-01-11T21:21:02.799437Z",
//      "updateTime": "2020-01-11T21:26:04.953352Z"
//    }
//  ]
//}