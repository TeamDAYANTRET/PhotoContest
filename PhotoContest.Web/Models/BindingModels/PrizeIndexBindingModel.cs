using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;

namespace PhotoContest.Web.Models.BindingModels
{
    public class PrizeIndexBindingModel
    {
        public int ContestId { get; set; }
        public int CountOfPrizes { get; set; }
    }
}