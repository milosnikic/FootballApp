using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballApp.API.Migrations
{
    public partial class AddStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var getAllMatchesForGroupSP = @"USE [FootballApp]
                        GO
                        /****** Object:  StoredProcedure [dbo].[GetAllMatchesForGroup]    Script Date: 28.7.2021. 22:55:48 ******/
                        SET ANSI_NULLS ON
                        GO
                        SET QUOTED_IDENTIFIER ON
                        GO

                        -- =============================================
                        -- Author:    Miloš Nikić
                        -- Create date: 15.6.2021.
                        -- Description:  Method to get all matches for group
                        -- =============================================
                        CREATE OR ALTER PROCEDURE [dbo].[GetAllMatchesForGroup]
                        -- Add the parameters for the stored procedure here
                        @GroupId INT
                        AS
                        BEGIN
                            -- SET NOCOUNT ON added to prevent extra result sets from
                            -- interfering with SELECT statements.
                            SET nocount ON;

                            SELECT *
                            INTO   #allmatches
                            FROM   (SELECT md.id,
                                            md.NAME,
                                            md.description,
                                            md.dateplaying,
                                            md.numberofplayers,
                                            md.locationid,
                                            md.groupid,
                                            t.matchdayid,
                                            mp.HomeId,
                                            mp.HomeGoals,
                                            mp.AwayId,
                                            mp.AwayGoals
                                    FROM   matchdays md
                                            LEFT JOIN teams t
                                                    ON t.matchdayid = md.id
                                            LEFT JOIN MatchPlayeds mp
                                                    ON t.Id = mp.HomeId or t.Id = mp.AwayId
                                    WHERE  md.groupid = 1
                                    GROUP  BY md.id,
                                                md.NAME,
                                                md.description,
                                                md.dateplaying,
                                                md.numberofplayers,
                                                md.locationid,
                                                md.groupid,
                                                t.matchdayid,
                                                mp.HomeId,
                                                mp.HomeGoals,
                                                mp.AwayId,
                                                mp.AwayGoals) AS x

                            SELECT *
                            INTO   #allstatuses
                            FROM   (SELECT ms.matchdayid
                                    FROM   matchstatuses ms
                                    GROUP  BY ms.matchdayid) AS z

                            SELECT am.Id,
                                    am.Name,
                                    am.Description,
                                    am.DatePlaying,
                                    am.NumberOfPlayers,
                                    l.NAME AS Location,
                                    cy.NAME AS City,
                                    co.NAME AS Country,
                                    CASE
                                    WHEN am.matchdayid IS NULL THEN CAST(0 AS BIT)
                                    WHEN am.matchdayid IS NOT NULL THEN CAST(1 AS BIT)
                                    END                                      AS IsOrganized,
                                    Sum(ms.confirmed * 1)                    AS NumberOfConfirmedPlayers,
                                    CASE
                                    WHEN am.matchdayid IS NULL
                                            AND Sum(ms.confirmed * 1) = numberofplayers THEN CAST(1 AS BIT)
                                    ELSE CAST(0 AS BIT)
                                    END                                      AS CanBeOrganized,
                                    h.Name as HomeName,
                                    am.HomeGoals,
                                    a.Name as AwayName,
                                    am.AwayGoals
                            FROM   #allmatches am
                                    LEFT JOIN #allstatuses ast
                                            ON am.id = ast.matchdayid
                                    LEFT JOIN matchstatuses ms
                                            ON am.id = ms.matchdayid
                                    JOIN locations l
                                    ON l.id = am.locationid
                                    JOIN cities cy
                                    ON cy.id = l.cityid
                                    JOIN countries co
                                    ON co.id = l.countryid
                                    LEFT JOIN Teams h
                                    ON h.Id = am.HomeId
                                    LEFT JOIN Teams a
                                    ON a.Id = am.AwayId
                            GROUP  BY am.id,
                                        am.NAME,
                                        am.description,
                                        am.dateplaying,
                                        am.numberofplayers,
                                        am.locationid,
                                        am.groupid,
                                        am.matchdayid,
                                        ast.matchdayid,
                                        l.NAME,
                                        cy.NAME,
                                        co.NAME,
                                        h.Name,
                                        am.HomeGoals,
                                        a.Name,
                                        am.AwayGoals
                            HAVING am.DatePlaying <= GETDATE()
                            ORDER BY am.DatePlaying ASC

                            DROP TABLE #allmatches

                            DROP TABLE #allstatuses
                        END

                        ";
            var getLatestFiveMatchesForUserSP = @"USE [FootballApp]
                    GO
                    /****** Object:  StoredProcedure [dbo].[GetLatestFiveMatchesForUser]    Script Date: 28.7.2021. 22:59:08 ******/
                    SET ANSI_NULLS ON
                    GO
                    SET QUOTED_IDENTIFIER ON
                    GO

                    -- =============================================
                    -- Author:    Miloš Nikić
                    -- Create date: 12.6.2021.
                    -- Description:  Method is used to fetch all users latest 5 matches
                    -- =============================================
                    CREATE OR ALTER PROCEDURE [dbo].[GetLatestFiveMatchesForUser]
                    -- Add the parameters for the stored procedure here
                    @UserId INT
                    AS
                    BEGIN
                        -- SET NOCOUNT ON added to prevent extra result sets from
                        -- interfering with SELECT statements.
                        SET nocount ON;

                        SELECT *
                        INTO   #matchesplayed
                        FROM   (SELECT ht.MatchdayId as Id,
                                        mp.dateplayed,
                                        mp.homegoals,
                                        mp.awaygoals,
                                        home.NAME AS HomeName,
                                        away.NAME AS AwayName,
                                        home.id   AS HomeId,
                                        NULL      AS AwayId,
                                        ht.matchdayid,
                                        tms.Id as TeamMemberStatisticsId
                                FROM   matchplayeds mp
                                        JOIN teams ht
                                        ON ht.id = mp.homeid
                                        JOIN teammembers htm
                                        ON htm.teamid = mp.homeid
                                        JOIN TeamMemberStatistics tms
                                        ON htm.Id = tms.TeamMemberId
                                        JOIN users hu
                                        ON hu.id = htm.userid
                                        JOIN teams home
                                        ON home.id = mp.homeid
                                        JOIN teams away
                                        ON away.id = mp.awayid
                                WHERE  hu.id = @UserId
                                UNION
                                SELECT at.MatchdayId as Id,
                                        mp.dateplayed,
                                        mp.homegoals,
                                        mp.awaygoals,
                                        home.NAME AS HomeName,
                                        away.NAME AS AwayName,
                                        NULL      AS HomeId,
                                        away.id   AS AwayId,
                                        at.matchdayid,
                                        tms.Id as TeamMemberStatisticsId
                                FROM   matchplayeds mp
                                        JOIN teams at
                                        ON at.id = mp.awayid
                                        JOIN teammembers atm
                                        ON atm.teamid = mp.awayid
                                        JOIN TeamMemberStatistics tms
                                        ON atm.Id = tms.TeamMemberId
                                        JOIN users au
                                        ON au.id = atm.userid
                                        JOIN teams home
                                        ON home.id = mp.homeid
                                        JOIN teams away
                                        ON away.id = mp.awayid
                                WHERE  au.id = @UserId) AS x

                        SELECT TOP(5) mp.Id,
                                mp.DatePlayed,
                                CASE
                                WHEN mp.homegoals > mp.awaygoals
                                        AND mp.homeid IS NULL THEN 'LOSE'
                                WHEN mp.homegoals > mp.awaygoals
                                        AND mp.homeid IS NOT NULL THEN 'WIN'
                                WHEN mp.homegoals = mp.awaygoals THEN 'DRAW'
                                END                            AS Result,
                                HomeName,
                                AwayName,
                                Concat(co.NAME, ', ', cy.NAME, ', ', l.Name) AS Place,
                                tms.Goals,
                                tms.Assists,
                                tms.Rating
                        FROM   #matchesplayed mp
                                JOIN Matchdays md
                                ON md.Id = mp.Id
                                JOIN locations l
                                ON md.LocationId = l.id
                                JOIN countries co
                                ON co.id = l.countryid
                                JOIN cities cy
                                ON cy.id = l.cityid
                                JOIN TeamMemberStatistics tms
                                ON mp.TeamMemberStatisticsId = tms.Id
                        ORDER BY 
                                mp.DatePlayed DESC

                        DROP TABLE #matchesplayed
                    END

                    ";

            var getMatchHistoryForUserSP = @"USE [FootballApp]
GO
/****** Object:  StoredProcedure [dbo].[GetMatchHistoryForUser]    Script Date: 28.7.2021. 23:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:    Miloš Nikić
-- Create date: 12.6.2021.
-- Description:  Method is used to fetch all played matches for player
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetMatchHistoryForUser]
  -- Add the parameters for the stored procedure here
  @UserId INT
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET nocount ON;

      SELECT *
      INTO   #matchesplayed
      FROM   (SELECT ht.MatchdayId as Id,
                     mp.dateplayed,
                     mp.homegoals,
                     mp.awaygoals,
                     home.NAME AS HomeName,
                     away.NAME AS AwayName,
                     home.id   AS HomeId,
                     NULL      AS AwayId,
                     ht.matchdayid
              FROM   matchplayeds mp
                     JOIN teams ht
                       ON ht.id = mp.homeid
                     JOIN teammembers htm
                       ON htm.teamid = mp.homeid
                     JOIN users hu
                       ON hu.id = htm.userid
                     JOIN teams home
                       ON home.id = mp.homeid
                     JOIN teams away
                       ON away.id = mp.awayid
              WHERE  hu.id = @UserId
              UNION
              SELECT at.MatchdayId as Id,
                     mp.dateplayed,
                     mp.homegoals,
                     mp.awaygoals,
                     home.NAME AS HomeName,
                     away.NAME AS AwayName,
                     NULL      AS HomeId,
                     away.id   AS AwayId,
                     at.matchdayid
              FROM   matchplayeds mp
                     JOIN teams at
                       ON at.id = mp.awayid
                     JOIN teammembers atm
                       ON atm.teamid = mp.awayid
                     JOIN users au
                       ON au.id = atm.userid
                     JOIN teams home
                       ON home.id = mp.homeid
                     JOIN teams away
                       ON away.id = mp.awayid
              WHERE  au.id = @UserId) AS x

      SELECT mp.Id,
             mp.DatePlayed,
             CASE
               WHEN mp.homegoals > mp.awaygoals
                    AND mp.homeid IS NULL THEN 'LOSE'
               WHEN mp.homegoals > mp.awaygoals
                    AND mp.homeid IS NOT NULL THEN 'WIN'
               WHEN mp.homegoals = mp.awaygoals THEN 'DRAW'
             END                            AS Result,
             HomeName,
             AwayName,
             Concat(co.NAME, ', ', cy.NAME, ', ', l.Name) AS Place
      FROM   #matchesplayed mp
			 JOIN Matchdays md
			   ON md.Id = mp.Id
             JOIN locations l
               ON md.LocationId = l.id
             JOIN countries co
               ON co.id = l.countryid
             JOIN cities cy
               ON cy.id = l.cityid
	  ORDER BY 
			mp.DatePlayed ASC

      DROP TABLE #matchesplayed
  END

";
            var getOrganizedMatchInformationSP = @"USE [FootballApp]
GO
/****** Object:  StoredProcedure [dbo].[GetOrganizedMatchInformation]    Script Date: 28.7.2021. 23:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:    Miloš Nikić
-- Create date: 12.6.2021.
-- Description:  Method is used to fetch data for organized match
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetOrganizedMatchInformation]
  -- Add the parameters for the stored procedure here
  @MatchdayId INT
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET nocount ON;

      SELECT mp.Id,
             mp.DatePlayed,
             mp.HomeGoals,
             mp.AwayGoals,
             ht.Id AS HomeId,
			 null as AwayId,
			 ht.Name,
			 hu.Id as UserId,
             hu.Username,
			 hu.Firstname,
			 hu.Lastname,
			 hu.Created,
			 ph.Image,
             tms.Goals,
             tms.Assists,
             tms.MinutesPlayed,
             tms.Rating,
			 CONCAT(co.Name, ', ', cy.Name, ', ', l.Name ) as Place
      FROM   matchplayeds mp
             JOIN teams ht
               ON ht.id = mp.homeid
             JOIN teammembers htm
               ON htm.teamid = mp.homeid
             JOIN users hu
               ON hu.id = htm.userid
             JOIN teammemberstatistics tms
               ON tms.teammemberid = htm.id
                  AND tms.matchplayedid = mp.id
                  AND tms.teamid = ht.id
			 LEFT JOIN photos ph
				ON ph.UserId = hu.Id
				  AND ph.IsMain = 1
			 JOIN Matchdays md 
				ON md.Id = ht.MatchdayId
			 JOIN Locations l
				ON l.Id = md.LocationId
			 JOIN Cities cy
				ON cy.Id = l.CityId
			 JOIN Countries co 
				ON co.Id = l.CountryId
      WHERE  ht.matchdayid = @MatchdayId
      UNION
      SELECT mp.Id,
             mp.DatePlayed,
             mp.HomeGoals,
             mp.AwayGoals,
             null AS HomeId,
			 at.Id as AwayId,
			 at.Name,
			 au.Id as UserId,
             au.Username,
			 au.Firstname,
			 au.Lastname,
			 au.Created,
			 ph.Image,
             tms.Goals,
             tms.Assists,
             tms.MinutesPlayed,
             tms.Rating,
			 CONCAT(co.Name, ', ', cy.Name, ', ', l.Name ) as Place
      FROM   matchplayeds mp
             JOIN teams at
               ON at.id = mp.awayid
             JOIN teammembers atm
               ON atm.teamid = mp.awayid
             JOIN users au
               ON au.id = atm.userid
             JOIN teammemberstatistics tms
               ON tms.teammemberid = atm.id
                  AND tms.matchplayedid = mp.id
                  AND tms.teamid = at.id
			 LEFT JOIN photos ph
			   ON ph.UserId = au.Id
				  AND ph.IsMain = 1
			 JOIN Matchdays md 
				ON md.Id = at.MatchdayId
			 JOIN Locations l
				ON l.Id = md.LocationId
			 JOIN Cities cy
				ON cy.Id = l.CityId
			 JOIN Countries co 
				ON co.Id = l.CountryId
      WHERE  at.matchdayid = @MatchdayId
  END

";
            migrationBuilder.Sql(getAllMatchesForGroupSP);
            migrationBuilder.Sql(getLatestFiveMatchesForUserSP);
            migrationBuilder.Sql(getMatchHistoryForUserSP);
            migrationBuilder.Sql(getOrganizedMatchInformationSP);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropSP = "DROP PROC ";
            migrationBuilder.Sql(dropSP + "[dbo].[GetAllMatchesForGroup]");
            migrationBuilder.Sql(dropSP + "[dbo].[GetLatestFiveMatchesForUser]");
            migrationBuilder.Sql(dropSP + "[dbo].[GetMatchHistoryForUser]");
            migrationBuilder.Sql(dropSP + "[dbo].[GetOrganizedMatchInformation]");
        }
    }
}
