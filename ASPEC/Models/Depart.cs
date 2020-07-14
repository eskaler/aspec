using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ASPEC.Models
{
    public partial class Depart
    {
        public Depart()
        {
            InverseParent = new HashSet<Depart>();
            Point = new HashSet<Point>();
        }

        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string PrjMark { get; set; }
        public string Scheme { get; set; }

        public virtual Depart Parent { get; set; }

        [JsonIgnore]
        public virtual ICollection<Depart> InverseParent { get; set; }
        [JsonIgnore]
        public virtual ICollection<Point> Point { get; set; }
    }
}
