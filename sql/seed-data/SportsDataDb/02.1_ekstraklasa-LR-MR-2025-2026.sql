-- ================================================================
-- EKSTRAKLASA 2025/2026 - (full 1-17)
-- ================================================================
-- Created: 04.12.2025

USE SportsDataDb;

DECLARE 
    @CountryId UNIQUEIDENTIFIER, 
    @LeagueId UNIQUEIDENTIFIER,
    @LeagueId1 UNIQUEIDENTIFIER,
    @CurrentDateTime DATETIME2 = GETDATE(),

    -- Team IDs (18 drużyn)
    @TeamId1 UNIQUEIDENTIFIER = 'e4b7d5c8-9f6a-4d2e-8a3c-1b0f8c0f4d2a',   -- Jagiellonia Białystok
    @TeamId2 UNIQUEIDENTIFIER = 'a6c9f7d1-2b34-4e9c-8f13-0d7a2e5b1c9f',   -- Legia Warszawa
    @TeamId3 UNIQUEIDENTIFIER = 'b9f8d3a7-1e6c-4a2b-9f4d-3e0a1c8f7d2b',   -- Lech Poznań
    @TeamId4 UNIQUEIDENTIFIER = 'd0e7f9b8-3a1c-4f6d-9b5a-7c2e8f0a1d3b',   -- Widzew Łódź
    @TeamId5 UNIQUEIDENTIFIER = 'f1a3c5d7-6e8b-4f1c-8b0d-5e2f3a7c9b1d',   -- Raków Częstochowa
    @TeamId6 UNIQUEIDENTIFIER = 'c3d9f6a2-8b4e-4a7f-9c2e-1d0a5f3b7e8c',   -- Pogoń Szczecin
    @TeamId7 UNIQUEIDENTIFIER = 'a2f8c5d9-7b1e-4c3d-8a9f-2e0d7c1b5a6f',   -- Cracovia
    @TeamId8 UNIQUEIDENTIFIER = 'b7e4d1a3-9c6f-4e0a-8b2d-3f1c5e7a9d8b',   -- Górnik Zabrze
    @TeamId9 UNIQUEIDENTIFIER = 'd9a7f8b3-2e0c-4d1f-9a5b-6c3e8f7d0a1b',   -- Wisła Płock
    @TeamId10 UNIQUEIDENTIFIER = 'f0c3e9a7-8d4b-4f6c-1a2e-7b5d9f0c3a8e',  -- Lechia Gdańsk
    @TeamId11 UNIQUEIDENTIFIER = 'e8b2d7f3-1a4c-4e9f-8b5d-0c7f1a6e3d9b',  -- Radomiak Radom
    @TeamId12 UNIQUEIDENTIFIER = 'c5a9e3b1-7f8d-4a0c-9e2b-3d1f6c7a8e0b',  -- Motor Lublin
    @TeamId13 UNIQUEIDENTIFIER = 'b0f6c1d9-3e7a-4b2d-8f5c-9a1e0d3b7c4f',  -- GKS Katowice
    @TeamId14 UNIQUEIDENTIFIER = 'd7a3e9f0-5c1b-4f8d-9a2e-6b0c7f4d1e8a',  -- Zagłębie Lubin
    @TeamId15 UNIQUEIDENTIFIER = 'f4c1b7a9-8d0e-4a3f-9c5b-2e7d1f6a0c3b',  -- Korona Kielce
    @TeamId16 UNIQUEIDENTIFIER = 'a9e2d5f7-1b3c-4e0a-8f6d-7c0b9a1e3d5f',  -- Piast Gliwice
    @TeamId17 UNIQUEIDENTIFIER = 'c0d7f8a1-4b2e-4c9f-8a5d-1e3b6f0c7d9a',  -- Bruk-Bet Termalica Nieciecza
    @TeamId18 UNIQUEIDENTIFIER = 'e1b9c3d7-6f0a-4d2e-9b5c-3a7f8e1d0b6c',  -- Arka Gdynia

    @TeamId19 UNIQUEIDENTIFIER = '8a46089c-f7aa-4270-9742-21a84ec92460',  -- Stal Mielec
    @TeamId20 UNIQUEIDENTIFIER = '4138333e-69dd-41fb-ad30-47bf2b0e4c31',  -- Slask Wroclaw
    @TeamId21 UNIQUEIDENTIFIER = '0bc33659-7471-4fae-945f-f24f60a38ae0',  -- Puszcza Niepolomice
    @TeamId22 UNIQUEIDENTIFIER = 'f7bf7c73-4609-48bf-b8db-66abdaf8c79c',  -- Warta Poznan
    @TeamId23 UNIQUEIDENTIFIER = '015f9edb-00f6-4e63-8a03-ee77348e6572',  -- Ruch Chorzow
    @TeamId24 UNIQUEIDENTIFIER = '823587b3-3b37-4c48-b202-f281d150d49c',  -- LKS Lodz
    @TeamId25 UNIQUEIDENTIFIER = '445cfbe0-607e-4b79-9c83-18dcc7abe73f',  -- Miedz Legnica

    -- Round IDs
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

