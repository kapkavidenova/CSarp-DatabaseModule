﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    public class CarDTO
    {
        public string Make { get; set; }
        public string Model{ get; set; }

        public long TravelledDistance{ get; set; }

        public IEnumerable<int> PartsId { get; set; }

    }
}
