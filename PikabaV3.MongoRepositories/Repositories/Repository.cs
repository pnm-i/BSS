using System.Collections.Generic;
using MongoDB.Bson;
using PikabaV3.MongoRepositories.Context;
using PikabaV3.MongoRepositories.Interfaces;

namespace PikabaV3.MongoRepositories.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected MongoContext Context;

        protected Repository(MongoContext context)
        {
            Context = context;
        }
        public abstract IEnumerable<T> GetAll();
        public abstract T Get(ObjectId id);
        public abstract bool Add(T item);
        public abstract bool Update(ObjectId id, T item);
        public abstract bool Remove(ObjectId id);
    }
}
