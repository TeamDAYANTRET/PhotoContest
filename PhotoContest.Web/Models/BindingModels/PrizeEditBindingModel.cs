using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoContest.Web.Models.BindingModels
{
    public class PrizeEditBindingModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int Place { get; set; }
    }
}