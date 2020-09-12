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
        public DataTemplate GoalTodaysListTemplate { get; set; }
        public DataTemplate RoutineTodaysListTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            TodaysListTileDisplayObject displayObject = item as TodaysListTileDisplayObject;
            if (displayObject.Type == TileType.Goal)
            {
                return GoalTodaysListTemplate;
            }
            else if (displayObject.Type == TileType.Routine)
            {
                return RoutineTodaysListTemplate;
            }
            else
            {
                return EventTodaysListTemplate;
            }
        }
    }
}
