using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCaitlin.Models
{
    public class TodaysListTileDisplayObject
    {
        public string AvailableStartTime { get; set; }
        public string AvailableEndTime { get; set; }
        public string ActualStartTime { get; set; }
        public string ActualEndTime { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string TimeDifference { get; set; }
        public string Type { get; set; }
        public string Photo { get; set; }
        public string TimeMessage {get; set;}
        public bool IsComplete { get; set; }
        public bool InProgress { get; set; }
        public bool IsSubListAvailable { get; set; }
        public int Index { get; set; }
    }
}
