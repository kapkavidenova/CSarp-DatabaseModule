using SoftJail.Data.Models;
using SoftJail.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class ImportOfficerDto
    {
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        [XmlElement("Name")]
        public string FullName { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [XmlElement("Money")]
        public decimal Money { get; set; }

        [Required]
        [XmlElement("Position")]
        public string  Position { get; set; }

        [Required]
        [XmlElement("Weapon")]
        public string Weapon { get; set; }

        //[Required]
        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [Required]
        [XmlArray("Prisoners")]
        public List<ImportOffiPcerPrisonerDto> Prisoners { get; set; }


    }
}
