using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace PikabaV3.Models.Entities
{
    public class Product
    {
        public ObjectId Id { get; set; }
        public Owner Owner { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public List<Comment> Comments { get; set; }
        public List<ObjectId> Category_Ids { get; set; }
    }
}