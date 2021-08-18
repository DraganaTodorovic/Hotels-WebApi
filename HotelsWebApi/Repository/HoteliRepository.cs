using HotelsWebApi.Interfaces;
using HotelsWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace HotelsWebApi.Repository
{
    public class HoteliRepository : IHoteliRepository, IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IQueryable<Hotel> GetAll()
        {
            return db.Hoteli.OrderBy(x => x.GodinaOtvaranja);
        }

        public Hotel GetById(int id)
        {
            return db.Hoteli.Find(id);
        }

        public void Add(Hotel hotel)
        {
            db.Hoteli.Add(hotel);
            db.SaveChanges();
        }

        public void Update(Hotel hotel)
        {
            db.Entry(hotel).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public void Delete(Hotel hotel)
        {
            db.Hoteli.Remove(hotel);
            db.SaveChanges();
        }

        public IQueryable<Hotel> GetByZaposleni(int zaposleni)
        {
            return db.Hoteli.Where(x => x.BrojZaposlenih >= zaposleni).OrderBy(x => x.BrojZaposlenih);
        }

        public IQueryable<Hotel> Kapacitet(int najmanje, int najvise)
        {
            return db.Hoteli.Where(x => x.BrojSoba > najmanje && x.BrojSoba < najvise).OrderByDescending(x => x.BrojSoba);
        }

        public IQueryable<LanacProsekDTO> GetProsekZaposlenih()
        {
            var hoteli = GetAll();
            var rezultat = hoteli.GroupBy(x => x.Lanac, x => x.BrojZaposlenih, (lanac, broj) => new LanacProsekDTO()
            {
                Id = lanac.Id,
                Naziv = lanac.Naziv,
                Prosek = broj.Average(),
            }).OrderByDescending(x => x.Prosek);

            return rezultat;
        }

        public IQueryable<LanacSobeDTO> PostSobe(int granica)
        {
            var hoteli = GetAll();
            var rezultat = hoteli.GroupBy(x => x.Lanac, x => x.BrojSoba, (lanac, broj) => new LanacSobeDTO()
            {
                Id = lanac.Id,
                Naziv = lanac.Naziv,
                Sobe = broj.Sum(),
            }).Where(x => x.Sobe > granica).OrderBy(x => x.Sobe);

            return rezultat;
        }
    }
}