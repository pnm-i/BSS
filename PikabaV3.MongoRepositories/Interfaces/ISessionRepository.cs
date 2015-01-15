using PikabaV3.Models;
using PikabaV3.Models.Entities;

namespace PikabaV3.MongoRepositories.Interfaces
{
    public interface ISessionRepository : IRepository<UserSession>
    {
        /// <summary>
        /// Get session object from db by cookie uuid.
        /// </summary>
        /// <param name="cookieUuid">Cookie uuid</param>
        /// <returns>Session object</returns>
        UserSession Get(string cookieUuid);

        /// <summary>
        /// Get user from db by login data model.
        /// </summary>
        /// <param name="loginData">Object login data</param>
        /// <returns>User object</returns>
        User GetUser(LoginModel loginData);
    }
}
