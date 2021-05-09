using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string FileName { get; set; } //optional
        public string Description { get; set; } //optional

        [Required]
        public string Path { get; set; }

        [Required]
        public string Signature { get; set; }

        [Required]
        public string Owner { get; set; } //student email

        [Required]
        public virtual Task Task { get; set; }

        [ForeignKey("Task")]
        public Guid TaskId { get; set; }


    }
}
