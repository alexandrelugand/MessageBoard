using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MessageBoard.Controllers;
using MessageBoard.Data;
using MessageBoard.Tests.Fakes.Data;
using MessageBoard.Tests.Fakes.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageBoard.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        private FakeMessageBoardRepository _messageBoardRepository;
        private HomeController _homeController;

        [TestInitialize]
        public void Init()
        {
            _messageBoardRepository = new FakeMessageBoardRepository();
            _homeController = new HomeController(new MockMailService(), _messageBoardRepository);
        }

        [TestMethod]
        [TestCategory("HomeController")]
        public void HomeController_IndexCanRender()
        {
            var result = _homeController.Index();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("HomeController")]
        public void HomeController_IndexHasData()
        {
            var result = _homeController.Index() as ViewResult;
            var topics = result.Model as IEnumerable<Topic>;

            Assert.IsNotNull(result.Model);
            Assert.IsNotNull(topics);
            Assert.IsTrue(topics.Count() > 0);
        }
    }
}
