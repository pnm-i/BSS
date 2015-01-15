using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories.Context;
using PikabaV3.MongoRepositories.Interfaces;

namespace PikabaV3.MongoRepositories.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MongoContext context) : base(context) { }

        /// <summary>
        /// Get all products from db
        /// </summary>
        /// <returns>All products</returns>
        public override IEnumerable<Product> GetAll()
        {
            return Context.Products.AsQueryable();
        }

        /// <summary>
        /// Get all products Seller by him id.
        /// </summary>
        /// <param name="sellerId">Seller id</param>
        /// <returns>all Seller products</returns>
        public IEnumerable<Product> GetSellerProducts(ObjectId sellerId)
        {
            var query = Query.EQ("Owner.User_Id", sellerId);
            var result = Context.Products.Find(query).ToList();
            return result;
        }

        /// <summary>
        /// Get one Product by id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns>One product</returns>
        public override Product Get(ObjectId id)
        {
            return Context.Products.FindOneById(id);
        }

        /// <summary>
        /// Create new Product
        /// </summary>
        /// <param name="product">Object Product</param>
        /// <returns>true - OK; false - fail</returns>
        public override bool Add(Product product)
        {
            return Context.Products.Insert(product).Ok;
        }

        /// <summary>
        /// Update product by id
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <param name="product">Object product</param>
        /// <returns>true - OK; false - fail</returns>
        public override bool Update(ObjectId productId, Product product)
        {
            var query = Query.EQ("_id", productId);
            var update = MongoDB.Driver.Builders.Update
                .Set("Title", product.Title)
                .Set("Price", product.Price.ToString(CultureInfo.InvariantCulture))
                .Set("Description", product.Description);
            return Context.Products.Update(query, update).DocumentsAffected > 0;
        }

        /// <summary>
        /// Remove one product by id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns>true - OK; false - fail</returns>
        public override bool Remove(ObjectId id)
        {
            var query = Query.EQ("_id", id);
            return Context.Products.Remove(query).DocumentsAffected > 0;
        }

        /// <summary>
        /// Get latest comments about this product
        /// </summary>
        /// <param name="entityId">Product Id</param>
        /// <param name="countLastComment">Amount last added comment</param>
        /// <returns>Last added comment</returns>
        public IEnumerable<Comment> GetEntityComments(ObjectId entityId, int countLastComment)
        {
            var queryComments = Fields.Slice("Comments", -countLastComment);
            var queryEntity = Query.EQ("_id", entityId);
            var cursor = Context.Products.Find(queryEntity).SetFields(queryComments);
            IEnumerable<Comment> comments = new List<Comment>();
            foreach (var i in cursor)
            {
                comments = i.Comments;
            }
            return comments;
        }

        /// <summary>
        /// Get one comment about this Product
        /// </summary>
        /// <param name="entityId">Product id</param>
        /// <param name="commentId">Comment id</param>
        /// <returns>One comment</returns>
        public Comment GetComment(ObjectId entityId, ObjectId commentId)
        {
            var queryEntity = Query.EQ("_id", entityId);
            var queryComment = Query.EQ("Comments._id", commentId);
            var query = Query.And(queryEntity, queryComment);
            var cursor = Context.Products.Find(query);
            var comment = new Comment();
            foreach (var i in cursor)
            {
                comment = i.Comments.Find(x => x.Id == commentId);
            }
            return comment;
        }

        /// <summary>
        /// Create new comment about product
        /// </summary>
        /// <param name="entityId">Product id</param>
        /// <param name="comment">Object comment</param>
        /// <returns>true - OK; false - fail</returns>
        public bool AddComment(ObjectId entityId, Comment comment)
        {
            var query = Query.EQ("_id", entityId);
            var update = MongoDB.Driver.Builders.Update.Push("Comments", comment.ToBsonDocument());
            return Context.Products.Update(query, update).Ok;
        }

        /// <summary>
        /// Update comment about Product
        /// </summary>
        /// <param name="entityId">Product id</param>
        /// <param name="commentId">Comment id</param>
        /// <param name="comment">Object comment</param>
        /// <returns>true - OK; false - fail</returns>
        public bool UpdateComment(ObjectId entityId, ObjectId commentId, Comment comment)
        {
            var queryEntity = Query.EQ("_id", entityId);
            var queryComment = Query.EQ("Comments._id", commentId);
            var query = Query.And(queryEntity, queryComment);
            var update = MongoDB.Driver.Builders.Update.Set("Comments.$.Text", comment.Text);
            return Context.Products.Update(query, update).DocumentsAffected > 0;
        }

        /// <summary>
        /// Remove comment about this product
        /// </summary>
        /// <param name="entityId">Product id</param>
        /// <param name="commentId">Comment id</param>
        /// <returns>true - OK; false - fail</returns>
        public bool RemoveComment(ObjectId entityId, ObjectId commentId)
        {
            var queryEntity = Query.EQ("_id", entityId);
            var queryComment = Query.EQ("Comments._id", commentId);
            var query = Query.And(queryEntity, queryComment);
            var update = MongoDB.Driver.Builders.Update.Pull("Comments", new BsonDocument
            {
                { "_id", commentId }
            });
            return Context.Products.Update(query, update).DocumentsAffected > 0;
        }
    }
}