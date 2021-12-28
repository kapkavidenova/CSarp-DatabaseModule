using BookShop.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ExportDto
{
    [XmlType("Book")]
    public class ExportBookDto
    {
        [Range(50, 5000)]
        [XmlAttribute("Pages")]
        public int Pages { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        [XmlElement("Name")]
        public string BookName { get; set; }

        //[Required]
        //[XmlElement("Genre")]
        //public Genre Genre { get; set; }

        [Required]
        [XmlElement("Date")]
        public string Date { get; set; }
    }
}
