using Microsoft.AspNetCore.Mvc;
using SimchaFund.Data;
using SimchaFund.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimchaFund.Web.Controllers
{
    public class Simchas : Controller
    {
        private string _connectionString =
          "Data Source=.\\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=true;";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult New(Simcha simcha)
        {
            var db = new DbManager(_connectionString);
            db.NewSimcha(simcha);
            return Redirect("/");
        }

        public IActionResult Contributions(int simchaId)
        {
            var db = new DbManager(_connectionString);
            var contributors = db.GetContributors();
            foreach (var con in contributors)
            {
                con.Balance = db.GetBalance(con.Id);
                con.Amount = db.GetAmountForSimcha(simchaId, con.Id);
                if (con.Amount == 0)
                {
                    con.Amount = 5;
                    con.Include = false;
                }
                else
                {
                    con.Include = true;
                }
            }

            return View(new ContributionsViewModel
            {
                Contributors = contributors,
                SimchaId = simchaId,
                SimchaName = db.GetSimchaName(simchaId)
            });
        }

        public IActionResult UpdateContributions(List<Contributor> contributors, int simchaId)
        {
            var db = new DbManager(_connectionString);
            contributors = contributors.Where(c => c.Include).ToList();
            List<Contribution> contributions = new List<Contribution>();
            foreach (var c in contributors)
            {
                contributions.Add(new Contribution
                {
                    ContributorId = c.Id,
                    SimchaId = simchaId,
                    Amount = c.Amount
                });
            }
            db.UpdateContributions(contributions, simchaId);
            return Redirect("/");
        }

    }
}
