using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using PikabaV3.API.Controllers;
using PikabaV3.GeneralProvider;
using PikabaV3.Logic;
using PikabaV3.Models;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories;

namespace PikabaV3.API.Tests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void Login_ShouldVerifyCallMethods()
        {
            // Arrange
            var cookieUuid = "a26ecaf0-a6f8-4ebe-f04f-161bb4ed366a";
            var user = new User();
            var loginModel = new LoginModel();
            var session = new UserSession();

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sessions.GetUser(loginModel)).Returns(user);
            mockService.Setup(s => s.Sessions.Add(session)).Returns(true);

            var mockModelFactory = new Mock<IModelFactory>();
            mockModelFactory.Setup(s => s.Create(user, loginModel, cookieUuid)).Returns(session);

            var accountController = new AccountController(mockModelFactory.Object, mockService.Object, new Validator())
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            accountController.LogIn(cookieUuid, loginModel);

            // Assert
            mockService.Verify(m => m.Sessions.GetUser(loginModel), Times.Once());
            mockService.Verify(m => m.Sessions.Add(session), Times.Once());
            mockModelFactory.Verify(m => m.Create(user, loginModel, cookieUuid), Times.Once());
        }

        [Test]
        public void GetAccount_ShouldVerifyCallMethod()
        {
            // Arrange
            var cookieUuid = "a26ecaf0-a6f8-4ebe-f04f-161bb4ed366a";

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sessions.Get(cookieUuid)).Returns(new UserSession());

            var accountController = new AccountController(new ModelFactory(), mockService.Object, new Validator())
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            accountController.GetAccount(cookieUuid);

            // Assert
            mockService.Verify(m => m.Sessions.Get(cookieUuid));
        }

        [Test]
        public void GetAccount_ShouldReturnUrlForBuyer()
        {
            // Arrange
            var cookieUuid = "a26ecaf0-a6f8-4ebe-f04f-161bb4ed366a";
            var buyerId = ObjectId.GenerateNewId();
            string locationUrl = "http://localhost:49909/api/buyer/" + buyerId;
            var sessionBuyer = new UserSession { UserRole = UserRole.Buyer, User_Id = buyerId };

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sessions.Get(cookieUuid)).Returns(sessionBuyer);

            var accountController = new AccountController(new ModelFactory(), mockService.Object, new Validator())
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(s => s.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            accountController.Url = mockUrlHelper.Object;

            // Act
            var response = accountController.GetAccount(cookieUuid);

            // Assert
            Assert.AreEqual(locationUrl, response.Headers.Location.AbsoluteUri);
        }

        [Test]
        public void GetAccount_ShouldReturnUrlForSeller()
        {
            // Arrange
            var cookieUuid = "a26ecaf0-a6f8-4ebe-f04f-161bb4ed366a";
            var sellerId = ObjectId.GenerateNewId();
            string locationUrl = "http://localhost:49909/api/seller/" + sellerId;
            var sessionSeller = new UserSession { UserRole = UserRole.Seller, User_Id = sellerId };

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sessions.Get(cookieUuid)).Returns(sessionSeller);

            var accountController = new AccountController(new ModelFactory(), mockService.Object, new Validator())
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(s => s.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            accountController.Url = mockUrlHelper.Object;

            // Act
            var response = accountController.GetAccount(cookieUuid);

            // Assert
            Assert.AreEqual(locationUrl, response.Headers.Location.AbsoluteUri);
        }
    }
}
