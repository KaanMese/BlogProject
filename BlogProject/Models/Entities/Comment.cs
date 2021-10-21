using BlogProject.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models.Entities
{
    public class Comment
    {
        public int ID { get; set; }

        [Required]
        public int UserID { get; set; }
        public virtual AppUser User { get; set; }

        
        public int? ContentID { get; set; }
        public virtual Content Content { get; set; }

        [Required]
        public string Text { get; set; }
        public DateTime CreDate { get; set; }
        public bool IsApprove { get; set; }

        public int? CommentID { get; set; }
        public virtual List<Comment> SubComments { get; set; }

    }
}