using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTO.Users;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {

        private static string ResultDirectoryPath = ".. / .. / .. / Datasets/Results";
        public static void Main(string[] args)
        {
           ProductShopContext productShopContext = new ProductShopContext();

            //InitializeMapper();

            //productShopContext.Database.EnsureDeleted();
            //productShopContext.Database.EnsureCreated();

            //string inputJson = File.ReadAllText("../../../Datasets/users.json");
            //var result = ImportUsers(productShopContext, inputJson);

            //string inputJson = File.ReadAllText("../../../Datasets/products.json");
            //var result = ImportProducts(productShopContext, inputJson);


            //string inputJson = File.ReadAllText("../../../Datasets/categories.json");
            //var result = ImportCategories(productShopContext, inputJson);

            //string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //var result = ImportCategoryProducts(productShopContext, inputJson);


            //string json = GetProductsInRange(productShopContext);
            //string json = GetSoldProducts(productShopContext);
            //string json = GetCategoriesByProductsCount(productShopContext);
            string json = GetUsersWithProducts(productShopContext);


           // EnsureDirectoryExists(ResultDirectoryPath);

            if (!Directory.Exists(ResultDirectoryPath))
            {
               Directory.CreateDirectory(ResultDirectoryPath);
            }

            Console.WriteLine(json);

            //File.WriteAllText(ResultDirectoryPath + "/products-in-range.json", json);

            //File.WriteAllText(ResultDirectoryPath + "/users-sold-products.json", json);

            File.WriteAllText(ResultDirectoryPath + "/users-and-products.json", json);


        }

        //Query 2. Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        //Query 3. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            Product[] products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }


        //Query 4. Import Categories

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            Category[] categories = JsonConvert.DeserializeObject<Category[]>(inputJson)
            .Where(c => c.Name != null)
            .ToArray();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        //Query 5. Import Categories and Products

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            CategoryProduct[] categoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.CategoryProducts.AddRange(categoryProducts);

            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";
        }

        //1. Query and Export Data
        //Query 6. Export Successfully Sold Products

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products               
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                   name = p.Name,
                   price = p.Price.ToString("f2"),
                   seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .ToList();

            string json = JsonConvert.SerializeObject(products, Formatting.Indented);

            return json;
        }

        //Query 6. Export Successfully Sold Products

         public static string GetSoldProducts(ProductShopContext context)
         {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                //.Select(u => new
                //{
                //    firstName = u.FirstName,
                //    lastName = u.LastName,
                //    soldProducts = u.ProductsSold                
                //    .Select(p => new
                //    {
                //        name = p.Name,
                //        price = p.Price,
                //        buyerFirstName = p.Buyer.FirstName,
                //        buyerLastName = p.Buyer.LastName
                //    })
                //    .ToArray()

                //})
                .ProjectTo<UserWithSoldProductsDTO>()
                .ToArray();

            string json = JsonConvert.SerializeObject(users, Formatting.Indented);

            return json;
         }

        //Export Categories by Products Count

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories                
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count,
                    averagePrice = c.CategoryProducts
                    .Average(cp => cp.Product.Price)
                    .ToString("f2"),
                    totalRevenue = c.CategoryProducts
                    .Sum(cp => cp.Product.Price)
                    .ToString("f2")
                })
                .OrderByDescending(c => c.productsCount)
                .ToArray();

            string json = JsonConvert.SerializeObject(categories,Formatting.Indented);

            return json;
        }

        //Query 7. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .Select(u => new
                {
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Count(),
                        products = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                        .ToArray()
                    }
                })
                .OrderByDescending(u => u.soldProducts.count)
                .ToArray();

            var resultObj = new
            {
                usersCount = users.Length,
                users = users
            };

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };
            string json = JsonConvert.SerializeObject(resultObj, settings);

            return json;
        }
    }
}