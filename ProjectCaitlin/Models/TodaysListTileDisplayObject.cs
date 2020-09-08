using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProjectCaitlin.Models
{
    public enum TileType
    {
        Routine, Goal, Event
    }

    public class TodaysListTileDisplayObject : IComparable<TodaysListTileDisplayObject>, INotifyPropertyChanged
    {
        public TimeSpan AvailableStartTime { get; set; }
        public TimeSpan AvailableEndTime { get; set; }
        public string ActualStartTime { get; set; }
        public string ActualEndTime { get; set; }
        public Color BorderColor { get; set; }
        public Color FrameBgColorComplete { set; private get; }
        public Color FrameBgColorInComplete { set; private get; }
        public Color FrameBgColor { get { return (isComplete) ? FrameBgColorComplete : FrameBgColorInComplete; } }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string TimeDifference { get; set; }
        public TileType Type { get; set; }
        public string Photo { get; set; }
        public string TimeMessage {get; set;}
        private bool isComplete;
        public bool IsComplete { 
            get { return this.isComplete; } 
            set {
                BorderColor = Color.Black; 
                this.isComplete = value; 
                OnPropertyChanged("IsComplete"); 
                OnPropertyChanged("BorderColor");
                OnPropertyChanged("FrameBgColor");
                OnPropertyChanged("IsNotComplete");
            } }
        public bool IsNotComplete { get { return !isComplete; } }
        private bool inProgress;
        public bool InProgress { get { return this.inProgress; } set { this.inProgress = value; OnPropertyChanged(); } }
        public bool IsSubListAvailable { get; set; }
        public int Index { get; set; }
        public object GratisObject { get; set; }
        public ICommand TouchCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public TodaysListTileDisplayObject() { }

        public TodaysListTileDisplayObject(bool inProgress, bool isComplete)
        {
            this.inProgress = inProgress;
            this.isComplete = isComplete;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public int CompareTo(TodaysListTileDisplayObject that)
        {
            if(this.AvailableStartTime < that.AvailableStartTime)
            {
                return -1;
            }else if (this.AvailableStartTime > that.AvailableStartTime)
            {
                return 1;
            }
            return this.AvailableStartTime.CompareTo(that.AvailableStartTime);
        }

    }
}
