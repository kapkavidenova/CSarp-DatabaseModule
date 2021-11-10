using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data;
using System;

namespace P03_FootballBetting
{
    public class StartUp
    {
        static void Main(string[] args)
        {
           FootballBettingContext db = new FootballBettingContext();///////////////me

            db.Database.Migrate();
            

           // db.Database.EnsureDeleted();

           Console.WriteLine("Db created successfully!");

           // db.Database.EnsureCreated();
        }
    }
}
