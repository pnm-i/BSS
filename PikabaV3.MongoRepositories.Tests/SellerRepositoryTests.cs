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
    public class SellerRepositoryTests
    {
        private MongoContext context;
        private IRepository<Seller> sellerRepository;
        private TestDataProvider dataProvider;

        public SellerRepositoryTests()
        {
            context = new MongoContext();
            sellerRepository = new SellerRepository(context);
            dataProvider = new TestDataProvider();
            context.Sellers.RemoveAll();
        }

        [Test]
        public void GetAll_ShouldReturnTwoSellers()
        {
            var sellers = dataProvider.CreateSellers();
            // Fill collection
            foreach (var i in sellers)
            {
                context.Sellers.Insert(i);
            }
            // Act
            IEnumerable<Seller> returnedSellers = sellerRepository.GetAll();
            // Assert
            Assert.AreEqual(2, returnedSellers.Count());
            // Clean collection
            context.Sellers.RemoveAll();
        }

        [Test]
        public void Get_ShouldReturnSellerById()
        {
            // Arrange
            Seller seller = dataProvider.CreateSeller();
            var sellerId = seller.Id;
            // Fill collection
            context.Sellers.Insert(seller);
            // Act
            Seller result = sellerRepository.Get(sellerId);
            // Assert
            Assert.AreEqual(sellerId, result.Id);
            // Clean collection
            context.Sellers.RemoveAll();
        }

        [Test]
        public void Add_ShouldReturnTrue()
        {
            // Arrange
            var sellerId = ObjectId.GenerateNewId();
            Seller seller = dataProvider.CreateSeller();
            seller.Id = sellerId;
            // Act
            bool result = sellerRepository.Add(seller);
            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(sellerId, context.Sellers.FindOneById(sellerId).Id);
            // Clean collection
            context.Sellers.RemoveAll();
        }

        [Test]
        public void Update_ShouldUpdateOldSeller()
        {
            // Arrange
            Seller oldsSeller = dataProvider.CreateSeller();
            context.Sellers.Insert(oldsSeller);
            // Update old seller
            oldsSeller.Location = "Update location";
            // Act
            bool result = sellerRepository.Update(oldsSeller.Id, oldsSeller);
            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(oldsSeller.Location, context.Sellers.FindOneById(oldsSeller.Id).Location);
            // Clean collection
            context.Sellers.RemoveAll();
        }
    }
}
