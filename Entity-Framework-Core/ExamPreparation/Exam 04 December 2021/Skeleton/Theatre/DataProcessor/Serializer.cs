namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.Linq;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;
    using static Theatre.DataProcessor.ExportDto.ActorDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context
                .Theatres
               .Where(t => t.Tickets.Count >= 20 && t.NumberOfHalls >= numbersOfHalls )
               .ToList()
               .Select(t => new TheatreExportDto
               {
                   Name = t.Name,
                   Halls = t.NumberOfHalls,
                   TotalIncome = t.Tickets
                               .Where(x => x.RowNumber >= 1 && x.RowNumber <= 5)
                               .ToList()
                               .Sum(x => x.Price),

                   Tickets = t.Tickets
                       .Where(a => a.RowNumber >= 1 && a.RowNumber <= 5)
                       .ToList()
                   .Select(a => new TicketExportDto()
                   {

                       Price = a.Price,
                       RowNumber = a.RowNumber

                   })
                   .OrderByDescending(a => a.Price)
                   .ToList()
               })
               .OrderByDescending(t => t.Halls)
               .ThenBy(t => t.Name)
               .ToList();

            var json = JsonConvert.SerializeObject(theatres, Formatting.Indented);
            return json;

        }
    }
}
