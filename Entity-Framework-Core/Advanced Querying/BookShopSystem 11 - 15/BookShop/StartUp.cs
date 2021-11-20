namespace BookShop
{
    using Data;
    using global::BookShop.Models.Enums;
    using Initializer;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            var db = new BookShopContext();
            {
                //DbInitializer.ResetDatabase(db);



                //var lengthCheck = int.Parse(Console.ReadLine());
                //Console.WriteLine(CountBooks(db, lengthCheck));

                //Console.WriteLine(result);

                //Console.WriteLine(CountCopiesByAuthor(db));

                //Console.WriteLine(GetTotalProfitByCategory(db));

                //Console.WriteLine(GetMostRecentBooks(db));

                //IncreasePrices(db);

                Console.WriteLine(RemoveBooks(db));

            }
        }

        //11. Count Books

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var countBooks = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();


            return countBooks;
        }

        //12. Total Book Copies

        public static string CountCopiesByAuthor(BookShopContext context)
        {

            StringBuilder sb = new StringBuilder();

            var books = context.Authors
                .Select(b => new
                {
                    FullName = b.FirstName + " " + b.LastName,
                    TotalCount = b.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(b => b.TotalCount)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.FullName} - {book.TotalCount}");
            }

            return sb.ToString().TrimEnd();
        }


        //13. Profit by Category

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();


            var profit = context.Categories
                .Select(b => new
                {
                    Name = b.Name,
                    TotalProfit = b.CategoryBooks
                                    .Select(cb => cb.Book)
                                    .Sum(b => b.Copies * b.Price)
                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(b => b.Name)
                .ToList();

            foreach (var category in profit)
            {
                sb.AppendLine($"{category.Name} ${category.TotalProfit:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //14. Most Recent Books

        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var recentBooks = context.Categories
               .Select(b => new
               {
                   CategoryName = b.Name,
                   MostRecentBooks = b.CategoryBooks
                           .Select(cb => cb.Book)
                           .OrderByDescending(b => b.ReleaseDate)
                           .Take(3)
                           .Select(b => new
                           {
                               BookTitle = b.Title,
                               ReleaseYear = b.ReleaseDate.Value.Year
                           })
                           .ToList()
               })
               .OrderBy(c => c.CategoryName)
               .ToList();

            foreach (var item in recentBooks)
            {
                sb.AppendLine($"--{item.CategoryName}");

                foreach (var book in item.MostRecentBooks)
                {
                    sb.AppendLine($"{book.BookTitle} ({book.ReleaseYear})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //15. Increase Prices

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var item in books)
            {
                item.Price += 5;
            }

            context.SaveChanges();
        }


        // 16. Remove Books

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            int count = books.Count();

            var categories = context.BooksCategories
                .Where(bc => bc.Book.Copies < 4200)
                .ToList();

            context.BooksCategories.RemoveRange(categories);
            context.Books.RemoveRange(books);

            context.SaveChanges();

            return count;
        }

    }

}
    
    