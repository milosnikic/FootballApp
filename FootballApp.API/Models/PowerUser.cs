using System.Collections.Generic;
using System.Linq;

namespace FootballApp.API.Models
{
    public class PowerUser : User
    {
        public int NumberOfGroupsCreated { get; set; } = 0;
        public ICollection<Group> GroupsCreated { get; set; }
    }
}