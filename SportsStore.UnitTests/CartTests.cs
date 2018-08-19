﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Arrange - создание тестовой продукции
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            // Arrange - создание новой корзины
            Cart target = new Cart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();

            // Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Excisting_Lines()
        {
            // Arrange - создание тестовой продукции
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            // Arrange - создание новой корзины
            Cart target = new Cart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] results = target.Lines.OrderBy(c => c.Product.ProductID).ToArray();

            // Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Arrange - создание тестовой продукции
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };

            // Arrange - создание новой корзины
            Cart target = new Cart();

            // Arrange - добавление некоторой продукции в корзину
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            // Act
            target.RemoveList(p2);

            // Assert
            Assert.AreEqual(target.Lines.Where(c => c.Product == p2).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Arrange - создание тестовой продукции
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            // Arrange - создание новой корзины
            Cart target = new Cart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();

            // Assert
            Assert.AreEqual(result, 450M);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Arrange - создание тестовой продукции
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            // Arrange - создание новой корзины
            Cart target = new Cart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            // Act - сброс корзины
            target.Clear();

            // Assert
            Assert.AreEqual(target.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Arrange - создание mock - хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Apples"},
            }.AsQueryable());

            // Arrange - создание Cart
            Cart cart = new Cart();

            // Arrange - создание контроллера
            CartController target = new CartController(mock.Object, null);

            // Act - добавление продукта в корзину
            target.AddToCart(cart, 1, null);

            // Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            // Arrange - создание mock - хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Apples"},
            }.AsQueryable());

            // Arrange - создание Cart
            Cart cart = new Cart();

            // Arrange - создание контроллера
            CartController target = new CartController(mock.Object, null);

            // Act - добавление продукта в корзину
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

            // Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Arrange - создание Cart
            Cart cart = new Cart();

            // Arrange - создание контроллера
            CartController target = new CartController(null, null);

            // Act - вызов метода действия Index
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl")
                .ViewData.Model;

            // Assert
            Assert.AreEqual(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");

        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Arrange - создание mock-процесса заказа
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Arrange - создание пустой корзины
            Cart cart = new Cart();

            // Arrange - создание подробностей доставки
            ShippingDetails shippingDetails = new ShippingDetails();

            // Arrange - создание экземпляра контроллера
            CartController target = new CartController(null, mock.Object);

            // Act
            ViewResult result = target.Checkout(cart, shippingDetails);

            // Assert - проверяем, что заказ не прошел к процессу.
            mock.Verify(m =>
               m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            // Assert - проверяем, что метод возвращает представление по умолчанию.
            Assert.AreEqual("", result.ViewName);

            // Assert - проверяем, что мы передаём недопустимую модель в представление.
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Arrange - создание mock-процесса заказа
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Arrange - создание корзины c предметами
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            // Arrange - создание экземпляра контроллера
            CartController target = new CartController(null, mock.Object);

            // Act - пробуем checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            // Assert - проверяем, что заказ прошел к процессу.
            mock.Verify(m =>
               m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());

            // Assert - проверяем, что метод возвращает Completed-представление.
            Assert.AreEqual("Completed", result.ViewName);

            // Assert - проверяем, что мы передаём допустимую модель в представление.
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);

        }
    }
}
