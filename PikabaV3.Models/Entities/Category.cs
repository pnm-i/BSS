using MongoDB.Bson;

namespace PikabaV3.Models.Entities
{
    public class Category
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public ObjectId Parent_Id { get; set; }
    }
}