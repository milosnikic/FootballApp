using System.Collections.Generic;

namespace FootballApp.API.Models
{
    public class PowerUser : User
    {
        public int NumberOfGroupsCreated { get; set; } = 0;
        public ICollection<Group> GroupsCreated { get; set; }
    }
}