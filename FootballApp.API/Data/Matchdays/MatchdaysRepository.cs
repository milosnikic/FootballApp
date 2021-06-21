using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using FootballApp.API.Models.Views;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Matchdays
{
    public class MatchdaysRepository : Repository<Matchday>, IMatchdaysRepository
    {
        public MatchdaysRepository(DataContext context)
            : base(context)
        {

        }

        public async Task<Matchday> GetMatchdayWithAdditionalInformation(int matchId)
        {
            var match = await DataContext.Matchdays
                                        .Include(m => m.Location)
                                        .ThenInclude(l => l.City)
                                        .ThenInclude(l => l.Country)
                                        .Include(m => m.MatchStatuses)
                                        .ThenInclude(m => m.User)
                                        .ThenInclude(u => u.Photos)
                                        .Include(m => m.Group)
                                        .FirstOrDefaultAsync(m => m.Id == matchId);
            return match;
        }

        public async Task<ICollection<Matchday>> GetUpcomingMatchesForGroup(int groupId)
        {
            var upcomingMatches = await DataContext.Matchdays
                                                   .Where(m => m.Group.Id == groupId && m.DatePlaying > DateTime.Now)
                                                   .Include(m => m.Location)
                                                   .ThenInclude(l => l.City)
                                                   .ThenInclude(l => l.Country)
                                                   .Include(m => m.MatchStatuses)
                                                   .OrderBy(m => m.DatePlaying)
                                                   .OrderBy(m => m.Location.CountryId)
                                                   .ToListAsync();
            return upcomingMatches;
        }

        public async Task<ICollection<MatchStatus>> GetUpcomingMatchesForUser(int userId)
        {
            var upcomingMatches = await DataContext.MatchStatuses
                                                    .Where(m => m.UserId == userId && m.Matchday.DatePlaying > DateTime.Now)
                                                    .Include(m => m.Matchday)
                                                    .ThenInclude(m => m.MatchStatuses)
                                                    .Include(m => m.Matchday)
                                                    .ThenInclude(m => m.Location)
                                                    .ThenInclude(l => l.City)
                                                    .ThenInclude(l => l.Country)
                                                    .OrderBy(m => m.Matchday.DatePlaying)
                                                    .ToListAsync();
            return upcomingMatches;
        }

        public async Task<ICollection<Matchday>> GetUpcomingMatchesApplicableForUser(int userId)
        {
            var upcomingMatches = await DataContext.Matchdays
                                                    .Include(m => m.Group)
                                                    .ThenInclude(g => g.Memberships)
                                                    .Where(m =>
                                                     m.Group.Memberships.FirstOrDefault(u => u.UserId == userId) != null && // This line is related to user participation in group
                                                     m.MatchStatuses.FirstOrDefault(ms => ms.UserId == userId) == null &&   // This line is related to user not having match status
                                                     m.DatePlaying > DateTime.Now                                           // This line ensures that match is hasn't been played
                                                    )
                                                    .Include(m => m.Location)
                                                    .ThenInclude(l => l.City)
                                                    .ThenInclude(l => l.Country)
                                                    .Include(m => m.MatchStatuses)
                                                    .OrderBy(m => m.DatePlaying)
                                                    .ToListAsync();

            return upcomingMatches;
        }

        public async Task<KeyValuePair<bool, string>> OrganizeMatchPlayed(OrganizeMatchDto organizeMatchDto)
        {
            var matchday = await DataContext.Matchdays.FirstOrDefaultAsync(x => x.Id == organizeMatchDto.MatchdayId);
            if (matchday == null)
            {
                return new KeyValuePair<bool, string>(false, "Matchday with specified id doesn't exist");
            }

            if (matchday.DatePlaying >= DateTime.Now)
            {
                return new KeyValuePair<bool, string>(false, "Matchday has not passed, so you are not able to add result");
            }

            Team homeTeam, awayTeam;
            MatchPlayed matchPlayed;
            CreateMatchPlayed(organizeMatchDto, matchday, out homeTeam, out awayTeam, out matchPlayed);

            AddHomeTeamPlayers(organizeMatchDto, homeTeam, matchPlayed);
            AddAwayTeamPlayers(organizeMatchDto, awayTeam, matchPlayed);

            return new KeyValuePair<bool, string>(true, "Match played successfully created");
        }

        private void CreateMatchPlayed(OrganizeMatchDto organizeMatchDto, Matchday matchday, out Team homeTeam, out Team awayTeam, out MatchPlayed matchPlayed)
        {
            homeTeam = new Team()
            {
                Name = organizeMatchDto.HomeTeamName,
                Matchday = matchday
            };
            awayTeam = new Team()
            {
                Name = organizeMatchDto.AwayTeamName,
                Matchday = matchday
            };
            DataContext.Teams.Add(homeTeam);
            DataContext.Teams.Add(awayTeam);

            DataContext.SaveChanges();

            matchPlayed = new MatchPlayed()
            {
                HomeGoals = organizeMatchDto.HomeGoals,
                AwayGoals = organizeMatchDto.AwayGoals,
                DatePlayed = matchday.DatePlaying,
                Home = homeTeam,
                Away = awayTeam,
            };

            DataContext.MatchPlayeds.Add(matchPlayed);

            DataContext.SaveChanges();
        }

        private void AddHomeTeamPlayers(OrganizeMatchDto organizeMatchDto, Team homeTeam, MatchPlayed matchPlayed)
        {
            foreach (var player in organizeMatchDto.HomeTeamMembers)
            {
                var teamMember = new TeamMember()
                {
                    Position = player.Position ?? Position.UKNWN,
                    Team = homeTeam,
                    UserId = player.UserId,
                };

                var teamMemberStatistics = new TeamMemberStatistic()
                {
                    Goals = player.Statistics.Goals,
                    Assists = player.Statistics.Assists,
                    MinutesPlayed = player.Statistics.MinutesPlayed,
                    Rating = player.Statistics.Rating,
                    Team = homeTeam,
                    MatchPlayed = matchPlayed,
                };
                
                DataContext.TeamMembers.Add(teamMember);
                DataContext.SaveChanges();

                teamMemberStatistics.TeamMember = teamMember;
                DataContext.TeamMemberStatistics.Add(teamMemberStatistics);
                DataContext.SaveChanges();

                teamMember.TeamMemberStatistics = teamMemberStatistics;
                DataContext.SaveChanges();
            }
        }
        
        private void AddAwayTeamPlayers(OrganizeMatchDto organizeMatchDto, Team awayTeam, MatchPlayed matchPlayed)
        {
            foreach (var player in organizeMatchDto.AwayTeamMembers)
            {
                var teamMember = new TeamMember()
                {
                    Position = player.Position ?? Position.UKNWN,
                    Team = awayTeam,
                    UserId = player.UserId,
                };

                var teamMemberStatistics = new TeamMemberStatistic()
                {
                    Goals = player.Statistics.Goals,
                    Assists = player.Statistics.Assists,
                    MinutesPlayed = player.Statistics.MinutesPlayed,
                    Rating = player.Statistics.Rating,
                    Team = awayTeam,
                    MatchPlayed = matchPlayed,
                };


                DataContext.TeamMembers.Add(teamMember);
                DataContext.SaveChanges();

                teamMemberStatistics.TeamMember = teamMember;
                DataContext.TeamMemberStatistics.Add(teamMemberStatistics);
                DataContext.SaveChanges();

                teamMember.TeamMemberStatistics = teamMemberStatistics;
                DataContext.SaveChanges();
            }
        }

        public async Task<ICollection<OrganizedMatchInformationView>> GetOrganizedMatchInformation(int matchdayId)
        {
            var storedProcedureQuery = "EXEC [dbo].[GetOrganizedMatchInformation] @MatchdayId = {0}";
            var query = DataContext.Query<OrganizedMatchInformationView>().FromSql(storedProcedureQuery, matchdayId);
            return await query.ToListAsync();
        }

        public async Task<ICollection<MatchHistoryView>> GetMatchHistoryForUser(int userId)
        {
            var storedProcedureQuery = "EXEC [dbo].[GetMatchHistoryForUser] @UserId = {0}";
            var query = DataContext.Query<MatchHistoryView>().FromSql(storedProcedureQuery, userId);
            return await query.ToListAsync();
        }

        public async Task<ICollection<GroupMatchHistoryView>> GetMatchHistoryForGroup(int groupId)
        {
            var storedProcedureQuery = "EXEC [dbo].[GetAllMatchesForGroup] @GroupId = {0}";
            var query = DataContext.Query<GroupMatchHistoryView>().FromSql(storedProcedureQuery, groupId);
            return await query.ToListAsync();
        }

        public async Task<ICollection<LatestFiveMatchesView>> GetLatestFiveMatchesForUser(int userId)
        {
            var storedProcedureQuery = "EXEC [dbo].[GetLatestFiveMatchesForUser] @UserId = {0}";
            var query = DataContext.Query<LatestFiveMatchesView>().FromSql(storedProcedureQuery, userId);
            return await query.ToListAsync();
        }

        public DataContext DataContext
        {
            get
            {
                return Context as DataContext;
            }
        }

    }
}