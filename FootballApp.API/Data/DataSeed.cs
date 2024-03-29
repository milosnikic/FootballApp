using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
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

            if (!_context.Countries.Any())
            {
                var countries = new List<Country>
                            {
                                new Country { Name = "Serbia", Flag = "../../../../assets/sr.png" },
                                new Country { Name = "Bosnia and Herzegovina", Flag = "../../../../assets/bih.png" },
                                new Country { Name = "Croatia", Flag = "../../../../assets/cro.png" },
                                new Country { Name = "Montenegro", Flag = "../../../../assets/mne.png" }
                            };
                _context.Countries.AddRange(countries);
                _context.SaveChanges();

                var cities = new List<City>
                            {
                                new City { Name = "Belgrade", Country = countries[0]},
                                new City { Name = "Novi Sad", Country = countries[0]},
                                new City { Name = "Nis", Country = countries[0]},
                                new City { Name = "Krusevac", Country = countries[0]},
                                new City { Name = "Kragujevac", Country = countries[0]},

                                new City { Name = "Sarajevo", Country = countries[1]},
                                new City { Name = "Bijeljina", Country = countries[1]},
                                new City { Name = "Tuzla", Country = countries[1]},
                                new City { Name = "Zenica", Country = countries[1]},
                                new City { Name = "Novi Travnik", Country = countries[1]},

                                new City { Name = "Split", Country = countries[2]},
                                new City { Name = "Umag", Country = countries[2]},
                                new City { Name = "Zagreb", Country = countries[2]},
                                new City { Name = "Sibenik", Country = countries[2]},
                                new City { Name = "Opatija", Country = countries[2]},

                                new City { Name = "Ulcinj", Country = countries[3]},
                                new City { Name = "Podgorica", Country = countries[3]},
                                new City { Name = "Niksic", Country = countries[3]},
                                new City { Name = "Kolasin", Country = countries[3]},
                                new City { Name = "Bar", Country = countries[3]},
                            };


                _context.Cities.AddRange(cities);
                _context.SaveChanges();
            }

            if (!_context.Users.Any() || _context.Users.Count() < 10)
            {
                var users = new List<User>
                            {
                                new PowerUser { Username = "milos", Firstname = "Milos", Lastname = "Nikic", City = _context.Cities.FirstOrDefault(c => c.Name == "Belgrade"), Country = _context.Countries.FirstOrDefault(c => c.Name == "Serbia"),
                                           Email = "milos.nikic@gmail.com", DateOfBirth = new DateTime(1996,12,31), Created = DateTime.Now, IsActive = true,
                                           Gender = Gender.Male, LastActive = null},
                                new PowerUser { Username = "darko", Firstname = "Darko", Lastname = "Nikic", City = _context.Cities.FirstOrDefault(c => c.Name == "Belgrade"), Country = _context.Countries.FirstOrDefault(c => c.Name == "Serbia"),
                                           Email = "darko.nikic@gmail.com", DateOfBirth = new DateTime(1981,12,05), Created = DateTime.Now, IsActive = true,
                                           Gender = Gender.Male, LastActive = null},
                                new PowerUser { Username = "milutin", Firstname = "Milutin", Lastname = "Nikic", City = _context.Cities.FirstOrDefault(c => c.Name == "Belgrade"), Country = _context.Countries.FirstOrDefault(c => c.Name == "Serbia"),
                                           Email = "milutin.nikic@gmail.com", DateOfBirth = new DateTime(1956,1,1), Created = DateTime.Now, IsActive = true,
                                           Gender = Gender.Male, LastActive = null},
                                new CommonUser { Username = "snezana", Firstname = "Snezana", Lastname = "Nikic", City = _context.Cities.FirstOrDefault(c => c.Name == "Belgrade"), Country = _context.Countries.FirstOrDefault(c => c.Name == "Serbia"),
                                           Email = "snezana.nikic@gmail.com", DateOfBirth = new DateTime(1986,3,10), Created = DateTime.Now, IsActive = true,
                                           Gender = Gender.Female, LastActive = null},
                            };

                AddRandomUsers(users);

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

        private void AddRandomUsers(List<User> users)
        {
            Randomizer.Seed = new Random(8675309);
            var cities = _context.Cities.ToList();
            var countries = _context.Countries.ToList();

            for (int i = 0; i < 100; i++)
            {
                var randomUser = new Faker<CommonUser>()
                    .RuleFor(u => u.Firstname, f => f.Person.FirstName)
                    .RuleFor(u => u.Lastname, f => f.Person.LastName)
                    .RuleFor(u => u.Username, f => f.Person.UserName)
                    .RuleFor(u => u.City, f => f.PickRandom(cities))
                    .RuleFor(u => u.Country, f => f.PickRandom(countries))
                    .RuleFor(u => u.Email, f => f.Person.Email)
                    .RuleFor(u => u.DateOfBirth, f => f.Person.DateOfBirth)
                    .RuleFor(u => u.Created, DateTime.Now)
                    .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                    .RuleFor(u => u.IsActive, true);

                users.Add(randomUser.Generate());
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
