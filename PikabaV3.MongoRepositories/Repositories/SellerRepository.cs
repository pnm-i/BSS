using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories.Context;

namespace PikabaV3.MongoRepositories.Repositories
{
    public class SellerRepository : Repository<Seller>
    {
        public SellerRepository(MongoContext context) : base(context) { }

        /// <summary>
        /// Get all Sellers
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Seller> GetAll()
        {
            return Context.Sellers.Find(Query.EQ("Role", UserRole.Seller.ToString())).ToList();
        }

        /// <summary>
        /// Get one Seller by id
        /// </summary>
        /// <param name="sellerId">Seller id</param>
        /// <returns>Seller</returns>
        public override Seller Get(ObjectId sellerId)
        {
            var seller = Context.Sellers.FindOneById(sellerId);
            return seller;
        }

        /// <summary>
        /// Create new seller
        /// </summary>
        /// <param name="seller">Object Seller</param>
        /// <returns>true - OK; false - fail</returns>
        public override bool Add(Seller seller)
        {
            return Context.Sellers.Insert(seller).Ok;
        }

        /// <summary>
        /// Update Seller profile
        /// </summary>
        /// <param name="sellerId">Seller id</param>
        /// <param name="seller">Object update</param>
        /// <returns>true - OK; false - fail</returns>
        public override bool Update(ObjectId sellerId, Seller seller)
        {
            var query = Query.EQ("_id", sellerId);
            var update = MongoDB.Driver.Builders.Update
                .Set("DisplayName", seller.DisplayName)
                .Set("Email", seller.Email)
                .Set("Location", seller.Location)
                .Set("Phone", seller.Phone);
            return Context.Sellers.Update(query, update).DocumentsAffected > 0;
        }

        public override bool Remove(ObjectId id)
        {
            throw new System.NotImplementedException();
        }
    }
}