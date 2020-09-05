using ProjectCaitlin.Methods;
using ProjectCaitlin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectCaitlin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodaysList : ContentPage
    {
        List<TodaysListTileDisplayObject> Items { get; set; }

        public TodaysList()
        {
            InitializeComponent();

            Items = PopulateTiles(App.User.goals, App.User.routines, App.User.CalendarEvents);

            TodaysListCollectionView.ItemsSource = Items;
        }

        private TodaysListTileDisplayObject ToTile(goal _goal)
        {
            TodaysListTileDisplayObject goalTile = new TodaysListTileDisplayObject()
            {
                Type = "Goal",
                AvailableEndTime = _goal.availableEndTime.ToString(),
                AvailableStartTime = _goal.availableStartTime.ToString(),
                ActualEndTime = _goal.dateTimeCompleted.ToString(),
                Title = _goal.title,
                SubTitle = "Starts in few minutes",
                IsComplete = _goal.isComplete,
                InProgress = _goal.isInProgress,
                IsSubListAvailable = _goal.isSublistAvailable,
                Photo = _goal.photo
            };
            return goalTile;
        }

        private TodaysListTileDisplayObject ToTile(routine _routine)
        {
            TodaysListTileDisplayObject goalTile = new TodaysListTileDisplayObject()
            {
                Type = "Routine",
                AvailableEndTime = _routine.availableEndTime.ToString(),
                AvailableStartTime = _routine.availableStartTime.ToString(),
                ActualEndTime = _routine.dateTimeCompleted.ToString(),
                Title = _routine.title,
                SubTitle = "This will run for an hour",
                IsComplete = _routine.isComplete,
                InProgress = _routine.isInProgress,
                IsSubListAvailable = _routine.isSublistAvailable,
                Photo = _routine.photo
            };
            return goalTile;
        }

        private TodaysListTileDisplayObject ToTile(EventsItems _event)
        {
            TodaysListTileDisplayObject goalTile = new TodaysListTileDisplayObject()
            {
                Type = "Event",
                AvailableEndTime = _event.Start.DateTime.ToString("h:mm tt"),
                AvailableStartTime = _event.End.DateTime.ToString("h:mm tt"),
                TimeDifference = _event.Start.DateTime.ToString("h:mm tt") + " - " + _event.End.DateTime.ToString("h:mm tt"),
                Title = _event.EventName,
                SubTitle = _event.Description,
                Photo = "eventIcon.jpg"
            };
            return goalTile;
        }


        private void MergeTiles(List<TodaysListTileDisplayObject> inputlist, List<TodaysListTileDisplayObject> outputList)
        {
            foreach(TodaysListTileDisplayObject item in inputlist)
            {
                outputList.Add(item);
            }
        }

        private List<TodaysListTileDisplayObject> ToTileList(List<goal> goalList)
        {
            List<TodaysListTileDisplayObject> goalTiles = new List<TodaysListTileDisplayObject>();
            
            for(int idx=0; idx < goalList.Count; idx++)
            {
                TodaysListTileDisplayObject tile = ToTile(goalList[idx]);
                tile.Index = idx;
                goalTiles.Add(tile);
            }
            return goalTiles;
        }

        private List<TodaysListTileDisplayObject> ToTileList(List<routine> routineList)
        {
            List<TodaysListTileDisplayObject> routineTiles = new List<TodaysListTileDisplayObject>();
            for (int idx = 0; idx < routineList.Count; idx++)
            {
                TodaysListTileDisplayObject tile = ToTile(routineList[idx]);
                tile.Index = idx;
                routineTiles.Add(tile);
            }
            return routineTiles;
        }

        private List<TodaysListTileDisplayObject> ToTileList(List<EventsItems> eventList)
        {
            List<TodaysListTileDisplayObject> eventTiles = new List<TodaysListTileDisplayObject>();
            for (int idx = 0; idx < eventList.Count; idx++)
            {
                TodaysListTileDisplayObject tile = ToTile(eventList[idx]);
                tile.Index = idx;
                eventTiles.Add(tile);
            }
            return eventTiles;
        }

        private List<TodaysListTileDisplayObject> PopulateTiles(List<goal> goals, List<routine> routines, List<EventsItems> events)
        {
            List <TodaysListTileDisplayObject> Tiles = ToTileList(goals);
            MergeTiles(ToTileList(routines), Tiles);
            MergeTiles(ToTileList(events), Tiles);

            //Tiles.Sort();
            return Tiles;
        }
    }
}