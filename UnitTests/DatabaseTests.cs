using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POS_PointOfSales.ViewModels;
using POS_ViewsLibrary;
using POS_DataLibrary;
using POS_SellersApp.ViewModels;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace UnitTests
{
    [TestClass]
    public class DatabaseTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewProduct_ThrowsException_WhenEmptyImagePathIsProvided()
        {
            var db = new Database();
            string path = "";
                Product product = new Product()
                {
                    CategoryId = 1,
                    Name = "ProductTest",
                    Price = 5.2M,
                    UPCCode = "SAL01"

                };
                db.addProduct(product, path);
        }
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void AddNewProduct_ThrowsException_WhenProductNameISNull()
        {
            var db = new Database();
            string path = "Test.png";
            Product product = new Product()
            {
                CategoryId = 1,
                Name = null,
                Price = 5.2M,
                UPCCode = "SAL01"

            };
            db.addProduct(product, path);
        }
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void AddNewProduct_ThrowsException_WhenImagePathISNotCorrect()
        {
            var db = new Database();
            string path = "Test.jpg";
            Product product = new Product()
            {
                CategoryId = 1,
                Name = null,
                Price = 5.2M,
                UPCCode = "SAL01"

            };
            db.addProduct(product, path);
        }
        [TestMethod]
        public void GetTopFivItemsPerMotnhShouldReturnFiveItems()
        {
            var db = new Database();
            ObservableCollection<Sales> salesListTest = db.getTopItemsPerMonth(9, 2016);
            Assert.AreEqual(5, salesListTest.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void SQLTransactionRollsBackWhenOrderIsNotValidButOrderItemsIsValid()
        {
            var db = new Database();
            Order order = new Order
            {
                OrderId = 1,
                Date = DateTime.Now,
                StoreNo = null,
                UserId = "SEL01",
                Tax = 0.15M,
                OrderAmount = 0

            };
            List<OrderItems> orderItemsTestList = new List<OrderItems>();
            orderItemsTestList.Add(new OrderItems
            {
                CategoryId = 1,
                CategoryName = "Meals",
                Name = "Test",
                OrderId = 1,
                Price = 2.4M,
                Quantity = 1,
                Tax = 0.15,
                UPCCode = "SEL01"
            });
            db.saveOrderAndOrderItems(order, orderItemsTestList);
        }
    }
}
