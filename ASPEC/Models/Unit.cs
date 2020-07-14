using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ASPEC.Models
{
    public partial class Unit
    {
        public Unit()
        {
            Measurement = new HashSet<Measurement>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Measurement> Measurement { get; set; }
    }
}
