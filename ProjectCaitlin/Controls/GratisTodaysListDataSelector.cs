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
            if (item.GetType() == typeof(goal))
            {
                return ((item as goal).isComplete) ? GoalTodaysListCompleteTemplate : GoalTodaysListNotCompleteTemplate;
            }
            else if (item.GetType() == typeof(routine))
            {
                return ((item as routine).isComplete) ? RoutineTodaysListCompleteTemplate : RoutineTodaysListNotCompleteTemplate;
            }
            else
            {
                return EventTodaysListTemplate;
            }
        }
    }
}
