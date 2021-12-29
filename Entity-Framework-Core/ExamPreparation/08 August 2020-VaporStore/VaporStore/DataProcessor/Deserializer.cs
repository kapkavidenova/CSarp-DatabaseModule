namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Dto;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
	{
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var output = new StringBuilder();
            var games = JsonConvert.DeserializeObject<IEnumerable<GameImportDto>>(jsonString);

            foreach (var gameDto in games)
            {
                if (!IsValid(gameDto) || gameDto.Tags.Count() == 0)
                {
                    output.AppendLine("Invalid Data");
                    continue;
                }

                var genre = context.Genres.FirstOrDefault(g => g.Name == gameDto.Genre) ?? new Genre { Name = gameDto.Genre };

                var developer = context.Developers.FirstOrDefault(x => x.Name == gameDto.Developer) ?? new Developer { Name = gameDto.Developer };

                var game = new Game
                {
                    Name = gameDto.Name,
                    Genre = genre,
                    Developer = developer,
                    Price = gameDto.Price,
                    ReleaseDate = gameDto.ReleaseDate.Value,

                };

                foreach (var jsonTag in gameDto.Tags)
                {
                    var tag = context.Tags.FirstOrDefault(t => t.Name == jsonTag)
                        ?? new Tag { Name = jsonTag };
                    game.GameTags.Add(new GameTag { Tag = tag });
                }

                context.Games.Add(game);
                context.SaveChanges();
                output.AppendLine($"Added {gameDto.Name} ({gameDto.Genre}) with {gameDto.Tags.Count()} tags");
            }
            return output.ToString().TrimEnd();

        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
			var output = new StringBuilder();
			var users = JsonConvert.DeserializeObject<IEnumerable<ImportUserDto>>(jsonString);

            foreach (var userDto in users)
            {
                if (!IsValid(userDto) || userDto.Cards.All(IsValid))
                {
					output.AppendLine("Invalid Data");
					continue;
                }

				var user = new User
				{
					FullName = userDto.FullName,
					Username = userDto.Username,
					Email = userDto.Email,
					Age = userDto.Age,
					Cards = userDto.Cards
					.Select(c => new Card
					{
						Cvc = c.CVC,
						Number = c.Number,
						Type = c.Type.Value
					
					})
					.ToList(),

				};

				context.Users.Add(user);
				context.SaveChanges();
				output.AppendLine($"Imported {userDto.Username} with {userDto.Cards.Count()} cards");
            }

			return output.ToString().TrimEnd();

		}

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var output = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PurchaseImportDto[]), new XmlRootAttribute("Purchases"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var purchases = (PurchaseImportDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            foreach (var purchaseDto in purchases)
            {
                if (!IsValid(purchaseDto))
                {
                    output.AppendLine("Invalid Data");
                    continue;
                }

                bool parsedDate = DateTime.TryParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);

                if (!parsedDate)
                {
                    output.AppendLine("Invalid Data");
                    continue;
                }

                var purchase = new Purchase
                {
                    Date = date,
                    Type = purchaseDto.Type.Value,
                    ProductKey = purchaseDto.Key,
                    Card = context.Cards.FirstOrDefault(c => c.Number == purchaseDto.Card),
                    Game = context.Games.FirstOrDefault(g => g.Name == purchaseDto.GameName)
                };
                context.Purchases.Add(purchase);

                var username = context.Users.Where(x => x.Id == purchase.Card.UserId)
                    .Select(x => x.Username).FirstOrDefault();

                output.AppendLine($"Imported {purchaseDto.GameName} for {username}");

            }

            context.SaveChanges();
            return output.ToString().TrimEnd();
        }

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}