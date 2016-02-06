using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_WOMU.Models.ViewModels
{
    class CountModelView
    {
        public string Message { get; set; }
        public double CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int ItemID { get; set; }
        public int TotPris { get; set; }
    }
}
