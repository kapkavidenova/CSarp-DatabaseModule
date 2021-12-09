using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DTO.ImportDTO
{
    //[XmlType("Customer")]
    //public class ImportCustomerDto
    //{
    //    [XmlElement("name")]
    //    public string Name { get; set; }

    //    [XmlElement("birthDate")]
    //    public DateTime BirthDate { get; set; }

    //    [XmlAnyElement("isYoungDriver")]
    //    public string IsYoungDriver { get; set; }
        [XmlType("Customer")]
        public class ImportCustomerDto
        {
            [XmlElement("name")]
            public string Name { get; set; }

            [XmlElement("birthDate")]
            public DateTime BirthDate { get; set; }

            [XmlElement("isYoungDriver")]
            public bool IsYoungDriver { get; set; }
        }


    //}
}

