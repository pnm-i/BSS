using System;
using System.Linq;
using MongoDB.Bson;
using NUnit.Framework;
using PikabaV3.GeneralProvider;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories.Context;
using PikabaV3.MongoRepositories.Interfaces;
using PikabaV3.MongoRepositories.Repositories;
using System.Collections.Generic;
using MongoDB.Driver.Builders;

namespace PikabaV3.MongoRepositories.Tests
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private MongoContext context;
        private IUserRepository userRepository;
        private TestDataProvider dataProvider;

        public UserRepositoryTests()
        {
            context = new MongoContext();
            userRepository = new UserRepository(context);
            dataProvider = new TestDataProvider();
            context.Users.RemoveAll();
        }

        [Test]
        public void GetAll_ShouldReturnTwoUsers()
        {
            var users = dataProvider.CreateUsers();
            // Fill collection
            foreach (var i in users)
            {
                context.Users.Insert(i);
            }
            // Act
            IEnumerable<User> returnedUsers = userRepository.GetAll();
            // Assert
            Assert.AreEqual(2, returnedUsers.Count());
            // Clean collection
            context.Users.RemoveAll();
        }

        [Test]
        public void Get_ShouldReturnUserById()
        {
            var user = dataProvider.CreateUser();
            // Fill collection
            context.Users.Insert(user);
            // Act
            User returnedUser = userRepository.Get(user.Id);
            // Assert
            Assert.AreEqual(user.Id, returnedUser.Id);
            // Clean collection
            context.Users.RemoveAll();
        }

        [Test]
        public void Add_ShouldReturnTrue()
        {
            // Arrange
            var user = dataProvider.CreateUser();
            // Act
            bool result = userRepository.Add(user);
            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(user.Id, context.Users.FindOneById(user.Id).Id);
            // Clean collection
            context.Users.RemoveAll();
        }

        [Test]
        public void Update_ShouldUpdateOldUserData()
        {
            // Arrange
            var oldUser = dataProvider.CreateUser();
            context.Users.Insert(oldUser);
            // Update user
            oldUser.Email = "update@mail.ru";
            // Act
            bool result = userRepository.Update(oldUser.Id, oldUser);
            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(oldUser.Email, context.Users.FindOneById(oldUser.Id).Email);
            // Clean collection
            context.Users.RemoveAll();
        }

        [Test]
        public void ChangePassword_ShouldReturnTrueAndChangedPassword()
        {
            // Arrange
            string newPassword = "wtf";
            var user = dataProvider.CreateUser();
            context.Users.Insert(user);
            // Act
            bool result = userRepository.ChangePassword(user.Id, newPassword);
            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newPassword, context.Users.FindOneById(user.Id).Password);
            // Clean collection
            context.Users.RemoveAll();
        }

        [Test]
        public void Remove_ShouldReturnTrueAndNull()
        {
            // Arrange
            var user = dataProvider.CreateUser();
            context.Users.Insert(user);
            // Act
            bool result = userRepository.Remove(user.Id);
            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(context.Users.FindOneById(user.Id));
            // Clean collection
            context.Users.RemoveAll();
        }

        [Test]
        public void FindUserEmail_ShouldReturnEmailAndNull()
        {
            // Arrange
            string email = "wtf@mail.ru";
            var user = dataProvider.CreateUser();
            context.Users.Insert(user);
            // Act
            string resultEmail = userRepository.FindUserEmail(user.Email);
            string resultNull = userRepository.FindUserEmail(email);

            // Assert
            Assert.AreEqual(user.Email, resultEmail);
            Assert.IsNull(resultNull);
            // Clean collection
            context.Users.RemoveAll();
        }

        [Test]
        public void GetUserComments_ShouldReturnLatestUserComments()
        {
            // Arrange
            int amountComments = 5;
            var user = dataProvider.CreateUserWitchTwentyComments();
            context.Users.Insert(user);
            // Act
            IEnumerable<Comment> result = userRepository.GetEntityComments(user.Id, amountComments);
            // Assert
            Assert.AreEqual(amountComments, result.Count());
            // Clean collection
            context.Users.RemoveAll();
        }

        [Test]
        public void GetComment_ShouldReturnCommentById()
        {
            // Arrange
            var commentId = ObjectId.Parse("53fe07ff30ebabcbfd522a14");
            var user = dataProvider.CreateUser();
            user.Comments.Add(new Comment
            {
                Id = commentId,
                Text = "comment Test",
                DateCreation = DateTime.Now,
                Owner = new Owner
                {
                    DisplayName = "Pedro",
                    Email = "pedro@mail.ru",
                    User_Id = ObjectId.Parse("5421cc37e2556e1de898874e")
                }
            });
            context.Users.Insert(user);
            // Act
            Comment result = userRepository.GetComment(user.Id, commentId);
            // Assert
            Assert.AreEqual(commentId, result.Id);
            // Clean collection
            context.Users.RemoveAll();
        }

        [Test]
        public void AddComment_ShouldReturnTrueAndAddedComment()
        {
            // Arrange
            var user = dataProvider.CreateUser();
            var comment = dataProvider.CreateComment();
            var userId = user.Id;
            var commentId = comment.Id;
            context.Users.Insert(user);
            // Act
            bool result = userRepository.AddComment(userId, comment);
            // Find comment
            var queryEntity = Query.EQ("_id", userId);
            var queryComment = Query.EQ("Comments._id", commentId);
            var query = Query.And(queryEntity, queryComment);
            var cursor = context.Users.Find(query);
            var addedComment = new Comment();
            foreach (var i in cursor)
            {
                addedComment = i.Comments.Find(x => x.Id == commentId);
            }
            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(addedComment.Id, commentId);
            // Clean collection
            context.Users.RemoveAll();
        }

        [Test]
        public void UpdateComment_ShouldReturnTrueAndUpdatedComment()
        {
            // Arrange
            var user = dataProvider.CreateUser();
            var userId = user.Id;
            var comment = user.Comments.First();
            var commentId = comment.Id;
            // Create user
            context.Users.Insert(user);
            // Update comment
            comment.Text = "Updated comment";
            // Act
            bool result = userRepository.UpdateComment(userId, commentId, comment);
            // Find comment
            var queryEntity = Query.EQ("_id", userId);
            var queryComment = Query.EQ("Comments._id", commentId);
            var query = Query.And(queryEntity, queryComment);
            var cursor = context.Users.Find(query);
            var updatedComment = new Comment();
            foreach (var i in cursor)
            {
                updatedComment = i.Comments.Find(x => x.Id == commentId);
            }
            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(updatedComment.Text, comment.Text);
            // Clean collection
            context.Users.RemoveAll();
        }

        [Test]
        public void RemoveComment_ShouldReturnTrueAndNull()
        {
            // Arrange
            var user = dataProvider.CreateUser();
            var userId = user.Id;
            var comment = user.Comments.First();
            var commentId = comment.Id;
            // Create user
            context.Users.Insert(user);
            // Act
            bool result = userRepository.RemoveComment(userId, commentId);
            // Find comment
            var queryEntity = Query.EQ("_id", userId);
            var queryComment = Query.EQ("Comments._id", commentId);
            var query = Query.And(queryEntity, queryComment);
            var cursor = context.Users.Find(query);
            Comment deletedComment = null;
            foreach (var i in cursor)
            {
                deletedComment = i.Comments.Find(x => x.Id == commentId);
            }
            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(deletedComment);
            // Clean collection
            context.Users.RemoveAll();
        }
    }
}
