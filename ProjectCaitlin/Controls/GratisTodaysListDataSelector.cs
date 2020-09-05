using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ProjectCaitlin.Models;

namespace ProjectCaitlin.Controls
{
    public class GratisTodaysListDataSelector : Xamarin.Forms.DataTemplateSelector
    {

        public DataTemplate EventTodaysListTemplate { get; set; }

        public DataTemplate GoalTodaysListCompleteTemplate { get; set; }
        public DataTemplate GoalTodaysListNotCompleteTemplate { get; set; }
        
        public DataTemplate RoutineTodaysListCompleteTemplate { get; set; }
        public DataTemplate RoutineTodaysListNotCompleteTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            TodaysListTileDisplayObject displayObject = item as TodaysListTileDisplayObject;
            if (string.Equals(displayObject.Type,"Goal"))
            {
                return (displayObject.IsComplete) ? GoalTodaysListCompleteTemplate : GoalTodaysListNotCompleteTemplate;
            }
            else if (string.Equals(displayObject.Type, "Routine"))
            {
                return (displayObject.IsComplete) ? RoutineTodaysListCompleteTemplate : RoutineTodaysListNotCompleteTemplate;
            }
            else
            {
                return EventTodaysListTemplate;
            }
        }
    }
}
