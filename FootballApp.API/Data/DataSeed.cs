using System;
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
            if (!_context.Users.Any())
            {
                var users = new List<User>
                            {
                                new User { Username = "milos", Firstname = "Milos", Lastname = "Nikic", City = "Belgrade", Country = "Serbia",
                                           Email = "milos.nikic@gmail.com", DateOfBirth = new DateTime(1996,12,31), Created = DateTime.Now, IsActive = true,
                                           Gender = Gender.Male, LastActive = null},
                                new User { Username = "darko", Firstname = "Darko", Lastname = "Nikic", City = "Belgrade", Country = "Serbia",
                                           Email = "darko.nikic@gmail.com", DateOfBirth = new DateTime(1981,12,05), Created = DateTime.Now, IsActive = true,
                                           Gender = Gender.Male, LastActive = null},
                                new User { Username = "milutin", Firstname = "Milutin", Lastname = "Nikic", City = "Belgrade", Country = "Serbia",
                                           Email = "milutin.nikic@gmail.com", DateOfBirth = new DateTime(1956,1,1), Created = DateTime.Now, IsActive = true,
                                           Gender = Gender.Male, LastActive = null},
                                new User { Username = "snezana", Firstname = "Snezana", Lastname = "Nikic", City = "Belgrade", Country = "Serbia",
                                           Email = "snezana.nikic@gmail.com", DateOfBirth = new DateTime(1986,3,10), Created = DateTime.Now, IsActive = true,
                                           Gender = Gender.Female, LastActive = null},
                            };
                string password = "Test123*";
                foreach (var user in users)
                {   
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(password, out passwordHash, out passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    _context.Users.Add(user);
                }

                _context.SaveChanges();
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}