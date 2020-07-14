using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ASPEC.Models
{
    public partial class RadiationType
    {
        public RadiationType()
        {
            Task = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Task> Task { get; set; }
    }
}
