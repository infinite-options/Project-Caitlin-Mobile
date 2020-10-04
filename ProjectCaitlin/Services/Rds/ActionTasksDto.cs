using ProjectCaitlin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCaitlin.Services.Rds
{
    public class ActionTasksDto
    {
        public string at_unique_id { get; set; }
        public string at_title { get; set; }
        public string goal_routine_id { get; set; }
        public string at_sequence { get; set; }
        public string is_available { get; set; }
        public string is_complete { get; set; }
        public string is_in_progress { get; set; }
        public string is_sublist_available { get; set; }
        public string is_must_do { get; set; }
        public string photo { get; set; }
        public string is_timed { get; set; }
        public string datetime_completed { get; set; }
        public string datetime_started { get; set; }
        public string expected_completion_time { get; set; }
        public string available_start_time { get; set; }
        public string available_end_time { get; set; }

        internal atObject ToATObjects()
        {
            atObject at = new atObject();
            at.id = at_unique_id;
            at.title = at_title;
            at.grId = goal_routine_id;
            at.isMustDo = string.IsNullOrEmpty(is_must_do) ? true : (bool)Boolean.Parse(is_must_do);
            at.isSublistAvailable = string.IsNullOrEmpty(is_sublist_available) ? true : (bool)Boolean.Parse(is_sublist_available);
            at.photo = photo;
            at.expectedCompletionTime = DateTime.Parse(expected_completion_time).TimeOfDay;
            at.dateTimeCompleted = DateTime.Parse(datetime_completed);
            at.availableStartTime = DateTime.Parse(available_start_time).TimeOfDay;
            at.availableEndTime = DateTime.Parse(available_end_time).TimeOfDay;
            return at;
        }


        internal task ToTask()
        {
            task at = new task();
            at.id = at_unique_id;
            at.title = at_title;
            at.grId = goal_routine_id;
            at.isMustDo = string.IsNullOrEmpty(is_must_do) ? true : (bool)Boolean.Parse(is_must_do);
            at.isSublistAvailable = string.IsNullOrEmpty(is_sublist_available) ? true : (bool)Boolean.Parse(is_sublist_available);
            at.photo = photo;
            at.expectedCompletionTime =  DateTime.Parse(expected_completion_time).TimeOfDay;
            at.dateTimeCompleted = DateTime.Parse(datetime_completed);
            if (!string.IsNullOrEmpty(available_start_time))
            {
                at.availableStartTime = DateTime.Parse(available_start_time).TimeOfDay;
            }
            if (!string.IsNullOrEmpty(available_end_time))
            {
                at.availableEndTime = DateTime.Parse(available_end_time).TimeOfDay;
            }
            return at;
        }

        internal action ToAction()
        {
            action at = new action();
            at.id = at_unique_id;
            at.title = at_title;
            at.grId = goal_routine_id;
            at.isMustDo = string.IsNullOrEmpty(is_must_do) ? true : (bool)Boolean.Parse(is_must_do);
            at.isSublistAvailable = string.IsNullOrEmpty(is_sublist_available) ? true : (bool)Boolean.Parse(is_sublist_available);
            at.photo = photo;
            at.expectedCompletionTime = DateTime.Parse(expected_completion_time).TimeOfDay;
            at.dateTimeCompleted = DateTime.Parse(datetime_completed);
            if (!string.IsNullOrEmpty(available_start_time))
            {
                at.availableStartTime = DateTime.Parse(available_start_time).TimeOfDay;
            }
            if (!string.IsNullOrEmpty(available_end_time))
            {
                at.availableEndTime = DateTime.Parse(available_end_time).TimeOfDay;
            }
            return at;
        }

    }
}
