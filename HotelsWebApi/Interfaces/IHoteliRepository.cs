using HotelsWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelsWebApi.Interfaces
{
    public interface IHoteliRepository
    {
        IQueryable<Hotel> GetAll();
        Hotel GetById(int id);
        void Add(Hotel hotel);
        void Update(Hotel hotel);
        void Delete(Hotel hotel);
        IQueryable<Hotel> GetByZaposleni(int zaposleni);
        IQueryable<Hotel> Kapacitet(int najmanje, int najvise);
        IQueryable<LanacProsekDTO> GetProsekZaposlenih();
        IQueryable<LanacSobeDTO> PostSobe(int granica);
    }
}
