using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class sUnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
                {
                    new Product {ProductID = 1, Name = "P1"},
                    new Product {ProductID = 2, Name = "P2"},
                    new Product {ProductID = 3, Name = "P3"},
                    new Product {ProductID = 4, Name = "P4"},
                    new Product {ProductID = 5, Name = "P5"}
                }.AsQueryable());

            var controller = new ProductController(mock.Object) { PageSize = 3 };

            //Act
            var result = (ProductsListViewModel)controller.List(null, 2).Model;

            //Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //arrange
            HtmlHelper myHelper = null;
            var pagingInfo = new PagingInfo
                {
                    CurrentPage = 2,
                    ItemsPerPage = 10,
                    TotalItems = 28
                };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //act
            // ReSharper disable ExpressionIsAlwaysNull
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);
            // ReSharper restore ExpressionIsAlwaysNull

            //assert
            Assert.AreEqual(result.ToString(),
                            @"<a href=""Page1"">1</a>" +
                            @"<a class=""selected"" href=""Page2"">2</a>" +
                            @"<a href=""Page3"">3</a>");


        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
                {
                    new Product {ProductID = 1, Name = "P1"},
                    new Product {ProductID = 2, Name = "P2"},
                    new Product {ProductID = 3, Name = "P3"},
                    new Product {ProductID = 4, Name = "P4"},
                    new Product {ProductID = 5, Name = "P5"}
                }.AsQueryable());
            
            //arrange
            var controller = new ProductController(mock.Object) { PageSize = 3 };

            //act
            var result = (ProductsListViewModel)controller.List(null, 2).Model;

            //assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            //arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
                {
                    new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                    new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                    new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                    new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                    new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
                }.AsQueryable());
           //arrange
            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //action
            //controller.List() have default second parameter = 1
            Product[] result = ((ProductsListViewModel) controller.List("Cat2").Model).Products.ToArray();

            //assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[0].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            //arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
                {
                    new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                    new Product {ProductID = 2, Name = "P2", Category = "Apples"},
                    new Product {ProductID = 3, Name = "P3", Category = "Plums"},
                    new Product {ProductID = 4, Name = "P4", Category = "Oranges"}
                }.AsQueryable());

            NavController target = new NavController(mock.Object);

            //act
            string[] results = ((IEnumerable<string>) target.Menu().Model).ToArray();

            //assert
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            //arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
                {
                    new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                    new Product {ProductID = 2, Name = "P2", Category = "Oranges"}
                }.AsQueryable());
            var target = new NavController(mock.Object);
            const string categoryToSelect = "Apples";

            //act
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            //assert
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            //arange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
                {
                    new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                    new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                    new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                    new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                    new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
                }.AsQueryable());

            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;

            //act
            int res1 = ((ProductsListViewModel)target.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductsListViewModel)target.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductsListViewModel)target.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)target.List(null).Model).PagingInfo.TotalItems;

            //assert
            Assert.AreEqual(res1,2);
            Assert.AreEqual(res2,2);
            Assert.AreEqual(res3,1);
            Assert.AreEqual(resAll,5);


        }
    }
}
