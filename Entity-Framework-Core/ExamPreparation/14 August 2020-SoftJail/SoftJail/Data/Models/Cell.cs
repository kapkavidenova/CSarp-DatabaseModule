﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.Data.Models
{
    public class Cell
    {
        public Cell()
        {
            this.Prisoners = new HashSet<Prisoner>();
        }
        public int Id { get; set; }

        [Range(1,1000)]
        public int CellNumber { get; set; }

        //[Required]
        public bool HasWindow { get; set; }

        public int DepartmentId { get; set; }

        //[Required]
        public virtual Department Department { get; set; }

        public ICollection<Prisoner> Prisoners { get; set; }
    }
}
