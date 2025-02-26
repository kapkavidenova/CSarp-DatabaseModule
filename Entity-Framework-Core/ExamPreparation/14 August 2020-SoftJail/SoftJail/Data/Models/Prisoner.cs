﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Prisoner
    {
        public Prisoner()
        {
            this.Mails = new HashSet<Mail>();
            this.PrisonerOfficers = new HashSet<OfficerPrisoner>();
        }
        public int Id { get; set; }

        [Required]
        //[MaxLength(20)]
        public string FullName { get; set; }

        [Required]
        public string Nickname { get; set; }

        //[Range(18,65)]
        public int Age { get; set; }

        //[Required]
        public DateTime IncarcerationDate { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public decimal? Bail { get; set; }


        public int? CellId { get; set; }

        public virtual Cell Cell { get; set; }

        public ICollection<Mail> Mails { get; set; }

        public ICollection<OfficerPrisoner> PrisonerOfficers { get; set; }
      
    }
}