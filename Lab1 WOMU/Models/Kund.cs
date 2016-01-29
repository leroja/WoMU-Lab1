using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab1_WOMU.Models
{
    public class Kund
    {
        public int KundID { get; set; }
        public string FörNamn { get; set; }
        public string Efternamn { get; set; }
        public string PostAdress { get; set; }
        public int PostNr { get; set; }
        public string Ort { get; set; }
        public string Epost {get; set; }
        public int Telefonnummer { get; set; }

    }
}