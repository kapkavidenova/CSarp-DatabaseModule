﻿using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentDto
    {
      
        [Required]
        [MaxLength(25)]
        [MinLength(3)]
        public string Name { get; set; }

        public ICollection<Cell> Cells { get; set; }
    }
}
