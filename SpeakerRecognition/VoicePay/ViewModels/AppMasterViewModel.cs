using System;
using System.Collections.ObjectModel;
using VoicePay.Views;
using VoicePay.Views.Enrollment;
using VoicePay.Views.Store;

namespace VoicePay.ViewModels
{
    public class AppMasterViewModel : BaseViewModel
    {
        public ObservableCollection<MasterPageMenuItem> MenuItems { get; set; }

        public AppMasterViewModel()
        {
            MenuItems = new ObservableCollection<MasterPageMenuItem>(new[]
            {
                new MasterPageMenuItem { Id = 0, Title = "Store", TargetType = typeof(CategoriesPage), IconPath = "ic_view_dashboard.png" },
                new MasterPageMenuItem { Id = 1, Title = "Train Voiceprint", TargetType = typeof(WelcomePage), IconPath = "ic_face.png" },
                new MasterPageMenuItem { Id = 2, Title = "Settings", IconPath = "ic_book_open_page_variant.png" }
            });
        }
    }
}
