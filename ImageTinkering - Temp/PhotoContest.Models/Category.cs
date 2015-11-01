﻿namespace PhotoContest.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Category 
    {
        private ICollection<Contest> contests;

        public Category()
        {
            this.contests = new HashSet<Contest>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public virtual ICollection<Contest> Contests { get { return this.contests; } set { this.contests = value; } }
    }
}
