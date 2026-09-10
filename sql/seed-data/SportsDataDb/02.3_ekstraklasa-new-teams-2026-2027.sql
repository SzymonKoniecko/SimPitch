-- ================================================================
-- EKSTRAKLASA 2026/2027 - NEW TEAMS
-- Description: Adds teams promoted/new for 2026/2027 that are missing
--   from the Team table (checked by name, so safe to re-run).
-- Created: 10.09.2026
-- ================================================================

USE SportsDataDb

DECLARE
    @CountryId UNIQUEIDENTIFIER,
    @LeagueId UNIQUEIDENTIFIER,

    @StadiumIdWislaKrakow UNIQUEIDENTIFIER = NEWID(),
    @StadiumIdWieczysta UNIQUEIDENTIFIER = NEWID(),

    @TeamIdWislaKrakow UNIQUEIDENTIFIER = NEWID(),
    @TeamIdWieczysta UNIQUEIDENTIFIER = NEWID();

SELECT
    @CountryId = Id
FROM dbo.Country
WHERE [Code] = 'PL'

SELECT
    @LeagueId = Id
FROM dbo.League
WHERE [Name] = 'PKO BP Ekstraklasa' AND CountryId = @CountryId

BEGIN TRANSACTION

BEGIN TRY

    -- ================================================================
    -- STADIUMS
    -- ================================================================

    IF NOT EXISTS (SELECT 1 FROM dbo.Stadium WHERE [Name] = 'Stadion Miejski im. Henryka Reymana')
    INSERT INTO dbo.Stadium (Id, [Name], Capacity)
        VALUES (@StadiumIdWislaKrakow, 'Stadion Miejski im. Henryka Reymana', 33326)

    IF NOT EXISTS (SELECT 1 FROM dbo.Stadium WHERE [Name] = 'Stadion Wieczystej Kraków')
    INSERT INTO dbo.Stadium (Id, [Name], Capacity)
        VALUES (@StadiumIdWieczysta, 'Stadion Wieczystej Kraków', 2207)

    -- Re-fetch ids in case the stadiums already existed from a previous run
    SELECT @StadiumIdWislaKrakow = Id FROM dbo.Stadium WHERE [Name] = 'Stadion Miejski im. Henryka Reymana'
    SELECT @StadiumIdWieczysta = Id FROM dbo.Stadium WHERE [Name] = 'Stadion Wieczystej Kraków'

    -- ================================================================
    -- TEAMS
    -- ================================================================

    IF NOT EXISTS (SELECT 1 FROM dbo.Team WHERE [Name] = 'Wisła Kraków')
    INSERT INTO dbo.Team (Id, [Name], CountryId, StadiumId, ShortName)
        VALUES (@TeamIdWislaKrakow, 'Wisła Kraków', @CountryId, @StadiumIdWislaKrakow, 'WIS')

    IF NOT EXISTS (SELECT 1 FROM dbo.Team WHERE [Name] = 'Wieczysta Kraków')
    INSERT INTO dbo.Team (Id, [Name], CountryId, StadiumId, ShortName)
        VALUES (@TeamIdWieczysta, 'Wieczysta Kraków', @CountryId, @StadiumIdWieczysta, 'WIE')

    -- Re-fetch ids in case the teams already existed from a previous run
    SELECT @TeamIdWislaKrakow = Id FROM dbo.Team WHERE [Name] = 'Wisła Kraków'
    SELECT @TeamIdWieczysta = Id FROM dbo.Team WHERE [Name] = 'Wieczysta Kraków'

    -- ================================================================
    -- COMPETITION MEMBERSHIP (2026/2027)
    -- ================================================================

    IF NOT EXISTS (SELECT 1 FROM dbo.CompetitionMembership WHERE TeamId = @TeamIdWislaKrakow AND LeagueId = @LeagueId AND SeasonYear = '2026/2027')
    INSERT INTO dbo.CompetitionMembership (Id, TeamId, LeagueId, SeasonYear)
        VALUES (NEWID(), @TeamIdWislaKrakow, @LeagueId, '2026/2027')

    IF NOT EXISTS (SELECT 1 FROM dbo.CompetitionMembership WHERE TeamId = @TeamIdWieczysta AND LeagueId = @LeagueId AND SeasonYear = '2026/2027')
    INSERT INTO dbo.CompetitionMembership (Id, TeamId, LeagueId, SeasonYear)
        VALUES (NEWID(), @TeamIdWieczysta, @LeagueId, '2026/2027')

    COMMIT TRANSACTION
    PRINT '✅ SUKCES! Ekstraklasa 2026/2027 - new teams added (Wisła Kraków, Wieczysta Kraków)'

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT '❌ Cannot insert! Ekstraklasa 2026/2027 new teams'
    PRINT 'Error: ' + ERROR_MESSAGE()
END CATCH
