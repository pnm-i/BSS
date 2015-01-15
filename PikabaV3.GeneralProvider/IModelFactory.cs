using PikabaV3.Models;
using PikabaV3.Models.Entities;

namespace PikabaV3.GeneralProvider
{
    public interface IModelFactory
    {
        /// <summary>
        /// Create product by model.
        /// </summary>
        /// <param name="seller">Seller obj</param>
        /// <param name="model">ProductModel obj</param>
        /// <returns>Product obj</returns>
        Product Create(Seller seller, ProductModel model);

        /// <summary>
        /// Create product to update by model.
        /// </summary>
        /// <param name="model">ProductModel obj</param>
        /// <returns>Product obj</returns>
        Product Create(ProductModel model);

        /// <summary>
        /// Create category by model.
        /// </summary>
        /// <param name="model">CategoryModel obj</param>
        /// <returns>Category obj</returns>
        Category Create(CategoryModel model);

        /// <summary>
        /// Create comment by model, wich owner seller.
        /// </summary>
        /// <param name="seller">Seller obj</param>
        /// <param name="model">CommentModel obj</param>
        /// <returns>Comment obj</returns>
        Comment Create(Seller seller, CommentModel model);

        /// <summary>
        /// Create comment by model, wich owner buyer.
        /// </summary>
        /// <param name="user">User obj</param>
        /// <param name="model">CommentModel obj</param>
        /// <returns>Comment obj</returns>
        Comment Create(User user, CommentModel model);

        /// <summary>
        /// Create comment to update, by model.
        /// </summary>
        /// <param name="model">CommentModel data</param>
        /// <returns>Comment obj</returns>
        Comment Create(CommentModel model);

        /// <summary>
        /// Create seller by register model.
        /// </summary>
        /// <param name="model">RegisterSellerModel obj</param>
        /// <returns>Seller obj</returns>
        Seller Create(RegisterSellerModel model);

        /// <summary>
        /// Create seller entity to update by model.
        /// </summary>
        /// <param name="model">UpdateSellerModel obj</param>
        /// <returns>Seller obj</returns>
        Seller Create(UpdateSellerModel model);

        /// <summary>
        /// Create user by model.
        /// </summary>
        /// <param name="model">RegisterUserModel obj</param>
        /// <returns>User obj</returns>
        User Create(RegisterUserModel model);

        /// <summary>
        /// Create user to update by model.
        /// </summary>
        /// <param name="model">UpdateUserModel obj</param>
        /// <returns>User obj</returns>
        User Create(UpdateUserModel model);

        /// <summary>
        /// Create session by user data and Login model.
        /// </summary>
        /// <param name="userData">User obj</param>
        /// <param name="loginModel">LoginModel obj</param>
        /// <param name="cookieUuid">Cookie Uuid</param>
        /// <returns>UserSession obj</returns>
        UserSession Create(User userData, LoginModel loginModel, string cookieUuid);

        /// <summary>
        /// Create password by model.
        /// </summary>
        /// <param name="model">ChangePasswordModel obj</param>
        /// <returns>String obj (new password)</returns>
        string Create(ChangePasswordModel model);
    }
}
