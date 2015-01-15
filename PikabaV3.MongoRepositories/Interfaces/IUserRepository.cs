using MongoDB.Bson;
using PikabaV3.Models.Entities;

namespace PikabaV3.MongoRepositories.Interfaces
{
    public interface IUserRepository : IRepository<User>, ICommentRepository
    {
        /// <summary>
        /// Change password any users, buyer or sellers
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="newPassword">String new password</param>
        /// <returns>true - OK; false - fail</returns>
        bool ChangePassword(ObjectId userId, string newPassword);

        /// <summary>
        /// Find email to check busy
        /// </summary>
        /// <param name="newEmail">New email</param>
        /// <returns>Busy email</returns>
        string FindUserEmail(string newEmail);
    }
}
