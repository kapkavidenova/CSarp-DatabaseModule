using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Data.Models;

namespace Theatre.DataProcessor.ImportDto
{
    public class TicketJsonDto
    {
        public int Id { get; set; }

        [Range(typeof(decimal), "1.00", "100.00")]
        public decimal Price { get; set; }

        [Range(typeof(sbyte), "1", "10")]
        public sbyte RowNumber { get; set; }

        public int PlayId { get; set; }
    }
}
