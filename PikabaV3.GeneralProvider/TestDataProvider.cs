using System;
using System.Collections.Generic;
using System.Globalization;
using PikabaV3.Models;
using PikabaV3.Models.Entities;
using MongoDB.Bson;

namespace PikabaV3.GeneralProvider
{
    /// <summary>
    /// Create fake data for tests
    /// </summary>
    public class TestDataProvider
    {
        public Product CreateProduct()
        {
            var product = new Product
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
            };
            return product;
        }

        public Product CreateProductWitchTwentyComments()
        {
            var product = new Product
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
                Comments = new List<Comment>()
            };
            for (int i = 1; i <= 20; i++)
            {
                product.Comments.Add(new Comment
                {
                    Id = ObjectId.GenerateNewId(),
                    Text = "comment " + i.ToString(CultureInfo.InvariantCulture),
                    DateCreation = DateTime.Now,
                    Owner = new Owner
                    {
                        DisplayName = "Pedro",
                        Email = "pedro@mail.ru",
                        User_Id = ObjectId.Parse("5421cc37e2556e1de898874e")
                    }
                });
            }

            return product;
        }

        public ProductModel CreateProductModel()
        {
            var productModel = new ProductModel
            {
                Category_Ids = new List<string> { "5419e1bee2556e18f865f3bd" },
                Title = "TitleTest",
                Description = "DescriptionTest1",
                Price = 10
            };
            return productModel;
        }

        public UserSession CreateSession()
        {
            var session = new UserSession
            {
                Id = ObjectId.GenerateNewId(),
                CookieUuid = "a26ecaf0-a6f8-4ebe-f04f-161bb4ed366a",
                UserRole = UserRole.Seller,
                User_Id = ObjectId.Parse("5419e1bee2556e18f865f3bd")
            };
            return session;
        }

        public Seller CreateSeller()
        {
            var seller = new Seller
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
                };
            return seller;
        }

        public IEnumerable<Seller> CreateSellers()
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
                },
                new Seller
                {
                    Id = ObjectId.Parse("6419e1bee2556e18f865f3bd"),
                    DisplayName = "Ashot",
                    Email = "ashot@mail.ru",
                    Password = "ashot",
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
                    Phone = "0632112121"
                }
            };

            return sellers;
        }

        public Comment CreateComment()
        {
            var comment = new Comment
            {
                Id = ObjectId.GenerateNewId(),
                Owner = CreateProduct().Owner,
                DateCreation = DateTime.Now,
                Text = "Test Text Comment"
            };
            return comment;
        }

        public CommentModel CreateCommentModel()
        {
            var commentModel = new CommentModel
            {
                Text = "Test Text Comment"
            };
            return commentModel;
        }

        public IEnumerable<Product> CreateProducts()
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
                }
            };
            return products;
        }

        public IEnumerable<User> CreateUsers()
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
                },
                new User
                {
                    Id = ObjectId.Parse("6421cc37e2556e1de898874e"),
                    DisplayName = "Pupkin",
                    Email = "pupkin@mail.ru",
                    Password = "pupkin",
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

        public User CreateUser()
        {
            var user = new User
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
            };
            return user;
        }

        public User CreateUserWitchTwentyComments()
        {
            var user = new User
            {
                Id = ObjectId.Parse("5421cc37e2556e1de898874e"),
                DisplayName = "Pedro",
                Email = "pedro@mail.ru",
                Password = "pedro",
                DateRegister = DateTime.Now,
                IsActive = true,
                Role = UserRole.Buyer,
                Comments = new List<Comment>()
            };
            for (int i = 1; i <= 20; i++)
            {
                user.Comments.Add(new Comment
                {
                    Id = ObjectId.GenerateNewId(),
                    Text = "comment " + i.ToString(CultureInfo.InvariantCulture),
                    DateCreation = DateTime.Now,
                    Owner = new Owner
                    {
                        DisplayName = "Allex",
                        Email = "allex@mail.ru",
                        User_Id = ObjectId.Parse("5419e1bee2556e18f865f3bd")
                    }
                });
            }
            return user;
        }

        public IEnumerable<Category> CreateCategories()
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

        public Category CreateCategory()
        {
            var category = new Category
            {
                Id = ObjectId.Parse("53fd73b37714792308071266"),
                Name = "Auto Moto Vehicles"
            };
            return category;
        }

        public LoginModel CreateLoginModel()
        {
            var model = new LoginModel
            {
                Email = "pedro@mail.ru",
                Password = "pedro",
            };
            return model;
        }
    }
}
