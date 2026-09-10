-- ================================================================
-- UEFA CONFERENCE LEAGUE 2026/2027 - ROUNDS ONLY (League phase, 1-6)
-- Description: Creates the LeagueRound rows for the new season.
--   No MatchRound (fixtures) are inserted here — the real league-phase
--   draw/fixture list for 2026/2027 was not available at authoring time.
--   Fixtures/results should be added in a follow-up file once known.
-- Created: 10.09.2026
-- ================================================================

USE SportsDataDb;
GO

BEGIN TRANSACTION;
BEGIN TRY

    DECLARE @LeagueId UNIQUEIDENTIFIER;
    DECLARE @SeasonYear NVARCHAR(20) = '2026/2027';

    SELECT TOP 1 @LeagueId = Id FROM dbo.League WHERE [Name] = 'UEFA Conference League';

    IF @LeagueId IS NULL
    BEGIN
        THROW 50000, 'League ''UEFA Conference League'' not found. Please run the initial seed scripts.', 1;
    END

    MERGE dbo.LeagueRound AS target
    USING (VALUES (1), (2), (3), (4), (5), (6)) AS source(RoundNum)
    ON target.LeagueId = @LeagueId AND target.SeasonYear = @SeasonYear AND target.Round = source.RoundNum
    WHEN NOT MATCHED THEN
        INSERT (Id, LeagueId, SeasonYear, Round) VALUES (NEWID(), @LeagueId, @SeasonYear, source.RoundNum);

    COMMIT TRANSACTION;
    PRINT '✅ SUKCES! UEFA Conference League 2026/2027 - rounds 1-6 created';
    PRINT '   - fixtures (MatchRound) not seeded yet, add in a follow-up file';

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT '❌ Cannot insert! UEFA Conference League 2026/2027';
    PRINT 'Error: ' + ERROR_MESSAGE();
END CATCH
