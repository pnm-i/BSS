using System.Collections.Generic;
using MongoDB.Bson;
using PikabaV3.Models.Entities;

namespace PikabaV3.MongoRepositories.Interfaces
{
    public interface ICommentRepository
    {
        /// <summary>
        /// Get the last comments some entity.
        /// </summary>
        /// <param name="entityId">Entity id</param>
        /// <param name="countLastComment">Count last comment</param>
        /// <returns></returns>
        IEnumerable<Comment> GetEntityComments(ObjectId entityId, int countLastComment);

        /// <summary>
        /// Get one comment entity by id.
        /// </summary>
        /// <param name="entityId">Entity id</param>
        /// <param name="commentId">Comment id</param>
        /// <returns></returns>
        Comment GetComment(ObjectId entityId, ObjectId commentId);

        /// <summary>
        /// Add comment to some entity.
        /// </summary>
        /// <param name="entityId">Entity id</param>
        /// <param name="comment">Comment object</param>
        /// <returns></returns>
        bool AddComment(ObjectId entityId, Comment comment);

        /// <summary>
        /// Remove comment from some entity by id
        /// </summary>
        /// <param name="entityId">Entity id</param>
        /// <param name="commentId">Comment id</param>
        /// <returns></returns>
        bool RemoveComment(ObjectId entityId, ObjectId commentId);

        /// <summary>
        /// Update comment some entity.
        /// </summary>
        /// <param name="entityId">Entity id</param>
        /// <param name="commentId">Comment id</param>
        /// <param name="comment">Updated comment object</param>
        /// <returns></returns>
        bool UpdateComment(ObjectId entityId, ObjectId commentId, Comment comment);
    }
}
