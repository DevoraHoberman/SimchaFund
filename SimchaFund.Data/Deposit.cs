﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaFund.Data
{
    public class Deposit
    {
        public int DepositId { get; set; }
        public decimal Amount { get; set; }
        public int ContributorId { get; set; }
        public DateTime Date { get; set; }
    }
}
