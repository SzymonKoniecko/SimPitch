USE SportsDataDb
DECLARE 
    @CountryId1 UNIQUEIDENTIFIER = NEWID(), @CountryId2 UNIQUEIDENTIFIER = NEWID(),
    @CountryId3 UNIQUEIDENTIFIER = NEWID(), @CountryId4 UNIQUEIDENTIFIER = NEWID(),
    @CountryId5 UNIQUEIDENTIFIER = NEWID(),
    @LeagueId1 UNIQUEIDENTIFIER = NEWID(), @LeagueId2 UNIQUEIDENTIFIER = NEWID(),
    @LeagueId3 UNIQUEIDENTIFIER = NEWID(), @LeagueId4 UNIQUEIDENTIFIER = NEWID(),
    @LeagueId5 UNIQUEIDENTIFIER = NEWID(), @LeagueId6 UNIQUEIDENTIFIER = NEWID(),


    @CurrentDateTime DATETIME2 = GETDATE()

BEGIN TRANSACTION

BEGIN TRY
    -- Insert into Country
    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.Country)
    INSERT INTO SportsDataDb.dbo.Country (Id, [Name], [Code])
    VALUES
        (@CountryId1, 'Poland', 'PL'),
        (@CountryId2, 'Germany', 'DE'),
        (@CountryId3, 'Spain', 'ES'),
        (@CountryId4, 'England', 'EN'),
        (@CountryId5, 'Italy', 'IT');

    -- Insert into League
    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.League)
    INSERT INTO SportsDataDb.dbo.League (Id, [Name], CountryId, MaxRound, Strength)
    VALUES
        (@LeagueId1, 'PKO BP Ekstraklasa', @CountryId1, 34, 1),
        (@LeagueId2, 'Bundesliga', @CountryId2, 34, 1),
        (@LeagueId3, 'La Liga', @CountryId3, 38, 1),
        (@LeagueId4, 'Premier League', @CountryId4, 38, 1),
        (@LeagueId5, 'Serie A', @CountryId5, 38, 1),
        (@LeagueId6, 'Betclic 1 Liga', @CountryId1, 34, 0.75);

    COMMIT TRANSACTION
    PRINT 'Data has been successfully inserted into all tables.'

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT 'An error occurred while inserting data.'
END CATCH
