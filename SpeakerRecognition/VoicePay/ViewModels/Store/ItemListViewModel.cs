using System;
using System.Threading.Tasks;
using MvvmHelpers;
using VoicePay.Models;
using VoicePay.Services;
using VoicePay.Services.Interfaces;
using VoicePay.Views.Store;
using Xamarin.Forms;

namespace VoicePay.ViewModels.Store
{
    public class ItemListViewModel : BaseViewModel
    {
        private readonly IItemsService _itemsService;
        private readonly string _categoryName;
        public ObservableRangeCollection<Item> Products { get; set; }

        public ItemListViewModel(string categoryName) : this(categoryName, new ItemsService()) { }
        public ItemListViewModel(string categoryName, IItemsService itemsService)
        {
            _itemsService = itemsService;
            _categoryName = categoryName;
            Products = new ObservableRangeCollection<Item>();
        }

        public async Task GoToReview(Item item)
        {
            var bindingContext = new ProductReviewViewModel(item);
            await MasterNavigateTo(new ProductReviewPage { BindingContext = bindingContext });
        }

        public async Task OnAppearing()
        {
            IsBusy = true;

            var products = await _itemsService.GetByCategory(_categoryName);
            Products.ReplaceRange(products);

            IsBusy = false;
        }
    }
}
