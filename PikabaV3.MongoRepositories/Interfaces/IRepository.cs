using System.Collections.Generic;
using MongoDB.Bson;

namespace PikabaV3.MongoRepositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Get all items from db.
        /// </summary>
        /// <returns>All items</returns>
        IEnumerable<T> GetAll();
        
        /// <summary>
        /// Get one item from db by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Item</returns>
        T Get(ObjectId id);
       
        /// <summary>
        /// Add item in db.
        /// </summary>
        /// <param name="item">Item object</param>
        /// <returns>True if added; False if error</returns>
        bool Add(T item);
       
        /// <summary>
        /// Update item by id.
        /// </summary>
        /// <param name="id">Item id</param>
        /// <param name="item">Updatetd item object</param>
        /// <returns>True if updated; False if error</returns>
        bool Update(ObjectId id, T item);
        
        /// <summary>
        /// Remove item from db by id.
        /// </summary>
        /// <param name="id">Itme id</param>
        /// <returns>True if removed; False if error</returns>
        bool Remove(ObjectId id);
    }
}
