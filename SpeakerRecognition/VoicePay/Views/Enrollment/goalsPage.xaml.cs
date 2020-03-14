using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VoicePay.Views.Enrollment
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class goalsPage : ContentPage
    {
        public goalsPage()
        {
            InitializeComponent();

            Timer timerCalc;
            int mins = 0; int secs = 0; int milliseconds = 0;

            timerCalc = new Timer();

            activity1.Clicked += delegate
            {
                timerCalc.Interval = 1;
                timerCalc.Elapsed += Timer_Elapsed;
                timerCalc.Start();
            };

            Done.Clicked += delegate
            {
                timerCalc.Stop();
                mins = 0; secs = 0; milliseconds = 0;
                Device.BeginInvokeOnMainThread(delegate
                {
                    timer.Text = String.Format("{0}:{1:00}:{2:000}", mins, secs, milliseconds);
                });
            };

            void Timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                milliseconds++;
                if (milliseconds >= 1000)
                {
                    secs++;
                    milliseconds = 0;
                }
                if (secs == 59)
                {
                    mins++;
                    secs = 0;
                }

                Device.BeginInvokeOnMainThread(delegate {

                    timer.Text = String.Format("{0}:{1:00}:{2:000}", mins, secs, milliseconds);
                });
            }

            activity2.Clicked += delegate
            {
                timerCalc.Interval = 1;
                timerCalc.Elapsed += Timer_Elapsed;
                timerCalc.Start();
            };

            Done2.Clicked += delegate
            {
                timerCalc.Stop();
                mins = 0; secs = 0; milliseconds = 0;
                Device.BeginInvokeOnMainThread(delegate
                {
                    timer.Text = String.Format("{0}:{1:00}:{2:000}", mins, secs, milliseconds);
                });
            };

            activity3.Clicked += delegate
            {
                timerCalc.Interval = 1;
                timerCalc.Elapsed += Timer_Elapsed;
                timerCalc.Start();
            };

            Done3.Clicked += delegate
            {
                timerCalc.Stop();
                mins = 0; secs = 0; milliseconds = 0;
                Device.BeginInvokeOnMainThread(delegate
                {
                    timer.Text = String.Format("{0}:{1:00}:{2:000}", mins, secs, milliseconds);
                });
            };

            activity4.Clicked += delegate
            {
                timerCalc.Interval = 1;
                timerCalc.Elapsed += Timer_Elapsed;
                timerCalc.Start();
            };

            Done4.Clicked += delegate
            {
                timerCalc.Stop();
                mins = 0; secs = 0; milliseconds = 0;
                Device.BeginInvokeOnMainThread(delegate
                {
                    timer.Text = String.Format("{0}:{1:00}:{2:000}", mins, secs, milliseconds);
                });
            };
        }
    }
}