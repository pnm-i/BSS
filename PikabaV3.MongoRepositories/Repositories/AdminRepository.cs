using PikabaV3.MongoRepositories.Context;

namespace PikabaV3.MongoRepositories.Repositories
{
    /// <summary>
    /// Temporary repository
    /// </summary>
    public class AdminRepository
    {
        private readonly MongoContext _db = new MongoContext();

        public void DropDataBase()
        {
            _db.DropDatabase();
        }

        public void DropCollection(string collname)
        {
            _db.DropCollection(collname);
        }
    }
}