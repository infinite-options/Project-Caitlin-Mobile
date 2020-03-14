using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using VoicePay.Models;
using VoicePay.Services;
using VoicePay.Services.Interfaces;
using VoicePay.Views.Store;
using Xamarin.Forms;

namespace VoicePay.ViewModels.Store
{
    public class CategoriesViewModel : BaseViewModel
    {
        private readonly ICategoriesService _categoriesService;

        private ObservableRangeCollection<Category> _categories;
        public ObservableRangeCollection<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; RaisePropertyChanged(); }
        }

        public ICommand CategoryTappedCommand { get; private set; }
        public ICommand GoToCartReviewCommand { get; private set; }


        public CategoriesViewModel() : this(new CategoriesService()) { }
        public CategoriesViewModel(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;

            Categories = new ObservableRangeCollection<Category>();

            CategoryTappedCommand = new Command<Category>(async (cat) => await CategoryTapped(cat));
            GoToCartReviewCommand = new Command(async () => await GoToCartReview());
        }


        public async Task OnAppearing()
        {
            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            IsBusy = true;

            var categories = await _categoriesService.GetAll();
            Categories.ReplaceRange(categories);

            IsBusy = false;
        }

        private async Task CategoryTapped(Category cat)
        {
            await MasterNavigateTo(new ItemListPage(cat.Description));
        }

        private async Task GoToCartReview()
        {
            await MasterNavigateTo(new CartReviewPage());
        }
    }
}
