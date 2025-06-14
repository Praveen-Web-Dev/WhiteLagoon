﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
    public class Villa
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }

        public string? Description { get; set; }
        [Display(Name = "Price per Night")]
        [Range(10,1000)]
        public required double Price { get; set; }
    
        public int Sqft { get; set; }
        [Range(1, 100, ErrorMessage = "Value must be between 1 and 100.")]
        public int Occupancy { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
        [Display(Name="Image Url")]
        public string? ImageUrl { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }


    }
}
