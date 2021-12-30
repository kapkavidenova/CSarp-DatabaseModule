using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Prisoner")]
    public class ImportOffiPcerPrisonerDto
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }
    }
}
