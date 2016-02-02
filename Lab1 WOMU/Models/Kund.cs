using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lab1_WOMU.Models
{
    public class Kund
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KundID { get; set; }
        [Required]
        public string FörNamn { get; set; }
        [Required]
        public string Efternamn { get; set; }
        [Required]
        public string PostAdress { get; set; }
        public int PostNr { get; set; }
        [Required]
        public string Ort { get; set; }
        [Required]
        public string Epost {get; set; }
        public int Telefonnummer { get; set; }

    }
}