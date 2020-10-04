using ProjectCaitlin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCaitlin.Services.Rds
{
    class GratisDto
    {
        public string gr_unique_id { get; set; }
        public string gr_title { get; set; }
        public string user_id { get; set; }
        public string is_available { get; set; }
        public bool is_complete { get; set; }
        public bool is_in_progress { get; set; }
        public bool is_displayed_today { get; set; }
        public bool is_persistent { get; set; }
        public string is_sublist_available { get; set; }
        public string is_timed { get; set; }
        public string photo { get; set; }
        public string start_day_and_time { get; set; }
        public string end_day_and_time { get; set; }
        public string repeat { get; set; }
        public string repeat_type { get; set; }
        public string repeat_ends_on { get; set; }
        public int repeat_occurences { get; set; }
        public int repeat_every { get; set; }
        public string repeat_frequency { get; set; }
        // public List<repeatWeekdays> repeat_week_days { get; set; }
        public string repeat_week_days { get; set; }
        public string datetime_started { get; set; }
        public string datetime_completed { get; set; }
        public string expected_completion_time { get; set; }
        public object completed { get; set; }


        public GratisObject ToGratisObject()
        {
            if (is_persistent)
            {
                return new routine()
                {
                    title = gr_title,
                    id = gr_unique_id,
                    photo = photo,
                    isInProgress = is_in_progress,
                    isComplete = is_complete,
                    isSublistAvailable = string.IsNullOrEmpty(is_sublist_available)? false : Boolean.Parse(is_sublist_available),
                    expectedCompletionTime = DateTime.Parse(expected_completion_time).TimeOfDay,
                    dateTimeCompleted = DateTime.Parse(datetime_completed),
                    availableStartTime = DateTime.Parse(start_day_and_time).TimeOfDay,
                    availableEndTime = DateTime.Parse(end_day_and_time).TimeOfDay
                };
            }
            return new goal()
            {
                title = gr_title,
                id = gr_unique_id,
                photo = photo,
                isInProgress = is_in_progress,
                isComplete = is_complete,
                isSublistAvailable = string.IsNullOrEmpty(is_sublist_available) ? false : Boolean.Parse(is_sublist_available),
                expectedCompletionTime = DateTime.Parse(expected_completion_time).TimeOfDay,
                dateTimeCompleted = DateTime.Parse(datetime_completed),
                availableStartTime = DateTime.Parse(start_day_and_time).TimeOfDay,
                availableEndTime = DateTime.Parse(end_day_and_time).TimeOfDay
            };
        }

        //public routine ToRoutine()
        //{

        //}

        //public goal ToGoal()
        //{

        //}

    }
}
