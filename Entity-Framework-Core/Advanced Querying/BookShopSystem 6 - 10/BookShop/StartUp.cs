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

                var categoryArr = Console.ReadLine();
                //string result = GetBooksByCategory(db, categoryArr);

                //string date = Console.ReadLine();
                //string result = GetBooksReleasedBefore(db, date);

                //string input = Console.ReadLine();
                //string result = GetAuthorNamesEndingIn(db, input);

                //string input = Console.ReadLine();
                //string result = GetBookTitlesContaining(db, input);

                string input = Console.ReadLine();
                string result = GetBooksByAuthor(db, input);

                Console.WriteLine(result);

            }
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

        //7. Released Before Date

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();

            var parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books
                .Where(b => b.ReleaseDate < parsedDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price,
                    b.ReleaseDate
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();

            foreach (var item in books)
            {
                sb.AppendLine($"{item.Title} - {item.EditionType} - ${item.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //8. Author Search

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName,

                })
                .OrderBy(a => a.FullName)
                .ToList();

            foreach (var item in authors)
            {
                sb.AppendLine(item.FullName);
            }

            return sb.ToString().TrimEnd();
        }

        // 9. Book Search

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            foreach (var item in books)
            {
                sb.AppendLine(item);
            }

            return sb.ToString().TrimEnd();
        }

        //10. Book Search by Author

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(a => a.Author.LastName.ToLower().StartsWith(input.ToLower()))
                    .Select(a => new
                    {
                        a.BookId,
                        a.Title,
                        FullName = a.Author.FirstName + " " + a.Author.LastName
                    })
                    .OrderBy(x => x.BookId)
                    .ToList();


            foreach (var title in books)
            {
                sb.AppendLine($"{title.Title} ({title.FullName})");
            }

            return sb.ToString().TrimEnd();
        }

    }

}
    
    