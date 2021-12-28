namespace BookShop.DataProcessor
{
    using System;
    using System.IO;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;
    using BookShop.DataProcessor.ExportDto;
    using BookShop.Data.Models.Enums;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            //var books = context.Authors
            //     .ToArray()
            //     .Select(x => new
            //     {
            //         AuthorName = x.FirstName + " " + x.LastName,
            //         Books = x.AuthorsBooks
            //        .OrderByDescending(x => x.Book.Price)
            //        .Select(b => new
            //        {
            //            BookName = b.Book.Name,
            //            BookPrice = b.Book.Price.ToString("F2")
            //        })
            //        .ToArray()
            //     })
            //     .ToArray()
            //     .OrderByDescending(b => b.Books.Length)
            //     .ToArray();

            //return JsonConvert.SerializeObject(books, Formatting.Indented);

            var books = context
            .Authors
            .Select(a => new
            {
                AuthorName = a.FirstName + " " + a.LastName,
                Books = a.AuthorsBooks
                    .OrderByDescending(p => p.Book.Price)
                    .Select(b => new
                    {
                        BookName = b.Book.Name,
                        BookPrice = b.Book.Price.ToString("F2")
                    })
                    .ToArray()
            })
            .ToArray()
            .OrderByDescending(b => b.Books.Length)
            .ThenBy(b => b.AuthorName)
            .ToArray();

            return JsonConvert.SerializeObject(books, Formatting.Indented);

        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var sb = new StringBuilder();
            using StringWriter sw = new StringWriter();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportBookDto[]), new XmlRootAttribute("Books"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);


            ExportBookDto[] books = context.Books
                .Where(b => b.PublishedOn < date && b.Genre == Genre.Science)
                .OrderByDescending(b => b.Pages)
                .ThenByDescending(b => b.PublishedOn)
                .ToArray()
                .Select(x => new ExportBookDto
                {
                    Pages = x.Pages,
                    BookName = x.Name,
                    Date = x.PublishedOn.ToString("d", CultureInfo.InvariantCulture)

                })
                .Take(10)
                .ToArray();


            xmlSerializer.Serialize(sw, books, namespaces);
            return sb.ToString().TrimEnd();
        }
    }
}