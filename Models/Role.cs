﻿using System;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace FormAuthCore.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
