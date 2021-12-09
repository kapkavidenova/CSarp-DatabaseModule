using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DTO.ExportDTO
{
    [XmlType("car")]
    public class ExportCarsFromBmvDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }


        [XmlAttribute("model")]
        public string Model { get; set; }


        [XmlAttribute("travelled-distance")]
        public long TraveledDistance { get; set; }
    }
}
