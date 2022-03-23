using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimchaFund.Data;
using SimchaFund.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SimchaFund.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString =
            "Data Source=.\\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=true;";


        public IActionResult Index()
        {
            var db = new DbManager(_connectionString);
            var simchas = db.GetSimchas();
            int totalContributors = db.ContributorTotal();
            return View(new ViewModel
            {
                Simchas = simchas,
                ContributorCount = totalContributors                
            });
        }
      
    }
}
