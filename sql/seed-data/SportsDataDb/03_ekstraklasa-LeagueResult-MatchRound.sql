USE SportsDataDb;

DECLARE 
    @CountryId UNIQUEIDENTIFIER, 
    @LeagueId UNIQUEIDENTIFIER,
    @LeagueId1 UNIQUEIDENTIFIER,
    @CurrentDateTime DATETIME2 = GETDATE(),

    @TeamId1 UNIQUEIDENTIFIER = 'e4b7d5c8-9f6a-4d2e-8a3c-1b0f8c0f4d2a',   -- Jaga
    @TeamId2 UNIQUEIDENTIFIER = 'a6c9f7d1-2b34-4e9c-8f13-0d7a2e5b1c9f',   -- Legia
    @TeamId3 UNIQUEIDENTIFIER = 'b9f8d3a7-1e6c-4a2b-9f4d-3e0a1c8f7d2b',   -- Lech
    @TeamId4 UNIQUEIDENTIFIER = 'd0e7f9b8-3a1c-4f6d-9b5a-7c2e8f0a1d3b',   -- Widzew Łódź
    @TeamId5 UNIQUEIDENTIFIER = 'f1a3c5d7-6e8b-4f1c-8b0d-5e2f3a7c9b1d',   -- Rakow
    @TeamId6 UNIQUEIDENTIFIER = 'c3d9f6a2-8b4e-4a7f-9c2e-1d0a5f3b7e8c',   -- Pogon
    @TeamId7 UNIQUEIDENTIFIER = 'a2f8c5d9-7b1e-4c3d-8a9f-2e0d7c1b5a6f',   -- Cracovia
    @TeamId8 UNIQUEIDENTIFIER = 'b7e4d1a3-9c6f-4e0a-8b2d-3f1c5e7a9d8b',   -- Górnik
    @TeamId9 UNIQUEIDENTIFIER = 'd9a7f8b3-2e0c-4d1f-9a5b-6c3e8f7d0a1b',   -- Wisla Plock
    @TeamId10 UNIQUEIDENTIFIER = 'f0c3e9a7-8d4b-4f6c-1a2e-7b5d9f0c3a8e',  -- Lechia
    @TeamId11 UNIQUEIDENTIFIER = 'e8b2d7f3-1a4c-4e9f-8b5d-0c7f1a6e3d9b',  -- Radomiak
    @TeamId12 UNIQUEIDENTIFIER = 'c5a9e3b1-7f8d-4a0c-9e2b-3d1f6c7a8e0b',  -- Motor Lublin
    @TeamId13 UNIQUEIDENTIFIER = 'b0f6c1d9-3e7a-4b2d-8f5c-9a1e0d3b7c4f',  -- GKS
    @TeamId14 UNIQUEIDENTIFIER = 'd7a3e9f0-5c1b-4f8d-9a2e-6b0c7f4d1e8a',  -- Zaglebie Lubin
    @TeamId15 UNIQUEIDENTIFIER = 'f4c1b7a9-8d0e-4a3f-9c5b-2e7d1f6a0c3b',  -- Korona
    @TeamId16 UNIQUEIDENTIFIER = 'a9e2d5f7-1b3c-4e0a-8f6d-7c0b9a1e3d5f',  -- Piast
    @TeamId17 UNIQUEIDENTIFIER = 'c0d7f8a1-4b2e-4c9f-8a5d-1e3b6f0c7d9a',  -- Termalika
    @TeamId18 UNIQUEIDENTIFIER = 'e1b9c3d7-6f0a-4d2e-9b5c-3a7f8e1d0b6c',  -- Arka

    @TeamId19 UNIQUEIDENTIFIER = '8a46089c-f7aa-4270-9742-21a84ec92460',  -- Stal Mielec
    @TeamId20 UNIQUEIDENTIFIER = '4138333e-69dd-41fb-ad30-47bf2b0e4c31',  -- Slask Wroclaw
    @TeamId21 UNIQUEIDENTIFIER = '0bc33659-7471-4fae-945f-f24f60a38ae0',   -- Puszcza Niepolomice

    @RoundId1 UNIQUEIDENTIFIER = NEWID(),
    @RoundId2 UNIQUEIDENTIFIER = NEWID(),
    @RoundId3 UNIQUEIDENTIFIER = NEWID(),
    @RoundId4 UNIQUEIDENTIFIER = NEWID(),
    @RoundId5 UNIQUEIDENTIFIER = NEWID(),
    @RoundId6 UNIQUEIDENTIFIER = NEWID();

SELECT 
    @CountryId = Id
FROM dbo.Country
WHERE [Code] = 'PL'

SELECT
    @LeagueId = Id
FROM dbo.League
WHERE [Name] = 'PKO BP Ekstraklasa' AND CountryId = @CountryId

SELECT
    @LeagueId1 = Id
FROM dbo.League
WHERE [Name] = 'Betclic 1 Liga' AND CountryId = @CountryId
BEGIN TRANSACTION

