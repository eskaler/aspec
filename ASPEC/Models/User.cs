using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ASPEC.Models
{
    public partial class User
    {
        public User()
        {
            Shift = new HashSet<Shift>();
            TaskCreator = new HashSet<Task>();
            TaskExecutor = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string SecondName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public DateTime BirthDate { get; set; }
        [JsonIgnore]
        public string Login { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string Fio
        {
            get
            {
                if (SecondName == "Любой")
                    return "Любой";
                return $"{SecondName} {FirstName[0]}.{Patronymic[0]}.";
            }
        }
        public virtual Role Role { get; set; }

        [JsonIgnore]
        public virtual ICollection<Shift> Shift { get; set; }
        [JsonIgnore]
        public virtual ICollection<Task> TaskCreator { get; set; }
        [JsonIgnore]
        public virtual ICollection<Task> TaskExecutor { get; set; }
    }
}
