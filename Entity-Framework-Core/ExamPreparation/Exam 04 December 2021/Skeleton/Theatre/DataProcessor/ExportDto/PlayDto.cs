using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto
{
    class PlayDto
    {
        [XmlType("Play")]
        public class ExportPlayDto
        {
            [XmlAttribute]
            public string Title { get; set; }

            [XmlAttribute]
            public string Duration { get; set; }


            [XmlAttribute]
            public string Rating { get; set; }

            [XmlAttribute]
            public string Genre { get; set; }

        }
    }
}
