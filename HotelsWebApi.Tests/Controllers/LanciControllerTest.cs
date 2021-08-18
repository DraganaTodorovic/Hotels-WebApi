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
    public class LanciControllerTest
    {
        [TestMethod]
        public void GetReturnsOldest()
        {
            // Arrange
            List<Lanac> lanci = new List<Lanac>();
            lanci.Add(new Lanac() { Id = 1, Naziv = "Lanac 1", GodinaOsnivanja = 1919, Hotels = null });
            lanci.Add(new Lanac() { Id = 2, Naziv = "Lanac 2", GodinaOsnivanja = 1897, Hotels = null });
            lanci.Add(new Lanac() { Id = 3, Naziv = "Lanac 3", GodinaOsnivanja = 1927, Hotels = null });
            lanci.Add(new Lanac() { Id = 4, Naziv = "Lanac 4", GodinaOsnivanja = 1950, Hotels = null });

            List<Lanac> subList = new List<Lanac>();
            List<Lanac> pretraga()
            {
                lanci.OrderBy(p => p.GodinaOsnivanja);
                subList.Add(lanci.ElementAt(0));
                subList.Add(lanci.ElementAt(1));

                return subList;
            }

            var mockRepository = new Mock<ILanciRepository>();
            mockRepository.Setup(x => x.GetNajstariji()).Returns(pretraga().AsQueryable());
            var controller = new LanciController(mockRepository.Object);

            // Act
            IQueryable<LanacDTO> result = controller.GetNajstariji();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(subList.Count, result.ToList().Count);
            Assert.AreEqual(subList.ElementAt(0).Id, result.ToList().ElementAt(0).Id);
            Assert.AreEqual(subList.ElementAt(0).Naziv, result.ToList().ElementAt(0).Naziv);
            Assert.AreEqual(subList.ElementAt(0).GodinaOsnivanja, result.ToList().ElementAt(0).GodinaOsnivanja);

            Assert.AreEqual(subList.ElementAt(1).Id, result.ToList().ElementAt(1).Id);
            Assert.AreEqual(subList.ElementAt(1).Naziv, result.ToList().ElementAt(1).Naziv);
            Assert.AreEqual(subList.ElementAt(1).GodinaOsnivanja, result.ToList().ElementAt(1).GodinaOsnivanja);

        }
    }
}
