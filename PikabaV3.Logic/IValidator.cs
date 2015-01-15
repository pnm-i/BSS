using MongoDB.Bson;

namespace PikabaV3.Logic
{
    public interface IValidator
    {
        /// <summary>
        /// Check exceeded limit created products
        /// </summary>
        /// <param name="sellerId">Seller id</param>
        /// <returns>true - exceeded; false - not exceeded</returns>
        bool IsExceededLimitCreationProducts(ObjectId sellerId);

        /// <summary>
        /// Check busy email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true - if busy; false - if not</returns>
        bool IsBusyEmail(string email);

        /// <summary>
        /// Check validation id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>true - valid; false - nor valid</returns>
        bool CheckValidId(string id);
    }
}
