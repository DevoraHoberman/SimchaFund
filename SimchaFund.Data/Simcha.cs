using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaFund.Data
{
    public class Simcha
    {
        public int SimchaId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int ContributorCount { get; set; }
        public decimal Total { get; set; }
    }
}
