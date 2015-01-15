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
    public class CategoryControllerTests
    {
        [Test]
        public void GetCategories_ShouldVerifyCallMethod()
        {
            // Arrange
            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Categories.GetAll()).Returns(new List<Category>());

            var categoryController = new CategoryController(new ModelFactory(), mockService.Object, new Validator())
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            categoryController.GetCategories();

            // Assert
            mockService.Verify(m => m.Categories.GetAll(), Times.Once());
        }

        [Test]
        public void GetCategory_ShouldVerifyCallMethods()
        {
            // Arrange
            var categoryId = ObjectId.GenerateNewId();

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(categoryId.ToString())).Returns(true);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Categories.Get(categoryId)).Returns(new Category());

            var categoryController = new CategoryController(new ModelFactory(), mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            categoryController.GetCategory(categoryId.ToString());

            // Assert
            mockValidator.Verify(m => m.CheckValidId(categoryId.ToString()), Times.Once());
            mockService.Verify(m => m.Categories.Get(categoryId), Times.Once());
        }

        [Test]
        public void PostCategory_ShouldVerifyCallMethods()
        {
            // Arrange
            var categoryModel = new CategoryModel();
            var category = new Category();

            var mockModelFactory = new Mock<IModelFactory>();
            mockModelFactory.Setup(s => s.Create(categoryModel)).Returns(category);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Categories.Add(category)).Returns(true);

            var categoryController = new CategoryController(mockModelFactory.Object, mockService.Object, new Validator())
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            categoryController.PostCategory(categoryModel);

            // Assert
            mockModelFactory.Verify(m => m.Create(categoryModel), Times.Once());
            mockService.Verify(m => m.Categories.Add(category), Times.Once());
        }

        [Test]
        public void PutCategory_ShouldVerifyCallMethods()
        {
            // Arrange
            var categoryId = ObjectId.GenerateNewId();
            var categoryModel = new CategoryModel();
            var category = new Category();

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(categoryId.ToString())).Returns(true);

            var mockModelFactory = new Mock<IModelFactory>();
            mockModelFactory.Setup(s => s.Create(categoryModel)).Returns(category);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Categories.Update(categoryId, category)).Returns(true);

            var categoryController = new CategoryController(mockModelFactory.Object, mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            categoryController.PutCategory(categoryId.ToString(), categoryModel);

            // Assert
            mockValidator.Verify(m => m.CheckValidId(categoryId.ToString()), Times.Once());
            mockModelFactory.Verify(m => m.Create(categoryModel), Times.Once());
            mockService.Verify(m => m.Categories.Update(categoryId, category), Times.Once());
        }

        [Test]
        public void DeleteCategory_ShouldVerifyCallMethods()
        {
            // Arrange
            var categoryId = ObjectId.GenerateNewId();

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(categoryId.ToString())).Returns(true);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Categories.Remove(categoryId)).Returns(true);

            var categoryController = new CategoryController(new ModelFactory(), mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            categoryController.DeleteCategory(categoryId.ToString());

            // Assert
            mockValidator.Verify(m => m.CheckValidId(categoryId.ToString()), Times.Once());
            mockService.Verify(m => m.Categories.Remove(categoryId), Times.Once());
        }
    }
}
