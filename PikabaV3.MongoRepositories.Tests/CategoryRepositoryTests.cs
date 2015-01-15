using System.Collections.Generic;
using System.Linq;
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
    public class CategoryRepositoryTests
    {
        private MongoContext context;
        private IRepository<Category> categoryRepository;
        private TestDataProvider dataProvider;

        public CategoryRepositoryTests()
        {
            context = new MongoContext();
            categoryRepository = new CategoryRepository(context);
            dataProvider = new TestDataProvider();
            context.Categories.RemoveAll();
        }

        [Test]
        public void GetAll_ShouldReturnEightCtegories()
        {
            var categories = dataProvider.CreateCategories();
            // Fill collection
            foreach (var c in categories)
            {
                context.Categories.Insert(c);
            }
            // Act
            IEnumerable<Category> returnedCategories = categoryRepository.GetAll();
            // Assert
            Assert.AreEqual(8, returnedCategories.Count());
            // Clean collection
            context.Categories.RemoveAll();
        }

        [Test]
        public void Get_ShouldReturnCategoryById()
        {
            // Arrange
            Category category = dataProvider.CreateCategory();
            var categoryId = category.Id;
            // Fill collection
            context.Categories.Insert(category);
            // Act
            Category result = categoryRepository.Get(categoryId);
            // Assert
            Assert.AreEqual(categoryId, result.Id);
            // Clean collection
            context.Categories.RemoveAll();
        }

        [Test]
        public void Add_ShouldReturnTrue()
        {
            // Arrange
            var categoryId = ObjectId.GenerateNewId();
            Category category = dataProvider.CreateCategory();
            category.Id = categoryId;
            // Act
            bool result = categoryRepository.Add(category);
            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(categoryId, context.Categories.FindOneById(categoryId).Id);
            // Clean collection
            context.Categories.RemoveAll();
        }

        [Test]
        public void Update_ShouldUpdateOldCategory()
        {
            // Arrange
            Category oldCategory = dataProvider.CreateCategory();
            context.Categories.Insert(oldCategory);
            // Update old category
            oldCategory.Name = "Update name";
            // Act
            bool result = categoryRepository.Update(oldCategory.Id, oldCategory);
            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(oldCategory.Name, context.Categories.FindOneById(oldCategory.Id).Name);
            // Clean collection
            context.Categories.RemoveAll();
        }

        [Test]
        public void Remove_ShouldReturnTrueAndNull()
        {
            // Arrange
            Category category = dataProvider.CreateCategory();
            var categoryId = category.Id;
            context.Categories.Insert(category);
            // Act
            bool result = categoryRepository.Remove(categoryId);
            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(context.Categories.FindOneById(categoryId));
            // Clean collection
            context.Categories.RemoveAll();
        }
    }
}
