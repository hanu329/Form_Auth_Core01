using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace FormAuthCore.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Company { get; set; }
        public string Position { get; set; }
    }
}
