using SimchaFund.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimchaFund.Web.Models
{
    public class HistoryViewModel
    {
        public decimal Balance { get; set; }
        public string Name { get; set; }
        public List<History> Histories { get; set; }
        
    }
}
