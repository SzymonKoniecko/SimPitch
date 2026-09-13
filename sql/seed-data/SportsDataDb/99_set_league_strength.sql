USE SportsDataDb;
BEGIN TRANSACTION;
BEGIN TRY
WITH LeagueSeason AS (
    SELECT l.Id AS LeagueId, l.Name AS LeagueName, s.SeasonYear
    FROM SportsDataDb.dbo.League l
    CROSS JOIN (VALUES
        ('2026/2027'), ('2025/2026'), ('2024/2025'), ('2023/2024'),
        ('2022/2023'), ('2021/2022')
    ) AS s(SeasonYear)
    WHERE l.Name IN (
        'PKO BP Ekstraklasa',
        'Premier League',
        'La Liga',
        'Serie A',
        'Bundesliga',
        'Betclic 1 Liga',
        'UEFA Conference League'
    )
),
Stats AS (
    -- Total matches for the league/season is derived from SeasonStats itself
    -- (SUM(MatchesPlayed) counts every team's played matches, so dividing by
    -- 2 gives the actual match count) rather than requiring granular
    -- MatchRound/LeagueRound rows to exist. Most historical seasons only
    -- have the final SeasonStats table seeded, with no match-by-match data,
    -- so requiring MatchRound rows here silently dropped them.
    SELECT
        ls.LeagueId,
        ls.SeasonYear,
        CAST(SUM(ss.GoalsFor) AS DECIMAL(10,4)) /
        CAST(SUM(ss.MatchesPlayed) / 2.0 AS DECIMAL(10,4)) AS Strength
    FROM LeagueSeason ls
    JOIN SportsDataDb.dbo.SeasonStats ss
        ON ss.LeagueId = ls.LeagueId
        AND ss.SeasonYear = ls.SeasonYear
    GROUP BY ls.LeagueId, ls.SeasonYear
    HAVING SUM(ss.MatchesPlayed) > 0
)

INSERT INTO SportsDataDb.dbo.LeagueStrength (Id, LeagueId, SeasonYear, Strength)
SELECT NewID(), s.LeagueId, s.SeasonYear, s.Strength
FROM Stats s
LEFT JOIN SportsDataDb.dbo.LeagueStrength ls
    ON ls.LeagueId = s.LeagueId
    AND ls.SeasonYear = s.SeasonYear
WHERE ls.LeagueId IS NULL; -- tylko brakujące


COMMIT TRANSACTION;
PRINT 'League strengths calculated';

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error occurred while calculating league strengths.';
    PRINT ERROR_MESSAGE();
END CATCH;
