using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using PikabaV3.NET.Models;

namespace PikabaV3.NET
{
    public class NetClientV1
    {
        public List<Product> GetAllPoducts()
        {
            try
            {
                // Create a request for the URL
                WebRequest request = WebRequest.Create("http://localhost:49909/api/products");
                // Get the response
                WebResponse response = request.GetResponse();
                // Get the status
                string status = ((HttpWebResponse)response).StatusDescription;
                // Get the stream containing content returned by the server.
                Stream stream = response.GetResponseStream();
                if (stream != null)
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(stream);
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    // Deserialize string respone to List<Product>
                    var products = JsonConvert.DeserializeObject<List<Product>>(responseFromServer);
                    reader.Close();
                    return products;
                }
                response.Close();
            }
            catch (WebException ex)
            {
                string result = ex.ToString();
                WebExceptionStatus status = ex.Status;
                if (status == WebExceptionStatus.ProtocolError)
                {
                    Console.WriteLine("The server return protocol error");
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    Console.WriteLine((int)response.StatusCode + " - " + response.StatusCode);
                }
            }
            return null;
        }

        public Product GetPoduct(string productId)
        {
            try
            {
                // Create a request for the URL
                WebRequest request = WebRequest.Create("http://localhost:49909/api/product/" + productId);
                // Get the response
                WebResponse response = request.GetResponse();
                // Get the status
                string status = ((HttpWebResponse)response).StatusDescription;
                // Get the stream containing content returned by the server.
                Stream stream = response.GetResponseStream();
                if (stream != null)
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(stream);
                    // Read the content.
                    var responseFromServer = reader.ReadToEnd();
                    // Deserialize string respone to List<Product>
                    var product = JsonConvert.DeserializeObject<Product>(responseFromServer);
                    reader.Close();
                    return product;
                }
                response.Close();
            }
            catch (WebException ex)
            {
                string result = ex.ToString();
                WebExceptionStatus status = ex.Status;
                if (status == WebExceptionStatus.ProtocolError)
                {
                    Console.WriteLine("The server return protocol error");
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    Console.WriteLine((int)response.StatusCode + " - " + response.StatusCode);
                }
            }
            return null;
        }

        public bool PostProduct(ProductModel product)
        {
            try
            {
                // Create a request using a URL that can receive a post
                WebRequest request = WebRequest.Create("http://localhost:49909/api/product");
                // Set the Method property of the request to POST
                request.Method = "POST";
                // Create POST data and convert it to a byte array
                string json = JsonConvert.SerializeObject(product);
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                request.ContentType = "application/json";
                request.ContentLength = bytes.Length;
                // Get the request stream
                Stream stream = request.GetRequestStream();
                // Write the data to the request stream
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
                // Get the response
                WebResponse response = request.GetResponse();
                // Get the status
                string status = ((HttpWebResponse)response).StatusDescription;
                // Get the stream containing content returned by the server.
                stream = response.GetResponseStream();
                if (stream != null)
                {
                    // Open the stream using a StreamReader for easy access
                    StreamReader reader = new StreamReader(stream);
                    // Read the content
                    string responseFromServ = reader.ReadToEnd();
                    Console.WriteLine(responseFromServ);
                    reader.Close();
                    stream.Close();
                }
                response.Close();
                return true;
            }
            catch (WebException ex)
            {
                string result = ex.ToString();
                WebExceptionStatus status = ex.Status;
                if (status == WebExceptionStatus.ProtocolError)
                {
                    Console.WriteLine("The server return protocol error");
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    Console.WriteLine((int)response.StatusCode + " - " + response.StatusCode);
                }
            }
            return false;
        }

        public bool PutProduct(string productId, ProductModel product)
        {
            try
            {
                // Create a request using a URL that can receive a post
                WebRequest request = WebRequest.Create("http://localhost:49909/api/product/" + productId);
                // Set the Method property of the request to POST
                request.Method = "PUT";
                // Create POST data and convert it to a byte array
                string json = JsonConvert.SerializeObject(product);
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                request.ContentType = "application/json";
                request.ContentLength = bytes.Length;
                // Get the request stream
                Stream stream = request.GetRequestStream();
                // Write the data to the request stream
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
                // Get the response
                WebResponse response = request.GetResponse();
                // Get the status
                string status = ((HttpWebResponse)response).StatusDescription;
                // Get the stream containing content returned by the server.
                stream = response.GetResponseStream();
                if (stream != null)
                {
                    // Open the stream using a StreamReader for easy access
                    StreamReader reader = new StreamReader(stream);
                    // Read the content
                    string responseFromServ = reader.ReadToEnd();
                    Console.WriteLine(responseFromServ);
                    reader.Close();
                    stream.Close();
                }
                response.Close();
                return true;
            }
            catch (WebException ex)
            {
                string result = ex.ToString();
                WebExceptionStatus status = ex.Status;
                if (status == WebExceptionStatus.ProtocolError)
                {
                    Console.WriteLine("The server return protocol error");
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    Console.WriteLine((int)response.StatusCode + " - " + response.StatusCode);
                }
            }
            return false;
        }

        public bool DeleteProduct(string productId, string cookieId)
        {
            try
            {
                // Create a request using a URL that can receive a post
                WebRequest request = WebRequest.Create("http://localhost:49909/api/product/" + productId + "/" + cookieId);
                // Set the Method property of the request to POST
                request.Method = "DELETE";
                WebResponse response = request.GetResponse();
                // Get the status
                string status = ((HttpWebResponse)response).StatusDescription;
                response.Close();
                return true;
            }
            catch (WebException ex)
            {
                string result = ex.ToString();
                WebExceptionStatus status = ex.Status;
                if (status == WebExceptionStatus.ProtocolError)
                {
                    Console.WriteLine("The server return protocol error");
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    Console.WriteLine((int)response.StatusCode + " - " + response.StatusCode);
                }
            }
            return false;
        }
    }
}
