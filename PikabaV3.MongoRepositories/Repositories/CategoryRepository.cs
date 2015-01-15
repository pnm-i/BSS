using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories.Context;

namespace PikabaV3.MongoRepositories.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        public CategoryRepository(MongoContext context) : base(context) { }

        /// <summary>
        /// Get all category
        /// </summary>
        /// <returns>All category</returns>
        public override IEnumerable<Category> GetAll()
        {
            return Context.Categories.AsQueryable();
        }

        /// <summary>
        /// Get one category
        /// </summary>
        /// <param name="id">Id category</param>
        /// <returns>Category by id</returns>
        public override Category Get(ObjectId id)
        {
            return Context.Categories.FindOneById(id);
        }

        /// <summary>
        /// Created new category
        /// </summary>
        /// <param name="category">Object category</param>
        /// <returns>true - OK; false - fail</returns>
        public override bool Add(Category category)
        {
            return Context.Categories.Insert(category).Ok;
        }

        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="id">Id category</param>
        /// <param name="category">Object category</param>
        /// <returns>true - OK; false - fail</returns>
        public override bool Update(ObjectId id, Category category)
        {
            var query = Query.EQ("_id", id);
            var update = MongoDB.Driver.Builders.Update
                .Set("Name", category.Name)
                .Set("Parent_Id", category.Parent_Id);
            return Context.Categories.Update(query, update).DocumentsAffected > 0;
        }

        /// <summary>
        /// Removed category by id
        /// </summary>
        /// <param name="id">Id category</param>
        /// <returns>true - OK; false - fail</returns>
        public override bool Remove(ObjectId id)
        {
            return Context.Categories.Remove(Query.EQ("_id", id)).DocumentsAffected > 0;
        }
    }
}