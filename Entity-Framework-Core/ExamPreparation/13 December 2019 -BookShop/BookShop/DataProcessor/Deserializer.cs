namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            var sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BookDto[]), new XmlRootAttribute("Books"));

            using StringReader sr = new StringReader(xmlString);
            BookDto[] dtos = (BookDto[])xmlSerializer.Deserialize(sr);

            HashSet<Book> books = new HashSet<Book>();

            foreach (var bookDto in dtos)
            {

                if (!IsValid(bookDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var date = DateTime.ParseExact(bookDto.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                Book b = new Book()
                {
                    Name = bookDto.Name,
                    Genre = (Genre)bookDto.Genre,
                    Price = bookDto.Price,
                    Pages = bookDto.Pages,
                    PublishedOn = date
                };
                books.Add(b);
                sb.AppendLine(String.Format(SuccessfullyImportedBook, b.Name, b.Price));
            }

            context.Books.AddRange(books);
            context.SaveChanges();

            return sb.ToString().TrimEnd();


        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var dtos = JsonConvert.DeserializeObject<AuthorDto[]>(jsonString);

            HashSet<Author> authors = new HashSet<Author>();
            var booksId = context.Books.Select(b => b.Id).ToArray();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

            bool isExistEmail = authors.FirstOrDefault(x => x.Email == dto.Email) != null;

                if (isExistEmail)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var a = new Author
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Phone = dto.Phone,
                    Email = dto.Email

                };
               // var uniqueBookIds = dto.AuthorsBooks.Distinct();
                foreach (var bookDto in dto.AuthorsBooks)
                {
                    var book = context.Books.Find(bookDto.BookId);
                    if (book == null)
                    {
                        continue;
                    }

                    a.AuthorsBooks.Add(new AuthorBook
                    {
                        Author = a,
                        Book = book
                    });
                   
                }

                if (a.AuthorsBooks.Count ==0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                authors.Add(a);
                sb.AppendLine(string.Format(SuccessfullyImportedAuthor, (a.FirstName + " " + a.LastName), a.AuthorsBooks.Count));
            }
            context.Authors.AddRange(authors);
            context.SaveChanges();

            string result = sb.ToString().TrimEnd();

            return result;
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

    }
}