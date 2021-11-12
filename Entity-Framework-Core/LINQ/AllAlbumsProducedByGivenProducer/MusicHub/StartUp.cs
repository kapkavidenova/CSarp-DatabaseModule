namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            //Console.WriteLine("Db created!!!!!!!!!!!!");
           Console.WriteLine(ExportAlbumsInfo(context, 9));
            

        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .ToList()
                .Where(p => p.ProducerId == producerId)
                .Select(x => new
                {
                    AlbumName = x.Name,
                    ReleaseDate = x.ReleaseDate,
                    ProducerName = x.Producer.Name,
                    Songs = x.Songs
                            .Select(s => new
                            {
                                SongName = s.Name,
                                Price = s.Price,
                                WriterName = s.Writer.Name

                            })
                            .OrderByDescending(x => x.SongName)
                            .ThenBy(x => x.WriterName)
                            .ToList(),
                    TotalAlbumPrice = x.Price
                }) 
                .OrderByDescending(x=>x.TotalAlbumPrice)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var item in albums)
            {
                sb.AppendLine($"-AlbumName: {item.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {item.ReleaseDate.ToString("MM/dd/yyyy")}");
                sb.AppendLine($"-ProducerName: {item.ProducerName}");
                sb.AppendLine($"-Songs:");

                int counter = 1;

                foreach (var song in item.Songs)
                {
                    sb.AppendLine($"---#{counter}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.Price:f2}");
                    sb.AppendLine($"---Writer: {song.WriterName}");

                    counter++;
                }
                sb.AppendLine($"-AlbumPrice: {item.TotalAlbumPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
