using System;
using MongoDB.Bson;

namespace PikabaV3.Models.Entities
{
    public class Comment
    {
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public DateTime DateCreation { get; set; }
        public Owner Owner { get; set; }
    }
}