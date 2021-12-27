namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {

            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayXmlDto[]), new XmlRootAttribute("Plays"));

            using StringReader sr = new StringReader(xmlString);


           var dtos = (PlayXmlDto[])
                xmlSerializer.Deserialize(sr);

            HashSet<Play> plays = new HashSet<Play>();

            foreach (var dto in dtos)
            {

                bool IsGenreValid = Enum.TryParse(typeof(Genre), dto.Genre, out var genre);

                bool IsDurationValid = TimeSpan.TryParseExact(
                        dto.Duration, "c", CultureInfo.InvariantCulture, out var duration);

                if (TimeSpan.Parse(dto.Duration).Hours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (!IsValid(dto) || !IsDurationValid || !IsGenreValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
               
                
                Play p = new Play()
                {
                    Title = dto.Title,
                    Duration = duration,
                    Rating = dto.Rating,
                    Genre =(Genre)genre,
                    Description = dto.Description,
                    Screenwriter = dto.Screenwriter
                };

                plays.Add(p);
                sb.AppendLine(String.Format(SuccessfulImportPlay, p.Title, p.Genre, p.Rating));
            }
            context.Plays.AddRange(plays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

            public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CastXmlDto[]), new XmlRootAttribute("Casts"));

            using StringReader sr = new StringReader(xmlString);


            var CastDtos = (CastXmlDto[])
                xmlSerializer.Deserialize(sr);

            HashSet<Cast> casts = new HashSet<Cast>();

            foreach (var castDto in CastDtos)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var cast = new Cast()
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId
                };

                casts.Add(cast);

                string mainOrLesser = cast.IsMainCharacter ? "main" : "lesser";

                sb.AppendLine(string.Format(SuccessfulImportActor,
                   cast.FullName, mainOrLesser));

            }
            context.Casts.AddRange(casts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var dtos = JsonConvert.DeserializeObject<TheatreJsonDto[]>(jsonString);

            HashSet<Theatre> theatres = new HashSet<Theatre>();
            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var theatre = new Theatre
                {
                    Name = dto.Name,
                    NumberOfHalls = dto.NumberOfHalls,
                    Director = dto.Director

                };

                foreach (var ticketDto in dto.Tickets)
                {
                    if (!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    theatre.Tickets.Add(new Ticket()
                    {
                        Price = ticketDto.Price,
                        RowNumber = ticketDto.RowNumber,
                        PlayId = ticketDto.PlayId,
                        TheatreId = theatre.Id,
                        Theatre = theatre
                    });

                }
                    theatres.Add(theatre);
                    sb.AppendLine(String.Format(SuccessfulImportTheatre,
                    theatre.Name, theatre.Tickets.Count.ToString()));
            }

                context.Theatres.AddRange(theatres);
                context.SaveChanges();
                return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
