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
    public class LanciController : ApiController
    {
        ILanciRepository _repository { get; set; }
        public LanciController(ILanciRepository repository)
        {
            _repository = repository;
        }

        //GET api/lanci
        public IQueryable<LanacDTO> GetAll()
        {
            return _repository.GetAll().ProjectTo<LanacDTO>();
        }

        //GET api/lanci/id
        [Authorize]
        [ResponseType(typeof(Lanac))]
        public IHttpActionResult GetById(int id)
        {
            var lanac = _repository.GetById(id);
            if (lanac == null)
            {
                return NotFound();
            }
            return Ok(lanac);
        }

        [Route("api/tradicija")]
        [Authorize]
        public IQueryable<LanacDTO> GetNajstariji()
        {
            return _repository.GetNajstariji().ProjectTo<LanacDTO>();
        }

        //public IHttpActionResult GetNajstariji()
        //{
        //    var najstariji = _repository.GetNajstariji();
        //    if (najstariji.Count() == 0)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(najstariji.ProjectTo<LanacDTO>());
        //}

    }
}
