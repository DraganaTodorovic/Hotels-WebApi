using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelsWebApi.Models
{
    public class Lanac
    {
        public int Id { get; set; }
        [Required]
        [StringLength(75, ErrorMessage = "Polje je obavezno i moze sadrzati maksimum 75 karaktera!")]
        public string Naziv { get; set; }
        [Required]
        [Range(1850, 2010)]
        public int GodinaOsnivanja { get; set; }
        public ICollection<Hotel> Hotels { get; set; }
    }
}