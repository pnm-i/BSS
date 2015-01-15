using System.Collections.Generic;
using MongoDB.Bson;
using PikabaV3.Models.Entities;

namespace PikabaV3.MongoRepositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>, ICommentRepository
    {
        /// <summary>
        /// Get all products Seller by him id.
        /// </summary>
        /// <param name="sellerId">Seller id</param>
        /// <returns>all Seller products</returns>
        IEnumerable<Product> GetSellerProducts(ObjectId sellerId);
    }
}
