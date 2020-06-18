using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectCaitlin.ViewModel;
using ProjectCaitlin.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.AudioRecorder;
using VoiceRecognition.View;
using VoiceRecognition.ViewModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms.Markup;

namespace ProjectCaitlin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GreetingPage : ContentPage
    {

        public GreetingViewModel greetingViewModel;


        TimeSpan morningStart = new TimeSpan(6, 0, 0);
        TimeSpan morningEnd = new TimeSpan(11, 0, 0);
        TimeSpan afternoonStart = new TimeSpan(11, 0, 0);
        TimeSpan afternoonEnd = new TimeSpan(18, 0, 0);
        TimeSpan eveningStart = new TimeSpan(18, 0, 0);
        TimeSpan eveningEnd = new TimeSpan(23, 59, 59);
        TimeSpan nightStart = new TimeSpan(0, 0, 0);
        TimeSpan nightEnd = new TimeSpan(6, 0, 0);
        AudioRecorderService recorder;

        public GreetingPage()
        {

            InitializeComponent();

            greetingViewModel = new GreetingViewModel(this);
            BindingContext = greetingViewModel;

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromHex("#E9E8E8");
            SetupUI();

            recorder = new AudioRecorderService
            {
                StopRecordingOnSilence = true, //will stop recording after 2 seconds (default)
                StopRecordingAfterTimeout = true,  //stop recording after a max timeout (defined below)
                TotalAudioTimeout = TimeSpan.FromSeconds(10) //audio will stop recording after 15 seconds
            };

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(35000);
                    if (identifyButton! != null && !identifyButton.IsVisible)
                    {
                        greetingViewModel.CMDIdentifyAndEnroll();
                    }
                }
            });
            trackBar.Text = SlideToActView.States.Manual;
            trackBarFrame.BackgroundColor = Color.Gray;
        }
        private void SetupUI()
        {
            UserImage.Source = App.User.aboutMe.pic;
            GreetingsTitleLabel.Text = GetTitleDayMessage();
            FirstNameLabel.Text = App.User.firstName;
            MessageCardLabel.Text = App.User.aboutMe.message_card;
            MessageLabel.Text = App.User.aboutMe.message_day;

            if (App.User.people.Count == 0)
            {
                importantPeopleSL.IsVisible = false;
            }

        }

        public String GetTitleDayMessage()
        {
            var completeTime = DateTime.Now.TimeOfDay;
            if (morningStart <= completeTime && completeTime < morningEnd)
            {
                //Console.WriteLine("Morning");

                return "Good Morning";
            }
            if (afternoonStart <= completeTime && completeTime < afternoonEnd)
            {
                //Console.WriteLine("Afternoon");

                return "Good Afternoon";
            }
            if (eveningStart <= completeTime && completeTime < eveningEnd)
            {
                //Console.WriteLine("Evening");

                return "Good Evening";
            }
            if (nightStart <= completeTime && completeTime <= nightEnd)
            {
                //Console.WriteLine("Night");

                return "Good Night";
            }

            return "";
        }

        async void LogoutBtnClick(object sender, EventArgs e)
        {
            Application.Current.Properties.Remove("access_token");
            Application.Current.Properties.Remove("refreshToken");
            Application.Current.Properties.Remove("user_id");

            await Navigation.PushAsync(new LoginPage());
        }

        async void btn1Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListViewPage());
        }

        async void btn2Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GoalsRoutinesTemplate());
        }

        public void UpdateSlider(Color color, String label) {
            if (color != null)
            {
                trackBarFrame.BackgroundColor = color;
            }
            trackBar.Text = label; 
        }

        public void ResetSlider()
        {
            trackBarFrame.BackgroundColor = Color.Gray;
            trackBar.Text = identifyButton.IsVisible ? SlideToActView.States.Manual : SlideToActView.States.AlwaysOn;
        }

        void Handle_SlideCompleted(object sender, System.EventArgs e)
        {
            if (trackBar.Text == SlideToActView.States.Manual)
            {
                identifyButton.IsVisible = false;
                trackBar.Text = SlideToActView.States.AlwaysOn;

            }
            else if (trackBar.Text == SlideToActView.States.AlwaysOn)
            {
                identifyButton.IsVisible = true;
                trackBar.Text = SlideToActView.States.Manual;
            }
        }

        void enrollClicked(object sender, EventArgs e)
        {
            // multi-thread timer for speaker
            /*Task.Run(() =>
            {
                int i = 30;
                Device.StartTimer(new TimeSpan(0, 0, 60), () =>
                {
                    timer.IsVisible = true;

                    // do something every 60 seconds
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        timer.Text = i + "";
                        i--;
                    });
                    if (i <= 0)
                        return false;
                    else
                        return true; // return true to repeat counting, false to stop timer
                });
                greetingViewModel.CMDIdentifyAndEnroll();

            });

            timer.IsVisible = false;*/

            //if (identifyButton.IsVisible)
            //{
            Task.Run(() => { greetingViewModel.CMDIdentifyAndEnroll_Manual(); });

            Task.Factory.StartNew(() => {
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    this.timer.IsVisible = true;
                    //});

                    for (int i = 30; i > 0; i--)
                    {
                        System.Threading.Thread.Sleep(1000);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            //this.timer.Text = string.Format("{0}", i);
                            this.trackBar.Text = string.Format("{0}\nTime Left {1}",SlideToActView.States.Recording,i);
                        });
                    }
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    this.trackBar.Text = "Sending to Azure";
                    //    //this.timer.IsVisible = false;
                    //});
                });
            //}
            //else
            //{
            //    //Task.Run(() => { greetingViewModel.CMDIdentifyAndEnroll(); });
            //}
        }

        void identifyClicked(object sender, EventArgs e)
        {
            greetingViewModel.CMDIdentifyAndEnroll();
            Task.Factory.StartNew(() => {
                System.Threading.Thread.Sleep(500);
                int i = 0;
                while (greetingViewModel.IsRecording())
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        this.trackBar.Text = string.Format("{0}\n{1}",SlideToActView.States.Recording,i);
                    });
                    i += 1;
                    System.Threading.Thread.Sleep(1000);
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.trackBar.Text = SlideToActView.States.SendingToAzure;
                    //this.timer.IsVisible = false;
                });
            });
        }
    }

    //Slide button for the speaker.
    public class SlideToActView : AbsoluteLayout
    {
        public static class States { 
            public static String Manual { get { return "Manual"; } }
            public static String AlwaysOn { get { return "Always On"; } }
            public static String Recording { get { return "Recording"; } }
            public static String SendingToAzure { get { return "Sending to Azure"; } }
            public static String SendingToFirebase { get { return "Sending to Firebase"; } }
        }

        public static readonly BindableProperty ThumbProperty =
            BindableProperty.Create(
                "Thumb", typeof(View), typeof(SlideToActView),
                defaultValue: default(View), propertyChanged: OnThumbChanged);

        public View Thumb
        {
            get { return (View)GetValue(ThumbProperty); }
            set { SetValue(ThumbProperty, value); }
        }

        private static void OnThumbChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((SlideToActView)bindable).OnThumbChangedImpl((View)oldValue, (View)newValue);
        }

        protected virtual void OnThumbChangedImpl(View oldValue, View newValue)
        {
            OnSizeChanged(this, EventArgs.Empty);
        }

        public static readonly BindableProperty TrackBarProperty =
            BindableProperty.Create(
                "TrackBar", typeof(View), typeof(SlideToActView),
                defaultValue: default(View), propertyChanged: OnTrackBarChanged);

        public View TrackBar
        {
            get { return (View)GetValue(TrackBarProperty); }
            set { SetValue(TrackBarProperty, value); }
        }

        private static void OnTrackBarChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((SlideToActView)bindable).OnTrackBarChangedImpl((View)oldValue, (View)newValue);
        }

        protected virtual void OnTrackBarChangedImpl(View oldValue, View newValue)
        {
            OnSizeChanged(this, EventArgs.Empty);
        }

        private PanGestureRecognizer _panGesture = new PanGestureRecognizer();
        private View _gestureListener;
        public SlideToActView()
        {
            _panGesture.PanUpdated += OnPanGestureUpdated;
            SizeChanged += OnSizeChanged;

            _gestureListener = new ContentView { BackgroundColor = Color.White, Opacity = 0.05 };
            _gestureListener.GestureRecognizers.Add(_panGesture);
        }

        public event EventHandler SlideCompleted;

        private const double _fadeEffect = 0.5;
        private const uint _animLength = 50;
        async void OnPanGestureUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (Thumb == null | TrackBar == null)
                return;

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    await TrackBar.FadeTo(_fadeEffect, _animLength);
                    break;

                case GestureStatus.Running:
                    // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                    var x = Math.Max(0, e.TotalX);
                    if (x > (Width - Thumb.Width))
                        x = (Width - Thumb.Width);

                    if (e.TotalX < Thumb.TranslationX)
                        return;
                    Thumb.TranslationX = x;
                    break;

                case GestureStatus.Completed:
                    var posX = Thumb.TranslationX;

                    // Reset translation applied during the pan (snap effect)
                    await TrackBar.FadeTo(1, _animLength);
                    await Thumb.TranslateTo(0, 0, _animLength * 2, Easing.CubicIn);

                    if (posX >= (Width - Thumb.Width - 10/* keep some margin for error*/))
                        SlideCompleted?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        void OnSizeChanged(object sender, EventArgs e)
        {
            if (Width == 0 || Height == 0)
                return;
            if (Thumb == null || TrackBar == null)
                return;


            Children.Clear();

            SetLayoutFlags(TrackBar, AbsoluteLayoutFlags.SizeProportional);
            SetLayoutBounds(TrackBar, new Rectangle(0, 0, 1, 1));
            Children.Add(TrackBar);

            SetLayoutFlags(Thumb, AbsoluteLayoutFlags.None);
            SetLayoutBounds(Thumb, new Rectangle(0, 0, this.Width / 5, this.Height));
            Children.Add(Thumb);

            SetLayoutFlags(_gestureListener, AbsoluteLayoutFlags.SizeProportional);
            SetLayoutBounds(_gestureListener, new Rectangle(0, 0, 1, 1));
            Children.Add(_gestureListener);
        }
    }
}
