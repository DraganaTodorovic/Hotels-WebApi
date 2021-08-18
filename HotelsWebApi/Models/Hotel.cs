using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelsWebApi.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(80, ErrorMessage = "Polje je obavezno i moze sadrzati maksimum 80 karaktera!")]
        public string Naziv { get; set; }
        [Required]
        [Range(1951, 2019)]
        public int GodinaOtvaranja { get; set; }
        [Required]
        [Range(2, int.MaxValue)]
        public int BrojZaposlenih { get; set; }
        [Required]
        [Range(10, 999)]
        public int BrojSoba { get; set; }
        [Required]
        public virtual int LanacId { get; set; }
        public virtual Lanac Lanac { get; set; }
    }
}