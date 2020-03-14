using System;
using System.Threading.Tasks;
using System.Windows.Input;
using VoicePay.Helpers;
using VoicePay.Models;
using Xamarin.Forms;

namespace VoicePay.ViewModels.Store
{
    public class ProductReviewViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ICommand AddCommand { get; set; }

        private int _quantity = 1;
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; RaisePropertyChanged(); }
        }

        public ProductReviewViewModel(Item item)
        {
            Item = item;
            AddCommand = new Command(async () => await AddProduct());
        }

        private async Task AddProduct()
        {
            var shouldAdd = await DisplayAlert("Shop", $"Are you sure you want to add {Quantity} {Item.Name} to your cart?", "Yes", "No");
            if (shouldAdd)
            {
                Cart.Instance.AddItem(Item, Quantity);
                await MasterDetail.Detail.Navigation.PopToRootAsync();
            }
        }
    }
}
