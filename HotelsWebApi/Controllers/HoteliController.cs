using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelsWebApi.Interfaces;
using HotelsWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace HotelsWebApi.Controllers
{
    public class HoteliController : ApiController
    {
        IHoteliRepository _repository { get; set; }
        public HoteliController(IHoteliRepository repository)
        {
            _repository = repository;
        }

        //GET api/hoteli
        public IQueryable<HotelDTO> GetAll()
        {
            return _repository.GetAll().ProjectTo<HotelDTO>();
        }

        //GET api/hoteli/id
        [Authorize]
        [ResponseType(typeof(Hotel))]
        public IHttpActionResult GetById(int id)
        {
            var hotel = _repository.GetById(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<HotelDTO>(hotel));
        }

        //POST api/hoteli
        [Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repository.Add(hotel);
            return CreatedAtRoute("DefaultApi", new { id = hotel.Id }, hotel);
        }

        //PUT api/hoteli/id
        [Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(int id, Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != hotel.Id)
            {
                return BadRequest();
            }
            try
            {
                _repository.Update(hotel);
            }
            catch
            {
                return BadRequest();
            }
            return Ok(hotel);
        }

        //DELETE api/hoteli/id
        [Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(int id)
        {
            var hotel = _repository.GetById(id);
            if (hotel == null)
            {
                return NotFound();
            }
            _repository.Delete(hotel);
            return Ok();
        }

        //GET api/hoteli?zaposleni={minimum}
        [Authorize]
        public IQueryable<HotelDTO> GetByZaposleni(int zaposleni)
        {
            return _repository.GetByZaposleni(zaposleni).ProjectTo<HotelDTO>();
        }

        //POST api/kapacitet
        [Route("api/kapacitet")]
        [Authorize]
        public IQueryable<HotelDTO> PostKapacitet(int najmanje, int najvise)
        {
            return _repository.Kapacitet(najmanje, najvise).ProjectTo<HotelDTO>();
        }

        //GET api/zaposleni
        [Route("api/zaposleni")]
        [Authorize]
        public IHttpActionResult GetProsekZaposlenih()
        {
            var hoteli = _repository.GetProsekZaposlenih();
            if (hoteli.Count() == 0)
            {
                return NotFound();
            }
            return Ok(hoteli);
        }

        //POST api/sobe
        [Route("api/sobe")]
        [Authorize]
        public IHttpActionResult PostSobe(int granica)
        {
            var hoteli = _repository.PostSobe(granica);
            if (hoteli.Count() == 0)
            {
                return NotFound();
            }
            return Ok(hoteli);
        }
    }
}
