using System;
using System.Collections.Generic;

namespace ASPEC.Models
{
    public partial class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
