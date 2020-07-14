using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ASPEC.Models
{
    public partial class Role
    {
        public Role()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> User { get; set; }
    }
}
