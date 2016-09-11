using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NsTestTask;
using NsTestTask.Controllers;
using NsTestTask.Models;
using Moq;

namespace NsTestTask.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(m => m.GetUsersList()).Returns(new List<User>());
            mock.Setup(m => m.GetJobsList()).Returns(new List<Job>());
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void TestStart()
        {
            HomeController controller = new HomeController();
            int result = controller.TestStart();
            Assert.IsNotNull(result);
        }
    }
}
