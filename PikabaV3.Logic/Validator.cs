using System.Linq;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using PikabaV3.MongoRepositories;

namespace PikabaV3.Logic
{
    public class Validator : IValidator
    {
        private readonly IPikabaV3Service _service;

        public Validator()
        {
            _service = new PikabaV3Service();
        }

        public Validator(IPikabaV3Service service)
        {
            _service = service;
        }

        /// <summary>
        /// Check exceeded limit created products
        /// </summary>
        /// <param name="sellerId">Seller id</param>
        /// <returns>true - exceeded; false - not exceeded</returns>
        public virtual bool IsExceededLimitCreationProducts(ObjectId sellerId)
        {
            var products = _service.Products.GetSellerProducts(sellerId);
            if (products.Count() > 10)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check busy email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true - if busy; false - if not</returns>
        public virtual bool IsBusyEmail(string email)
        {
            if (_service.Users.FindUserEmail(email) != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check validation id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>true - valid; false - nor valid</returns>
        public virtual bool CheckValidId(string id)
        {
            var pattern = "^[0-9a-fA-F]{24}$";
            var regex = new Regex(pattern);
            var result = regex.IsMatch(id);
            return result;
        }
    }
}