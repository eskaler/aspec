using System;
using System.Collections.Generic;

namespace ASPEC.Models
{
    public partial class Measurement
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int UnitId { get; set; }
        public double? Value { get; set; }
        public DateTime? DateTime { get; set; }
        public int? DeviceId { get; set; }

        public virtual Device Device { get; set; }
        public virtual Task Task { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
