using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using PikabaV3.Api.Controllers;
using PikabaV3.GeneralProvider;
using PikabaV3.Logic;
using PikabaV3.Models;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories;

namespace PikabaV3.API.Tests.Controllers
{
    [TestFixture]
    public class ProductControllerTests
    {
        [Test]
        public void GetAllProducts_ShouldVerifyCallMethod()
        {
            // Arrange
            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Products.GetAll());
            var productController = new ProductController(new ModelFactory(), mockService.Object, new Validator())
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            productController.GetAllProducts();

            // Assert
            mockService.Verify(m => m.Products.GetAll(), Times.Once());
        }

        [Test]
        public void GetProductsSeller_ShouldVerifyCallMethod()
        {
            // Arrange
            var sellerId = ObjectId.GenerateNewId();

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(sellerId.ToString())).Returns(true);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Products.GetSellerProducts(sellerId));

            var productController = new ProductController(new ModelFactory(), mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            productController.GetProductsSeller(sellerId.ToString());

            // Assert
            mockValidator.Verify(m => m.CheckValidId(sellerId.ToString()), Times.Once());
            mockService.Verify(m => m.Products.GetSellerProducts(sellerId), Times.Once());
        }
        
        [Test]
        public void GetProduct_ShouldVerifyCallMethod()
        {
            // Arrange
            var productId = ObjectId.GenerateNewId();

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(productId.ToString())).Returns(true);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Products.Get(productId)).Returns(new Product());

            var productController = new ProductController(new ModelFactory(), mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            productController.GetProduct(productId.ToString());

            // Assert
            mockValidator.Verify(m => m.CheckValidId(productId.ToString()), Times.Once());
            mockService.Verify(m => m.Products.Get(productId), Times.Once());
        }

        [Test]
        public void PostProduct_ShouldVerifyCallMethods()
        {
            // Arrange
            var cookieUuid = "a26ecaf0-a6f8-4ebe-f04f-161bb4ed366a";
            var sellerId = ObjectId.GenerateNewId();
            var productModel = new ProductModel();
            var product = new Product { Owner = new Owner { User_Id = sellerId } };
            var session = new UserSession { User_Id = sellerId };
            var seller = new Seller { IsActive = true };

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sessions.Get(cookieUuid)).Returns(session);
            mockService.Setup(s => s.Sellers.Get(sellerId)).Returns(seller);
            mockService.Setup(s => s.Products.Add(product)).Returns(true);
            mockService.Setup(s => s.Products.GetSellerProducts(sellerId)).Returns(new List<Product> { product });

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.IsExceededLimitCreationProducts(sellerId)).Returns(false);

            var mockModelFactory = new Mock<IModelFactory>();
            mockModelFactory.Setup(s => s.Create(seller, productModel)).Returns(product);

            var productController = new ProductController(mockModelFactory.Object, mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            productController.PostProduct(productModel, cookieUuid);

            // Assert
            mockService.Verify(m => m.Sessions.Get(cookieUuid), Times.Once());
            mockService.Verify(m => m.Sellers.Get(sellerId), Times.Once());
            mockValidator.Verify(m => m.IsExceededLimitCreationProducts(sellerId), Times.Once());
            mockModelFactory.Verify(m => m.Create(seller, productModel), Times.Once());
            mockService.Verify(m => m.Products.Add(product), Times.Once());
        }

        [Test]
        public void PutProduct_ShouldVerifyCallMethods()
        {
            // Arrange
            var cookieUuid = "a26ecaf0-a6f8-4ebe-f04f-161bb4ed366a";
            var productId = ObjectId.GenerateNewId();
            var userId = ObjectId.GenerateNewId();
            var productModel = new ProductModel();
            var product = new Product { Owner = new Owner { User_Id = userId } };
            var session = new UserSession { User_Id = userId };

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(productId.ToString())).Returns(true);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sessions.Get(cookieUuid)).Returns(session);
            mockService.Setup(s => s.Products.Get(productId)).Returns(product);
            mockService.Setup(s => s.Products.Update(productId, product)).Returns(true);

            var mockModelFactory = new Mock<IModelFactory>();
            mockModelFactory.Setup(s => s.Create(productModel)).Returns(product);

            var productController = new ProductController(mockModelFactory.Object, mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            productController.PutProduct(productModel, productId.ToString(), cookieUuid);

            // Assert
            mockValidator.Verify(m => m.CheckValidId(productId.ToString()), Times.Once());
            mockService.Verify(m => m.Sessions.Get(cookieUuid), Times.Once());
            mockService.Verify(m => m.Products.Get(productId), Times.Once());
            mockService.Verify(m => m.Products.Update(productId, product), Times.Once());
            mockModelFactory.Verify(m => m.Create(productModel), Times.Once());
        }

        [Test]
        public void DeleteProduct_ShouldVerifyCallMethods()
        {
            // Arrange
            var cookieUuid = "a26ecaf0-a6f8-4ebe-f04f-161bb4ed366a";
            var productId = ObjectId.GenerateNewId();
            var userId = ObjectId.GenerateNewId();
            var session = new UserSession { User_Id = userId };
            var product = new Product { Owner = new Owner { User_Id = userId } };

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(productId.ToString())).Returns(true);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sessions.Get(cookieUuid)).Returns(session);
            mockService.Setup(s => s.Products.Get(productId)).Returns(product);
            mockService.Setup(s => s.Products.Remove(productId)).Returns(true);

            var productController = new ProductController(new ModelFactory(), mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            productController.DeleteProduct(productId.ToString(), cookieUuid);

            // Assert
            mockValidator.Verify(m => m.CheckValidId(productId.ToString()), Times.Once());
            mockService.Verify(m => m.Sessions.Get(cookieUuid), Times.Once());
            mockService.Verify(m => m.Products.Get(productId), Times.Once());
            mockService.Verify(m => m.Products.Remove(productId), Times.Once());
        }

        [Test]
        public void AddComment_ShouldVerifyCallMethods()
        {
            // Arrange
            var cookieUuid = "a26ecaf0-a6f8-4ebe-f04f-161bb4ed366a";
            var productId = ObjectId.GenerateNewId();
            var userId = ObjectId.GenerateNewId();
            var session = new UserSession { User_Id = userId };
            var seller = new Seller { IsActive = true };
            var comment = new Comment();
            var commentModel = new CommentModel();

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(productId.ToString())).Returns(true);

            var mockModelFactory = new Mock<IModelFactory>();
            mockModelFactory.Setup(s => s.Create(seller, commentModel)).Returns(comment);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sessions.Get(cookieUuid)).Returns(session);
            mockService.Setup(s => s.Sellers.Get(userId)).Returns(seller);
            mockService.Setup(s => s.Products.AddComment(productId, comment)).Returns(true);

            var productController = new ProductController(mockModelFactory.Object, mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            productController.AddComment(productId.ToString(), cookieUuid, commentModel);

            // Assert
            mockValidator.Verify(m => m.CheckValidId(productId.ToString()), Times.Once());
            mockService.Verify(m => m.Sessions.Get(cookieUuid), Times.Once());
            mockService.Verify(m => m.Sellers.Get(userId), Times.Once());
            mockModelFactory.Verify(m => m.Create(seller, commentModel), Times.Once());
            mockService.Verify(m => m.Products.AddComment(productId, comment), Times.Once());
        }

        [Test]
        public void PutComment_ShouldVerifyCallMethods()
        {
            // Arrange
            var cookieUuid = "a26ecaf0-a6f8-4ebe-f04f-161bb4ed366a";
            var productId = ObjectId.GenerateNewId();
            var commentId = ObjectId.GenerateNewId();
            var userId = ObjectId.GenerateNewId();
            var session = new UserSession { User_Id = userId };
            var comment = new Comment { Owner = new Owner { User_Id = userId } };
            var commentModel = new CommentModel();

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(productId.ToString())).Returns(true);
            mockValidator.Setup(s => s.CheckValidId(commentId.ToString())).Returns(true);

            var mockModelFactory = new Mock<IModelFactory>();
            mockModelFactory.Setup(s => s.Create(commentModel)).Returns(comment);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sessions.Get(cookieUuid)).Returns(session);
            mockService.Setup(s => s.Products.GetComment(productId, commentId)).Returns(comment);
            mockService.Setup(s => s.Products.UpdateComment(productId, commentId, comment)).Returns(true);

            var productController = new ProductController(mockModelFactory.Object, mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            productController.PutComment(productId.ToString(), commentId.ToString(), cookieUuid, commentModel);

            // Assert
            mockValidator.Verify(m => m.CheckValidId(productId.ToString()), Times.Once());
            mockValidator.Verify(m => m.CheckValidId(commentId.ToString()), Times.Once());
            mockService.Verify(m => m.Sessions.Get(cookieUuid), Times.Once());
            mockModelFactory.Verify(m => m.Create(commentModel), Times.Once());
            mockService.Verify(m => m.Products.GetComment(productId, commentId), Times.Once());
            mockService.Verify(m => m.Products.UpdateComment(productId, commentId, comment), Times.Once());
        }
        
        [Test]
        public void DeleteComment_ShouldVerifyCallMethods()
        {
            // Arrange
            var cookieUuid = "a26ecaf0-a6f8-4ebe-f04f-161bb4ed366a";
            var productId = ObjectId.GenerateNewId();
            var commentId = ObjectId.GenerateNewId();
            var userId = ObjectId.GenerateNewId();
            var session = new UserSession { User_Id = userId };
            var comment = new Comment { Owner = new Owner { User_Id = userId } };

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(productId.ToString())).Returns(true);
            mockValidator.Setup(s => s.CheckValidId(commentId.ToString())).Returns(true);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sessions.Get(cookieUuid)).Returns(session);
            mockService.Setup(s => s.Products.GetComment(productId, commentId)).Returns(comment);
            mockService.Setup(s => s.Products.RemoveComment(productId, commentId)).Returns(true);

            var productController = new ProductController(new ModelFactory(), mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            productController.DeleteComment(productId.ToString(), commentId.ToString(), cookieUuid);

            // Assert
            mockValidator.Verify(m => m.CheckValidId(productId.ToString()), Times.Once());
            mockValidator.Verify(m => m.CheckValidId(commentId.ToString()), Times.Once());
            mockService.Verify(m => m.Sessions.Get(cookieUuid), Times.Once());
            mockService.Verify(m => m.Products.GetComment(productId, commentId), Times.Once());
            mockService.Verify(m => m.Products.RemoveComment(productId, commentId));
        }
    }
}
