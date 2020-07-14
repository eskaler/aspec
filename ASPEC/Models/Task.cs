using System;
using System.Collections.Generic;

namespace ASPEC.Models
{
    public partial class Task
    {
        public Task()
        {
            Measurement = new HashSet<Measurement>();
        }

        public int Id { get; set; }
        public int CreatorId { get; set; }
        public int ExecutorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int RadiationTypeId { get; set; }
        public int StatusId { get; set; }
        public Guid? PointId { get; set; }

        

        public virtual User Creator { get; set; }
        public virtual User Executor { get; set; }
        public virtual Point Point { get; set; }
        public virtual RadiationType RadiationType { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<Measurement> Measurement { get; set; }
    }
}
