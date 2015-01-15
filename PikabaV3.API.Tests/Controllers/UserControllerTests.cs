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
    public class UserControllerTests
    {
        [Test]
        public void GetAllUsers_ShouldVerifyCallMethod()
        {
            // Arrange
            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Users.GetAll()).Returns(new List<User>());

            var userController = new UserController(new ModelFactory(), mockService.Object, new Validator())
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            userController.GetAllUsers();

            // Assert
            mockService.Verify(m => m.Users.GetAll(), Times.Once);
        }

        [Test]
        public void GetUser_ShouldVerifyCallMethods()
        {
            // Arrange
            var userId = ObjectId.GenerateNewId();

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(userId.ToString())).Returns(true);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Users.Get(userId)).Returns(new User());

            var userController = new UserController(new ModelFactory(), mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            userController.GetUser(userId.ToString());

            // Assert
            mockService.Verify(m => m.Users.Get(userId), Times.Once());
            mockValidator.Verify(m => m.CheckValidId(userId.ToString()), Times.Once());
        }

        [Test]
        public void PostUser_ShouldVerifyCallMethods()
        {
            // Arrange
            var user = new User { Email = "test@mail.ru" };
            var registerUserModel = new RegisterUserModel { Email = user.Email };

            var mockModelFactory = new Mock<IModelFactory>();
            mockModelFactory.Setup(s => s.Create(registerUserModel)).Returns(user);

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.IsBusyEmail(user.Email)).Returns(false);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Users.Add(user)).Returns(true);

            var userController = new UserController(mockModelFactory.Object, mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            userController.PostUser(registerUserModel);

            // Assert
            mockService.Verify(m => m.Users.Add(user), Times.Once());
            mockValidator.Verify(m => m.IsBusyEmail(user.Email), Times.Once());
            mockModelFactory.Verify(m => m.Create(registerUserModel), Times.Once());
        }

        [Test]
        public void PutUser_ShouldVerifyCallMethods()
        {
            // Arrange
            var userId = ObjectId.GenerateNewId();
            var user = new User();
            var updateUserModel = new UpdateUserModel();

            var mockModelFactory = new Mock<IModelFactory>();
            mockModelFactory.Setup(s => s.Create(updateUserModel)).Returns(user);

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(userId.ToString())).Returns(true);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Users.Update(userId, user)).Returns(true);

            var userController = new UserController(mockModelFactory.Object, mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            userController.PutUser(updateUserModel, userId.ToString());

            // Assert
            mockService.Verify(m => m.Users.Update(userId, user), Times.Once());
            mockValidator.Verify(m => m.CheckValidId(userId.ToString()), Times.Once());
            mockModelFactory.Verify(m => m.Create(updateUserModel), Times.Once());
        }

        [Test]
        public void ChangePassword_ShouldVerifyCallMethods()
        {
            // Arrange
            var userId = ObjectId.GenerateNewId();
            var session = new UserSession { User_Id = userId };
            var passwordModel = new ChangePasswordModel { NewPassword = "newPassword" };

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(userId.ToString())).Returns(true);

            var mockModelFactory = new Mock<IModelFactory>();
            mockModelFactory.Setup(s => s.Create(passwordModel)).Returns(passwordModel.NewPassword);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sessions.Get(userId)).Returns(session);
            mockService.Setup(s => s.Users.ChangePassword(userId, passwordModel.NewPassword));

            var userController = new UserController(mockModelFactory.Object, mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            userController.ChangePassword(userId.ToString(), passwordModel);

            // Assert
            mockValidator.Verify(m => m.CheckValidId(userId.ToString()), Times.Once());
            mockService.Verify(m => m.Sessions.Get(userId), Times.Once());
            mockModelFactory.Verify(m => m.Create(passwordModel), Times.Once());
            mockService.Verify(m => m.Users.ChangePassword(userId, passwordModel.NewPassword));
        }

        [Test]
        public void DeleteUser_ShouldVerifyCallMethods()
        {
            // Arrange
            var userId = ObjectId.GenerateNewId();

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(userId.ToString())).Returns(true);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Users.Remove(userId)).Returns(true);

            var userController = new UserController(new ModelFactory(), mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            userController.DeleteUser(userId.ToString());

            // Assert
            mockValidator.Verify(m => m.CheckValidId(userId.ToString()), Times.Once());
            mockService.Verify(m => m.Users.Remove(userId), Times.Once());
        }

        [Test]
        public void AddComment_ShouldVerifyCallMethods()
        {
            // Arrange
            var cookieUuid = "a26ecaf0-a6f8-4ebe-f04f-161bb4ed366a";
            var userId = ObjectId.GenerateNewId();
            var user = new User { Id = userId };
            var session = new UserSession { User_Id = userId };
            var commentModel = new CommentModel();
            var comment = new Comment { Id = ObjectId.GenerateNewId() };

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(userId.ToString())).Returns(true);

            var mockModelFactory = new Mock<IModelFactory>();
            mockModelFactory.Setup(s => s.Create(user, commentModel)).Returns(comment);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sessions.Get(cookieUuid)).Returns(session);
            mockService.Setup(s => s.Users.Get(userId)).Returns(user);
            mockService.Setup(s => s.Users.AddComment(userId, comment)).Returns(true);

            var userController = new UserController(mockModelFactory.Object, mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            userController.AddComment(userId.ToString(), cookieUuid, commentModel);

            // Assert
            mockValidator.Verify(m => m.CheckValidId(userId.ToString()), Times.Once());
            mockService.Verify(m => m.Sessions.Get(cookieUuid), Times.Once());
            mockService.Verify(m => m.Users.Get(userId), Times.Once());
            mockService.Verify(m => m.Users.AddComment(userId, comment), Times.Once());
            mockModelFactory.Verify(m => m.Create(user, commentModel), Times.Once());
        }


        [Test]
        public void PutComment_ShouldVerifyCallMethods()
        {
            // Arrange
            var cookieUuid = "a26ecaf0-a6f8-4ebe-f04f-161bb4ed366a";
            var userId = ObjectId.GenerateNewId();
            var commentId = ObjectId.GenerateNewId();
            var session = new UserSession { User_Id = userId };
            var commentModel = new CommentModel();
            var comment = new Comment { Owner = new Owner { User_Id = userId } };
            
            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(userId.ToString())).Returns(true);
            mockValidator.Setup(s => s.CheckValidId(commentId.ToString())).Returns(true);

            var mockModelFactory = new Mock<IModelFactory>();
            mockModelFactory.Setup(s => s.Create(commentModel)).Returns(comment);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sessions.Get(cookieUuid)).Returns(session);
            mockService.Setup(s => s.Users.GetComment(userId, commentId)).Returns(comment);
            mockService.Setup(s => s.Users.UpdateComment(userId, commentId, comment)).Returns(true);

            var userController = new UserController(mockModelFactory.Object, mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            userController.PutComment(userId.ToString(), commentId.ToString(), cookieUuid, commentModel);

            // Assert
            mockValidator.Verify(m => m.CheckValidId(userId.ToString()), Times.Once());
            mockValidator.Verify(m => m.CheckValidId(commentId.ToString()), Times.Once());
            mockService.Verify(m => m.Sessions.Get(cookieUuid), Times.Once());
            mockService.Verify(m => m.Users.GetComment(userId, commentId), Times.Once());
            mockService.Verify(m => m.Users.UpdateComment(userId, commentId, comment), Times.Once());
            mockModelFactory.Verify(m => m.Create(commentModel), Times.Once());
        }

        [Test]
        public void DeleteComment_ShouldVerifyCallMethods()
        {
            // Arrange
            var cookieUuid = "a26ecaf0-a6f8-4ebe-f04f-161bb4ed366a";
            var userId = ObjectId.GenerateNewId();
            var commentId = ObjectId.GenerateNewId();
            var session = new UserSession { User_Id = userId };
            var comment = new Comment { Owner = new Owner { User_Id = userId } };

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(s => s.CheckValidId(userId.ToString())).Returns(true);
            mockValidator.Setup(s => s.CheckValidId(commentId.ToString())).Returns(true);

            var mockService = new Mock<IPikabaV3Service>();
            mockService.Setup(s => s.Sessions.Get(cookieUuid)).Returns(session);
            mockService.Setup(s => s.Users.GetComment(userId, commentId)).Returns(comment);
            mockService.Setup(s => s.Users.RemoveComment(userId, commentId)).Returns(true);


            var userController = new UserController(new ModelFactory(), mockService.Object, mockValidator.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            userController.DeleteComment(userId.ToString(), commentId.ToString(), cookieUuid);

            // Assert
            mockValidator.Verify(m => m.CheckValidId(userId.ToString()), Times.Once());
            mockValidator.Verify(m => m.CheckValidId(commentId.ToString()), Times.Once());
            mockService.Verify(m => m.Sessions.Get(cookieUuid), Times.Once());
            mockService.Verify(m => m.Users.GetComment(userId, commentId), Times.Once());
            mockService.Verify(m => m.Users.RemoveComment(userId, commentId), Times.Once());
        }
    }
}
