﻿using BookShop.Data.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.Data.Models
{
    public class Book
    {
        public Book()
        {
            this.AuthorsBooks = new HashSet<AuthorBook>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Range(50,5000)]
        public int Pages { get; set; }

        [Required]
        public DateTime PublishedOn { get; set; }

        [JsonIgnore]
        public virtual ICollection<AuthorBook> AuthorsBooks { get; set; }
    }
}
