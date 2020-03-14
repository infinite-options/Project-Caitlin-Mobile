using System;
using System.Collections.Generic;
using VoicePay.Models;
using System.Linq;

namespace VoicePay.Helpers
{
    public class Cart
    {
        #region Singleton
        private static Cart _instance = new Cart();
        public static Cart Instance => _instance;
        #endregion

        private readonly List<CartItem> _items = new List<CartItem>();

        public void AddItem(Item item, int quantity)
        {
            var registeredProduct = _items.FirstOrDefault(i => i.Name == item.Name);
            if (registeredProduct == null)
            {
                _items.Add(new CartItem(item) { Quantity = quantity });
            }
            else
            {
                registeredProduct.Quantity += quantity;
            }
        }

        public List<CartItem> GetAllItems()
        {
            return _items;
        }

        public double Checkout()
        {
            return _items.Sum(p => p.Total);
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}