-- Pobranie ID kraju i ligi
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

    -- ================================================================
    -- TWORZENIE KOLEJEK (LeagueRound)
    -- ================================================================
    
    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.LeagueRound WHERE SeasonYear = '2025/2026')
    INSERT INTO dbo.LeagueRound (Id, LeagueId, SeasonYear, Round)
        VALUES 
            (@RoundId1, @LeagueId, '2025/2026', 1),
            (@RoundId2, @LeagueId, '2025/2026', 2),
            (@RoundId3, @LeagueId, '2025/2026', 3),
            (@RoundId4, @LeagueId, '2025/2026', 4),
            (@RoundId5, @LeagueId, '2025/2026', 5),
            (@RoundId6, @LeagueId, '2025/2026', 6),
            (@RoundId7, @LeagueId, '2025/2026', 7),
            (@RoundId8, @LeagueId, '2025/2026', 8),
            (@RoundId9, @LeagueId, '2025/2026', 9),
            (@RoundId10, @LeagueId, '2025/2026', 10),
            (@RoundId11, @LeagueId, '2025/2026', 11),
            (@RoundId12, @LeagueId, '2025/2026', 12),
            (@RoundId13, @LeagueId, '2025/2026', 13),
            (@RoundId14, @LeagueId, '2025/2026', 14),
            (@RoundId15, @LeagueId, '2025/2026', 15),
            (@RoundId16, @LeagueId, '2025/2026', 16),
            (@RoundId17, @LeagueId, '2025/2026', 17),
            (@RoundId18, @LeagueId, '2025/2026', 18),
            (@RoundId19, @LeagueId, '2025/2026', 19),
            (@RoundId20, @LeagueId, '2025/2026', 20),
            (@RoundId21, @LeagueId, '2025/2026', 21),
            (@RoundId22, @LeagueId, '2025/2026', 22),
            (@RoundId23, @LeagueId, '2025/2026', 23),
            (@RoundId24, @LeagueId, '2025/2026', 24),
            (@RoundId25, @LeagueId, '2025/2026', 25),
            (@RoundId26, @LeagueId, '2025/2026', 26),
            (@RoundId27, @LeagueId, '2025/2026', 27),
            (@RoundId28, @LeagueId, '2025/2026', 28),
            (@RoundId29, @LeagueId, '2025/2026', 29),
            (@RoundId30, @LeagueId, '2025/2026', 30),
            (@RoundId31, @LeagueId, '2025/2026', 31),
            (@RoundId32, @LeagueId, '2025/2026', 32),
            (@RoundId33, @LeagueId, '2025/2026', 33),
            (@RoundId34, @LeagueId, '2025/2026', 34)

    -- ================================================================
    -- WSTAWIANIE MECZÓW (MatchRound) - 306 meczów
    -- ================================================================
    
    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.MatchRound WHERE RoundId IN (
        SELECT Id FROM LeagueRound WHERE SeasonYear = '2025/2026'
    ))
    INSERT INTO dbo.MatchRound (Id, RoundId, HomeTeamId, AwayTeamId, HomeGoals, AwayGoals, IsDraw, IsPlayed)
        VALUES 
        -- Kolejka 1 (18-21 lipca 2025) - ROZEGRANA
        (NEWID(), @RoundId1, @TeamId9, @TeamId15, 2, 0, 0, 1),
        (NEWID(), @RoundId1, @TeamId3, @TeamId7, 1, 4, 0, 1),
        (NEWID(), @RoundId1, @TeamId4, @TeamId14, 1, 0, 0, 1),
        (NEWID(), @RoundId1, @TeamId8, @TeamId10, 2, 1, 0, 1),
        (NEWID(), @RoundId1, @TeamId11, @TeamId6, 5, 1, 0, 1),
        (NEWID(), @RoundId1, @TeamId13, @TeamId5, 0, 1, 0, 1),
        (NEWID(), @RoundId1, @TeamId12, @TeamId18, 1, 0, 0, 1),
        (NEWID(), @RoundId1, @TeamId1, @TeamId17, 0, 4, 0, 1),
        (NEWID(), @RoundId1, @TeamId2, @TeamId16, null, null, null, 0), -- Legia vs Piast


        -- Kolejka 2 (25-27 lipca 2025) - ROZEGRANA
        (NEWID(), @RoundId2, @TeamId7, @TeamId17, 2, 0, 0, 1),
        (NEWID(), @RoundId2, @TeamId18, @TeamId11, 1, 1, 1, 1),
        (NEWID(), @RoundId2, @TeamId16, @TeamId8, 0, 1, 0, 1),
        (NEWID(), @RoundId2, @TeamId6, @TeamId12, 4, 1, 0, 1),
        (NEWID(), @RoundId2, @TeamId10, @TeamId3, 3, 4, 0, 1),
        (NEWID(), @RoundId2, @TeamId5, @TeamId9, 1, 2, 0, 1),
        (NEWID(), @RoundId2, @TeamId1, @TeamId4, 3, 2, 0, 1),
        (NEWID(), @RoundId2, @TeamId15, @TeamId2, 0, 2, 0, 1),
        (NEWID(), @RoundId2, @TeamId13, @TeamId14, 2, 2, 1, 1),

        -- Kolejka 3 (1-3 sierpnia 2025) - ROZEGRANA
        (NEWID(), @RoundId3, @TeamId14, @TeamId15, 1, 1, 1, 1),
        (NEWID(), @RoundId3, @TeamId9, @TeamId16, 2, 0, 0, 1),
        (NEWID(), @RoundId3, @TeamId17, @TeamId6, 1, 1, 1, 1),
        (NEWID(), @RoundId3, @TeamId4, @TeamId13, 3, 0, 0, 1),
        (NEWID(), @RoundId3, @TeamId3, @TeamId8, 2, 1, 0, 1),
        (NEWID(), @RoundId3, @TeamId7, @TeamId10, 2, 2, 1, 1),
        (NEWID(), @RoundId3, @TeamId11, @TeamId5, 3, 1, 0, 1),
        (NEWID(), @RoundId3, @TeamId2, @TeamId18, 0, 0, 1, 1),
        (NEWID(), @RoundId3, @TeamId12, @TeamId1, null, null, null, 0), -- Motor vs Jaga


        -- Kolejka 4 (8-11 sierpnia 2025) - ROZEGRANA
        (NEWID(), @RoundId4, @TeamId18, @TeamId6, 2, 1, 0, 1),
        (NEWID(), @RoundId4, @TeamId2, @TeamId13, 3, 1, 0, 1),
        (NEWID(), @RoundId4, @TeamId10, @TeamId12, 3, 3, 1, 1),
        (NEWID(), @RoundId4, @TeamId8, @TeamId17, 0, 1, 0, 1),
        (NEWID(), @RoundId4, @TeamId15, @TeamId11, 3, 0, 0, 1),
        (NEWID(), @RoundId4, @TeamId5, @TeamId14, null, null, null, 0),
        (NEWID(), @RoundId4, @TeamId1, @TeamId7, 5, 2, 0, 1),
        (NEWID(), @RoundId4, @TeamId16, @TeamId3, null, null, null, 0),
        (NEWID(), @RoundId4, @TeamId4, @TeamId9, 1, 1, 1, 1),

        -- Kolejka 5 (15-17 sierpnia 2025) - ROZEGRANA
        (NEWID(), @RoundId5, @TeamId14, @TeamId10, 6, 2, 0, 1),
        (NEWID(), @RoundId5, @TeamId7, @TeamId4, 1, 0, 0, 1),
        (NEWID(), @RoundId5, @TeamId12, @TeamId16, 0, 0, 1, 1),
        (NEWID(), @RoundId5, @TeamId13, @TeamId18, 4, 1, 0, 1),
        (NEWID(), @RoundId5, @TeamId3, @TeamId15, 1, 1, 1, 1),
        (NEWID(), @RoundId5, @TeamId11, @TeamId1, 1, 2, 0, 1),
        (NEWID(), @RoundId5, @TeamId6, @TeamId8, 0, 3, 0, 1),
        (NEWID(), @RoundId5, @TeamId9, @TeamId2, 1, 0, 0, 1),
        (NEWID(), @RoundId5, @TeamId17, @TeamId5, 2, 3, 0, 1),

        -- Kolejka 6 (22-25 sierpnia 2025) - ROZEGRANA
        (NEWID(), @RoundId6, @TeamId11, @TeamId17, 1, 1, 1, 1),
        (NEWID(), @RoundId6, @TeamId4, @TeamId6, 1, 2, 0, 1),
        (NEWID(), @RoundId6, @TeamId15, @TeamId12, 2, 0, 0, 1),
        (NEWID(), @RoundId6, @TeamId8, @TeamId13, 3, 0, 0, 1),
        (NEWID(), @RoundId6, @TeamId16, @TeamId7, 0, 0, 1, 1),
        (NEWID(), @RoundId6, @TeamId10, @TeamId18, 1, 0, 0, 1),
        (NEWID(), @RoundId6, @TeamId9, @TeamId14, 2, 1, 0, 1),
        (NEWID(), @RoundId6, @TeamId5, @TeamId3, 2, 2, 1, 1),
        (NEWID(), @RoundId6, @TeamId2, @TeamId1, 0, 0, 1, 1),

        -- Kolejka 7 (24-31 sierpnia 2025) - ROZEGRANA
        (NEWID(), @RoundId7, @TeamId18, @TeamId9, 1, 0, 0, 1),
        (NEWID(), @RoundId7, @TeamId13, @TeamId11, 3, 2, 0, 1),
        (NEWID(), @RoundId7, @TeamId17, @TeamId15, 1, 3, 0, 1),
        (NEWID(), @RoundId7, @TeamId14, @TeamId16, 2, 2, 1, 1),
        (NEWID(), @RoundId7, @TeamId8, @TeamId12, 0, 1, 0, 1),
        (NEWID(), @RoundId7, @TeamId1, @TeamId10, 2, 0, 0, 1),
        (NEWID(), @RoundId7, @TeamId3, @TeamId4, 2, 1, 0, 1),
        (NEWID(), @RoundId7, @TeamId7, @TeamId2, 2, 1, 0, 1),
        (NEWID(), @RoundId7, @TeamId6, @TeamId5, 2, 0, 0, 1),

        -- Kolejka 8 (12-15 września 2025) - ROZEGRANA
        (NEWID(), @RoundId8, @TeamId10, @TeamId13, 2, 0, 0, 1),
        (NEWID(), @RoundId8, @TeamId3, @TeamId14, 1, 2, 0, 1),
        (NEWID(), @RoundId8, @TeamId15, @TeamId6, 1, 0, 0, 1),
        (NEWID(), @RoundId8, @TeamId16, @TeamId1, 1, 1, 1, 1),
        (NEWID(), @RoundId8, @TeamId12, @TeamId17, 1, 1, 1, 1),
        (NEWID(), @RoundId8, @TeamId2, @TeamId11, 4, 1, 0, 1),
        (NEWID(), @RoundId8, @TeamId4, @TeamId18, 2, 0, 0, 1),
        (NEWID(), @RoundId8, @TeamId5, @TeamId8, 0, 1, 0, 1),
        (NEWID(), @RoundId8, @TeamId9, @TeamId7, 0, 0, 1, 1),

        -- Kolejka 9 (19-21 września 2025) - ROZEGRANA
        (NEWID(), @RoundId9, @TeamId13, @TeamId7, 0, 3, 0, 1),
        (NEWID(), @RoundId9, @TeamId1, @TeamId9, null, null, null, 0),
        (NEWID(), @RoundId9, @TeamId11, @TeamId16, 1, 0, 0, 1),
        (NEWID(), @RoundId9, @TeamId18, @TeamId15, 0, 0, 1, 1),
        (NEWID(), @RoundId9, @TeamId17, @TeamId3, 0, 2, 0, 1),
        (NEWID(), @RoundId9, @TeamId5, @TeamId2, 1, 1, 1, 1),
        (NEWID(), @RoundId9, @TeamId14, @TeamId12, 2, 2, 1, 1),
        (NEWID(), @RoundId9, @TeamId6, @TeamId10, 3, 4, 0, 1),
        (NEWID(), @RoundId9, @TeamId8, @TeamId4, 3, 2, 0, 1),

        -- Kolejka 10 (26-29 września 2025) - ROZEGRANA
        (NEWID(), @RoundId10, @TeamId9, @TeamId13, 1, 1, 1, 1),
        (NEWID(), @RoundId10, @TeamId15, @TeamId10, 3, 0, 0, 1),
        (NEWID(), @RoundId10, @TeamId16, @TeamId17, 3, 2, 0, 1),
        (NEWID(), @RoundId10, @TeamId7, @TeamId8, 1, 1, 1, 1),
        (NEWID(), @RoundId10, @TeamId4, @TeamId5, 0, 1, 0, 1),
        (NEWID(), @RoundId10, @TeamId3, @TeamId1, 2, 2, 1, 1),
        (NEWID(), @RoundId10, @TeamId2, @TeamId6, 1, 0, 0, 1),
        (NEWID(), @RoundId10, @TeamId14, @TeamId18, 4, 0, 0, 1),
        (NEWID(), @RoundId10, @TeamId12, @TeamId11, 2, 2, 1, 1),

        -- Kolejka 11 (3-5 października 2025) - ROZEGRANA
        (NEWID(), @RoundId11, @TeamId10, @TeamId9, 1, 1, 1, 1),
        (NEWID(), @RoundId11, @TeamId6, @TeamId16, 2, 1, 0, 1),
        (NEWID(), @RoundId11, @TeamId17, @TeamId4, 2, 4, 0, 1),
        (NEWID(), @RoundId11, @TeamId11, @TeamId14, 3, 1, 0, 1),
        (NEWID(), @RoundId11, @TeamId18, @TeamId7, 2, 1, 0, 1),
        (NEWID(), @RoundId11, @TeamId1, @TeamId15, 3, 1, 0, 1),
        (NEWID(), @RoundId11, @TeamId5, @TeamId12, 2, 0, 0, 1),
        (NEWID(), @RoundId11, @TeamId13, @TeamId3, 0, 1, 0, 1),
        (NEWID(), @RoundId11, @TeamId8, @TeamId2, 3, 1, 0, 1),

        -- Kolejka 12 (17-20 października 2025) - ROZEGRANA
        (NEWID(), @RoundId12, @TeamId12, @TeamId13, 5, 2, 0, 1),
        (NEWID(), @RoundId12, @TeamId4, @TeamId11, 3, 2, 0, 1),
        (NEWID(), @RoundId12, @TeamId15, @TeamId8, 1, 1, 1, 1),
        (NEWID(), @RoundId12, @TeamId1, @TeamId18, 4, 0, 0, 1),
        (NEWID(), @RoundId12, @TeamId7, @TeamId5, 2, 0, 0, 1),
        (NEWID(), @RoundId12, @TeamId3, @TeamId6, 2, 2, 1, 1),
        (NEWID(), @RoundId12, @TeamId16, @TeamId10, 1, 2, 0, 1),
        (NEWID(), @RoundId12, @TeamId14, @TeamId2, 3, 1, 0, 1),
        (NEWID(), @RoundId12, @TeamId9, @TeamId17, 3, 1, 0, 1),

        -- Kolejka 13 (24-26 października 2025) - ROZEGRANA
        (NEWID(), @RoundId13, @TeamId17, @TeamId14, 1, 1, 1, 1),
        (NEWID(), @RoundId13, @TeamId12, @TeamId4, 3, 0, 0, 1),
        (NEWID(), @RoundId13, @TeamId18, @TeamId16, 2, 1, 0, 1),
        (NEWID(), @RoundId13, @TeamId6, @TeamId7, 2, 1, 0, 1),
        (NEWID(), @RoundId13, @TeamId13, @TeamId15, 1, 0, 0, 1),
        (NEWID(), @RoundId13, @TeamId5, @TeamId10, 2, 1, 0, 1),
        (NEWID(), @RoundId13, @TeamId8, @TeamId1, 2, 1, 0, 1),
        (NEWID(), @RoundId13, @TeamId2, @TeamId3, 0, 0, 1, 1),
        (NEWID(), @RoundId13, @TeamId11, @TeamId9, 1, 1, 1, 1),

        -- Kolejka 14 (31 października - 3 listopada 2025) - ROZEGRANA
        (NEWID(), @RoundId14, @TeamId17, @TeamId13, 0, 3, 0, 1),
        (NEWID(), @RoundId14, @TeamId16, @TeamId15, 0, 0, 1, 1),
        (NEWID(), @RoundId14, @TeamId8, @TeamId18, 5, 1, 0, 1),
        (NEWID(), @RoundId14, @TeamId3, @TeamId12, 2, 2, 1, 1),
        (NEWID(), @RoundId14, @TeamId1, @TeamId5, 1, 2, 0, 1),
        (NEWID(), @RoundId14, @TeamId4, @TeamId2, 1, 1, 1, 1),
        (NEWID(), @RoundId14, @TeamId10, @TeamId11, 1, 2, 0, 1),
        (NEWID(), @RoundId14, @TeamId9, @TeamId6, 2, 0, 0, 1),
        (NEWID(), @RoundId14, @TeamId7, @TeamId14, 0, 0, 1, 1),

        -- Kolejka 15 (7-9 listopada 2025) - ROZEGRANA
        (NEWID(), @RoundId15, @TeamId11, @TeamId7, 3, 0, 0, 1),
        (NEWID(), @RoundId15, @TeamId14, @TeamId8, 2, 0, 0, 1),
        (NEWID(), @RoundId15, @TeamId10, @TeamId4, 2, 1, 0, 1),
        (NEWID(), @RoundId15, @TeamId12, @TeamId9, 1, 1, 1, 1),
        (NEWID(), @RoundId15, @TeamId13, @TeamId16, 1, 3, 0, 1),
        (NEWID(), @RoundId15, @TeamId15, @TeamId5, 1, 4, 0, 1),
        (NEWID(), @RoundId15, @TeamId2, @TeamId17, 1, 2, 0, 1),
        (NEWID(), @RoundId15, @TeamId6, @TeamId1, 1, 2, 0, 1),
        (NEWID(), @RoundId15, @TeamId18, @TeamId3, 3, 1, 0, 1),

        -- Kolejka 16 (21-24 listopada 2025) - ROZEGRANA
        (NEWID(), @RoundId16, @TeamId17, @TeamId18, 2, 0, 0, 1),
        (NEWID(), @RoundId16, @TeamId8, @TeamId9, 1, 1, 1, 1),
        (NEWID(), @RoundId16, @TeamId7, @TeamId12, 1, 2, 0, 1),
        (NEWID(), @RoundId16, @TeamId5, @TeamId16, 1, 3, 0, 1),
        (NEWID(), @RoundId16, @TeamId2, @TeamId10, 2, 2, 1, 1),
        (NEWID(), @RoundId16, @TeamId3, @TeamId11, 4, 1, 0, 1),
        (NEWID(), @RoundId16, @TeamId4, @TeamId15, 1, 3, 0, 1),
        (NEWID(), @RoundId16, @TeamId6, @TeamId14, 5, 1, 0, 1),
        (NEWID(), @RoundId16, @TeamId1, @TeamId13, null, null, null, 0),

        -- Kolejka 17 (28 listopada - 1 grudnia 2025) - ROZEGRANA
        (NEWID(), @RoundId17, @TeamId16, @TeamId4, 0, 2, 0, 1),
        (NEWID(), @RoundId17, @TeamId11, @TeamId8, 4, 0, 0, 1),
        (NEWID(), @RoundId17, @TeamId10, @TeamId17, 5, 1, 0, 1),
        (NEWID(), @RoundId17, @TeamId15, @TeamId7, 0, 1, 0, 1),
        (NEWID(), @RoundId17, @TeamId13, @TeamId6, 2, 0, 0, 1),
        (NEWID(), @RoundId17, @TeamId18, @TeamId5, 1, 4, 0, 1),
        (NEWID(), @RoundId17, @TeamId14, @TeamId1, 0, 0, 1, 1),
        (NEWID(), @RoundId17, @TeamId9, @TeamId3, 0, 0, 1, 1),
        (NEWID(), @RoundId17, @TeamId12, @TeamId2, 1, 1, 1, 1),

       -- Kolejka 18 (6 grudnia 2025)
        (NEWID(), @RoundId18, @TeamId18, @TeamId12, null, null, null, 0),
        (NEWID(), @RoundId18, @TeamId17, @TeamId1, null, null, null, 0),
        (NEWID(), @RoundId18, @TeamId10, @TeamId8, null, null, null, 0),
        (NEWID(), @RoundId18, @TeamId6, @TeamId11, null, null, null, 0),
        (NEWID(), @RoundId18, @TeamId15, @TeamId9, null, null, null, 0),
        (NEWID(), @RoundId18, @TeamId7, @TeamId3, null, null, null, 0),
        (NEWID(), @RoundId18, @TeamId5, @TeamId13, null, null, null, 0),
        (NEWID(), @RoundId18, @TeamId16, @TeamId2, null, null, null, 0),
        (NEWID(), @RoundId18, @TeamId14, @TeamId4, null, null, null, 0),

        -- Kolejka 19 (12 grudnia 2025)
        (NEWID(), @RoundId19, @TeamId9, @TeamId5, null, null, null, 0),
        (NEWID(), @RoundId19, @TeamId17, @TeamId7, null, null, null, 0),
        (NEWID(), @RoundId19, @TeamId2, @TeamId15, null, null, null, 0),
        (NEWID(), @RoundId19, @TeamId12, @TeamId6, null, null, null, 0),
        (NEWID(), @RoundId19, @TeamId14, @TeamId13, null, null, null, 0),
        (NEWID(), @RoundId19, @TeamId8, @TeamId16, null, null, null, 0),
        (NEWID(), @RoundId19, @TeamId11, @TeamId18, null, null, null, 0),
        (NEWID(), @RoundId19, @TeamId3, @TeamId10, null, null, null, 0),
        (NEWID(), @RoundId19, @TeamId4, @TeamId1, null, null, null, 0),

        -- Kolejka 20 (19 grudnia 2025)
        (NEWID(), @RoundId20, @TeamId9, @TeamId4, null, null, null, 0),
        (NEWID(), @RoundId20, @TeamId1, @TeamId2, null, null, null, 0),
        (NEWID(), @RoundId20, @TeamId13, @TeamId10, null, null, null, 0),
        (NEWID(), @RoundId20, @TeamId6, @TeamId15, null, null, null, 0),
        (NEWID(), @RoundId20, @TeamId16, @TeamId18, null, null, null, 0),
        (NEWID(), @RoundId20, @TeamId3, @TeamId5, null, null, null, 0),
        (NEWID(), @RoundId20, @TeamId12, @TeamId8, null, null, null, 0),
        (NEWID(), @RoundId20, @TeamId11, @TeamId17, null, null, null, 0),
        (NEWID(), @RoundId20, @TeamId7, @TeamId14, null, null, null, 0),

        -- Kolejka 21 (26 grudnia 2025)
        (NEWID(), @RoundId21, @TeamId4, @TeamId7, null, null, null, 0),
        (NEWID(), @RoundId21, @TeamId10, @TeamId12, null, null, null, 0),
        (NEWID(), @RoundId21, @TeamId15, @TeamId16, null, null, null, 0),
        (NEWID(), @RoundId21, @TeamId5, @TeamId1, null, null, null, 0),
        (NEWID(), @RoundId21, @TeamId18, @TeamId13, null, null, null, 0),
        (NEWID(), @RoundId21, @TeamId2, @TeamId9, null, null, null, 0),
        (NEWID(), @RoundId21, @TeamId8, @TeamId11, null, null, null, 0),
        (NEWID(), @RoundId21, @TeamId17, @TeamId6, null, null, null, 0),
        (NEWID(), @RoundId21, @TeamId14, @TeamId3, null, null, null, 0),

        -- Kolejka 22 (2 stycznia 2026)
        (NEWID(), @RoundId22, @TeamId6, @TeamId4, null, null, null, 0),
        (NEWID(), @RoundId22, @TeamId13, @TeamId9, null, null, null, 0),
        (NEWID(), @RoundId22, @TeamId12, @TeamId15, null, null, null, 0),
        (NEWID(), @RoundId22, @TeamId1, @TeamId16, null, null, null, 0),
        (NEWID(), @RoundId22, @TeamId3, @TeamId18, null, null, null, 0),
        (NEWID(), @RoundId22, @TeamId11, @TeamId2, null, null, null, 0),
        (NEWID(), @RoundId22, @TeamId7, @TeamId10, null, null, null, 0),
        (NEWID(), @RoundId22, @TeamId17, @TeamId5, null, null, null, 0),
        (NEWID(), @RoundId22, @TeamId8, @TeamId14, null, null, null, 0),

        -- Kolejka 23 (9 stycznia 2026)
        (NEWID(), @RoundId23, @TeamId4, @TeamId13, null, null, null, 0),
        (NEWID(), @RoundId23, @TeamId10, @TeamId6, null, null, null, 0),
        (NEWID(), @RoundId23, @TeamId15, @TeamId1, null, null, null, 0),
        (NEWID(), @RoundId23, @TeamId18, @TeamId11, null, null, null, 0),
        (NEWID(), @RoundId23, @TeamId5, @TeamId8, null, null, null, 0),
        (NEWID(), @RoundId23, @TeamId9, @TeamId17, null, null, null, 0),
        (NEWID(), @RoundId23, @TeamId14, @TeamId12, null, null, null, 0),
        (NEWID(), @RoundId23, @TeamId2, @TeamId7, null, null, null, 0),
        (NEWID(), @RoundId23, @TeamId16, @TeamId3, null, null, null, 0),

        -- Kolejka 24 (16 stycznia 2026)
        (NEWID(), @RoundId24, @TeamId9, @TeamId10, null, null, null, 0),
        (NEWID(), @RoundId24, @TeamId7, @TeamId4, null, null, null, 0),
        (NEWID(), @RoundId24, @TeamId17, @TeamId16, null, null, null, 0),
        (NEWID(), @RoundId24, @TeamId1, @TeamId18, null, null, null, 0),
        (NEWID(), @RoundId24, @TeamId13, @TeamId12, null, null, null, 0),
        (NEWID(), @RoundId24, @TeamId11, @TeamId15, null, null, null, 0),
        (NEWID(), @RoundId24, @TeamId3, @TeamId14, null, null, null, 0),
        (NEWID(), @RoundId24, @TeamId6, @TeamId5, null, null, null, 0),
        (NEWID(), @RoundId24, @TeamId8, @TeamId2, null, null, null, 0),

        -- Kolejka 25 (23 stycznia 2026)
        (NEWID(), @RoundId25, @TeamId5, @TeamId11, null, null, null, 0),
        (NEWID(), @RoundId25, @TeamId16, @TeamId9, null, null, null, 0),
        (NEWID(), @RoundId25, @TeamId12, @TeamId1, null, null, null, 0),
        (NEWID(), @RoundId25, @TeamId10, @TeamId7, null, null, null, 0),
        (NEWID(), @RoundId25, @TeamId15, @TeamId18, null, null, null, 0),
        (NEWID(), @RoundId25, @TeamId4, @TeamId17, null, null, null, 0),
        (NEWID(), @RoundId25, @TeamId14, @TeamId6, null, null, null, 0),
        (NEWID(), @RoundId25, @TeamId2, @TeamId13, null, null, null, 0),
        (NEWID(), @RoundId25, @TeamId3, @TeamId8, null, null, null, 0),

        -- Kolejka 26 (30 stycznia 2026)
        (NEWID(), @RoundId26, @TeamId13, @TeamId5, null, null, null, 0),
        (NEWID(), @RoundId26, @TeamId6, @TeamId10, null, null, null, 0),
        (NEWID(), @RoundId26, @TeamId18, @TeamId9, null, null, null, 0),
        (NEWID(), @RoundId26, @TeamId1, @TeamId14, null, null, null, 0),
        (NEWID(), @RoundId26, @TeamId8, @TeamId4, null, null, null, 0),
        (NEWID(), @RoundId26, @TeamId11, @TeamId3, null, null, null, 0),
        (NEWID(), @RoundId26, @TeamId7, @TeamId16, null, null, null, 0),
        (NEWID(), @RoundId26, @TeamId17, @TeamId15, null, null, null, 0),
        (NEWID(), @RoundId26, @TeamId12, @TeamId2, null, null, null, 0),

        -- Kolejka 27 (6 lutego 2026)
        (NEWID(), @RoundId27, @TeamId15, @TeamId8, null, null, null, 0),
        (NEWID(), @RoundId27, @TeamId9, @TeamId1, null, null, null, 0),
        (NEWID(), @RoundId27, @TeamId16, @TeamId6, null, null, null, 0),
        (NEWID(), @RoundId27, @TeamId3, @TeamId7, null, null, null, 0),
        (NEWID(), @RoundId27, @TeamId10, @TeamId13, null, null, null, 0),
        (NEWID(), @RoundId27, @TeamId5, @TeamId18, null, null, null, 0),
        (NEWID(), @RoundId27, @TeamId4, @TeamId11, null, null, null, 0),
        (NEWID(), @RoundId27, @TeamId2, @TeamId17, null, null, null, 0),
        (NEWID(), @RoundId27, @TeamId14, @TeamId12, null, null, null, 0),

        -- Kolejka 28 (13 lutego 2026)
        (NEWID(), @RoundId28, @TeamId11, @TeamId16, null, null, null, 0),
        (NEWID(), @RoundId28, @TeamId6, @TeamId3, null, null, null, 0),
        (NEWID(), @RoundId28, @TeamId13, @TeamId17, null, null, null, 0),
        (NEWID(), @RoundId28, @TeamId1, @TeamId10, null, null, null, 0),
        (NEWID(), @RoundId28, @TeamId8, @TeamId16, null, null, null, 0),
        (NEWID(), @RoundId28, @TeamId7, @TeamId5, null, null, null, 0),
        (NEWID(), @RoundId28, @TeamId18, @TeamId4, null, null, null, 0),
        (NEWID(), @RoundId28, @TeamId12, @TeamId9, null, null, null, 0),
        (NEWID(), @RoundId28, @TeamId15, @TeamId2, null, null, null, 0),

        -- Kolejka 29 (20 lutego 2026)
        (NEWID(), @RoundId29, @TeamId4, @TeamId6, null, null, null, 0),
        (NEWID(), @RoundId29, @TeamId10, @TeamId18, null, null, null, 0),
        (NEWID(), @RoundId29, @TeamId5, @TeamId15, null, null, null, 0),
        (NEWID(), @RoundId29, @TeamId17, @TeamId12, null, null, null, 0),
        (NEWID(), @RoundId29, @TeamId9, @TeamId14, null, null, null, 0),
        (NEWID(), @RoundId29, @TeamId2, @TeamId11, null, null, null, 0),
        (NEWID(), @RoundId29, @TeamId16, @TeamId7, null, null, null, 0),
        (NEWID(), @RoundId29, @TeamId3, @TeamId13, null, null, null, 0),
        (NEWID(), @RoundId29, @TeamId1, @TeamId8, null, null, null, 0),

        -- Kolejka 30 (27 lutego 2026)
        (NEWID(), @RoundId30, @TeamId14, @TeamId5, null, null, null, 0),
        (NEWID(), @RoundId30, @TeamId6, @TeamId9, null, null, null, 0),
        (NEWID(), @RoundId30, @TeamId13, @TeamId4, null, null, null, 0),
        (NEWID(), @RoundId30, @TeamId18, @TeamId16, null, null, null, 0),
        (NEWID(), @RoundId30, @TeamId11, @TeamId1, null, null, null, 0),
        (NEWID(), @RoundId30, @TeamId12, @TeamId3, null, null, null, 0),
        (NEWID(), @RoundId30, @TeamId7, @TeamId17, null, null, null, 0),
        (NEWID(), @RoundId30, @TeamId15, @TeamId10, null, null, null, 0),
        (NEWID(), @RoundId30, @TeamId8, @TeamId2, null, null, null, 0),

        -- Kolejka 31 (6 marca 2026)
        (NEWID(), @RoundId31, @TeamId4, @TeamId12, null, null, null, 0),
        (NEWID(), @RoundId31, @TeamId17, @TeamId11, null, null, null, 0),
        (NEWID(), @RoundId31, @TeamId3, @TeamId14, null, null, null, 0),
        (NEWID(), @RoundId31, @TeamId1, @TeamId13, null, null, null, 0),
        (NEWID(), @RoundId31, @TeamId10, @TeamId9, null, null, null, 0),
        (NEWID(), @RoundId31, @TeamId5, @TeamId6, null, null, null, 0),
        (NEWID(), @RoundId31, @TeamId16, @TeamId8, null, null, null, 0),
        (NEWID(), @RoundId31, @TeamId2, @TeamId18, null, null, null, 0),
        (NEWID(), @RoundId31, @TeamId7, @TeamId15, null, null, null, 0),

        -- Kolejka 32 (13 marca 2026)
        (NEWID(), @RoundId32, @TeamId12, @TeamId18, null, null, null, 0),
        (NEWID(), @RoundId32, @TeamId6, @TeamId17, null, null, null, 0),
        (NEWID(), @RoundId32, @TeamId9, @TeamId15, null, null, null, 0),
        (NEWID(), @RoundId32, @TeamId11, @TeamId5, null, null, null, 0),
        (NEWID(), @RoundId32, @TeamId8, @TeamId3, null, null, null, 0),
        (NEWID(), @RoundId32, @TeamId14, @TeamId16, null, null, null, 0),
        (NEWID(), @RoundId32, @TeamId13, @TeamId7, null, null, null, 0),
        (NEWID(), @RoundId32, @TeamId4, @TeamId10, null, null, null, 0),
        (NEWID(), @RoundId32, @TeamId2, @TeamId1, null, null, null, 0),

        -- Kolejka 33 (20 marca 2026)
        (NEWID(), @RoundId33, @TeamId1, @TeamId9, null, null, null, 0),
        (NEWID(), @RoundId33, @TeamId18, @TeamId8, null, null, null, 0),
        (NEWID(), @RoundId33, @TeamId7, @TeamId6, null, null, null, 0),
        (NEWID(), @RoundId33, @TeamId15, @TeamId4, null, null, null, 0),
        (NEWID(), @RoundId33, @TeamId16, @TeamId12, null, null, null, 0),
        (NEWID(), @RoundId33, @TeamId5, @TeamId2, null, null, null, 0),
        (NEWID(), @RoundId33, @TeamId10, @TeamId11, null, null, null, 0),
        (NEWID(), @RoundId33, @TeamId3, @TeamId13, null, null, null, 0),
        (NEWID(), @RoundId33, @TeamId17, @TeamId14, null, null, null, 0),

        -- Kolejka 34 (27-29 marca 2026)
        (NEWID(), @RoundId34, @TeamId8, @TeamId5, null, null, null, 0),
        (NEWID(), @RoundId34, @TeamId14, @TeamId18, null, null, null, 0),
        (NEWID(), @RoundId34, @TeamId4, @TeamId16, null, null, null, 0),
        (NEWID(), @RoundId34, @TeamId9, @TeamId7, null, null, null, 0),
        (NEWID(), @RoundId34, @TeamId11, @TeamId15, null, null, null, 0),
        (NEWID(), @RoundId34, @TeamId6, @TeamId1, null, null, null, 0),
        (NEWID(), @RoundId34, @TeamId13, @TeamId2, null, null, null, 0),
        (NEWID(), @RoundId34, @TeamId17, @TeamId10, null, null, null, 0),
        (NEWID(), @RoundId34, @TeamId12, @TeamId3, null, null, null, 0)

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


    	 (NEWID(), @TeamId1, '2023/2024', @LeagueId,  34, 18, 7,   9, 77,  45),
         (NEWID(), @TeamId20, '2023/2024', @LeagueId, 34, 18, 7,   9, 50,  31),
         (NEWID(), @TeamId2, '2023/2024', @LeagueId,  34, 16,  7,  11, 51, 39),
         (NEWID(), @TeamId6, '2023/2024', @LeagueId,  34, 16, 11,  7,  59, 38),
         (NEWID(), @TeamId3, '2023/2024', @LeagueId,  34, 14,  9,  11, 47, 41),
         (NEWID(), @TeamId8, '2023/2024', @LeagueId,  34, 15, 11,  8,  45, 41),
         (NEWID(), @TeamId5, '2023/2024', @LeagueId,  34, 14, 10,  10, 54, 39),
         (NEWID(), @TeamId14, '2023/2024', @LeagueId, 34, 13, 13,  8,  43, 50),
         (NEWID(), @TeamId4, '2023/2024', @LeagueId,  34, 13, 14,  7,  45, 46),
         (NEWID(), @TeamId16, '2023/2024', @LeagueId, 34, 9,  9,  16, 38,  35),
         (NEWID(), @TeamId19, '2023/2024', @LeagueId, 34, 11, 13, 10,  42, 48),
         (NEWID(), @TeamId21, '2023/2024', @LeagueId, 34, 9,  12, 13,  39, 49),
         (NEWID(), @TeamId7, '2023/2024', @LeagueId,  34, 8,  11, 15,  45, 46),
         (NEWID(), @TeamId15, '2023/2024', @LeagueId, 34, 8,  12, 14,  40, 44),
         (NEWID(), @TeamId11, '2023/2024', @LeagueId, 34, 10, 16,  8,  41, 58),
         -- downgraded
         (NEWID(), @TeamId22, '2023/2024', @LeagueId,   34, 9,  10, 15,  33, 43), --warta poznan
         (NEWID(), @TeamId23, '2023/2024', @LeagueId,   34, 6,  14, 14,  40, 55), --ruch chorzow
         (NEWID(), @TeamId24, '2023/2024', @LeagueId,   34, 6,  6,  22,  34, 75),  --lks lodz
         

    	 (NEWID(), @TeamId5, '2022/2023', @LeagueId,  34, 23, 6,  5,  63, 24),
         (NEWID(), @TeamId2, '2022/2023', @LeagueId, 34, 19, 9,  6,  57, 37),
         (NEWID(), @TeamId3, '2022/2023', @LeagueId,  34, 17, 10, 7,  51, 29),
         (NEWID(), @TeamId6, '2022/2023', @LeagueId,  34, 17, 9,  8,  57, 46),
         (NEWID(), @TeamId16, '2022/2023', @LeagueId,  34, 15,  8, 11, 40, 31),
         (NEWID(), @TeamId8, '2022/2023', @LeagueId, 34, 13, 9,  12, 45, 43),
         (NEWID(), @TeamId7, '2022/2023', @LeagueId,  34, 12, 10, 12, 41, 35),
         (NEWID(), @TeamId5, '2022/2023', @LeagueId,  34, 12, 9,  13, 37, 35),
         (NEWID(), @TeamId22, '2022/2023', @LeagueId, 34, 12, 9,  13, 35, 44),
         (NEWID(), @TeamId14, '2022/2023', @LeagueId,  34, 12, 8,  14, 34, 41),
         (NEWID(), @TeamId19, '2022/2023', @LeagueId, 34, 11, 10, 13, 36, 40),
         (NEWID(), @TeamId4, '2022/2023', @LeagueId, 34, 11, 8,  15, 38, 47),
         (NEWID(), @TeamId15, '2022/2023', @LeagueId, 34, 11,  8, 15, 39, 48),
         (NEWID(), @TeamId1, '2022/2023', @LeagueId,  34, 9,  14, 11, 48, 49),
         (NEWID(), @TeamId20, '2022/2023', @LeagueId, 34, 9,  11, 14, 35, 48),
         -- downgraded
         (NEWID(), @TeamId9, '2022/2023', @LeagueId,   34, 10,  7, 17,  41, 50), --wisla plock
         (NEWID(), @TeamId10, '2022/2023', @LeagueId,   34, 8,  6, 20,  28, 56), --lechia gdansk
         (NEWID(), @TeamId25, '2022/2023', @LeagueId,   34, 4,  11,  19,  33, 55); --miedz legnica

    COMMIT TRANSACTION
    PRINT '✅ SUKCES! Ekstraklasa 2025/2026'
    PRINT '   - max match rounds: 34'
    PRINT '   - matches: 306'
    PRINT '   - teams: 18'
    PRINT '   - Sezon: 2025/2026 SeasonStats 2024/2025 & 2023/2024 & 2022/2023'
    PRINT '   - Status:'
    PRINT '     • played rounds 1-17:'
    PRINT '     • not played rounds 18-34:'
    PRINT '   - last update 06.12.2025'
    PRINT '//////'

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT '❌ Cannot insert! Ekstraklasa 2025/2026'
    PRINT 'Error: ' + ERROR_MESSAGE()
END CATCH
