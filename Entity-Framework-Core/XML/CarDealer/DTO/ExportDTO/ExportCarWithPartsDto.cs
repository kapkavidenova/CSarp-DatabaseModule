using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.DTO.ExportDTO
{
    [XmlType("car")]
    public class ExportCarWithPartsDto
    {

        [XmlAttribute("make")]
        public string Make{ get; set; }


        [XmlAttribute("model")]
        public string Model { get; set; }


        [XmlAttribute("travelled-distance")]
        public long TravelledDistance{ get; set; }


        [XmlArray("parts")]
        public ExportCarWithPartsDto[] Parts { get; set; }

    }
}
//[XmlArray("parts")]
//public ImportCarPartDto[] Parts { get; set; }