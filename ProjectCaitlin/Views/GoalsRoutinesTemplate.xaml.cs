using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using ProjectCaitlin.ViewModel;

namespace ProjectCaitlin.Views
{
    public partial class GoalsRoutinesTemplate : ContentPage
    {
        readonly GoalsRoutinesTemplateViewModel pageModel;

        public GoalsRoutinesTemplate()
        { 
            InitializeComponent();
            pageModel = new GoalsRoutinesTemplateViewModel(this);
            BindingContext = pageModel;

        }

       



    }
}