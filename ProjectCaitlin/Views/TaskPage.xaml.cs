using ProjectCaitlin.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectCaitlin.Services;

namespace ProjectCaitlin.Views
{
    public delegate void UpdateParentRoutine();

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskPage : ContentPage
    {
        public TaskGridViewModel taskGridViewModel;

        FirebaseFunctionsService firebaseFunctionsService = new FirebaseFunctionsService();

        GRItemModel GRItemModel;
        public UpdateParentRoutine updateParentRoutine;

        public TaskPage(int routineNum, bool isRoutine)
        {
            InitializeComponent();

            taskGridViewModel = new TaskGridViewModel(this, routineNum, isRoutine, GRItemModel);
            BindingContext = taskGridViewModel;

        }

        public TaskPage(int routineNum, bool isRoutine, GRItemModel _GRItemModel)
        {
            InitializeComponent();

            GRItemModel = _GRItemModel;

            taskGridViewModel = new TaskGridViewModel(this, routineNum, isRoutine, GRItemModel);
            BindingContext = taskGridViewModel;

        }

        public TaskPage(int routineNum, bool isRoutine, GRItemModel _GRItemModel, UpdateParentRoutine updateParentRoutine) : this(routineNum, isRoutine, _GRItemModel)
        {
            this.updateParentRoutine = updateParentRoutine;
        }
        public async void close(object sender, EventArgs args)
        {
            if (App.ParentPage == "ListView")
                await Navigation.PushAsync(new ListViewPage());
            else
                await Navigation.PopAsync();
        }
    }

}
