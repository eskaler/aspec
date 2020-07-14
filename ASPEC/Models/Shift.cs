using System;
using System.Collections.Generic;

namespace ASPEC.Models
{
    public partial class Shift
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public virtual User User { get; set; }
    }
}
