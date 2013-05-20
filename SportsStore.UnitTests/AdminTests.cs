using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Linq;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;
namespace SportsStore.UnitTests
{
    /// <summary>
    /// Summary description for AdminTests
    /// </summary>
    [TestClass]
    public class AdminTests
    {
        public AdminTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [TestMethod]
        public void Index_Contains_All_Products()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ProductID = 1, Name = "P1"},
                new Product{ProductID = 2, Name = "P2"},
                new Product{ProductID = 3, Name = "P3"},
                new Product{ProductID = 4, Name = "P4"}
            }.AsQueryable());

            //arrange
            AdminController target = new AdminController(mock.Object);

            //act
            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            //assert
            Assert.AreEqual(result.Length, 4);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
            Assert.AreEqual("P4", result[3].Name);

        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product{ProductID = 1, Name = "P1"},
                new Product{ProductID = 2, Name = "P2"},
                new Product{ProductID = 3, Name = "P3"},
                new Product{ProductID = 4, Name = "P4"}
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            //act
            Product p1 = target.Edit(1).ViewData.Model as Product;
            Product p2 = target.Edit(2).ViewData.Model as Product;
            Product p3 = target.Edit(3).ViewData.Model as Product;

            //assert
            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);

        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product{ProductID = 1, Name = "P1"},
                new Product{ProductID = 2, Name = "P2"},
                new Product{ProductID = 3, Name = "P3"},
                new Product{ProductID = 4, Name = "P4"}
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            //act
            Product result = (Product)target.Edit(5).ViewData.Model;

            //assert
            Assert.IsNull(result);
        }
        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            AdminController target = new AdminController(mock.Object);
            Product product = new Product { Name = "Test" };

            //act
            ActionResult result = target.Edit(product);

            //assert
            mock.Verify(m => m.SaveProduct(product));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Canont_Save_Invalid_Changes()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            AdminController target = new AdminController(mock.Object);
            Product product = new Product { Name = "Test" };
            target.ModelState.AddModelError("error", "error");

            //act
            ActionResult result = target.Edit(product);

            //assert
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            //arrange
            Product prod = new Product { ProductID = 2, Name = "Test" };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1"},
                prod,
                new Product { ProductID = 3, Name = "P3"}
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            //act
            target.Delete(prod.ProductID);

            //assert
            mock.Verify(m => m.DeleteProduct(prod.ProductID));
        }
    }
}
