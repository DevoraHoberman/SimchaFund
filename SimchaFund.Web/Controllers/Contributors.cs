using Microsoft.AspNetCore.Mvc;
using SimchaFund.Data;
using SimchaFund.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimchaFund.Web.Controllers
{
    public class Contributors : Controller
    {
        private string _connectionString =
           "Data Source=.\\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=true;";

        public IActionResult Index()
        {
            var db = new DbManager(_connectionString);
            List<Contributor> contributors = db.GetContributors();
            foreach (var con in contributors)
            {
                con.Balance = db.GetBalance(con.Id);
            }
            decimal total = 0;
            for (int i = 0; i < contributors.Count; i++)
            {
                total += db.GetBalance(i);
            }

            var cvm = new ContributorViewModel
            {
                Contributors = contributors,
                Total = total
            };
            if (TempData["success-message"] != null)
            {
                cvm.Message = (string)TempData["success-message"];
            }
            return View(cvm);
        }

       public IActionResult Deposit(Deposit deposit)
        {
            var db = new DbManager(_connectionString);
            db.NewDeposit(deposit);
            TempData["success-message"] = "Deposit successfully recorded!";
            return Redirect("/contributors/index");
               
        }

        public IActionResult New(Contributor contributor, int initialDeposit)
        {
            var db = new DbManager(_connectionString);
            int id = db.NewContributor(contributor);
            db.NewDeposit(id, initialDeposit);
            TempData["success-message"] = "Contributor successfully recorded!";
            return Redirect("/contributors/index");
        }

        public IActionResult Edit(Contributor contributor)
        {
            var db = new DbManager(_connectionString);
            db.EditContributor(contributor);
            TempData["success-message"] = "Contributor updated successfully!";
            return Redirect("/contributors/index");
        }

        public IActionResult History(int contribid)
        {
            var db = new DbManager(_connectionString);
            List<History> history = db.GetHistory(contribid);
            List<History> histories = history.OrderBy(h => h.Date).ToList();
            var hvm = new HistoryViewModel
            {
                Balance = db.GetBalance(contribid),
                Name = db.GetContributorName(contribid),
                Histories = histories
            };
            return View(hvm);
        }
    }
}
