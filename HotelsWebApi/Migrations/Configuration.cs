namespace HotelsWebApi.Migrations
{
    using HotelsWebApi.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HotelsWebApi.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HotelsWebApi.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.Lanci.AddOrUpdate(
                   new Lanac() { Id = 1, Naziv = "Hilton Worldwide", GodinaOsnivanja = 1919 },
                   new Lanac() { Id = 2, Naziv = "Marriott International", GodinaOsnivanja = 1927 },
                   new Lanac() { Id = 3, Naziv = "Kempinski", GodinaOsnivanja = 1897 }
               );
            context.SaveChanges();

            context.Hoteli.AddOrUpdate(
                   new Hotel() { Id = 1, Naziv = "Sheraton Novi Sad", GodinaOtvaranja = 2018, BrojZaposlenih = 70, BrojSoba = 150, LanacId = 2 },
                   new Hotel() { Id = 2, Naziv = "Hilton Belgrade", GodinaOtvaranja = 2017, BrojZaposlenih = 100, BrojSoba = 242, LanacId = 1 },
                   new Hotel() { Id = 3, Naziv = "Palais Hansen", GodinaOtvaranja = 2013, BrojZaposlenih = 80, BrojSoba = 152, LanacId = 3 },
                   new Hotel() { Id = 4, Naziv = "Budapest Marriott", GodinaOtvaranja = 1994, BrojZaposlenih = 130, BrojSoba = 364, LanacId = 2 },
                   new Hotel() { Id = 5, Naziv = "Hilton Berlin", GodinaOtvaranja = 1991, BrojZaposlenih = 200, BrojSoba = 601, LanacId = 1 }
               );
            context.SaveChanges();

        }
    }
}
