using System;
using System.ComponentModel.DataAnnotations;

namespace NxApp.Models
{
    public class Note
    {
        [Key]
        public string Filename { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
