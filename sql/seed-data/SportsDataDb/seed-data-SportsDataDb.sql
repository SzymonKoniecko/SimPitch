USE SportsDataDb
DECLARE 
    @CountryId1 UNIQUEIDENTIFIER = NEWID(), @CountryId2 UNIQUEIDENTIFIER = NEWID(),
    @CountryId3 UNIQUEIDENTIFIER = NEWID(), @CountryId4 UNIQUEIDENTIFIER = NEWID(),
    @CountryId5 UNIQUEIDENTIFIER = NEWID(),
    @LeagueId1 UNIQUEIDENTIFIER = NEWID(), @LeagueId2 UNIQUEIDENTIFIER = NEWID(),
    @LeagueId3 UNIQUEIDENTIFIER = NEWID(), @LeagueId4 UNIQUEIDENTIFIER = NEWID(),
    @LeagueId5 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId1 UNIQUEIDENTIFIER = NEWID(), @StadiumId2 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId3 UNIQUEIDENTIFIER = NEWID(), @StadiumId4 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId5 UNIQUEIDENTIFIER = NEWID(), @StadiumId6 UNIQUEIDENTIFIER = NEWID(),
    @TeamId1 UNIQUEIDENTIFIER = NEWID(), @TeamId2 UNIQUEIDENTIFIER = NEWID(),
    @TeamId3 UNIQUEIDENTIFIER = NEWID(), @TeamId4 UNIQUEIDENTIFIER = NEWID(),
    @TeamId5 UNIQUEIDENTIFIER = NEWID(), @TeamId6 UNIQUEIDENTIFIER = NEWID(),
    @CurrentDateTime DATETIME2 = GETDATE()

BEGIN TRANSACTION

BEGIN TRY
    -- Insert into Country
    INSERT INTO SportsDataDb.dbo.Country (Id, [Name], [Code], CreatedAt, UpdatedAt)
    VALUES
        (@CountryId1, 'Poland', 'PL', @CurrentDateTime, @CurrentDateTime),
        (@CountryId2, 'Germany', 'DE', @CurrentDateTime, @CurrentDateTime),
        (@CountryId3, 'Spain', 'ES', @CurrentDateTime, @CurrentDateTime),
        (@CountryId4, 'England', 'EN', @CurrentDateTime, @CurrentDateTime),
        (@CountryId5, 'Italy', 'IT', @CurrentDateTime, @CurrentDateTime)

    -- Insert into League
    INSERT INTO SportsDataDb.dbo.League (Id, [Name], CountryId, [Sport], CreatedAt, UpdatedAt)
    VALUES
        (@LeagueId1, 'PKO BP Ekstraklasa', @CountryId1, 'Football', @CurrentDateTime, @CurrentDateTime),
        (@LeagueId2, 'Bundesliga', @CountryId2, 'Football', @CurrentDateTime, @CurrentDateTime),
        (@LeagueId3, 'La Liga', @CountryId3, 'Football', @CurrentDateTime, @CurrentDateTime),
        (@LeagueId4, 'Premier League', @CountryId4, 'Football', @CurrentDateTime, @CurrentDateTime),
        (@LeagueId5, 'Serie A', @CountryId5, 'Football', @CurrentDateTime, @CurrentDateTime)

    -- Insert into Stadium
    INSERT INTO SportsDataDb.dbo.Stadium (Id, [Name], Capacity, CreatedAt, UpdatedAt)
    VALUES
        (@StadiumId1, 'Municipal Stadium in Białystok', 22372, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId2, 'Allianz Arena', 75024, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId3, 'Spotify Camp Nou', 99354, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId4, 'Old Trafford', 74310, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId5, 'San Siro', 80018, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId6, 'Marshal Józef Piłsudski Municipal Stadium of Legia Warsaw', 31103, @CurrentDateTime, @CurrentDateTime)

    -- Insert into Team
    INSERT INTO SportsDataDb.dbo.Team (Id, [Name], CountryId, StadiumId, LeagueId, LogoUrl, ShortName, Sport)
    VALUES
        (@TeamId1, 'Jagiellonia Białystok', @CountryId1, @StadiumId1, @LeagueId1, 'http://example.com/jaga.png', 'JAG', 'Football'),
        (@TeamId2, 'Bayern Munich', @CountryId2, @StadiumId2, @LeagueId2, 'http://example.com/bayern.png', 'FCB', 'Football'),
        (@TeamId3, 'FC Barcelona', @CountryId3, @StadiumId3, @LeagueId3, 'http://example.com/barca.png', 'BAR', 'Football'),
        (@TeamId4, 'Manchester United', @CountryId4, @StadiumId4, @LeagueId4, 'http://example.com/manu.png', 'MUN', 'Football'),
        (@TeamId5, 'AC Milan', @CountryId5, @StadiumId5, @LeagueId5, 'http://example.com/milan.png', 'ACM', 'Football'),
        (@TeamId6, 'Legia Warsaw', @CountryId1, @StadiumId6, @LeagueId1, 'http://example.com/legia.png', 'LEG', 'Football')

    -- Insert into FootballSeasonStats
    INSERT INTO SportsDataDb.dbo.FootballSeasonStats (Id, TeamId, SeasonYear, LeagueId, MatchesPlayed, Wins, Losses, Draws, GoalsFor, GoalsAgainst, CreatedAt, UpdatedAt)
    VALUES
        (NEWID(), @TeamId1, '2023/2024', @LeagueId1, 24, 20, 3, 1, 55, 19, @CurrentDateTime, @CurrentDateTime),
        (NEWID(), @TeamId2, '2023/2024', @LeagueId2, 34, 25, 4, 5, 92, 28, @CurrentDateTime, @CurrentDateTime),
        (NEWID(), @TeamId3, '2023/2024', @LeagueId3, 38, 28, 4, 6, 85, 20, @CurrentDateTime, @CurrentDateTime),
        (NEWID(), @TeamId4, '2023/2024', @LeagueId4, 38, 22, 9, 7, 73, 42, @CurrentDateTime, @CurrentDateTime),
        (NEWID(), @TeamId5, '2023/2024', @LeagueId5, 38, 24, 7, 7, 68, 35, @CurrentDateTime, @CurrentDateTime),
        (NEWID(), @TeamId6, '2023/2024', @LeagueId1, 38, 24, 7, 7, 68, 35, @CurrentDateTime, @CurrentDateTime)

    COMMIT TRANSACTION
    PRINT 'Data has been successfully inserted into all tables.'

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT 'An error occurred while inserting data.'
END CATCH
