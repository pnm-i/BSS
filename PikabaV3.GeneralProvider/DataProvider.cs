using System;
using System.Collections.Generic;
using MongoDB.Bson;
using PikabaV3.Models.Entities;
using PikabaV3.MongoRepositories;

namespace PikabaV3.GeneralProvider
{
    public class DataProvider
    {
        /// <summary>
        /// Fill db initail data
        /// </summary>
        public void FillDataBase(IPikabaV3Service service)
        {
            foreach (var i in CreateCategories())
            {
                service.Categories.Add(i);
            }

            foreach (var i in CreateUsers())
            {
                service.Users.Add(i);
            }

            foreach (var i in CreateSellers())
            {
                service.Sellers.Add(i);
            }

            foreach (var i in CreateAdmins())
            {
                service.Sellers.Add(i);
            }

            foreach (var i in CreateProducts())
            {
                service.Products.Add(i);
            }

            service.Sessions.Add(CreateSession());
        }
        
        #region Initial data

        private IEnumerable<Category> CreateCategories()
        {
            var categories = new List<Category>
            {
               new Category
               {
                   Id = ObjectId.Parse("53fd73b37714792308071266"),
                   Name = "Auto Moto Vehicles"
               },
               new Category
               {
                   Id = ObjectId.Parse("53fd7448771479230807126a"),
                   Name = "Video Games & Consoles"
               },

               new Category
               {
                   Id = ObjectId.Parse("53fe07ff30ebabcbfd522a12"),
                   Name = "Mersedes",
                   Parent_Id = ObjectId.Parse("53fd73b37714792308071266")
               },
               new Category
               {
                   Id = ObjectId.Parse("53fe07ff30ebabcbfd522a13"),
                   Name = "BMW",
                   Parent_Id = ObjectId.Parse("53fd73b37714792308071266")
               },
               new Category
               {
                   Id = ObjectId.Parse("53fe07ff30ebabcbfd522a14"),
                   Name = "AUDI",
                   Parent_Id = ObjectId.Parse("53fd73b37714792308071266")
               },

               new Category
               {
                   Id = ObjectId.Parse("53fe0a2530ebabcbfd522a1b"),
                   Name = "Mobile Games",
                   Parent_Id = ObjectId.Parse("53fd7448771479230807126a")
               },
               new Category
               {
                   Id = ObjectId.Parse("53fe0a2530ebabcbfd522a1c"),
                   Name = "Computer Games",
                   Parent_Id = ObjectId.Parse("53fd7448771479230807126a")
               },
               new Category
               {
                   Id = ObjectId.Parse("53fe0a2530ebabcbfd522a1d"),
                   Name = "PS3 Games",
                   Parent_Id = ObjectId.Parse("53fd7448771479230807126a")
               },
            };
            return categories;
        }

