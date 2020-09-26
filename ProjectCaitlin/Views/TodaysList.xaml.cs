using ProjectCaitlin.Methods;
using ProjectCaitlin.Models;
using ProjectCaitlin.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Essentials;
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

        private TimeSpan MorningStartTime;
        private TimeSpan AfternoonStartTime;
        private TimeSpan EveningStartTime;
        private TimeSpan NightStartTime;


        ObservableCollection<TodaysListTileDisplayObjectGroup> Items { get; set; }

        private TapGestureRecognizer TapGestureRecognizer;
        private readonly TodaysListViewModel ViewModel;
        public TodaysList()
        {
            InitializeComponent();

            LoadUI();

            TapGestureRecognizer = new TapGestureRecognizer();
            ViewModel = new TodaysListViewModel(Navigation, new WeakReference<TodaysList>(this, false));
            BindingContext = ViewModel;
            RefreshViewInstance.Command = new Command(() => { Refresh(null, null); RefreshViewInstance.IsRefreshing = false; });
        }


        private TodaysListTileDisplayObject ToTile(goal _goal)
        {
            string startTime = DateTime.Today.Add(_goal.availableStartTime).ToString("hh:mm tt");
            string endTime = DateTime.Today.Add(_goal.availableEndTime).ToString("hh:mm tt");
            TodaysListTileDisplayObject goalTile = new TodaysListTileDisplayObject(_goal.isInProgress, _goal.isComplete)
            {
                Type = TileType.Goal,
                AvailableEndTime = _goal.availableEndTime,
                AvailableStartTime = _goal.availableStartTime,
                ActualEndTime = _goal.dateTimeCompleted.ToString(),
                Title = _goal.title,
                SubTitle = "This is available from: \n"+startTime+" to "+endTime,
                IsSubListAvailable = _goal.isSublistAvailable,
                Photo = _goal.photo,
                FrameBgColorComplete = Color.FromHex("#E9E8E8"),
                FrameBgColorInComplete = Color.FromHex("#FFFFFF")
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
            TimeSpan differene = _routine.availableEndTime - _routine.availableStartTime;
            TodaysListTileDisplayObject routineTile = new TodaysListTileDisplayObject(_routine.isInProgress, _routine.isComplete)
            {
                Type = TileType.Routine,
                AvailableEndTime = _routine.availableEndTime,
                AvailableStartTime = _routine.availableStartTime,
                ActualEndTime = _routine.dateTimeCompleted.ToString(),
                Title = _routine.title,
                SubTitle = "This takes: " + ((int) differene.TotalMinutes).ToString() + " minutes",
                IsSubListAvailable = _routine.isSublistAvailable,
                Photo = _routine.photo,
                FrameBgColorComplete = Color.FromHex("#E9E8E8"),
                FrameBgColorInComplete = Color.FromHex("#FFFFFF")
            };
            routineTile.TouchCommand = new Command(async () => ViewModel.OnTileTapped(routineTile));

            return routineTile;
        }

        private TodaysListTileDisplayObject ToTile(routine _routine, int index)
        {
            TodaysListTileDisplayObject tile = ToTile(_routine);
            tile.Index = index;
            return tile;
        }

        private TodaysListTileDisplayObject ToTile(EventsItems _event)
        {
            TodaysListTileDisplayObject eventTile = new TodaysListTileDisplayObject()
            {
                Type = TileType.Event,
                AvailableEndTime = _event.End.DateTime.LocalDateTime.TimeOfDay,
                AvailableStartTime = _event.Start.DateTime.LocalDateTime.TimeOfDay,
                TimeDifference = _event.Start.DateTime.LocalDateTime.ToString("h:mm tt") + " - " + _event.End.DateTime.LocalDateTime.ToString("h:mm tt"),
                Title = _event.EventName,
                SubTitle = _event.Description,
                Photo = "calendarFive.png"
            };
            return eventTile;
        }

        private TodaysListTileDisplayObject ToTile(EventsItems _event, int index)
        {
            TodaysListTileDisplayObject tile = ToTile(_event);
            tile.Index = index;
            return tile;
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
            List<TodaysListTileDisplayObject> goalTiles;
            goalTiles = goalList.Select((value, index) => ToTile(value, index)).ToList();
            return goalTiles;
        }

        private List<TodaysListTileDisplayObject> ToTileList(List<routine> routineList)
        {
            List<TodaysListTileDisplayObject> routineTiles;
            routineTiles = routineList.Select((value, index) => ToTile(value, index)).ToList();
            return routineTiles;
        }

        private List<TodaysListTileDisplayObject> ToTileList(List<EventsItems> eventList)
        {
            List<TodaysListTileDisplayObject> eventTiles;
            eventTiles = eventList.Select((value, index) => ToTile(value, index)).ToList();
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
            if(tile.AvailableStartTime < MorningStartTime)
            {
                return Morning;
            }else if(tile.AvailableStartTime < AfternoonStartTime)
            {
                return Morning;
            }else if (tile.AvailableStartTime < EveningStartTime)
            {
                return Afternoon;
            }
            else if (tile.AvailableStartTime < NightStartTime)
            {
                return Evening;
            }
            return Night;
        }

        private void LoadTiles(List<goal> goals, List<routine> routines, List<EventsItems> events)
        {
            var tiles = PopulateTiles(goals, routines, events);
            Items = GroupTiles(tiles);
            MainThread.BeginInvokeOnMainThread(() => {
                TodaysListCollectionView.ItemsSource = Items;
            });
        }


        private void SetTimeOfDayStartTime()
        {
            MorningStartTime = (string.IsNullOrWhiteSpace(App.User.aboutMe.timeSettings.morning)) ? new TimeSpan(6, 0, 0) : TimeSpan.Parse(App.User.aboutMe.timeSettings.morning);
            AfternoonStartTime = (string.IsNullOrWhiteSpace(App.User.aboutMe.timeSettings.afternoon)) ? new TimeSpan(12, 0, 0) : TimeSpan.Parse(App.User.aboutMe.timeSettings.afternoon);
            EveningStartTime = (string.IsNullOrWhiteSpace(App.User.aboutMe.timeSettings.evening)) ? new TimeSpan(16, 0, 0) : TimeSpan.Parse(App.User.aboutMe.timeSettings.evening);
            NightStartTime = (string.IsNullOrWhiteSpace(App.User.aboutMe.timeSettings.night)) ? new TimeSpan(20, 0, 0) : TimeSpan.Parse(App.User.aboutMe.timeSettings.night);
        }

        private void LoadUI()
        {
            SetTimeOfDayStartTime();
            if (Device.RuntimePlatform == Device.iOS)
            {
                var mainDisplay = DeviceDisplay.MainDisplayInfo;
                var height = mainDisplay.Height;
                TodaysListCollectionView.HeightRequest = height / 2 - 100;
            }
            if(Device.RuntimePlatform == Device.Android)
            {
                NavBar.HeightRequest = 100;
            }
            LoadTiles(App.User.goals, App.User.routines, App.User.CalendarEvents);
            TodaysListCollectionView.Header = new Label
            {
                Text = DateTime.Now.DayOfWeek.ToString(),
                FontSize = 40,
                Padding = new Thickness(0, 40, 0, 0),
                Margin = new Thickness(0, 40, 0, 0)
            };
            
        }

        private async void OnAboutMe_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GreetingPage());
        }

        private async void Refresh(object sender, EventArgs e)
        {
            LoadingIndicator.IsRunning = true;
            try
            {
                await ProjectCaitlin.Services.FirestoreService.Instance.LoadDatabase();
                await ProjectCaitlin.Services.GoogleService.Instance.LoadTodaysEvents();
                LoadUI();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
            finally
            {
                LoadingIndicator.IsRunning = false;
            }
        }
    }
}