using System.Collections.Generic;
using System.Linq;
using FootballApp.API.Models;

namespace FootballApp.API.Data
{
    public class DataSeed
    {
        private readonly DataContext _context;
        public DataSeed(DataContext context)
        {
            _context = context;

        }

        public void SeedDatabase()
        {
            if (!_context.Achievements.Any())
            {
                var achievements = new List<Achievement>
                                {
                                    new Achievement { Name = "Played over 10 games", Value = 101, Points = 10, Icon = "fas fa-award icon" },
                                    new Achievement { Name = "Played over 50 games", Value = 102, Points = 50, Icon = "fas fa-award icon" },
                                    new Achievement { Name = "Played over 100 games", Value = 103, Points = 100, Icon = "fas fa-award icon" },
                                    new Achievement { Name = "Played over 500 games", Value = 104, Points = 500, Icon = "fas fa-award icon" },
                                    new Achievement { Name = "Played over 1000 games", Value = 105, Points = 1000, Icon = "fas fa-award icon" },
                                    new Achievement { Name = "First match played", Value = 200, Points = 10, Icon = "fas fa-shield-alt" },
                                    new Achievement { Name = "First assist made", Value = 201, Points = 10, Icon = "fas fa-shield-alt" },
                                    new Achievement { Name = "First goal scored", Value = 202, Points = 10, Icon = "fas fa-shield-alt" },
                                    new Achievement { Name = "Most assists in single match", Value = 300, Points = 100, Icon = "fas fa-running icon" },
                                    new Achievement { Name = "Most goals in single match", Value = 301, Points = 100, Icon = "fas fa-running icon" },
                                    new Achievement { Name = "Most organized matches in last year", Value = 302, Points = 500, Icon = "fas fa-running icon" },
                                    new Achievement { Name = "Highest rated player in a match", Value = 400, Points = 50, Icon = "far fa-futbol icon" }
                                };
                _context.Achievements.AddRange(achievements);

                _context.SaveChanges();
            }
        }
    }
}