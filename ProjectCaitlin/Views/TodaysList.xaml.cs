using ProjectCaitlin.Methods;
using ProjectCaitlin.Models;
using ProjectCaitlin.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectCaitlin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodaysList : ContentPage
    {
        internal class PartOfDay
        {
            public string Name;
            public string Photo;
        }

        readonly PartOfDay EarlyMorning = new PartOfDay() { Name = "Early Morning", Photo="moon.png" };
        readonly PartOfDay Morning = new PartOfDay() { Name = "Morning", Photo = "sunrisemid.png" };
        readonly PartOfDay Afternoon = new PartOfDay() { Name = "Afternoon", Photo = "fullsun.png" };
        readonly PartOfDay Evening = new PartOfDay() { Name = "Evening", Photo="sunriselow.png" };
        readonly PartOfDay Night = new PartOfDay() { Name = "Night", Photo="moon.png"};

        ObservableCollection<TodaysListTileDisplayObjectGroup> Items { get; set; }

        private TapGestureRecognizer TapGestureRecognizer;
        private TodaysListViewModel ViewModel;
        public TodaysList()
        {
            InitializeComponent();

            LoadUI();

            TapGestureRecognizer = new TapGestureRecognizer();
            ViewModel = new TodaysListViewModel(Navigation, new WeakReference<TodaysList>(this, false));
            BindingContext = ViewModel;
            
        }

        private TodaysListTileDisplayObject ToTile(goal _goal)
        {
            TodaysListTileDisplayObject goalTile = new TodaysListTileDisplayObject(_goal.isInProgress, _goal.isComplete)
            {
                Type = TileType.Goal,
                AvailableEndTime = _goal.availableEndTime,
                AvailableStartTime = _goal.availableStartTime,
                ActualEndTime = _goal.dateTimeCompleted.ToString(),
                Title = _goal.title,
                SubTitle = "Starts in few minutes",
                IsSubListAvailable = _goal.isSublistAvailable,
                Photo = _goal.photo,
                FrameBgColorComplete = Color.FromHex("#E9E8E8"),
                FrameBgColorInComplete = Color.FromHex("#FFFFFF")
                //GratisObject = _goal
            };

            goalTile.TouchCommand = new Command(async () => ViewModel.OnTileTapped(goalTile));

            return goalTile;
        }

        private TodaysListTileDisplayObject ToTile(goal _goal, int index)
        {
            TodaysListTileDisplayObject tile = ToTile(_goal);
            tile.Index = index;
            return tile;
        }

        private TodaysListTileDisplayObject ToTile(routine _routine)
        {
            TodaysListTileDisplayObject routineTile = new TodaysListTileDisplayObject(_routine.isInProgress, _routine.isComplete)
            {
                Type = TileType.Routine,
                AvailableEndTime = _routine.availableEndTime,
                AvailableStartTime = _routine.availableStartTime,
                ActualEndTime = _routine.dateTimeCompleted.ToString(),
                Title = _routine.title,
                SubTitle = "This will run for an hour",
                IsSubListAvailable = _routine.isSublistAvailable,
                Photo = _routine.photo,
                //GratisObject = _routine
            };
            routineTile.TouchCommand = new Command(async () => ViewModel.OnTileTapped(routineTile));

            return routineTile;
        }

        private TodaysListTileDisplayObject ToTile(EventsItems _event)
        {
            TodaysListTileDisplayObject eventTile = new TodaysListTileDisplayObject()
            {
                Type = TileType.Event,
                AvailableEndTime = _event.Start.DateTime.LocalDateTime.TimeOfDay,
                AvailableStartTime = _event.End.DateTime.LocalDateTime.TimeOfDay,
                TimeDifference = _event.Start.DateTime.LocalDateTime.ToString("h:mm tt") + " - " + _event.End.DateTime.LocalDateTime.ToString("h:mm tt"),
                Title = _event.EventName,
                SubTitle = _event.Description,
                Photo = "eventIcon.jpg"
            };
            return eventTile;
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
            //List<TodaysListTileDisplayObject> goalTiles = new List<TodaysListTileDisplayObject>();
            List<TodaysListTileDisplayObject> goalTiles;
            //for (int idx = 0; idx < goalList.Count; idx++)
            //{
            //    TodaysListTileDisplayObject tile = ToTile(goalList[idx]);
            //    tile.Index = idx;
            //    goalTiles.Add(tile);
            //}
            goalTiles = goalList.Select((value, index) => ToTile(value, index)).ToList();
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

        private ObservableCollection<TodaysListTileDisplayObject> PopulateTiles(List<goal> goals, List<routine> routines, List<EventsItems> events)
        {
            List<TodaysListTileDisplayObject> Tiles = ToTileList(goals);
            MergeTiles(ToTileList(routines), Tiles);
            MergeTiles(ToTileList(events), Tiles);

            Tiles.Sort();
            return new ObservableCollection<TodaysListTileDisplayObject>(Tiles);
        }

        private ObservableCollection<TodaysListTileDisplayObjectGroup> GroupTiles(ObservableCollection<TodaysListTileDisplayObject> Tiles)
        {
            List<TodaysListTileDisplayObjectGroup> group = new List<TodaysListTileDisplayObjectGroup>();
            group = Tiles.GroupBy(tile => GetPartOfDay(tile)).Select( 
                group => new TodaysListTileDisplayObjectGroup(group.Key.Name, new ObservableCollection<TodaysListTileDisplayObject>(group.ToList())) 
                { GroupIcon = group.Key.Photo}).ToList();

            return new ObservableCollection<TodaysListTileDisplayObjectGroup>(group);
        }

        private PartOfDay GetPartOfDay(TodaysListTileDisplayObject tile)
        {
            if(tile.AvailableStartTime < new TimeSpan(6, 0, 0))
            {
                return Morning;
            }else if(tile.AvailableStartTime < new TimeSpan(12, 0, 0))
            {
                return Morning;
            }else if (tile.AvailableStartTime < new TimeSpan(14, 0, 0))
            {
                return Afternoon;
            }
            else if (tile.AvailableStartTime < new TimeSpan(20, 0, 0))
            {
                return Evening;
            }
            return Night;
        }

        private void LoadTiles(List<goal> goals, List<routine> routines, List<EventsItems> events)
        {
            var tiles = PopulateTiles(goals, routines, events);
            Items = GroupTiles(tiles);
            TodaysListCollectionView.ItemsSource = Items;
        }

        private void LoadUI()
        {
            LoadTiles(App.User.goals, App.User.routines, App.User.CalendarEvents);
            TodaysListCollectionView.Header = new Label
            {
                Text = DateTime.Now.DayOfWeek.ToString(),
                FontSize = 40,
                Padding = new Thickness(0, 80, 0, 0)
            };
            
        }

        private void OnAboutMe_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AboutMe());
        }

        private void Refresh(object sender, EventArgs e)
        {
            try
            {
                Task loadingStatus = ProjectCaitlin.Services.FirestoreService.Instance.LoadDatabase();
                loadingStatus.Wait();
                LoadUI();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }
    }
}