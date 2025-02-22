﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  GP.Application.CategoryQueries
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public DateTime DateCreated { get; set; }
        public string Date => DateCreated.ToString("MMMM dd, yyyy");
    }
}
