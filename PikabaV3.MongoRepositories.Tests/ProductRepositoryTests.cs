using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using NUnit.Framework;
using PikabaV3.GeneralProvider;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories.Context;
using PikabaV3.MongoRepositories.Interfaces;
using PikabaV3.MongoRepositories.Repositories;

namespace PikabaV3.MongoRepositories.Tests
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private MongoContext context;
        private IProductRepository productRepository;
        private TestDataProvider dataProvider;

        public ProductRepositoryTests()
        {
            context = new MongoContext();
            productRepository = new ProductRepository(context);
            dataProvider = new TestDataProvider();
            context.Products.RemoveAll();
        }

        [Test]
        public void GetAll_ShouldReturnTwoProducts()
        {
            var products = dataProvider.CreateProducts();
            // Fill collection
            foreach (var p in products)
            {
                context.Products.Insert(p);
            }
            // Act
            IEnumerable<Product> returnedProducts = productRepository.GetAll();
            // Assert
            Assert.AreEqual(2, returnedProducts.Count());
            // Clean collection
            context.Products.RemoveAll();
        }

        [Test]
        public void GetSellerProducts_ShouldReturnTwoProducts()
        {
            // Arrange
            var products = dataProvider.CreateProducts();
            var sellerId = ObjectId.Parse("5419e1bee2556e18f865f3bd");
            // Fill collection
            foreach (var p in products)
            {
                context.Products.Insert(p);
            }
            // Act
            IEnumerable<Product> returnedProducts = productRepository.GetSellerProducts(sellerId);
            // Assert
            Assert.AreEqual(2, returnedProducts.Count());
            // Clean collection
            context.Products.RemoveAll();
        }

        [Test]
        public void Get_ShouldReturnProductById()
        {
            // Arrange
            var product = dataProvider.CreateProduct();
            var productId = product.Id;
            // Fill collection
            context.Products.Insert(product);
            // Act
            Product result = productRepository.Get(productId);
            // Assert
            Assert.AreEqual(productId, result.Id);
            // Clean collection
            context.Products.RemoveAll();
        }

        [Test]
        public void Add_ShouldReturnTrue()
        {
            // Arrange
            var productId = ObjectId.GenerateNewId();
            var product = dataProvider.CreateProduct();
            product.Id = productId;
            // Act
            bool result = productRepository.Add(product);
            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(productId, context.Products.FindOneById(productId).Id);
            // Clean collection
            context.Products.RemoveAll();
        }

        [Test]
        public void Update_ShouldUpdateOldProduct()
        {
            // Arrange
            Product oldProduct = dataProvider.CreateProduct();
            context.Products.Insert(oldProduct);
            // Update old product
            oldProduct.Description = "Update Description";
            // Act
            bool result = productRepository.Update(oldProduct.Id, oldProduct);
            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(oldProduct.Description, context.Products.FindOneById(oldProduct.Id).Description);
            // Clean collection
            context.Products.RemoveAll();
        }

        [Test]
        public void Remove_ShouldReturnTrueAndNull()
        {
            // Arrange
            Product product = dataProvider.CreateProduct();
            var productId = product.Id;
            context.Products.Insert(product);
            // Act
            bool result = productRepository.Remove(productId);
            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(context.Products.FindOneById(productId));
            // Clean collection
            context.Products.RemoveAll();
        }

        [Test]
        public void GetProductComments_ShouldReturnLatestProductComments()
        {
            // Arrange
            int amountComments = 5;
            var product = dataProvider.CreateProductWitchTwentyComments();
            context.Products.Insert(product);
            // Act
            IEnumerable<Comment> result = productRepository.GetEntityComments(product.Id, amountComments);
            // Assert
            Assert.AreEqual(amountComments, result.Count());
            // Clean collection
            context.Products.RemoveAll();
        }

        [Test]
        public void GetComment_ShouldReturnCommentById()
        {
            // Arrange
            var commentId = ObjectId.Parse("53fe07ff30ebabcbfd522a14");
            var product = dataProvider.CreateProduct();
            product.Comments.Add(new Comment
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
            context.Products.Insert(product);
            // Act
            Comment result = productRepository.GetComment(product.Id, commentId);
            // Assert
            Assert.AreEqual(commentId, result.Id);
            // Clean collection
            context.Products.RemoveAll();
        }

        [Test]
        public void AddComment_ShouldReturnTrueAndAddedComment()
        {
            // Arrange
            var product = dataProvider.CreateProduct();
            var comment = dataProvider.CreateComment();
            var productId = product.Id;
            var commentId = comment.Id;
            context.Products.Insert(product);
            // Act
            bool result = productRepository.AddComment(productId, comment);
            // Find comment
            var queryEntity = Query.EQ("_id", productId);
            var queryComment = Query.EQ("Comments._id", commentId);
            var query = Query.And(queryEntity, queryComment);
            var cursor = context.Products.Find(query);
            var addedComment = new Comment();
            foreach (var i in cursor)
            {
                addedComment = i.Comments.Find(x => x.Id == commentId);
            }
            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(addedComment.Id, commentId);
            // Clean collection
            context.Products.RemoveAll();
        }

        [Test]
        public void UpdateComment_ShouldReturnTrueAndUpdatedComment()
        {
            // Arrange
            var product = dataProvider.CreateProduct();
            var productId = product.Id;
            var comment = product.Comments.First();
            var commentId = comment.Id;
            // Create product
            context.Products.Insert(product);
            // Update comment
            comment.Text = "Updated comment";
            // Act
            bool result = productRepository.UpdateComment(productId, commentId, comment);
            // Find comment
            var queryEntity = Query.EQ("_id", productId);
            var queryComment = Query.EQ("Comments._id", commentId);
            var query = Query.And(queryEntity, queryComment);
            var cursor = context.Products.Find(query);
            var updatedComment = new Comment();
            foreach (var i in cursor)
            {
                updatedComment = i.Comments.Find(x => x.Id == commentId);
            }
            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(updatedComment.Text, comment.Text);
            // Clean collection
            context.Products.RemoveAll();
        }

        [Test]
        public void RemoveComment_ShouldReturnTrueAndNull()
        {
            // Arrange
            var product = dataProvider.CreateProduct();
            var productId = product.Id;
            var comment = product.Comments.First();
            var commentId = comment.Id;
            // Create product
            context.Products.Insert(product);
            // Act
            bool result = productRepository.RemoveComment(productId, commentId);
            // Find comment
            var queryEntity = Query.EQ("_id", productId);
            var queryComment = Query.EQ("Comments._id", commentId);
            var query = Query.And(queryEntity, queryComment);
            var cursor = context.Products.Find(query);
            Comment deletedComment = null;
            foreach (var i in cursor)
            {
                deletedComment = i.Comments.Find(x => x.Id == commentId);
            }
            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(deletedComment);
            // Clean collection
            context.Products.RemoveAll();
        }
    }
}
