using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoContest.Web.Models.BindingModels
{
    public class EditUserProfileBindingModel
    {
        [MinLength(3)]
        public string FirstName { get; set; }
        [MinLength(3)]
        public string LastName { get; set; }
        public string AvatarPath { get; set; }
        [MinLength(8)]
        public string PhoneNumber { get; set; }
        [DataType(DataType.MultilineText)]
        public string AboutMe { get; set; }

    }
}