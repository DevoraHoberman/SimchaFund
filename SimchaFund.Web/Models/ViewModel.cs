using SimchaFund.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimchaFund.Web.Models
{
    public class ViewModel
    {
        public List<Simcha> Simchas { get; set; }
        public int ContributorCount{ get; set; }
    }
}
