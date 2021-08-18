using HotelsWebApi.Interfaces;
using HotelsWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelsWebApi.Repository
{
    public class LanciRepository : ILanciRepository, IDisposable
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

        public IQueryable<Lanac> GetAll()
        {
            return db.Lanci;
        }

        public Lanac GetById(int id)
        {
            return db.Lanci.Find(id);
        }

        public IQueryable<Lanac> GetNajstariji()
        {
            var result = db.Lanci.OrderBy(x => x.GodinaOsnivanja).Take(2);
            return result;
        }

    }
}