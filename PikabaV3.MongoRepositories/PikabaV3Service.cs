using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories.Context;
using PikabaV3.MongoRepositories.Interfaces;
using PikabaV3.MongoRepositories.Repositories;

namespace PikabaV3.MongoRepositories
{
    public interface IPikabaV3Service
    {
        IUserRepository Users { get; }
        ISessionRepository Sessions { get; }
        IRepository<Seller> Sellers { get; }
        IProductRepository Products { get; }
        IRepository<Category> Categories { get; }
    }

    public class PikabaV3Service : IPikabaV3Service
    {
        private IUserRepository _users;
        private ISessionRepository _sessions;
        private IRepository<Seller> _sellers;
        private IProductRepository _products;
        private IRepository<Category> _categories;

        public IUserRepository Users
        {
            get
            {
                if (_users == null)
                    _users = new UserRepository(new MongoContext());

                return _users;
            }
        }

        public ISessionRepository Sessions
        {
            get
            {
                if(_sessions == null)
                    _sessions = new SessionRepository(new MongoContext());
                return _sessions;
            }
        }
        
        public IRepository<Seller> Sellers
        {
            get
            {
                if(_sellers == null)
                    _sellers = new SellerRepository(new MongoContext());
                return _sellers;
            }
        }

        public IProductRepository Products
        {
            get
            {
                if(_products == null)
                    _products = new ProductRepository(new MongoContext());
                return _products;
            }
        }
        
        public IRepository<Category> Categories
        {
            get
            {
                if(_categories == null)
                    _categories = new CategoryRepository(new MongoContext());
                return _categories;
            }
        }
    }
}
