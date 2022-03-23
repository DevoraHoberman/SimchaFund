using SimchaFund.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimchaFund.Web.Models
{
    public class ContributorViewModel
    {
        public List<Contributor> Contributors { get; set; }
        public string Message { get; set; }
        public decimal Total { get; set; }
    }
}
