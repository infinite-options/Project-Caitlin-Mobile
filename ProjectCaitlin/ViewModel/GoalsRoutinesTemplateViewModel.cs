using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using FFImageLoading;
using PanCardView.Extensions;

namespace GoalsRoutinesTemplateViewModel.ViewModels
{
    public sealed class GoalsRoutinesTemplateViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _currentIndex;
        private int _imageCount = 1078;

        public GoalsRoutinesTemplateViewModel()
        {
            Items = new ObservableCollection<object>
            {
                new { Source = "toothbrushCircle.png",
                    Ind = _imageCount++,
                    Color = Color.Default,
                    Title = "Get ready for my day",
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!" },

                new { Source = "toothbrushCircle",
                    Ind = _imageCount++,
                    Color = Color.FromHex("FFBBBB"),
                    Title = "Meeting with Not Impossible Labs",
                    Length = "Should take about 1 hour" },

                new { Source = CreateSource(),
                    Ind = _imageCount++,
                    Color = Color.Default,
                    Title = "Make myself some lunch",
                    Length = "Takes from 10 to 30 minutes",
                    Text = "Click this card to start!"},

                new { Source = CreateSource(), Ind = _imageCount++, Color = Color.FromHex("BBD8FF"), Title = "Browse Pinterest projects", Length = "" },
                new { Source = CreateSource(), Ind = _imageCount++, Color = Color.Default, Title = "Eat dinner with mom", Length = "Takes about 30 minutes" },
                new { Source = CreateSource(), Ind = _imageCount++, Color = Color.Default, Title = "Finish my chemistry homework", Length = "Could take up to 1 hour" }
            };

            PanPositionChangedCommand = new Command(v =>
            {
                if (IsAutoAnimationRunning || IsUserInteractionRunning)
                {
                    return;
                }

                var index = CurrentIndex + (bool.Parse(v.ToString()) ? 1 : -1);
                if (index < 0 || index >= Items.Count)
                {
                    return;
                }
                CurrentIndex = index;
            });

            RemoveCurrentItemCommand = new Command(() =>
            {
                if (!Items.Any())
                {
                    return;
                }
                Items.RemoveAt(CurrentIndex.ToCyclicalIndex(Items.Count));
            });

            GoToLastCommand = new Command(() =>
            {
                CurrentIndex = Items.Count - 1;
            });
        }

        public ICommand PanPositionChangedCommand { get; }

        public ICommand RemoveCurrentItemCommand { get; }

        public ICommand GoToLastCommand { get; }

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                _currentIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentIndex)));
            }
        }

        public bool IsAutoAnimationRunning { get; set; }

        public bool IsUserInteractionRunning { get; set; }

        public ObservableCollection<object> Items { get; }

        private string CreateSource()
        {
            var source = $"toothbrushCircle.png";
            return source;
        }
    }
}