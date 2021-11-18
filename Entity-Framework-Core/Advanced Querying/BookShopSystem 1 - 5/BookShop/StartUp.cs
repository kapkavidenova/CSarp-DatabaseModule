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

                //string ageRestrictionString = Console.ReadLine();
                //string result = GetBooksByAgeRestriction(db, ageRestrictionString);

                //string result = GetGoldenBooks(db);
                //string result = GetBooksByPrice(db);

                //int year = int.Parse(Console.ReadLine());
                //string result = GetBooksNotReleasedIn(db, year);

                var categoryArr = Console.ReadLine();
                string result = GetBooksByCategory(db, categoryArr);

                Console.WriteLine(result);
            }
        }

        private static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();
            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var allBookTitles = context.Books
               .Where(b => b.AgeRestriction == ageRestriction)
               .OrderBy(b => b.Title)
               .Select(b => b.Title)
               .ToList();

            foreach (var item in allBookTitles)
            {
                sb.AppendLine(item);
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var books = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .Select(b => new { b.BookId, b.Title })
                .OrderBy(b => b.BookId)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(books => books.Price)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //5. Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year != year)
                .Select(b => new { b.BookId, b.Title })
                .OrderBy(b => b.BookId)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();

        }

        //6. Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();
            var categoryList = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLower())
                .ToArray();

            var books = context.Books
                .Where(b => b.BookCategories.Any(bc => categoryList.Contains(bc.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            foreach (var item in books)
            {
                sb.AppendLine(item);
            }

            return sb.ToString().TrimEnd();
        }

    }
}