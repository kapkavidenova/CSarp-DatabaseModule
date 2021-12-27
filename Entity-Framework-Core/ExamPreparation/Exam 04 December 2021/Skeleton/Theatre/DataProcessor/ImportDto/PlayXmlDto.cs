﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class PlayXmlDto
    {     
        [XmlElement("Title")]
        [Required]
        [MaxLength(50)]
        [MinLength(4)]
        public string Title { get; set; }

        [Required]
        [XmlElement("Duration")]
        public string Duration { get; set; }

        [Range(0.00, 10.00)]
        [XmlElement("Rating")]
        public float Rating { get; set; }

        [Required]
        [XmlElement("Genre")]
        public string Genre { get; set; }

        [Required]
        [MaxLength(700)]
        [XmlElement("Description")]
        public string Description { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        [XmlElement("Screenwriter")]
        public string Screenwriter { get; set; }

    }
}
