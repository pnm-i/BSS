using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
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
    public class SellerControllerTests
    {
        [Test]
        public void GetAllSellers_ShouldVerifyCallMethod()
        {
            // Arrange
            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sellers.GetAll()).Returns(new List<Seller>());

            var sellerController = new SellerController(new ModelFactory(), mockService.Object, new Validator())
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            sellerController.GetAllSellers();

            // Assert
            mockService.Verify(m => m.Sellers.GetAll(), Times.Once());
        }

        [Test]
        public void GetSeller_ShouldVerifyCallMethods()
        {
            // Arrange
            var sellerId = ObjectId.GenerateNewId();

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(sellerId.ToString())).Returns(true);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sellers.Get(sellerId)).Returns(new Seller());

            var sellerController = new SellerController(new ModelFactory(), mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            sellerController.GetSeller(sellerId.ToString());

            // Assert
            mockValidator.Verify(m => m.CheckValidId(sellerId.ToString()), Times.Once);
            mockService.Verify(m => m.Sellers.Get(sellerId), Times.Once());
        }

        [Test]
        public void PostSeller_ShouldVerifyCallMethods()
        {
            // Arrange
            var registerSellerModel = new RegisterSellerModel { Email = "test@mail.ru" };
            var seller = new Seller();

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.IsBusyEmail(registerSellerModel.Email)).Returns(false);

            var mockModelFactory = new Mock<IModelFactory>();
            mockModelFactory.Setup(s => s.Create(registerSellerModel)).Returns(seller);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sellers.Add(seller)).Returns(true);

            var sellerController = new SellerController(mockModelFactory.Object, mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            sellerController.PostSeller(registerSellerModel);

            // Assert
            mockValidator.Verify(m => m.IsBusyEmail(registerSellerModel.Email), Times.Once);
            mockModelFactory.Verify(m => m.Create(registerSellerModel), Times.Once());
            mockService.Verify(m => m.Sellers.Add(seller), Times.Once());
        }

        [Test]
        public void PutSeller_ShouldVerifyCallMethods()
        {
            // Arrange
            var sellerId = ObjectId.GenerateNewId();
            var updateSellerModel = new UpdateSellerModel();
            var seller = new Seller();

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(sellerId.ToString())).Returns(true);

            var mockModelFactory = new Mock<IModelFactory>();
            mockModelFactory.Setup(s => s.Create(updateSellerModel)).Returns(seller);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sellers.Update(sellerId, seller)).Returns(true);

            var sellerController = new SellerController(mockModelFactory.Object, mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            sellerController.PutSeller(updateSellerModel, sellerId.ToString());

            // Assert
            mockValidator.Verify(m => m.CheckValidId(sellerId.ToString()), Times.Once());
            mockModelFactory.Verify(m => m.Create(updateSellerModel), Times.Once());
            mockService.Verify(m => m.Sellers.Update(sellerId, seller), Times.Once());
        }
    }
}
