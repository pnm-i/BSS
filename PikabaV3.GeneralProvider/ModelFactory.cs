using System;
using System.Collections.Generic;
using MongoDB.Bson;
using PikabaV3.Models;
using PikabaV3.Models.Entities;

namespace PikabaV3.GeneralProvider
{
    public class ModelFactory : IModelFactory
    {
        /// <summary>
        /// Create product by model.
        /// </summary>
        /// <param name="seller">Seller obj</param>
        /// <param name="model">ProductModel obj</param>
        /// <returns>Product obj</returns>
        public virtual Product Create(Seller seller, ProductModel model)
        {
            var product = new Product
            {
                Id = ObjectId.GenerateNewId(),
                Title = model.Title,
                Price = model.Price,
                Description = model.Description,
                DateAdded = DateTime.Now,
                Owner = new Owner
                {
                    User_Id = seller.Id,
                    DisplayName = seller.DisplayName,
                    Email = seller.Email
                },
                Category_Ids = new List<ObjectId>()
            };
            foreach (var i in model.Category_Ids)
            {
                product.Category_Ids.Add(ObjectId.Parse(i));
            }
            return product;
        }

        /// <summary>
        /// Create product to update by model.
        /// </summary>
        /// <param name="model">ProductModel obj</param>
        /// <returns>Product obj</returns>
        public virtual Product Create(ProductModel model)
        {
            var product = new Product
            {
                Title = model.Title,
                Price = model.Price,
                Description = model.Description,
                Category_Ids = new List<ObjectId>()
            };
            foreach (var i in model.Category_Ids)
            {
                product.Category_Ids.Add(ObjectId.Parse(i));
            }
            return product;
        }

        /// <summary>
        /// Create category by model.
        /// </summary>
        /// <param name="model">CategoryModel obj</param>
        /// <returns>Category obj</returns>
        public virtual Category Create(CategoryModel model)
        {
            var category = new Category
            {
                Id = ObjectId.GenerateNewId(),
                Name = model.Name,
                Parent_Id = new ObjectId(model.Parent_Id)
            };
            return category;
        }

        /// <summary>
        /// Create comment by model, wich owner seller.
        /// </summary>
        /// <param name="seller">Seller obj</param>
        /// <param name="model">CommentModel obj</param>
        /// <returns>Comment obj</returns>
        public virtual Comment Create(Seller seller, CommentModel model)
        {
            var owner = new Owner
            {
                User_Id = seller.Id,
                DisplayName = seller.DisplayName,
                Email = seller.Email
            };
            var comment = new Comment
            {
                Id = ObjectId.GenerateNewId(),
                DateCreation = DateTime.Now,
                Text = model.Text,
                Owner = owner
            };
            return comment;
        }

        /// <summary>
        /// Create comment by model, wich owner buyer.
        /// </summary>
        /// <param name="user">User obj</param>
        /// <param name="model">CommentModel obj</param>
        /// <returns>Comment obj</returns>
        public virtual Comment Create(User user, CommentModel model)
        {
            var owner = new Owner
            {
                User_Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email
            };
            var comment = new Comment
            {
                Id = ObjectId.GenerateNewId(),
                DateCreation = DateTime.Now,
                Text = model.Text,
                Owner = owner
            };
            return comment;
        }

        /// <summary>
        /// Create comment to update, by model.
        /// </summary>
        /// <param name="model">CommentModel data</param>
        /// <returns>Comment obj</returns>
        public virtual Comment Create(CommentModel model)
        {
            var comment = new Comment
            {
                Text = model.Text
            };
            return comment;
        }

        /// <summary>
        /// Create seller by register model.
        /// </summary>
        /// <param name="model">RegisterSellerModel obj</param>
        /// <returns>Seller obj</returns>
        public virtual Seller Create(RegisterSellerModel model)
        {
            var seller = new Seller
            {
                Id = ObjectId.GenerateNewId(),
                DisplayName = model.DisplayName,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role,
                Location = model.Location,
                Phone = model.Phone,
                IsActive = true,
                DateRegister = DateTime.Now,
                Comments = new List<Comment>()
            };
            return seller;
        }

        /// <summary>
        /// Create seller entity to update by model.
        /// </summary>
        /// <param name="model">UpdateSellerModel obj</param>
        /// <returns>Seller obj</returns>
        public virtual Seller Create(UpdateSellerModel model)
        {
            var seller = new Seller
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                Location = model.Location,
                Phone = model.Phone,
            };
            return seller;
        }

        /// <summary>
        /// Create user by model.
        /// </summary>
        /// <param name="model">RegisterUserModel obj</param>
        /// <returns>User obj</returns>
        public virtual User Create(RegisterUserModel model)
        {
            var user = new User
            {
                Id = ObjectId.GenerateNewId(),
                DisplayName = model.DisplayName,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role,
                IsActive = true,
                DateRegister = DateTime.Now,
                Comments = new List<Comment>()
            };
            return user;
        }

        /// <summary>
        /// Create user to update by model.
        /// </summary>
        /// <param name="model">UpdateUserModel obj</param>
        /// <returns>User obj</returns>
        public virtual User Create(UpdateUserModel model)
        {
            var user = new User
            {
                DisplayName = model.DisplayName,
                Email = model.Email
            };
            return user;
        }

        /// <summary>
        /// Create session by user data and Login model.
        /// </summary>
        /// <param name="userData">User obj</param>
        /// <param name="loginModel">LoginModel obj</param>
        /// <param name="cookieUuid">Cookie Uuid</param>
        /// <returns>UserSession obj</returns>
        public virtual UserSession Create(User userData, LoginModel loginModel, string cookieUuid)
        {
            var session = new UserSession
            {
                Id = ObjectId.GenerateNewId(),
                User_Id = userData.Id,
                CookieUuid = cookieUuid,
                UserRole = userData.Role
            };
            return session;
        }

        /// <summary>
        /// Create password by model.
        /// </summary>
        /// <param name="model">ChangePasswordModel obj</param>
        /// <returns>String obj (new password)</returns>
        public virtual string Create(ChangePasswordModel model)
        {
            string newPassword = model.NewPassword;
            return newPassword;
        }
    }
}