-- ================================================================
-- EKSTRAKLASA 2026/2027 - ROUNDS ONLY (1-34)
-- Description: Creates the LeagueRound rows for the new season.
--   No MatchRound (fixtures) are inserted here — the real fixture list
--   for 2026/2027 was not available at authoring time. Fixtures/results
--   should be added in a follow-up file once known.
-- Created: 10.09.2026
-- ================================================================

USE SportsDataDb;

DECLARE
    @CountryId UNIQUEIDENTIFIER,
    @LeagueId UNIQUEIDENTIFIER,

    @RoundId1 UNIQUEIDENTIFIER = NEWID(),
    @RoundId2 UNIQUEIDENTIFIER = NEWID(),
    @RoundId3 UNIQUEIDENTIFIER = NEWID(),
    @RoundId4 UNIQUEIDENTIFIER = NEWID(),
    @RoundId5 UNIQUEIDENTIFIER = NEWID(),
    @RoundId6 UNIQUEIDENTIFIER = NEWID(),
    @RoundId7 UNIQUEIDENTIFIER = NEWID(),
    @RoundId8 UNIQUEIDENTIFIER = NEWID(),
    @RoundId9 UNIQUEIDENTIFIER = NEWID(),
    @RoundId10 UNIQUEIDENTIFIER = NEWID(),
    @RoundId11 UNIQUEIDENTIFIER = NEWID(),
    @RoundId12 UNIQUEIDENTIFIER = NEWID(),
    @RoundId13 UNIQUEIDENTIFIER = NEWID(),
    @RoundId14 UNIQUEIDENTIFIER = NEWID(),
    @RoundId15 UNIQUEIDENTIFIER = NEWID(),
    @RoundId16 UNIQUEIDENTIFIER = NEWID(),
    @RoundId17 UNIQUEIDENTIFIER = NEWID(),
    @RoundId18 UNIQUEIDENTIFIER = NEWID(),
    @RoundId19 UNIQUEIDENTIFIER = NEWID(),
    @RoundId20 UNIQUEIDENTIFIER = NEWID(),
    @RoundId21 UNIQUEIDENTIFIER = NEWID(),
    @RoundId22 UNIQUEIDENTIFIER = NEWID(),
    @RoundId23 UNIQUEIDENTIFIER = NEWID(),
    @RoundId24 UNIQUEIDENTIFIER = NEWID(),
    @RoundId25 UNIQUEIDENTIFIER = NEWID(),
    @RoundId26 UNIQUEIDENTIFIER = NEWID(),
    @RoundId27 UNIQUEIDENTIFIER = NEWID(),
    @RoundId28 UNIQUEIDENTIFIER = NEWID(),
    @RoundId29 UNIQUEIDENTIFIER = NEWID(),
    @RoundId30 UNIQUEIDENTIFIER = NEWID(),
    @RoundId31 UNIQUEIDENTIFIER = NEWID(),
    @RoundId32 UNIQUEIDENTIFIER = NEWID(),
    @RoundId33 UNIQUEIDENTIFIER = NEWID(),
    @RoundId34 UNIQUEIDENTIFIER = NEWID();

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
    -- TWORZENIE KOLEJEK (LeagueRound)
    -- ================================================================

    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.LeagueRound WHERE SeasonYear = '2026/2027' AND LeagueId = @LeagueId)
    INSERT INTO dbo.LeagueRound (Id, LeagueId, SeasonYear, Round)
        VALUES
            (@RoundId1, @LeagueId, '2026/2027', 1),
            (@RoundId2, @LeagueId, '2026/2027', 2),
            (@RoundId3, @LeagueId, '2026/2027', 3),
            (@RoundId4, @LeagueId, '2026/2027', 4),
            (@RoundId5, @LeagueId, '2026/2027', 5),
            (@RoundId6, @LeagueId, '2026/2027', 6),
            (@RoundId7, @LeagueId, '2026/2027', 7),
            (@RoundId8, @LeagueId, '2026/2027', 8),
            (@RoundId9, @LeagueId, '2026/2027', 9),
            (@RoundId10, @LeagueId, '2026/2027', 10),
            (@RoundId11, @LeagueId, '2026/2027', 11),
            (@RoundId12, @LeagueId, '2026/2027', 12),
            (@RoundId13, @LeagueId, '2026/2027', 13),
            (@RoundId14, @LeagueId, '2026/2027', 14),
            (@RoundId15, @LeagueId, '2026/2027', 15),
            (@RoundId16, @LeagueId, '2026/2027', 16),
            (@RoundId17, @LeagueId, '2026/2027', 17),
            (@RoundId18, @LeagueId, '2026/2027', 18),
            (@RoundId19, @LeagueId, '2026/2027', 19),
            (@RoundId20, @LeagueId, '2026/2027', 20),
            (@RoundId21, @LeagueId, '2026/2027', 21),
            (@RoundId22, @LeagueId, '2026/2027', 22),
            (@RoundId23, @LeagueId, '2026/2027', 23),
            (@RoundId24, @LeagueId, '2026/2027', 24),
            (@RoundId25, @LeagueId, '2026/2027', 25),
            (@RoundId26, @LeagueId, '2026/2027', 26),
            (@RoundId27, @LeagueId, '2026/2027', 27),
            (@RoundId28, @LeagueId, '2026/2027', 28),
            (@RoundId29, @LeagueId, '2026/2027', 29),
            (@RoundId30, @LeagueId, '2026/2027', 30),
            (@RoundId31, @LeagueId, '2026/2027', 31),
            (@RoundId32, @LeagueId, '2026/2027', 32),
            (@RoundId33, @LeagueId, '2026/2027', 33),
            (@RoundId34, @LeagueId, '2026/2027', 34)

    COMMIT TRANSACTION
    PRINT '✅ SUKCES! Ekstraklasa 2026/2027 - rounds 1-34 created'
    PRINT '   - fixtures (MatchRound) not seeded yet, add in a follow-up file'

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT '❌ Cannot insert! Ekstraklasa 2026/2027'
    PRINT 'Error: ' + ERROR_MESSAGE()
END CATCH
