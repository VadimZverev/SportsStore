using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Linq;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {

        [TestMethod]
        public void Index_Contains_All_Product()
        {
            // Arrange - создание имитированного хранилища.
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            }.AsQueryable());

            // Arrange - создание контроллера
            AdminController target = new AdminController(mock.Object);

            // Act
            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            // Assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);

        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            // Arrange - создание имитированного хранилища.
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            }.AsQueryable());

            // Arrange - создание контроллера
            AdminController target = new AdminController(mock.Object);

            // Act
            Product p1 = target.Edit(1).ViewData.Model as Product;
            Product p2 = target.Edit(2).ViewData.Model as Product;
            Product p3 = target.Edit(3).ViewData.Model as Product;

            // Assert
            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            // Arrange - создание имитированного хранилища.
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            }.AsQueryable());

            // Arrange - создание контроллера
            AdminController target = new AdminController(mock.Object);

            // Act
            Product result = (Product)target.Edit(4).ViewData.Model;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Arrange - создание имитированного хранилища.
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            // Arrange - создание контроллера
            AdminController target = new AdminController(mock.Object);
            // Arrange - создание продукта
            Product product = new Product { Name = "Test" };

            // Act - пробуем созранить продукт
            ActionResult result = target.Edit(product);

            // Assert - проверяемметод на тип результата
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Arrange - создание имитированного хранилища.
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            // Arrange - создание контроллера
            AdminController target = new AdminController(mock.Object);
            // Arrange - создание продукта
            Product product = new Product { Name = "Test" };
            // Arrange - добавляем ошибку в статус модели.
            target.ModelState.AddModelError("error", "error");

            // Act - пробуем созранить продукт
            ActionResult result = target.Edit(product);

            // Assert - проверяем, что хранилище не было вызвано
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());

            // Assert - проверяемметод на тип результата
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
