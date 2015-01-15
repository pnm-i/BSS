using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PikabaV3.Models.Entities
{
    public enum UserRole
    {
        Buyer,
        Seller,
        Admin
    }

    public class Owner
    {
        public ObjectId User_Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }

    public class User
    {
        public ObjectId Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        [BsonRepresentation(BsonType.String)]
        public UserRole Role { get; set; }
        public DateTime DateRegister { get; set; }
        public List<Comment> Comments { get; set; }
    }

    public class Seller : User
    {
        public string Location { get; set; }
        public string Phone { get; set; }

        public Seller()
        {
            Role = UserRole.Seller;
        }
    }
}