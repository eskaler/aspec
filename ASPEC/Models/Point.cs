using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ASPEC.Models
{
    public partial class Point
    {
        public Point()
        {
            Task = new HashSet<Task>();
        }

        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string PrjMark { get; set; }
        public double? PosX { get; set; }
        public double? PosY { get; set; }
        public double? PosZ { get; set; }

        public string PointInfo { get { return $"{PosX.ToString()} {PosY.ToString()}"; } }

        [JsonIgnore]
        public virtual Depart Parent { get; set; }
        [JsonIgnore]
        public virtual ICollection<Task> Task { get; set; }
    }
}
