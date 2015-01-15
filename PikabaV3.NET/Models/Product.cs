using System;
using System.Collections.Generic;

namespace PikabaV3.NET.Models
{
    public class ProductModel
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<string> Category_Ids { get; set; }
        public string CookieUuid { get; set; }
    }

    public class Product
    {
        public string Id { get; set; }
        public Owner Owner { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public List<string> Category_Ids { get; set; }
    }

    public class Owner
    {
        public string User_Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}
