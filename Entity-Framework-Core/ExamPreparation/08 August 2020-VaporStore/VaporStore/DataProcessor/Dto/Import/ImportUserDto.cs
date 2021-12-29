using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class ImportUserDto
    {

        [Required]
        [RegularExpression("^[A-Z]{a-z]{2,} [A-Z]{a-z]{2,}$")]
        public string FullName { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(3)]

        public string Username { get; set; }


        [Required]
        public string Email { get; set; }

        [Range(3,103)]
        public int Age { get; set; }

        public IEnumerable<CardImportDto> Cards { get; set; }
    }
}
