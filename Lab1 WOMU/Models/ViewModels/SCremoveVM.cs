using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab1_WOMU.Models.ViewModels
{
    public class SCremoveVM
    {
        public string Message { get; set; }
        public double CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int DeleteId { get; set; }
        public int totPris { get; set; }
    }
}