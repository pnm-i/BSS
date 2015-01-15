using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using PikabaV3.Models;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories.Context;
using PikabaV3.MongoRepositories.Interfaces;

namespace PikabaV3.MongoRepositories.Repositories
{
    public class SessionRepository : Repository<UserSession>, ISessionRepository
    {
        public SessionRepository(MongoContext context) : base(context) { }

        /// <summary>
        /// Get session by cookieUuid
        /// </summary>
        /// <param name="cookieUuid">Cookie id (uuid)</param>
        /// <returns>Session object</returns>
        public UserSession Get(string cookieUuid)
        {
            return Context.Sessions.FindOne(Query.EQ("CookieUuid", cookieUuid));
        }
        
        /// <summary>
        /// Get session by User id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Session object</returns>
        public override UserSession Get(ObjectId userId)
        {
            return Context.Sessions.FindOne(Query.EQ("User_Id", userId));
        }
        
        /// <summary>
        /// Get user to login method
        /// </summary>
        /// <param name="loginData">Object login</param>
        /// <returns>User object</returns>
        public User GetUser(LoginModel loginData)
        {
            var emailQuery = Query.EQ("Email", loginData.Email);
            var passwordQuery = Query.EQ("Password", loginData.Password);
            var query = Query.And(emailQuery, passwordQuery);
            var user = Context.Sellers.FindOne(query);
            return user;
        }
        
        /// <summary>
        /// Create new session if user is login
        /// </summary>
        /// <param name="session">Object login</param>
        /// <returns>true - OK; false - fail</returns>
        public override bool Add(UserSession session)
        {
            return Context.Sessions.Insert(session).Ok;
        }

        /// <summary>
        /// Remove session from db
        /// </summary>
        /// <param name="id">Session id</param>
        /// <returns>true - OK; false - fail</returns>
        public override bool Remove(ObjectId id)
        {
            return Context.Sessions.Remove(Query.EQ("_id", id)).DocumentsAffected > 0;
        }
        
        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <returns>Exeption</returns>
        public override IEnumerable<UserSession> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns>Exeption</returns>
        public override bool Update(ObjectId id, UserSession item)
        {
            throw new NotImplementedException();
        }
    }
}