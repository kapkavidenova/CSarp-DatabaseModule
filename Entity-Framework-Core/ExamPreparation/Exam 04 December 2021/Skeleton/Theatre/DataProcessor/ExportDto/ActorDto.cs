﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto
{
    class ActorDto
    {
        [XmlType("Actor")]
        public class ExportActorDto
        {
            [XmlAttribute]
            public string FullName { get; set; }

            [XmlAttribute]
            public string MainCharacter { get; set; }
        }
    }
}
