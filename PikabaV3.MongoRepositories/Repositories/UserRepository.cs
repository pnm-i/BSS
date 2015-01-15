using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories.Context;
using PikabaV3.MongoRepositories.Interfaces;

namespace PikabaV3.MongoRepositories.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MongoContext context) : base(context) { }

        /// <summary>
        /// Get all users (buyers) from db
        /// </summary>
        /// <returns>Buyers (buyers)</returns>
        public override IEnumerable<User> GetAll()
        {
            return Context.Users.Find(Query.EQ("Role", UserRole.Buyer.ToString())).ToList();
        }

        /// <summary>
        /// Get one user by id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User (buyer)</returns>
        public override User Get(ObjectId userId)
        {
            var user = Context.Users.FindOneById(userId);
            return user;
        }

        /// <summary>
        /// Create new user (buyer)
        /// </summary>
        /// <param name="user">Object user</param>
        /// <returns>true - OK; false - fail</returns>
        public override bool Add(User user)
        {
            return Context.Users.Insert(user).Ok;
        }

        /// <summary>
        /// Update profile user (buyer)
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="user">Object user</param>
        /// <returns>true - OK; false - fail</returns>
        public override bool Update(ObjectId userId, User user)
        {
            var query = Query.EQ("_id", userId);
            var update = MongoDB.Driver.Builders.Update
                .Set("DisplayName", user.DisplayName)
                .Set("Email", user.Email);
            return Context.Users.Update(query, update).DocumentsAffected > 0;
        }

        /// <summary>
        /// Change password any users, buyer or sellers
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="newPassword">String new password</param>
        /// <returns>true - OK; false - fail</returns>
        public bool ChangePassword(ObjectId userId, string newPassword)
        {
            var query = Query.EQ("_id", userId);
            var update = MongoDB.Driver.Builders.Update
                .Set("Password", newPassword);
            return Context.Users.Update(query, update).DocumentsAffected > 0;
        }

        /// <summary>
        /// Remove account any users, buyer or sellers
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>true - OK; false - fail</returns>
        public override bool Remove(ObjectId userId)
        {
            var query = Query.EQ("_id", userId);
            return Context.Users.Remove(query).DocumentsAffected > 0;
        }

        /// <summary>
        /// Find email to check busy
        /// </summary>
        /// <param name="newEmail">New email</param>
        /// <returns>Busy email</returns>
        public string FindUserEmail(string newEmail)
        {
            var query = Query.EQ("Email", newEmail);
            var user = Context.Sellers.Find(query);
            string email = null;
            foreach (var i in user)
            {
                email = i.Email;
            }
            return email;
        }

        /// <summary>
        /// Get last added comments
        /// </summary>
        /// <param name="entityId">User id</param>
        /// <param name="countLastComment">Count last added comment</param>
        /// <returns>Last added comment</returns>
        public IEnumerable<Comment> GetEntityComments(ObjectId entityId, int countLastComment)
        {
            var queryComments = Fields.Slice("Comments", -countLastComment);
            var queryEntity = Query.EQ("_id", entityId);
            var cursor = Context.Users.Find(queryEntity).SetFields(queryComments);
            IEnumerable<Comment> comments = new List<Comment>();
            foreach (var i in cursor)
            {
                comments = i.Comments;
            }
            return comments;
        }

        /// <summary>
        /// Get one comment about this User
        /// </summary>
        /// <param name="entityId">User id</param>
        /// <param name="commentId">Comment id</param>
        /// <returns>Comment user by id</returns>
        public Comment GetComment(ObjectId entityId, ObjectId commentId)
        {
            var queryEntity = Query.EQ("_id", entityId);
            var queryComment = Query.EQ("Comments._id", commentId);
            var query = Query.And(queryEntity, queryComment);
            var cursor = Context.Users.Find(query);
            var comment = new Comment();
            foreach (var i in cursor)
            {
                comment = i.Comments.Find(x => x.Id == commentId);
            }
            return comment;
        }

        /// <summary>
        /// Create new comment to soome User
        /// </summary>
        /// <param name="entityId">User Id</param>
        /// <param name="comment">Model comment</param>
        /// <returns>true - OK; false - fail</returns>
        public bool AddComment(ObjectId entityId, Comment comment)
        {
            var query = Query.EQ("_id", entityId);
            var update = MongoDB.Driver.Builders.Update.Push("Comments", comment.ToBsonDocument());
            return Context.Users.Update(query, update).DocumentsAffected > 0;
        }

        /// <summary>
        /// Update comment soome User
        /// </summary>
        /// <param name="entityId">Id comment User</param>
        /// <param name="commentId">Comment id</param>
        /// <param name="comment">Model comment</param>
        /// <returns>true - OK; false - fail</returns>
        public bool UpdateComment(ObjectId entityId, ObjectId commentId, Comment comment)
        {
            var queryEntity = Query.EQ("_id", entityId);
            var queryComment = Query.EQ("Comments._id", commentId);
            var query = Query.And(queryEntity, queryComment);
            var update = MongoDB.Driver.Builders.Update.Set("Comments.$.Text", comment.Text);
            //var update = Update.Set("Comments.$", comment.ToBsonDocument());// for replace all comment
            return Context.Users.Update(query, update).DocumentsAffected > 0;
        }

        /// <summary>
        /// Remove comment soome User by comment id
        /// </summary>
        /// <param name="entityId">User id</param>
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
            return Context.Users.Update(query, update).DocumentsAffected > 0;
        }
    }
}