        private IEnumerable<User> CreateUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = ObjectId.Parse("5421cc37e2556e1de898874e"),
                    DisplayName = "Pedro",
                    Email = "pedro@mail.ru",
                    Password = "pedro",
                    DateRegister = DateTime.Now,
                    IsActive = true,
                    Role = UserRole.Buyer,
                    Comments = new List<Comment>
                    {
                        new Comment
                        {
                            Id = ObjectId.GenerateNewId(),
                            Text = "Nice buyer, i recomend this man",
                            DateCreation = DateTime.Now,
                            Owner = new Owner
                            {
                                DisplayName = "Allex",
                                Email = "allex@mail.ru",
                                User_Id = ObjectId.Parse("5419e1bee2556e18f865f3bd")
                            }
                        }
                    }
                }
            };
            return users;
        }

        private IEnumerable<Seller> CreateSellers()
        {
            var sellers = new List<Seller>
            {
                new Seller
                {
                    Id = ObjectId.Parse("5419e1bee2556e18f865f3bd"),
                    DisplayName = "Allex",
                    Email = "allex@mail.ru",
                    Password = "allex",
                    DateRegister = DateTime.Now,
                    IsActive = true,
                    Role = UserRole.Seller,
                    Comments = new List<Comment>
                    {
                        new Comment
                        {
                            Id = ObjectId.GenerateNewId(),
                            Text = "Good seller, i recomend him products",
                            DateCreation = DateTime.Now,
                            Owner = new Owner
                            {
                                DisplayName = "Pedro",
                                Email = "pedro@mail.ru",
                                User_Id = ObjectId.Parse("5421cc37e2556e1de898874e")
                            }
                        }
                    },
                    Location = "Seattle",
                    Phone = "0664588556"
                }
            };
            return sellers;
        }

        private IEnumerable<Seller> CreateAdmins()
        {
            var admins = new List<Seller>
            {
                new Seller
                {
                    Id = ObjectId.Parse("5419f92ee2557018f85199c9"),
                    DisplayName = "Ivan",
                    Email = "ivan@mail.ru",
                    Password = "ivan",
                    DateRegister = DateTime.Now,
                    IsActive = true,
                    Role = UserRole.Admin,
                    Comments = new List<Comment>(),
                    Location = "USA",
                    Phone = "0632112121"
                }
            };
            return admins;
        }

        private IEnumerable<Product> CreateProducts()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = ObjectId.Parse("54230d5ae2556e168c71183d"),
                    Title = "BMW M5",
                    Description = "nice car, and very powerful",
                    Price = 20000,
                    Category_Ids = new List<ObjectId>
                    {
                        new ObjectId("53fd73b37714792308071266"),
                        new ObjectId("53fe07ff30ebabcbfd522a13")
                    },
                    DateAdded = DateTime.Now,
                    Owner = new Owner
                    {
                        DisplayName = "Allex",
                        Email = "allex@mail.ru",
                        User_Id = ObjectId.Parse("5419e1bee2556e18f865f3bd")
                    },
                    Comments = new List<Comment>
                    {
                        new Comment
                        {
                            Id = ObjectId.GenerateNewId(),
                            Text = "good car",
                            DateCreation = DateTime.Now,
                            Owner = new Owner
                            {
                                DisplayName = "Pedro",
                                Email = "pedro@mail.ru",
                                User_Id = ObjectId.Parse("5421cc37e2556e1de898874e")
                            }
                        }
                    }
                },
                new Product
                {
                    Id = ObjectId.Parse("54230e4be2556e255c02ea9a"),
                    Title = "BMW X6",
                    Description = "big vehicle",
                    Price = 27000,
                    Category_Ids = new List<ObjectId>
                    {
                        new ObjectId("53fd73b37714792308071266"),
                        new ObjectId("53fe07ff30ebabcbfd522a13")
                    },
                    DateAdded = DateTime.Now,
                    Owner = new Owner
                    {
                        DisplayName = "Allex",
                        Email = "allex@mail.ru",
                        User_Id = ObjectId.Parse("5419e1bee2556e18f865f3bd")
                    },
                    Comments = new List<Comment>
                    {
                        new Comment
                        {
                            Id = ObjectId.GenerateNewId(),
                            Text = "big vehicle",
                            DateCreation = DateTime.Now,
                            Owner = new Owner
                            {
                                DisplayName = "Pedro",
                                Email = "pedro@mail.ru",
                                User_Id = ObjectId.Parse("5421cc37e2556e1de898874e")
                            }
                        }
                    }
                },
                new Product
                {
                    Id = ObjectId.Parse("5423dc92e2556e2174aae9e4"),
                    Title = "Audi A8 2000 year",
                    Description = "big engine",
                    Price = 5000,
                    Category_Ids = new List<ObjectId>
                    {
                        new ObjectId("53fd73b37714792308071266"),
                        new ObjectId("53fe07ff30ebabcbfd522a14")
                    },
                    DateAdded = DateTime.Now,
                    Owner = new Owner
                    {
                        DisplayName = "Allex",
                        Email = "allex@mail.ru",
                        User_Id = ObjectId.Parse("5419e1bee2556e18f865f3bd")
                    },
                    Comments = new List<Comment>
                    {
                        new Comment
                        {
                            Id = ObjectId.GenerateNewId(),
                            Text = "norm car",
                            DateCreation = DateTime.Now,
                            Owner = new Owner
                            {
                                DisplayName = "Pedro",
                                Email = "pedro@mail.ru",
                                User_Id = ObjectId.Parse("5421cc37e2556e1de898874e")
                            }
                        }
                    }
                }
            };
            return products;
        }

        private UserSession CreateSession()
        {
            var session = new UserSession
            {
                CookieUuid = "a26ecaf0-a6f8-4ebe-f04f-161bb4ed366a",
                UserRole = UserRole.Seller,
                User_Id = ObjectId.Parse("5419e1bee2556e18f865f3bd")
            };
            return session;
        }

        #endregion
    }
}