USE SportsDataDb
DECLARE 
    @CountryId1 UNIQUEIDENTIFIER = NEWID(), @CountryId2 UNIQUEIDENTIFIER = NEWID(),
    @CountryId3 UNIQUEIDENTIFIER = NEWID(), @CountryId4 UNIQUEIDENTIFIER = NEWID(),
    @CountryId5 UNIQUEIDENTIFIER = NEWID(),
    @LeagueId1 UNIQUEIDENTIFIER = NEWID(), @LeagueId2 UNIQUEIDENTIFIER = NEWID(),
    @LeagueId3 UNIQUEIDENTIFIER = NEWID(), @LeagueId4 UNIQUEIDENTIFIER = NEWID(),
    @LeagueId5 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId2 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId3 UNIQUEIDENTIFIER = NEWID(), @StadiumId4 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId5 UNIQUEIDENTIFIER = NEWID(),
    @TeamId2 UNIQUEIDENTIFIER = NEWID(),
    @TeamId3 UNIQUEIDENTIFIER = NEWID(), @TeamId4 UNIQUEIDENTIFIER = NEWID(),
    @TeamId5 UNIQUEIDENTIFIER = NEWID(),
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
    INSERT INTO SportsDataDb.dbo.League (Id, [Name], CountryId, [Sport], MaxRound, CreatedAt, UpdatedAt)
    VALUES
        (@LeagueId1, 'PKO BP Ekstraklasa', @CountryId1, 'Football', 34, @CurrentDateTime, @CurrentDateTime),
        (@LeagueId2, 'Bundesliga', @CountryId2, 'Football', 34, @CurrentDateTime, @CurrentDateTime),
        (@LeagueId3, 'La Liga', @CountryId3, 'Football', 38, @CurrentDateTime, @CurrentDateTime),
        (@LeagueId4, 'Premier League', @CountryId4, 'Football', 38, @CurrentDateTime, @CurrentDateTime),
        (@LeagueId5, 'Serie A', @CountryId5, 'Football', 38, @CurrentDateTime, @CurrentDateTime)

    COMMIT TRANSACTION
    PRINT 'Data has been successfully inserted into all tables.'

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT 'An error occurred while inserting data.'
END CATCH
