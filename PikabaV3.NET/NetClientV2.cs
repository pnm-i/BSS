using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using PikabaV3.NET.Models;

namespace PikabaV3.NET
{
    public class NetClientV2
    {
        readonly HttpClient _client;

        public NetClientV2(string url)
        {
            _client = new HttpClient { BaseAddress = new Uri(url) };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public List<Product> GetAllPoducts()
        {
            var response = _client.GetAsync("api/products").Result;
            var products = JsonConvert.DeserializeObject<List<Product>>(response.Content.ReadAsStringAsync().Result);
            return products;
        }


        public Product GetPoduct(string productId)
        {
            var response = _client.GetAsync("api/product/" + productId).Result;
            var product = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);
            return product;
        }


        public bool PostProduct(ProductModel product)
        {
            string json = JsonConvert.SerializeObject(product);
            var content = new StringContent(json, Encoding.UTF8);
            var response = _client.PostAsync("api/product", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }


        public bool PutProduct(string productId, ProductModel product)
        {
            string json = JsonConvert.SerializeObject(product);
            var content = new StringContent(json, Encoding.UTF8);
            var response = _client.PutAsync("api/product/" + productId, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }


        public bool DeleteProduct(string productId, string cookieId)
        {
            var response = _client.DeleteAsync("api/product/" + productId + "/" + cookieId).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
