using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Abstract;
using Domain.Entities;
using WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // 准备-创建一个带有图像数据的Product
            Product prod = new Product
            {
                ProductID = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMineType = "Image/png"
            };

            // 准备-创建模仿存储库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                prod,
                new Product { ProductID = 3, Name = "P3" },
            }.AsQueryable());

            // 准备-创建控制器
            ProductController target = new ProductController(mock.Object);

            // 动作-调用GetImage动作方法
            ActionResult result = target.GetImage(2);

            // 断言
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.ImageMineType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // 准备-创建模仿存储库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
            }.AsQueryable());

            // 准备-创建控制器
            ProductController target = new ProductController(mock.Object);

            // 动作-调用GetImage动作方法
            ActionResult result = target.GetImage(100);

            // 断言
            Assert.IsNull(result);
        }
    }
}
