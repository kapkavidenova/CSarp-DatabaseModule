using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    public class SaleDTO
    {
        public int Carid { get; set; }

        public int CustomerId { get; set; }

        public decimal Discount { get; set; }
    }
}
