using System;
using System.Collections.Generic;
using PikabaV3.NET.Models;

namespace PikabaV3.NET
{
    public class NetClientManager
    {
        /// <summary>
        /// Version 1 of the work Client Console App
        /// </summary>
        public void RunNetClientV1()
        {
            var netClientV1 = new NetClientV1();

            // GET one product
            Product productV1 = netClientV1.GetPoduct("54230fbee2556e24748b7006");

            // GET all product
            List<Product> productsV1 = netClientV1.GetAllPoducts();

            // POST product
            bool postProductV1 = netClientV1.PostProduct(CreateNewProduct());

            // PUT product
            bool putProductV1 = netClientV1.PutProduct(productV1.Id, UpdatedProduct());

            // DELETE product
            bool deleteProductV1 = netClientV1.DeleteProduct(productV1.Id, "55a56f70-396b-415d-8d02-92332dda5437");
        }


        /// <summary>
        /// Version 2 of the work Client Console App
        /// </summary>
        public void RunNetClientV2()
        {
            var netClientV2 = new NetClientV2("http://localhost:49909/");

            // GET all products
            List<Product> allProductV2 = netClientV2.GetAllPoducts();

            // GET one product
            Product productV2 = netClientV2.GetPoduct("54230fbee2556e24748b7006");

            // POST new Product
            bool postProductV2 = netClientV2.PostProduct(CreateNewProduct());

            // PUT product
            bool putProductV2 = netClientV2.PutProduct(productV2.Id, UpdatedProduct());

            // DELETE product
            bool deleteProduct = netClientV2.DeleteProduct(productV2.Id, "55a56f70-396b-415d-8d02-92332dda5437");
        }


        private ProductModel CreateNewProduct()
        {
            var product = new ProductModel
            {
                Title = "Telega",
                Price = 1300,
                Description = "norm trandulet",
                Category_Ids = new List<string>
                {
                    "53fd73b37714792308071266",
                    "53fe07ff30ebabcbfd522a12"
                },
                CookieUuid = "55a56f70-396b-415d-8d02-92332dda5437"
            };
            return product;
        }

        private ProductModel UpdatedProduct()
        {
            var product = new ProductModel
            {
                Title = "PUT",
                Price = 1300,
                Description = "PUT",
                Category_Ids = new List<string>
                {
                    "53fd73b37714792308071266",
                    "53fe07ff30ebabcbfd522a12"
                },
                CookieUuid = "55a56f70-396b-415d-8d02-92332dda5437"
            };
            return product;
        }

        private void ShowProduct(Product p)
        {
            Console.WriteLine("Title: {0}\nDescr: {1}\nPrice: {2}$\nCreated: {3}\nOwner: {4}\n",
                        p.Title, p.Description, p.Price, p.DateAdded, p.Owner.DisplayName);
        }
    }
}
