using NUnit.Framework;
using System;
using VoicePay.Helpers;

namespace VoicePay.UnitTests
{
    [TestFixture()]
    public class CartTests
    {
        [TearDown]
        public void EmptyCart()
        {
            Cart.Instance.Clear();
        }

        [Test()]
        public void NormalSumSameItem()
        {
            Cart.Instance.AddItem(new Models.Item { Name = "Laptop", Price = 1000 }, 2);
            var total = Cart.Instance.Checkout();

            Assert.AreEqual(2000, total);
        }

        [Test()]
        public void NormalSumDifferentItems()
        {
            Cart.Instance.AddItem(new Models.Item { Name = "Laptop", Price = 1000 }, 2);
            Cart.Instance.AddItem(new Models.Item { Name = "Phone", Price = 500 }, 2);
            var total = Cart.Instance.Checkout();

            Assert.AreEqual(3000, total);
        }

        [Test()]
        public void DoubledSumSameItems()
        {
            Cart.Instance.AddItem(new Models.Item { Name = "Laptop", Price = 1000 }, 2);
            Cart.Instance.AddItem(new Models.Item { Name = "Laptop", Price = 1000 }, 3);
            var total = Cart.Instance.Checkout();

            Assert.AreEqual(5000, total);
        }

        [Test()]
        public void DoubledSumDifferentItems()
        {
            Cart.Instance.AddItem(new Models.Item { Name = "Laptop", Price = 1000 }, 2);
            Cart.Instance.AddItem(new Models.Item { Name = "Laptop", Price = 1000 }, 3);
            Cart.Instance.AddItem(new Models.Item { Name = "Phone", Price = 500 }, 2);
            Cart.Instance.AddItem(new Models.Item { Name = "Phone", Price = 500 }, 4);
            var total = Cart.Instance.Checkout();

            Assert.AreEqual(8000, total);
        }
    }
}
