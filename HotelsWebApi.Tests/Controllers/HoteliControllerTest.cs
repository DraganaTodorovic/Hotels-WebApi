using AutoMapper;
using HotelsWebApi.Controllers;
using HotelsWebApi.Interfaces;
using HotelsWebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;

namespace HotelsWebApi.Tests.Controllers
{
    [TestClass]
    public class HoteliControllerTest
    {
        [TestMethod]
        public void GetReturnsByIdOk()
        {
            // Arrange
            var mockRepository = new Mock<IHoteliRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(new Hotel { Id = 1, Naziv = "Sheraton Novi Sad", GodinaOtvaranja = 2018, BrojZaposlenih = 70, BrojSoba = 150, LanacId = 2 });

            var controller = new HoteliController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.GetById(1);
            var contentResult = actionResult as OkNegotiatedContentResult<HotelDTO>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Id);
        }

        [TestMethod]
        public void PutReturnsBadRequest()
        {
            // Arrange
            var mockRepository = new Mock<IHoteliRepository>();
            var controller = new HoteliController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Put(2, new Hotel { Id = 1, Naziv = "Sheraton Novi Sad", GodinaOtvaranja = 2018, BrojZaposlenih = 70, BrojSoba = 150, LanacId = 2 });

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostKapacitetReturnsMultipleObjects()
        {
            // Arrange
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Hotel, HotelDTO>();
            });
            List<Hotel> hoteli = new List<Hotel>();
            hoteli.Add(new Hotel()
            {
                Id = 1,
                Naziv = "Hotel 1",
                BrojSoba = 35,
                Lanac = new Lanac() { Id = 1, Naziv = "A", GodinaOsnivanja = 1966 }
            });
            hoteli.Add(new Hotel()
            {
                Id = 2,
                Naziv = "Hotel 2",
                BrojSoba = 30,
                Lanac = new Lanac() { Id = 1, Naziv = "A", GodinaOsnivanja = 1966 }
            });

            // Act
            var mockRepository = new Mock<IHoteliRepository>();
            mockRepository.Setup(x => x.Kapacitet(25, 50)).Returns(hoteli.AsQueryable());
            var controller = new HoteliController(mockRepository.Object);

            // Act
            IQueryable<HotelDTO> result = controller.PostKapacitet(25, 50);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(hoteli.Count, result.ToList().Count);
            Assert.AreEqual(hoteli.ElementAt(0).Id, result.ToList().ElementAt(0).Id);
            Assert.AreEqual(hoteli.ElementAt(0).Naziv, result.ToList().ElementAt(0).Naziv);
            Assert.AreEqual(hoteli.ElementAt(0).BrojSoba, result.ToList().ElementAt(0).BrojSoba);
            Assert.AreEqual(hoteli.ElementAt(0).Lanac.Naziv, result.ToList().ElementAt(0).LanacNaziv);

            Assert.AreEqual(hoteli.ElementAt(1).Id, result.ToList().ElementAt(1).Id);
            Assert.AreEqual(hoteli.ElementAt(1).Naziv, result.ToList().ElementAt(1).Naziv);
            Assert.AreEqual(hoteli.ElementAt(1).BrojSoba, result.ToList().ElementAt(1).BrojSoba);
            Assert.AreEqual(hoteli.ElementAt(1).Lanac.Naziv, result.ToList().ElementAt(1).LanacNaziv);
        }

        [TestMethod]
        public void GetReturnsMultipleObjects()
        {
            // Arrange
            List<Hotel> hoteli = new List<Hotel>();
            hoteli.Add(new Hotel()
            {
                Id = 1,
                Naziv = "Hotel 1",
                Lanac = new Lanac() { Id = 1, Naziv = "A", GodinaOsnivanja = 1966 }
            });
            hoteli.Add(new Hotel()
            {
                Id = 2,
                Naziv = "Hotel 2",
                Lanac = new Lanac() { Id = 2, Naziv = "B", GodinaOsnivanja = 1967 }
            });

            var mockRepository = new Mock<IHoteliRepository>();
            mockRepository.Setup(x => x.GetAll()).Returns(hoteli.AsQueryable());
            var controller = new HoteliController(mockRepository.Object);

            // Act
            IQueryable<HotelDTO> result = controller.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(hoteli.Count, result.ToList().Count);
            Assert.AreEqual(hoteli.ElementAt(0).Id, result.ToList().ElementAt(0).Id);
            Assert.AreEqual(hoteli.ElementAt(0).Naziv, result.ToList().ElementAt(0).Naziv);
            Assert.AreEqual(hoteli.ElementAt(0).Lanac.Naziv, result.ToList().ElementAt(0).LanacNaziv);

            Assert.AreEqual(hoteli.ElementAt(1).Id, result.ToList().ElementAt(1).Id);
            Assert.AreEqual(hoteli.ElementAt(1).Naziv, result.ToList().ElementAt(1).Naziv);
            Assert.AreEqual(hoteli.ElementAt(1).Lanac.Naziv, result.ToList().ElementAt(1).LanacNaziv);
        }

    }
}
