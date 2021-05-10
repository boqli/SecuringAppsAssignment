using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string CommentText { get; set; }

        [Required]
        public string CommenterId { get; set; }

        [Required]
        public virtual Assignment Assignment { get; set; }

        [ForeignKey("Assignment")]
        public Guid AssignmentID { get; set; }
    }
}
