using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Abstract;
using Domain.Entities;
using WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            // 准备-创建模仿存储库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
                new Product { ProductID = 3, Name = "P3" }
            });

            // 准备-创建控制器
            AdminController target = new AdminController(mock.Object);

            // 动作
            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            // 断言
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0].Name, "P1");
            Assert.AreEqual(result[1].Name, "P2");
            Assert.AreEqual(result[2].Name, "P3");
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            // 准备-创建模仿存储库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
                new Product { ProductID = 3, Name = "P3" }
            });

            // 准备-创建控制器
            AdminController target = new AdminController(mock.Object);

            // 动作
            Product p1 = target.Edit(1).ViewData.Model as Product;
            Product p2 = target.Edit(2).ViewData.Model as Product;
            Product p3 = target.Edit(3).ViewData.Model as Product;

            // 断言
            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            // 准备-创建模仿存储库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
                new Product { ProductID = 3, Name = "P3" }
            });

            // 准备-创建控制器
            AdminController target = new AdminController(mock.Object);

            // 动作
            Product result = (Product)target.Edit(4).ViewData.Model;

            // 断言
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Chages()
        {
            // 准备-创建模仿存储库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();

            // 准备-创建控制器
            AdminController target = new AdminController(mock.Object);
            Product product = new Product { Name = "Test" };

            // 动作
            ActionResult result = target.Edit(product);

            // 断言
            mock.Verify(m => m.SaveProduct(product));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Save_Invalid_Chages()
        {
            // 准备-创建模仿存储库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();

            // 准备-创建控制器
            AdminController target = new AdminController(mock.Object);
            Product product = new Product { Name = "Test" };
            target.ModelState.AddModelError("error", "error");

            // 动作
            ActionResult result = target.Edit(product);

            // 断言
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            Assert.IsNotInstanceOfType(result, typeof(RedirectResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            // 准备-创建一个产品
            Product prod = new Product { ProductID = 2, Name = "Test" };

            // 准备-创建模仿存储库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1" }
            });

            // 准备-创建控制器
            AdminController target = new AdminController(mock.Object);

            // 动作
            target.Delete(prod.ProductID);

            // 断言
            mock.Verify(m => m.DeleteProduct(prod.ProductID));
        }
    }
}
