using SimchaFund.Data;
using SimchaFund.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimchaFund.Web.Models
{
    public class ContributionsViewModel
    {
        public List<Contributor> Contributors { get; set; }
        public int SimchaId { get; set; }
        public string SimchaName { get; set; }
    }
}
