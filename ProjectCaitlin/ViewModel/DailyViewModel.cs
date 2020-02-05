using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using ProjectCaitlin;
using System.Runtime.CompilerServices;

namespace ProjectCaitlin.ViewModel
{
    public class DailyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static DailyViewModel _instance;
        public static DailyViewModel Instance
        {
            get { return _instance ?? (_instance = new DailyViewModel()); }
        }


        //-------------------------------------------- 1
        //--Update TEXT from CALENDAR
        private string btn1Text = "Updating your events";

        public string Btn1Text
        {
            get => btn1Text;

            set
            {
                btn1Text = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Btn1Text)));
            }
        }

        public ICommand UpdateBtn1 =>
            new Command(() => { Btn1Text = ListViewPage.eventNameList[0]; });


        //--Change numLABEL COLOR to TRANSPARENT
        private string _colorNumLbl1Trans = Color.Default.ToString();

        public string ColorNumLbl1Trans
        {
            get { return _colorNumLbl1Trans; }
            set
            {
                if (value == _colorNumLbl1Trans)
                    return;

                _colorNumLbl1Trans = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ColorNumLbl1Trans)));
            }
        }

        public ICommand UpdateColorNumLbl1Trans =>
            new Command(() => { ColorNumLbl1Trans = "#FAFAFA"; });

        //--Change dayLABEL COLOR to TRANSPARENT
        private string _colorDayLbl1Trans = Color.Default.ToString();

        public string ColorDayLbl1Trans
        {
            get { return _colorDayLbl1Trans; }
            set
            {
                if (value == _colorDayLbl1Trans)
                    return;

                _colorDayLbl1Trans = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ColorDayLbl1Trans)));
            }
        }

        public ICommand UpdateColorDayLbl1Trans =>
            new Command(() => { ColorDayLbl1Trans = "#FAFAFA"; });

        //EXPERIMENTAL
        //private string _colorLbl1Trans = "#FAFAFA";

        //public string ColorLbl1Trans
        //{
        //    get { return _colorLbl1Trans; }
        //}

        //public ICommand UpdateColorLbl1Trans =>
        //    new Command(() => { ColorLbl1Trans = "#757575"; });



        //--Change BUTTON COLOR from TRANSPARENT
        //public Color _colorBtnFromTrans = Color.FromHex("FAFAFA");

        //public Color ColorBtn1FromTrans
        //{
        //    get { return _colorBtnFromTrans; }
        //    set
        //    {
        //        if (value == _colorBtnFromTrans)
        //            return;

        //        _colorLblTrans = value;
        //        PropertyChanged(this, new PropertyChangedEventArgs(nameof(ColorBtn1FromTrans)));
        //    }
        //}

        //public ICommand UpdateColorBtn1FromTrans =>
        //    new Command(() => { ColorBtn1FromTrans = Color.FromHex("56b880"); });

        //--------------------------------------------


        //-------------------------------------------- 2
        private string btn2Text;

        public string Btn2Text
        {
            get => btn2Text;

            set
            {
                btn2Text = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Btn2Text)));
            }
        }

        public ICommand UpdateBtn2 =>
            new Command(() => { Btn2Text = ListViewPage.eventNameList[1]; });
        //-------------------------------------------- 

    }
}
