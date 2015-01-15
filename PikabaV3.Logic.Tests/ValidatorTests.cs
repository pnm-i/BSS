using System.Collections.Generic;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using PikabaV3.GeneralProvider;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories;

namespace PikabaV3.Logic.Tests
{
    [TestFixture]
    public class ValidatorTests
    {
        [Test]
        public void IsExceededLimitCreationProducts_ShouldReturnFalse()
        {
            // Arrange
            var sellerId = ObjectId.GenerateNewId();
            var dataProvider = new TestDataProvider();
            var products = dataProvider.CreateProducts();

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Products.GetSellerProducts(sellerId)).Returns(products);

            var validator = new Validator(mockService.Object);
            
            // Act
            bool result = validator.IsExceededLimitCreationProducts(sellerId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsExceededLimitCreationProducts_ShouldReturnTrue()
        {
            // Arrange
            var sellerId = ObjectId.GenerateNewId();
            var products = new List<Product>();
            for (int i = 0; i < 12; i++)
            {
                products.Add(new Product());
            }

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Products.GetSellerProducts(sellerId)).Returns(products);

            var validator = new Validator(mockService.Object);

            // Act
            bool result = validator.IsExceededLimitCreationProducts(sellerId);
            
            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsExceededLimitCreationProducts_ShouldCheckCallMethodRepository()
        {
            // Arrange
            var sellerId = ObjectId.GenerateNewId();
            var products = new List<Product>();
            for (int i = 0; i < 2; i++)
            {
                products.Add(new Product());
            }

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Products.GetSellerProducts(sellerId)).Returns(products);

            var validator = new Validator(mockService.Object);

            // Act
            bool result = validator.IsExceededLimitCreationProducts(sellerId);

            // Assert
            mockService.Verify(m => m.Products.GetSellerProducts(sellerId), Times.Once());
        }

        [Test]
        public void IsBusyEmail_ShouldReturnTrue()
        {
            // Arrange
            string busyEmail = "Allex@gmail.com";

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Users.FindUserEmail(busyEmail)).Returns(busyEmail);

            var validator = new Validator(mockService.Object);

            // Act
            var result = validator.IsBusyEmail(busyEmail);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsBusyEmail_ShouldReturnFalse()
        {
            // Arrange
            string freeEmail = "Allex@gmail.com";
            string returnNull = null;

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Users.FindUserEmail(freeEmail)).Returns(returnNull);

            var validator = new Validator(mockService.Object);

            // Act
            var result = validator.IsBusyEmail(freeEmail);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsBusyEmail_ShouldCallMethodRepository()
        {
            // Arrange
            string freeEmail = "Allex@gmail.com";

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Users.FindUserEmail(freeEmail)).Returns(freeEmail);

            var validator = new Validator(mockService.Object);

            // Act
            var result = validator.IsBusyEmail(freeEmail);

            // Assert
            mockService.Verify(m => m.Users.FindUserEmail(freeEmail), Times.Once());
        }

        [Test]
        public void CheckValidId_ShouldReturnTrueAndFalse()
        {
            // Arrange
            string validId = "54230d5ae2556e168c71183d";
            string inValidId = "123abc";
            var validator = new Validator();
            // Act
            bool resultValid = validator.CheckValidId(validId);
            bool resultInValid = validator.CheckValidId(inValidId);
            // Assert
            Assert.IsTrue(resultValid);
            Assert.IsFalse(resultInValid);
        }
    }
}
