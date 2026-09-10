-- ================================================================
-- EKSTRAKLASA 2025/2026 - SEASON STATS (computed from played matches)
-- Description: Aggregates dbo.MatchRound results for the 2025/2026
--   season (only matches with IsPlayed = 1 are counted, so this
--   reflects the season up to whatever round was actually seeded)
--   and inserts one SeasonStats row per team.
-- Created: 10.09.2026
-- ================================================================

USE SportsDataDb;

DECLARE @LeagueId UNIQUEIDENTIFIER;

SELECT @LeagueId = Id
FROM dbo.League
WHERE [Name] = 'PKO BP Ekstraklasa'

BEGIN TRANSACTION

BEGIN TRY

    IF NOT EXISTS (SELECT 1 FROM dbo.SeasonStats WHERE LeagueId = @LeagueId AND SeasonYear = '2025/2026')
    BEGIN

        ;WITH TeamMatches AS (
            SELECT
                mr.HomeTeamId AS TeamId,
                mr.HomeGoals AS GoalsFor,
                mr.AwayGoals AS GoalsAgainst,
                CASE WHEN mr.HomeGoals > mr.AwayGoals THEN 1 ELSE 0 END AS Win,
                CASE WHEN mr.HomeGoals < mr.AwayGoals THEN 1 ELSE 0 END AS Loss,
                mr.IsDraw
            FROM dbo.MatchRound mr
            JOIN dbo.LeagueRound lr ON lr.Id = mr.RoundId
            WHERE lr.LeagueId = @LeagueId AND lr.SeasonYear = '2025/2026' AND mr.IsPlayed = 1

            UNION ALL

            SELECT
                mr.AwayTeamId AS TeamId,
                mr.AwayGoals AS GoalsFor,
                mr.HomeGoals AS GoalsAgainst,
                CASE WHEN mr.AwayGoals > mr.HomeGoals THEN 1 ELSE 0 END AS Win,
                CASE WHEN mr.AwayGoals < mr.HomeGoals THEN 1 ELSE 0 END AS Loss,
                mr.IsDraw
            FROM dbo.MatchRound mr
            JOIN dbo.LeagueRound lr ON lr.Id = mr.RoundId
            WHERE lr.LeagueId = @LeagueId AND lr.SeasonYear = '2025/2026' AND mr.IsPlayed = 1
        )

        INSERT INTO dbo.SeasonStats (Id, TeamId, SeasonYear, LeagueId, MatchesPlayed, Wins, Losses, Draws, GoalsFor, GoalsAgainst)
        SELECT
            NEWID(),
            TeamId,
            '2025/2026',
            @LeagueId,
            COUNT(*),
            SUM(Win),
            SUM(Loss),
            SUM(CAST(IsDraw AS INT)),
            SUM(GoalsFor),
            SUM(GoalsAgainst)
        FROM TeamMatches
        GROUP BY TeamId

    END

    COMMIT TRANSACTION
    PRINT '✅ SUKCES! Ekstraklasa 2025/2026 - season stats computed from played matches'

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT '❌ Cannot insert! Ekstraklasa 2025/2026 season stats'
    PRINT 'Error: ' + ERROR_MESSAGE()
END CATCH
