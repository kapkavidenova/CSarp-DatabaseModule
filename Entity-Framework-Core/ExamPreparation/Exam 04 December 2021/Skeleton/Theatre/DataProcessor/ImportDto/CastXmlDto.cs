using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Theatre.Data.Models;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Cast")]
    public class CastXmlDto
    {

        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        [XmlElement("FullName")]
        public string FullName { get; set; }

        [Required]
        [XmlElement("IsMainCharacter")]
        public bool IsMainCharacter { get; set; }

        [Required]
        [MaxLength(15)]
        [RegularExpression(@"^((\+44)-([0-9]{2})-([0-9]{3})-([0-9]{4}))$")]
        public string PhoneNumber { get; set; }

        public int PlayId { get; set; }

    }
}