BEGIN TRY

    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.LeagueRound)
    INSERT INTO dbo.LeagueRound (Id, LeagueId, SeasonYear, Round)
        VALUES (@RoundId1, @LeagueId, '2025/2026', 1),
            (@RoundId2, @LeagueId, '2025/2026', 2),
            (@RoundId3, @LeagueId, '2025/2026', 3),
            (@RoundId4, @LeagueId, '2025/2026', 4),
            (@RoundId5, @LeagueId, '2025/2026', 5),
            (@RoundId6, @LeagueId, '2025/2026', 6)

    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.MatchRound)
    INSERT INTO dbo.MatchRound (Id, RoundId, HomeTeamId, AwayTeamId, HomeGoals, AwayGoals, IsDraw, IsPlayed)
        VALUES  
        (NEWID(), @RoundId1, @TeamId1, @TeamId17, 0, 4, 0, 1),
        (NEWID(), @RoundId1, @TeamId3, @TeamId7, 1, 4, 0, 1),
        (NEWID(), @RoundId1, @TeamId4, @TeamId14, 1, 0, 0, 1),
        (NEWID(), @RoundId1, @TeamId9, @TeamId15, 2, 0, 0, 1),
        (NEWID(), @RoundId1, @TeamId13, @TeamId5, 0, 1, 0, 1),
        (NEWID(), @RoundId1, @TeamId8, @TeamId10, 2, 1, 0, 1),
        (NEWID(), @RoundId1, @TeamId12, @TeamId18, 1, 0, 0, 1),
        (NEWID(), @RoundId1, @TeamId11, @TeamId6, 5, 1, 0, 1),
        (NEWID(), @RoundId1, @TeamId2, @TeamId16, null, null, null, 0), -- Legia vs Piast

        (NEWID(), @RoundId2, @TeamId7, @TeamId17, 2, 0, 0, 1),
        (NEWID(), @RoundId2, @TeamId18, @TeamId11, 1, 1, 1, 1),
        (NEWID(), @RoundId2, @TeamId16, @TeamId8, 0, 1, 0, 1),
        (NEWID(), @RoundId2, @TeamId6, @TeamId12, 4, 1, 0, 1),
        (NEWID(), @RoundId2, @TeamId10, @TeamId3, 3, 4, 0, 1),
        (NEWID(), @RoundId2, @TeamId5, @TeamId9, 1, 2, 0, 1),
        (NEWID(), @RoundId2, @TeamId1, @TeamId4, 3, 2, 0, 1),
        (NEWID(), @RoundId2, @TeamId15, @TeamId2, 0, 2, 0, 1),
        (NEWID(), @RoundId2, @TeamId13, @TeamId14, 2, 2, 1, 1),

        (NEWID(), @RoundId3, @TeamId14, @TeamId15, 1, 1, 1, 1),
        (NEWID(), @RoundId3, @TeamId9, @TeamId16, 2, 0, 0, 1),
        (NEWID(), @RoundId3, @TeamId17, @TeamId6, 1, 1, 1, 1),
        (NEWID(), @RoundId3, @TeamId4, @TeamId13, 3, 0, 0, 1),
        (NEWID(), @RoundId3, @TeamId3, @TeamId8, 2, 1, 0, 1),
        (NEWID(), @RoundId3, @TeamId7, @TeamId10, 2, 2, 1, 1),
        (NEWID(), @RoundId3, @TeamId11, @TeamId5, 3, 1, 0, 1),
        (NEWID(), @RoundId3, @TeamId12, @TeamId1, null, null, null, 0), -- Motor vs Jaga
        (NEWID(), @RoundId3, @TeamId2, @TeamId18, 0, 0, 1, 1),

        (NEWID(), @RoundId4, @TeamId15, @TeamId11, 3, 0, 0, 1),
        (NEWID(), @RoundId4, @TeamId8, @TeamId17, 0, 1, 0, 1),
        (NEWID(), @RoundId4, @TeamId18, @TeamId6, 2, 1, 0, 1),
        (NEWID(), @RoundId4, @TeamId16, @TeamId3, null, null, null, 0), -- Piast vs Lech
        (NEWID(), @RoundId4, @TeamId4, @TeamId9, 1, 1, 1, 1),
        (NEWID(), @RoundId4, @TeamId5, @TeamId14, null, null, null, 0), -- Rakow vs Zaglebie
        (NEWID(), @RoundId4, @TeamId1, @TeamId7, 5, 2, 0, 1),
        (NEWID(), @RoundId4, @TeamId2, @TeamId13, 3, 1, 0, 1),
        (NEWID(), @RoundId4, @TeamId10, @TeamId12, 3, 3, 1, 1),

        (NEWID(), @RoundId5, @TeamId14, @TeamId10, 6, 2, 0, 1),
        (NEWID(), @RoundId5, @TeamId7, @TeamId4, 1, 0, 0, 1),
        (NEWID(), @RoundId5, @TeamId12, @TeamId16, 0, 0, 1, 1),
        (NEWID(), @RoundId5, @TeamId13, @TeamId18, 4, 1, 0, 1),
        (NEWID(), @RoundId5, @TeamId3, @TeamId15, 1, 1, 0, 1),
        (NEWID(), @RoundId5, @TeamId11, @TeamId1, 1, 2, 0, 1),
        (NEWID(), @RoundId5, @TeamId6, @TeamId8, 0, 3, 0, 1),
        (NEWID(), @RoundId5, @TeamId9, @TeamId2, 1, 0, 0, 1),
        (NEWID(), @RoundId5, @TeamId17, @TeamId5, 2, 3, 0, 1),

        (NEWID(), @RoundId6, @TeamId11, @TeamId17, null, null, null, 0), -- Radomiak vs Termalica
        (NEWID(), @RoundId6, @TeamId4, @TeamId6, null, null, null, 0), -- Widzew vs Pogon
        (NEWID(), @RoundId6, @TeamId15, @TeamId12, null, null, null, 0), -- Korona vs Motor
        (NEWID(), @RoundId6, @TeamId5, @TeamId3, null, null, null, 0), -- Rakow vs Lech
        (NEWID(), @RoundId6, @TeamId8, @TeamId13, null, null, null, 0), -- Gornik vs GKS
        (NEWID(), @RoundId6, @TeamId16, @TeamId7, null, null, null, 0), -- Piast vs Cracovia
        (NEWID(), @RoundId6, @TeamId10, @TeamId18, null, null, null, 0), -- Lechia vs Arka
        (NEWID(), @RoundId6, @TeamId2, @TeamId1, null, null, null, 0), -- Legia vs Jaga
        (NEWID(), @RoundId6, @TeamId9, @TeamId14, null, null, null, 0); -- Wisla vs Zaglebie

    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.SeasonStats)
    INSERT INTO dbo.SeasonStats (Id, TeamId, SeasonYear, LeagueId, MatchesPlayed, Wins, Losses, Draws, GoalsFor, GoalsAgainst)
        VALUES
    	 (NEWID(), @TeamId3, '2024/2025', @LeagueId, 34, 22, 8, 4, 68, 31),
         (NEWID(), @TeamId5, '2024/2025', @LeagueId, 34, 20, 5, 9, 51, 23),
         (NEWID(), @TeamId1, '2024/2025', @LeagueId, 34, 17, 7, 10, 56, 42),
         (NEWID(), @TeamId6, '2024/2025', @LeagueId, 34, 17, 10, 7, 59, 40),
         (NEWID(), @TeamId2, '2024/2025', @LeagueId, 34, 15, 10, 9, 60, 45),
         (NEWID(), @TeamId7, '2024/2025', @LeagueId, 34, 14, 11, 9, 58, 53),
         (NEWID(), @TeamId12, '2024/2025', @LeagueId, 34, 14, 13, 7, 48, 59),
         (NEWID(), @TeamId13, '2024/2025', @LeagueId, 34, 14, 13, 7, 49, 47),
         (NEWID(), @TeamId8, '2024/2025', @LeagueId, 34, 13, 13, 8, 43, 39),
         (NEWID(), @TeamId16, '2024/2025', @LeagueId, 34, 11, 11, 12, 37, 36),
         (NEWID(), @TeamId15, '2024/2025', @LeagueId, 34, 11, 11, 12, 37, 45),
         (NEWID(), @TeamId11, '2024/2025', @LeagueId, 34, 11, 15, 8, 48, 52),
         (NEWID(), @TeamId4, '2024/2025', @LeagueId, 34, 11, 16, 7, 38, 49),
         (NEWID(), @TeamId10, '2024/2025', @LeagueId, 34, 10, 17, 7, 44, 59),
         (NEWID(), @TeamId14, '2024/2025', @LeagueId, 34, 10, 18, 6, 33, 51),
        -- downgraded
         (NEWID(), @TeamId19, '2024/2025', @LeagueId, 34, 7, 17, 10, 39, 56),
         (NEWID(), @TeamId20, '2024/2025', @LeagueId, 34, 6, 16, 12, 38, 53),
         (NEWID(), @TeamId21, '2024/2025', @LeagueId, 34, 6, 18, 10, 37, 63),
        -- promoted to @League
         (NEWID(), @TeamId18, '2024/2025', @LeagueId1, 34, 21, 4, 9, 63, 24),
         (NEWID(), @TeamId17, '2024/2025', @LeagueId1, 34, 21, 5, 8, 70, 39),
         (NEWID(), @TeamId9, '2024/2025', @LeagueId1, 34, 18, 6, 10, 58, 38);
    COMMIT TRANSACTION
    PRINT 'Results for the first 3 rounds of PKO BP Ekstraklasa (2025/2026) inserted successfully with season stats'
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT 'Error occurred while inserting match results.'
    PRINT ERROR_MESSAGE()
END CATCH