using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using VoicePay.Helpers;
using VoicePay.Models;
using Xamarin.Forms;

namespace VoicePay.ViewModels.Store
{
    public class CartReviewViewModel : BaseViewModel
    {
        public List<CartItem> Products { get; set; }
        public double Total { get; set; }
        public ICommand GoAndPayCommand { get; set; }

        public CartReviewViewModel()
        {
            Products = Cart.Instance.GetAllItems();
            Total = Cart.Instance.Checkout();

            GoAndPayCommand = new Command(async () => await GoAndPay());
        }

        private async Task GoAndPay()
        {
            await MasterNavigateTo(new Views.Payment.PaymentReviewPage());
        }
    }
}
