using ProjectCaitlin;
using ProjectCaitlin.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using System;
using System.ComponentModel;


namespace ProjectCaitlin.ViewModel
{
    public class StepsPageViewModel : BindableObject
    {
        private StepsPage mainPage;
        public int count = 0;
        public string Text { get; set; }
        public Command ItemTappedCommand { get; set; }
        public string photo { get; set; }
        public string TopImage { get; set; }
        public string TopLabel { get; set; }
        private ObservableCollection<object> _items = new ObservableCollection<object>() { };

        public StepsPageViewModel(StepsPage mainPage, int a, int b, bool isRoutine)
        {
            this.mainPage = mainPage;

            if (isRoutine)
            {
                TopImage = App.user.routines[a].photo;
                TopLabel = App.user.routines[a].title;
            }
            else
            {

            }
        }
    }
}
