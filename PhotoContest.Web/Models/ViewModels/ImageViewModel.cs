using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoContest.Web.Models.ViewModels
{
    public class ImageViewModel
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int VotesCount { get; set; }
    }
}