using MongoDB.Bson;
using NUnit.Framework;
using PikabaV3.GeneralProvider;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories.Context;
using PikabaV3.MongoRepositories.Interfaces;
using PikabaV3.MongoRepositories.Repositories;

namespace PikabaV3.MongoRepositories.Tests
{
    [TestFixture]
    public class SessionRepositoryTests
    {
        private MongoContext context;
        private ISessionRepository sessionRepository;
        private TestDataProvider dataProvider;

        public SessionRepositoryTests()
        {
            context = new MongoContext();
            sessionRepository = new SessionRepository(context);
            dataProvider = new TestDataProvider();
            context.Sessions.RemoveAll();
        }

        [Test]
        public void Get_ShouldReturnSessionByUserId()
        {
            // Arrange
            UserSession session = dataProvider.CreateSession();
            var userId = session.User_Id;
            // Fill collection
            context.Sessions.Insert(session);
            // Act
            UserSession result = sessionRepository.Get(userId);
            // Assert
            Assert.AreEqual(userId, result.User_Id);
            // Clean collection
            context.Sessions.RemoveAll();
        }

        [Test]
        public void Get_ShouldReturnSessionByCookieUuid()
        {
            // Arrange
            UserSession session = dataProvider.CreateSession();
            var cookieUuid = session.CookieUuid;
            // Fill collection
            context.Sessions.Insert(session);
            // Act
            UserSession result = sessionRepository.Get(cookieUuid);
            // Assert
            Assert.AreEqual(cookieUuid, result.CookieUuid);
            // Clean collection
            context.Sessions.RemoveAll();
        }

        [Test]
        public void GetUser_ShouldReturnUserByCredentials()
        {
            // Arrange
            var loginModel = dataProvider.CreateLoginModel();
            var user = dataProvider.CreateUser();
            context.Users.RemoveAll();
            // Fill collection
            context.Users.Insert(user);
            // Act
            User result = sessionRepository.GetUser(loginModel);
            // Assert
            Assert.AreEqual(user.Id, result.Id);
            // Clean collection
            context.Users.RemoveAll();
        }

        [Test]
        public void Add_ShouldReturnTrue()
        {
            // Arrange
            var sessionId = ObjectId.GenerateNewId();
            UserSession session = dataProvider.CreateSession();
            session.Id = sessionId;
            // Act
            bool result = sessionRepository.Add(session);
            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(sessionId, context.Sessions.FindOneById(sessionId).Id);
            // Clean collection
            context.Sessions.RemoveAll();
        }

        [Test]
        public void Remove_ShouldReturnTrueAndNull()
        {
            // Arrange
            UserSession session = dataProvider.CreateSession();
            var sessionId = session.Id;
            context.Sessions.Insert(session);
            // Act
            bool result = sessionRepository.Remove(sessionId);
            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(context.Sessions.FindOneById(sessionId));
            // Clean collection
            context.Sessions.RemoveAll();
        }
    }
}
