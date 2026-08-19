-- ================================================================
-- EKSTRAKLASA 2026/2027 - (full 1-4)
-- ================================================================
-- Created: 19.08.2026

USE SportsDataDb;

DECLARE 
    @CountryId UNIQUEIDENTIFIER, 
    @LeagueId UNIQUEIDENTIFIER,
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

    @TeamId26 UNIQUEIDENTIFIER = 'a1fd8510-34ec-4cf8-8e49-c6147da036ab',  -- Wieczysta Krakow
    @TeamId27 UNIQUEIDENTIFIER = 'f29d0263-a534-4e3d-94c6-e1f5ea4a903c',  -- Wisla Krakow

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

BEGIN TRANSACTION

BEGIN TRY

    -- ================================================================
    -- TWORZENIE KOLEJEK (LeagueRound)
    -- ================================================================
    
    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.LeagueRound WHERE SeasonYear = '2026/2027')
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

    -- ================================================================
    -- WSTAWIANIE MECZÓW (MatchRound) - 306 meczów
    -- ================================================================
    
    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.MatchRound WHERE RoundId IN (
        SELECT Id FROM LeagueRound WHERE SeasonYear = '2026/2027'
    ))
    INSERT INTO dbo.MatchRound (Id, RoundId, HomeTeamId, AwayTeamId, HomeGoals, AwayGoals, IsDraw, IsPlayed)
        VALUES 
        -- Kolejka 1
        (NEWID(), @RoundId1, @TeamId11, @TeamId26, 2, 1, 0, 1),
        (NEWID(), @RoundId1, @TeamId6, @TeamId2, 0, 1, 0, 1),
        (NEWID(), @RoundId1, @TeamId1, @TeamId15, 1, 0, 0, 1),
        (NEWID(), @RoundId1, @TeamId8, @TeamId20, 2, 1, 0, 1),
        (NEWID(), @RoundId1, @TeamId3, @TeamId7, 0, 0, 1, 1),
        (NEWID(), @RoundId1, @TeamId5, @TeamId9, 1, 2, 0, 1),
        (NEWID(), @RoundId1, @TeamId4, @TeamId12, 2, 2, 1, 1),
        (NEWID(), @RoundId1, @TeamId27, @TeamId13, 2, 1, 0, 1),
        (NEWID(), @RoundId1, @TeamId14, @TeamId16, 2, 0, 0, 1),


        -- Kolejka 2
        (NEWID(), @RoundId2, @TeamId9, @TeamId4, 0, 0, 1, 1),
        (NEWID(), @RoundId2, @TeamId12, @TeamId1, 1, 2, 0, 1),
        (NEWID(), @RoundId2, @TeamId16, @TeamId27, 4, 3, 0, 1),
        (NEWID(), @RoundId2, @TeamId26, @TeamId3, 1, 2, 0, 1),
        (NEWID(), @RoundId2, @TeamId2, @TeamId14, 3, 1, 0, 1),
        (NEWID(), @RoundId2, @TeamId20, @TeamId5, 2, 1, 0, 1),
        (NEWID(), @RoundId2, @TeamId13, @TeamId11, 3, 1, 0, 1),
        (NEWID(), @RoundId2, @TeamId7, @TeamId6, 0, 2, 0, 1),
        (NEWID(), @RoundId2, @TeamId15, @TeamId8, null, null, null, 0),

        -- Kolejka 3
        (NEWID(), @RoundId3, @TeamId27, @TeamId9, 2, 1, 0, 1),
        (NEWID(), @RoundId3, @TeamId11, @TeamId8, 1, 3, 0, 1),
        (NEWID(), @RoundId3, @TeamId6, @TeamId12, 3, 1, 0, 1),
        (NEWID(), @RoundId3, @TeamId15, @TeamId2, 1, 1, 1, 1),
        (NEWID(), @RoundId3, @TeamId20, @TeamId7, 0, 0, 1, 1),
        (NEWID(), @RoundId3, @TeamId3, @TeamId16, 3, 0, 0, 1),
        (NEWID(), @RoundId3, @TeamId13, @TeamId26, null, null, null, 1),
        (NEWID(), @RoundId3, @TeamId1, @TeamId4, 0, 2, 0, 1),
        (NEWID(), @RoundId3, @TeamId5, @TeamId14, null, null, null, 1),


        -- Kolejka 4
        (NEWID(), @RoundId4, @TeamId2, @TeamId11, 5, 0, 0, 1),
        (NEWID(), @RoundId4, @TeamId14, @TeamId20, 2, 1, 0, 1),
        (NEWID(), @RoundId4, @TeamId16, @TeamId26, 3, 4, 0, 1),
        (NEWID(), @RoundId4, @TeamId4, @TeamId15, 1, 2, 0, 1),
        (NEWID(), @RoundId4, @TeamId12, @TeamId13, 1, 0, 0, 1),
        (NEWID(), @RoundId4, @TeamId1, @TeamId6, null, null, null, 1),
        (NEWID(), @RoundId4, @TeamId7, @TeamId5, 2, 1, 0, 1),
        (NEWID(), @RoundId4, @TeamId9, @TeamId3, null, null, null, 1),
        (NEWID(), @RoundId4, @TeamId8, @TeamId27, 2, 1, 0, 1),

        -- Kolejka 5
        (NEWID(), @RoundId5, @TeamId15, @TeamId12, NULL, NULL, 0, 0), -- Korona Kielce - Motor Lublin
        (NEWID(), @RoundId5, @TeamId7,  @TeamId26, NULL, NULL, 0, 0), -- Cracovia - Wieczysta Kraków
        (NEWID(), @RoundId5, @TeamId11, @TeamId14, NULL, NULL, 0, 0), -- Radomiak Radom - Zagłębie Lubin
        (NEWID(), @RoundId5, @TeamId6,  @TeamId27, NULL, NULL, 0, 0), -- Pogoń Szczecin - Wisła Kraków
        (NEWID(), @RoundId5, @TeamId5,  @TeamId8,  NULL, NULL, 0, 0), -- Raków Częstochowa - Górnik Zabrze (przełożony)
        (NEWID(), @RoundId5, @TeamId13, @TeamId9,  NULL, NULL, 0, 0), -- GKS Katowice - Wisła Płock
        (NEWID(), @RoundId5, @TeamId20, @TeamId4,  NULL, NULL, 0, 0), -- Śląsk Wrocław - Widzew Łódź
        (NEWID(), @RoundId5, @TeamId16, @TeamId2,  NULL, NULL, 0, 0), -- Piast Gliwice - Legia Warszawa
        (NEWID(), @RoundId5, @TeamId3,  @TeamId1,  NULL, NULL, 0, 0), -- Lech Poznań - Jagiellonia Białystok (przełożony)

        -- Kolejka 6
        (NEWID(), @RoundId6, @TeamId12, @TeamId16, NULL, NULL, 0, 0), -- Motor Lublin - Piast Gliwice
        (NEWID(), @RoundId6, @TeamId27, @TeamId26, NULL, NULL, 0, 0), -- Wisła Kraków - Wieczysta Kraków
        (NEWID(), @RoundId6, @TeamId11, @TeamId7,  NULL, NULL, 0, 0), -- Radomiak Radom - Cracovia
        (NEWID(), @RoundId6, @TeamId2,  @TeamId20, NULL, NULL, 0, 0), -- Legia Warszawa - Śląsk Wrocław
        (NEWID(), @RoundId6, @TeamId5,  @TeamId1,  NULL, NULL, 0, 0), -- Raków Częstochowa - Jagiellonia Białystok
        (NEWID(), @RoundId6, @TeamId4,  @TeamId3,  NULL, NULL, 0, 0), -- Widzew Łódź - Lech Poznań
        (NEWID(), @RoundId6, @TeamId8,  @TeamId13, NULL, NULL, 0, 0), -- Górnik Zabrze - GKS Katowice
        (NEWID(), @RoundId6, @TeamId14, @TeamId6,  NULL, NULL, 0, 0), -- Zagłębie Lubin - Pogoń Szczecin
        (NEWID(), @RoundId6, @TeamId9,  @TeamId15, NULL, NULL, 0, 0), -- Wisła Płock - Korona Kielce

        -- Kolejka 7
        (NEWID(), @RoundId7, @TeamId12, @TeamId2,  NULL, NULL, 0, 0), -- Motor Lublin - Legia Warszawa
        (NEWID(), @RoundId7, @TeamId15, @TeamId27, NULL, NULL, 0, 0), -- Korona Kielce - Wisła Kraków
        (NEWID(), @RoundId7, @TeamId26, @TeamId14, NULL, NULL, 0, 0), -- Wieczysta Kraków - Zagłębie Lubin
        (NEWID(), @RoundId7, @TeamId7,  @TeamId8,  NULL, NULL, 0, 0), -- Cracovia - Górnik Zabrze
        (NEWID(), @RoundId7, @TeamId6,  @TeamId9,  NULL, NULL, 0, 0), -- Pogoń Szczecin - Wisła Płock
        (NEWID(), @RoundId7, @TeamId4,  @TeamId11, NULL, NULL, 0, 0), -- Widzew Łódź - Radomiak Radom
        (NEWID(), @RoundId7, @TeamId16, @TeamId13, NULL, NULL, 0, 0), -- Piast Gliwice - GKS Katowice
        (NEWID(), @RoundId7, @TeamId1,  @TeamId20, NULL, NULL, 0, 0), -- Jagiellonia Białystok - Śląsk Wrocław
        (NEWID(), @RoundId7, @TeamId3,  @TeamId5,  NULL, NULL, 0, 0), -- Lech Poznań - Raków Częstochowa

        -- Kolejka 8
        (NEWID(), @RoundId8, @TeamId27, @TeamId1,  NULL, NULL, 0, 0), -- Wisła Kraków - Jagiellonia Białystok
        (NEWID(), @RoundId8, @TeamId11, @TeamId16, NULL, NULL, 0, 0), -- Radomiak Radom - Piast Gliwice
        (NEWID(), @RoundId8, @TeamId2,  @TeamId4,  NULL, NULL, 0, 0), -- Legia Warszawa - Widzew Łódź
        (NEWID(), @RoundId8, @TeamId6,  @TeamId26, NULL, NULL, 0, 0), -- Pogoń Szczecin - Wieczysta Kraków
        (NEWID(), @RoundId8, @TeamId5,  @TeamId12, NULL, NULL, 0, 0), -- Raków Częstochowa - Motor Lublin
        (NEWID(), @RoundId8, @TeamId20, @TeamId15, NULL, NULL, 0, 0), -- Śląsk Wrocław - Korona Kielce
        (NEWID(), @RoundId8, @TeamId8,  @TeamId3,  NULL, NULL, 0, 0), -- Górnik Zabrze - Lech Poznań
        (NEWID(), @RoundId8, @TeamId14, @TeamId13, NULL, NULL, 0, 0), -- Zagłębie Lubin - GKS Katowice
        (NEWID(), @RoundId8, @TeamId9,  @TeamId7,  NULL, NULL, 0, 0), -- Wisła Płock - Cracovia

        -- Kolejka 9
        (NEWID(), @RoundId9, @TeamId12, @TeamId8,  NULL, NULL, 0, 0), -- Motor Lublin - Górnik Zabrze
        (NEWID(), @RoundId9, @TeamId15, @TeamId5,  NULL, NULL, 0, 0), -- Korona Kielce - Raków Częstochowa
        (NEWID(), @RoundId9, @TeamId27, @TeamId20, NULL, NULL, 0, 0), -- Wisła Kraków - Śląsk Wrocław
        (NEWID(), @RoundId9, @TeamId13, @TeamId7,  NULL, NULL, 0, 0), -- GKS Katowice - Cracovia
        (NEWID(), @RoundId9, @TeamId4,  @TeamId26, NULL, NULL, 0, 0), -- Widzew Łódź - Wieczysta Kraków
        (NEWID(), @RoundId9, @TeamId16, @TeamId6,  NULL, NULL, 0, 0), -- Piast Gliwice - Pogoń Szczecin
        (NEWID(), @RoundId9, @TeamId1,  @TeamId2,  NULL, NULL, 0, 0), -- Jagiellonia Białystok - Legia Warszawa
        (NEWID(), @RoundId9, @TeamId14, @TeamId9,  NULL, NULL, 0, 0), -- Zagłębie Lubin - Wisła Płock
        (NEWID(), @RoundId9, @TeamId3,  @TeamId11, NULL, NULL, 0, 0), -- Lech Poznań - Radomiak Radom

        -- Kolejka 10
        (NEWID(), @RoundId10, @TeamId26, @TeamId9,  NULL, NULL, 0, 0), -- Wieczysta Kraków - Wisła Płock
        (NEWID(), @RoundId10, @TeamId7,  @TeamId14, NULL, NULL, 0, 0), -- Cracovia - Zagłębie Lubin
        (NEWID(), @RoundId10, @TeamId11, @TeamId12, NULL, NULL, 0, 0), -- Radomiak Radom - Motor Lublin
        (NEWID(), @RoundId10, @TeamId2,  @TeamId27, NULL, NULL, 0, 0), -- Legia Warszawa - Wisła Kraków
        (NEWID(), @RoundId10, @TeamId6,  @TeamId15, NULL, NULL, 0, 0), -- Pogoń Szczecin - Korona Kielce
        (NEWID(), @RoundId10, @TeamId5,  @TeamId13, NULL, NULL, 0, 0), -- Raków Częstochowa - GKS Katowice
        (NEWID(), @RoundId10, @TeamId20, @TeamId3,  NULL, NULL, 0, 0), -- Śląsk Wrocław - Lech Poznań
        (NEWID(), @RoundId10, @TeamId16, @TeamId4,  NULL, NULL, 0, 0), -- Piast Gliwice - Widzew Łódź
        (NEWID(), @RoundId10, @TeamId1,  @TeamId8,  NULL, NULL, 0, 0), -- Jagiellonia Białystok - Górnik Zabrze

        -- Kolejka 11
        (NEWID(), @RoundId11, @TeamId12, @TeamId20, NULL, NULL, 0, 0), -- Motor Lublin - Śląsk Wrocław
        (NEWID(), @RoundId11, @TeamId26, @TeamId5,  NULL, NULL, 0, 0), -- Wieczysta Kraków - Raków Częstochowa
        (NEWID(), @RoundId11, @TeamId7,  @TeamId2,  NULL, NULL, 0, 0), -- Cracovia - Legia Warszawa
        (NEWID(), @RoundId11, @TeamId13, @TeamId6,  NULL, NULL, 0, 0), -- GKS Katowice - Pogoń Szczecin
        (NEWID(), @RoundId11, @TeamId4,  @TeamId27, NULL, NULL, 0, 0), -- Widzew Łódź - Wisła Kraków
        (NEWID(), @RoundId11, @TeamId8,  @TeamId16, NULL, NULL, 0, 0), -- Górnik Zabrze - Piast Gliwice
        (NEWID(), @RoundId11, @TeamId14, @TeamId1,  NULL, NULL, 0, 0), -- Zagłębie Lubin - Jagiellonia Białystok
        (NEWID(), @RoundId11, @TeamId9,  @TeamId11, NULL, NULL, 0, 0), -- Wisła Płock - Radomiak Radom
        (NEWID(), @RoundId11, @TeamId3,  @TeamId15, NULL, NULL, 0, 0), -- Lech Poznań - Korona Kielce

        -- Kolejka 12 
        (NEWID(), @RoundId12, @TeamId12, @TeamId14, NULL, NULL, 0, 0), -- Motor Lublin - Zagłębie Lubin
        (NEWID(), @RoundId12, @TeamId15, @TeamId13, NULL, NULL, 0, 0), -- Korona Kielce - GKS Katowice
        (NEWID(), @RoundId12, @TeamId27, @TeamId5,  NULL, NULL, 0, 0), -- Wisła Kraków - Raków Częstochowa
        (NEWID(), @RoundId12, @TeamId2,  @TeamId3,  NULL, NULL, 0, 0), -- Legia Warszawa - Lech Poznań
        (NEWID(), @RoundId12, @TeamId6,  @TeamId11, NULL, NULL, 0, 0), -- Pogoń Szczecin - Radomiak Radom
        (NEWID(), @RoundId12, @TeamId20, @TeamId26, NULL, NULL, 0, 0), -- Śląsk Wrocław - Wieczysta Kraków
        (NEWID(), @RoundId12, @TeamId4,  @TeamId8,  NULL, NULL, 0, 0), -- Widzew Łódź - Górnik Zabrze
        (NEWID(), @RoundId12, @TeamId16, @TeamId7,  NULL, NULL, 0, 0), -- Piast Gliwice - Cracovia
        (NEWID(), @RoundId12, @TeamId1,  @TeamId9,  NULL, NULL, 0, 0), -- Jagiellonia Białystok - Wisła Płock

        -- Kolejka 13 
        (NEWID(), @RoundId13, @TeamId26, @TeamId1,  NULL, NULL, 0, 0), -- Wieczysta Kraków - Jagiellonia Białystok
        (NEWID(), @RoundId13, @TeamId7,  @TeamId12, NULL, NULL, 0, 0), -- Cracovia - Motor Lublin
        (NEWID(), @RoundId13, @TeamId11, @TeamId20, NULL, NULL, 0, 0), -- Radomiak Radom - Śląsk Wrocław
        (NEWID(), @RoundId13, @TeamId5,  @TeamId6,  NULL, NULL, 0, 0), -- Raków Częstochowa - Pogoń Szczecin
        (NEWID(), @RoundId13, @TeamId13, @TeamId4,  NULL, NULL, 0, 0), -- GKS Katowice - Widzew Łódź
        (NEWID(), @RoundId13, @TeamId8,  @TeamId2,  NULL, NULL, 0, 0), -- Górnik Zabrze - Legia Warszawa
        (NEWID(), @RoundId13, @TeamId14, @TeamId15, NULL, NULL, 0, 0), -- Zagłębie Lubin - Korona Kielce
        (NEWID(), @RoundId13, @TeamId9,  @TeamId16, NULL, NULL, 0, 0), -- Wisła Płock - Piast Gliwice
        (NEWID(), @RoundId13, @TeamId3,  @TeamId27, NULL, NULL, 0, 0), -- Lech Poznań - Wisła Kraków

        -- Kolejka 14 
        (NEWID(), @RoundId14, @TeamId8,  @TeamId26, NULL, NULL, 0, 0), -- Górnik Zabrze - Wieczysta Kraków
        (NEWID(), @RoundId14, @TeamId1,  @TeamId13, NULL, NULL, 0, 0), -- Jagiellonia Białystok - GKS Katowice
        (NEWID(), @RoundId14, @TeamId15, @TeamId11, NULL, NULL, 0, 0), -- Korona Kielce - Radomiak Radom
        (NEWID(), @RoundId14, @TeamId3,  @TeamId14, NULL, NULL, 0, 0), -- Lech Poznań - Zagłębie Lubin
        (NEWID(), @RoundId14, @TeamId2,  @TeamId5,  NULL, NULL, 0, 0), -- Legia Warszawa - Raków Częstochowa
        (NEWID(), @RoundId14, @TeamId12, @TeamId9,  NULL, NULL, 0, 0), -- Motor Lublin - Wisła Płock
        (NEWID(), @RoundId14, @TeamId20, @TeamId16, NULL, NULL, 0, 0), -- Śląsk Wrocław - Piast Gliwice
        (NEWID(), @RoundId14, @TeamId4,  @TeamId6,  NULL, NULL, 0, 0), -- Widzew Łódź - Pogoń Szczecin
        (NEWID(), @RoundId14, @TeamId27, @TeamId7,  NULL, NULL, 0, 0), -- Wisła Kraków - Cracovia

        -- Kolejka 15
        (NEWID(), @RoundId15, @TeamId7,  @TeamId1,  NULL, NULL, 0, 0), -- Cracovia - Jagiellonia Białystok
        (NEWID(), @RoundId15, @TeamId13, @TeamId3,  NULL, NULL, 0, 0), -- GKS Katowice - Lech Poznań
        (NEWID(), @RoundId15, @TeamId16, @TeamId15, NULL, NULL, 0, 0), -- Piast Gliwice - Korona Kielce
        (NEWID(), @RoundId15, @TeamId6,  @TeamId20, NULL, NULL, 0, 0), -- Pogoń Szczecin - Śląsk Wrocław
        (NEWID(), @RoundId15, @TeamId11, @TeamId27, NULL, NULL, 0, 0), -- Radomiak Radom - Wisła Kraków
        (NEWID(), @RoundId15, @TeamId5,  @TeamId4,  NULL, NULL, 0, 0), -- Raków Częstochowa - Widzew Łódź
        (NEWID(), @RoundId15, @TeamId26, @TeamId12, NULL, NULL, 0, 0), -- Wieczysta Kraków - Motor Lublin
        (NEWID(), @RoundId15, @TeamId9,  @TeamId2,  NULL, NULL, 0, 0), -- Wisła Płock - Legia Warszawa
        (NEWID(), @RoundId15, @TeamId14, @TeamId8,  NULL, NULL, 0, 0), -- Zagłębie Lubin - Górnik Zabrze

        -- Kolejka 16 
        (NEWID(), @RoundId16, @TeamId15, @TeamId7, NULL, NULL, 0, 0), -- Korona Kielce - Cracovia
    (NEWID(), @RoundId16, @TeamId27, @TeamId12, NULL, NULL, 0, 0), -- Wisła Kraków - Motor Lublin
    (NEWID(), @RoundId16, @TeamId2, @TeamId26, NULL, NULL, 0, 0), -- Legia Warszawa - Wieczysta Kraków
    (NEWID(), @RoundId16, @TeamId5, @TeamId16, NULL, NULL, 0, 0), -- Raków Częstochowa - Piast Gliwice
    (NEWID(), @RoundId16, @TeamId20, @TeamId13, NULL, NULL, 0, 0), -- Śląsk Wrocław - GKS Katowice
    (NEWID(), @RoundId16, @TeamId4, @TeamId14, NULL, NULL, 0, 0), -- Widzew Łódź - Zagłębie Lubin
    (NEWID(), @RoundId16, @TeamId1, @TeamId11, NULL, NULL, 0, 0), -- Jagiellonia Białystok - Radomiak Radom
    (NEWID(), @RoundId16, @TeamId8, @TeamId9, NULL, NULL, 0, 0), -- Górnik Zabrze - Wisła Płock
    (NEWID(), @RoundId16, @TeamId3, @TeamId6, NULL, NULL, 0, 0), -- Lech Poznań - Pogoń Szczecin

 -- Kolejka 17, sezon 2026/2027
    (NEWID(), @RoundId17, @TeamId12, @TeamId3, NULL, NULL, 0, 0), -- Motor Lublin - Lech Poznań
    (NEWID(), @RoundId17, @TeamId26, @TeamId15, NULL, NULL, 0, 0), -- Wieczysta Kraków - Korona Kielce
    (NEWID(), @RoundId17, @TeamId7, @TeamId4, NULL, NULL, 0, 0), -- Cracovia - Widzew Łódź
    (NEWID(), @RoundId17, @TeamId11, @TeamId5, NULL, NULL, 0, 0), -- Radomiak Radom - Raków Częstochowa
    (NEWID(), @RoundId17, @TeamId6, @TeamId8, NULL, NULL, 0, 0), -- Pogoń Szczecin - Górnik Zabrze
    (NEWID(), @RoundId17, @TeamId13, @TeamId2, NULL, NULL, 0, 0), -- GKS Katowice - Legia Warszawa
    (NEWID(), @RoundId17, @TeamId16, @TeamId1, NULL, NULL, 0, 0), -- Piast Gliwice - Jagiellonia Białystok
    (NEWID(), @RoundId17, @TeamId14, @TeamId27, NULL, NULL, 0, 0), -- Zagłębie Lubin - Wisła Kraków
    (NEWID(), @RoundId17, @TeamId9, @TeamId20, NULL, NULL, 0, 0), -- Wisła Płock - Śląsk Wrocław

 -- Kolejka 18, sezon 2026/2027
    (NEWID(), @RoundId18, @TeamId12, @TeamId4, NULL, NULL, 0, 0), -- Motor Lublin - Widzew Łódź
    (NEWID(), @RoundId18, @TeamId15, @TeamId1, NULL, NULL, 0, 0), -- Korona Kielce - Jagiellonia Białystok
    (NEWID(), @RoundId18, @TeamId26, @TeamId11, NULL, NULL, 0, 0), -- Wieczysta Kraków - Radomiak Radom
    (NEWID(), @RoundId18, @TeamId7, @TeamId3, NULL, NULL, 0, 0), -- Cracovia - Lech Poznań
    (NEWID(), @RoundId18, @TeamId2, @TeamId6, NULL, NULL, 0, 0), -- Legia Warszawa - Pogoń Szczecin
    (NEWID(), @RoundId18, @TeamId13, @TeamId27, NULL, NULL, 0, 0), -- GKS Katowice - Wisła Kraków
    (NEWID(), @RoundId18, @TeamId20, @TeamId8, NULL, NULL, 0, 0), -- Śląsk Wrocław - Górnik Zabrze
    (NEWID(), @RoundId18, @TeamId16, @TeamId14, NULL, NULL, 0, 0), -- Piast Gliwice - Zagłębie Lubin
    (NEWID(), @RoundId18, @TeamId9, @TeamId5, NULL, NULL, 0, 0), -- Wisła Płock - Raków Częstochowa

 -- Kolejka 19, sezon 2026/2027
    (NEWID(), @RoundId19, @TeamId27, @TeamId16, NULL, NULL, 0, 0), -- Wisła Kraków - Piast Gliwice
    (NEWID(), @RoundId19, @TeamId11, @TeamId13, NULL, NULL, 0, 0), -- Radomiak Radom - GKS Katowice
    (NEWID(), @RoundId19, @TeamId6, @TeamId7, NULL, NULL, 0, 0), -- Pogoń Szczecin - Cracovia
    (NEWID(), @RoundId19, @TeamId5, @TeamId20, NULL, NULL, 0, 0), -- Raków Częstochowa - Śląsk Wrocław
    (NEWID(), @RoundId19, @TeamId4, @TeamId9, NULL, NULL, 0, 0), -- Widzew Łódź - Wisła Płock
    (NEWID(), @RoundId19, @TeamId1, @TeamId12, NULL, NULL, 0, 0), -- Jagiellonia Białystok - Motor Lublin
    (NEWID(), @RoundId19, @TeamId8, @TeamId15, NULL, NULL, 0, 0), -- Górnik Zabrze - Korona Kielce
    (NEWID(), @RoundId19, @TeamId14, @TeamId2, NULL, NULL, 0, 0), -- Zagłębie Lubin - Legia Warszawa
    (NEWID(), @RoundId19, @TeamId3, @TeamId26, NULL, NULL, 0, 0), -- Lech Poznań - Wieczysta Kraków

 -- Kolejka 20, sezon 2026/2027
    (NEWID(), @RoundId20, @TeamId12, @TeamId6, NULL, NULL, 0, 0), -- Motor Lublin - Pogoń Szczecin
    (NEWID(), @RoundId20, @TeamId26, @TeamId13, NULL, NULL, 0, 0), -- Wieczysta Kraków - GKS Katowice
    (NEWID(), @RoundId20, @TeamId7, @TeamId20, NULL, NULL, 0, 0), -- Cracovia - Śląsk Wrocław
    (NEWID(), @RoundId20, @TeamId2, @TeamId15, NULL, NULL, 0, 0), -- Legia Warszawa - Korona Kielce
    (NEWID(), @RoundId20, @TeamId4, @TeamId1, NULL, NULL, 0, 0), -- Widzew Łódź - Jagiellonia Białystok
    (NEWID(), @RoundId20, @TeamId16, @TeamId3, NULL, NULL, 0, 0), -- Piast Gliwice - Lech Poznań
    (NEWID(), @RoundId20, @TeamId8, @TeamId11, NULL, NULL, 0, 0), -- Górnik Zabrze - Radomiak Radom
    (NEWID(), @RoundId20, @TeamId14, @TeamId5, NULL, NULL, 0, 0), -- Zagłębie Lubin - Raków Częstochowa
    (NEWID(), @RoundId20, @TeamId9, @TeamId27, NULL, NULL, 0, 0), -- Wisła Płock - Wisła Kraków

 -- Kolejka 21, sezon 2026/2027
    (NEWID(), @RoundId21, @TeamId15, @TeamId4, NULL, NULL, 0, 0), -- Korona Kielce - Widzew Łódź
    (NEWID(), @RoundId21, @TeamId26, @TeamId16, NULL, NULL, 0, 0), -- Wieczysta Kraków - Piast Gliwice
    (NEWID(), @RoundId21, @TeamId27, @TeamId8, NULL, NULL, 0, 0), -- Wisła Kraków - Górnik Zabrze
    (NEWID(), @RoundId21, @TeamId11, @TeamId2, NULL, NULL, 0, 0), -- Radomiak Radom - Legia Warszawa
    (NEWID(), @RoundId21, @TeamId6, @TeamId1, NULL, NULL, 0, 0), -- Pogoń Szczecin - Jagiellonia Białystok
    (NEWID(), @RoundId21, @TeamId5, @TeamId7, NULL, NULL, 0, 0), -- Raków Częstochowa - Cracovia
    (NEWID(), @RoundId21, @TeamId13, @TeamId12, NULL, NULL, 0, 0), -- GKS Katowice - Motor Lublin
    (NEWID(), @RoundId21, @TeamId20, @TeamId14, NULL, NULL, 0, 0), -- Śląsk Wrocław - Zagłębie Lubin
    (NEWID(), @RoundId21, @TeamId3, @TeamId9, NULL, NULL, 0, 0), -- Lech Poznań - Wisła Płock

 -- Kolejka 22, sezon 2026/2027
    (NEWID(), @RoundId22, @TeamId12, @TeamId15, NULL, NULL, 0, 0), -- Motor Lublin - Korona Kielce
    (NEWID(), @RoundId22, @TeamId26, @TeamId7, NULL, NULL, 0, 0), -- Wieczysta Kraków - Cracovia
    (NEWID(), @RoundId22, @TeamId27, @TeamId6, NULL, NULL, 0, 0), -- Wisła Kraków - Pogoń Szczecin
    (NEWID(), @RoundId22, @TeamId2, @TeamId16, NULL, NULL, 0, 0), -- Legia Warszawa - Piast Gliwice
    (NEWID(), @RoundId22, @TeamId4, @TeamId20, NULL, NULL, 0, 0), -- Widzew Łódź - Śląsk Wrocław
    (NEWID(), @RoundId22, @TeamId1, @TeamId3, NULL, NULL, 0, 0), -- Jagiellonia Białystok - Lech Poznań
    (NEWID(), @RoundId22, @TeamId8, @TeamId5, NULL, NULL, 0, 0), -- Górnik Zabrze - Raków Częstochowa
    (NEWID(), @RoundId22, @TeamId14, @TeamId11, NULL, NULL, 0, 0), -- Zagłębie Lubin - Radomiak Radom
    (NEWID(), @RoundId22, @TeamId9, @TeamId13, NULL, NULL, 0, 0), -- Wisła Płock - GKS Katowice

 -- Kolejka 23, sezon 2026/2027
    (NEWID(), @RoundId23, @TeamId15, @TeamId9, NULL, NULL, 0, 0), -- Korona Kielce - Wisła Płock
    (NEWID(), @RoundId23, @TeamId26, @TeamId27, NULL, NULL, 0, 0), -- Wieczysta Kraków - Wisła Kraków
    (NEWID(), @RoundId23, @TeamId7, @TeamId11, NULL, NULL, 0, 0), -- Cracovia - Radomiak Radom
    (NEWID(), @RoundId23, @TeamId6, @TeamId14, NULL, NULL, 0, 0), -- Pogoń Szczecin - Zagłębie Lubin
    (NEWID(), @RoundId23, @TeamId13, @TeamId8, NULL, NULL, 0, 0), -- GKS Katowice - Górnik Zabrze
    (NEWID(), @RoundId23, @TeamId20, @TeamId2, NULL, NULL, 0, 0), -- Śląsk Wrocław - Legia Warszawa
    (NEWID(), @RoundId23, @TeamId16, @TeamId12, NULL, NULL, 0, 0), -- Piast Gliwice - Motor Lublin
    (NEWID(), @RoundId23, @TeamId1, @TeamId5, NULL, NULL, 0, 0), -- Jagiellonia Białystok - Raków Częstochowa
    (NEWID(), @RoundId23, @TeamId3, @TeamId4, NULL, NULL, 0, 0), -- Lech Poznań - Widzew Łódź

 -- Kolejka 24, sezon 2026/2027
    (NEWID(), @RoundId24, @TeamId27, @TeamId15, NULL, NULL, 0, 0), -- Wisła Kraków - Korona Kielce
    (NEWID(), @RoundId24, @TeamId11, @TeamId4, NULL, NULL, 0, 0), -- Radomiak Radom - Widzew Łódź
    (NEWID(), @RoundId24, @TeamId2, @TeamId12, NULL, NULL, 0, 0), -- Legia Warszawa - Motor Lublin
    (NEWID(), @RoundId24, @TeamId5, @TeamId3, NULL, NULL, 0, 0), -- Raków Częstochowa - Lech Poznań
    (NEWID(), @RoundId24, @TeamId13, @TeamId16, NULL, NULL, 0, 0), -- GKS Katowice - Piast Gliwice
    (NEWID(), @RoundId24, @TeamId20, @TeamId1, NULL, NULL, 0, 0), -- Śląsk Wrocław - Jagiellonia Białystok
    (NEWID(), @RoundId24, @TeamId8, @TeamId7, NULL, NULL, 0, 0), -- Górnik Zabrze - Cracovia
    (NEWID(), @RoundId24, @TeamId14, @TeamId26, NULL, NULL, 0, 0), -- Zagłębie Lubin - Wieczysta Kraków
    (NEWID(), @RoundId24, @TeamId9, @TeamId6, NULL, NULL, 0, 0), -- Wisła Płock - Pogoń Szczecin

 -- Kolejka 25, sezon 2026/2027
    (NEWID(), @RoundId25, @TeamId12, @TeamId5, NULL, NULL, 0, 0), -- Motor Lublin - Raków Częstochowa
    (NEWID(), @RoundId25, @TeamId15, @TeamId20, NULL, NULL, 0, 0), -- Korona Kielce - Śląsk Wrocław
    (NEWID(), @RoundId25, @TeamId26, @TeamId6, NULL, NULL, 0, 0), -- Wieczysta Kraków - Pogoń Szczecin
    (NEWID(), @RoundId25, @TeamId7, @TeamId9, NULL, NULL, 0, 0), -- Cracovia - Wisła Płock
    (NEWID(), @RoundId25, @TeamId13, @TeamId14, NULL, NULL, 0, 0), -- GKS Katowice - Zagłębie Lubin
    (NEWID(), @RoundId25, @TeamId4, @TeamId2, NULL, NULL, 0, 0), -- Widzew Łódź - Legia Warszawa
    (NEWID(), @RoundId25, @TeamId16, @TeamId11, NULL, NULL, 0, 0), -- Piast Gliwice - Radomiak Radom
    (NEWID(), @RoundId25, @TeamId1, @TeamId27, NULL, NULL, 0, 0), -- Jagiellonia Białystok - Wisła Kraków
    (NEWID(), @RoundId25, @TeamId3, @TeamId8, NULL, NULL, 0, 0), -- Lech Poznań - Górnik Zabrze

 -- Kolejka 26, sezon 2026/2027
    (NEWID(), @RoundId26, @TeamId26, @TeamId4, NULL, NULL, 0, 0), -- Wieczysta Kraków - Widzew Łódź
    (NEWID(), @RoundId26, @TeamId7, @TeamId13, NULL, NULL, 0, 0), -- Cracovia - GKS Katowice
    (NEWID(), @RoundId26, @TeamId11, @TeamId3, NULL, NULL, 0, 0), -- Radomiak Radom - Lech Poznań
    (NEWID(), @RoundId26, @TeamId2, @TeamId1, NULL, NULL, 0, 0), -- Legia Warszawa - Jagiellonia Białystok
    (NEWID(), @RoundId26, @TeamId6, @TeamId16, NULL, NULL, 0, 0), -- Pogoń Szczecin - Piast Gliwice
    (NEWID(), @RoundId26, @TeamId5, @TeamId15, NULL, NULL, 0, 0), -- Raków Częstochowa - Korona Kielce
    (NEWID(), @RoundId26, @TeamId20, @TeamId27, NULL, NULL, 0, 0), -- Śląsk Wrocław - Wisła Kraków
    (NEWID(), @RoundId26, @TeamId8, @TeamId12, NULL, NULL, 0, 0), -- Górnik Zabrze - Motor Lublin
    (NEWID(), @RoundId26, @TeamId9, @TeamId14, NULL, NULL, 0, 0), -- Wisła Płock - Zagłębie Lubin

 -- Kolejka 27, sezon 2026/2027
    (NEWID(), @RoundId27, @TeamId12, @TeamId11, NULL, NULL, 0, 0), -- Motor Lublin - Radomiak Radom
    (NEWID(), @RoundId27, @TeamId15, @TeamId6, NULL, NULL, 0, 0), -- Korona Kielce - Pogoń Szczecin
    (NEWID(), @RoundId27, @TeamId27, @TeamId2, NULL, NULL, 0, 0), -- Wisła Kraków - Legia Warszawa
    (NEWID(), @RoundId27, @TeamId13, @TeamId5, NULL, NULL, 0, 0), -- GKS Katowice - Raków Częstochowa
    (NEWID(), @RoundId27, @TeamId4, @TeamId16, NULL, NULL, 0, 0), -- Widzew Łódź - Piast Gliwice
    (NEWID(), @RoundId27, @TeamId8, @TeamId1, NULL, NULL, 0, 0), -- Górnik Zabrze - Jagiellonia Białystok
    (NEWID(), @RoundId27, @TeamId14, @TeamId7, NULL, NULL, 0, 0), -- Zagłębie Lubin - Cracovia
    (NEWID(), @RoundId27, @TeamId9, @TeamId26, NULL, NULL, 0, 0), -- Wisła Płock - Wieczysta Kraków
    (NEWID(), @RoundId27, @TeamId3, @TeamId20, NULL, NULL, 0, 0), -- Lech Poznań - Śląsk Wrocław

 -- Kolejka 28, sezon 2026/2027
    (NEWID(), @RoundId28, @TeamId15, @TeamId3, NULL, NULL, 0, 0), -- Korona Kielce - Lech Poznań
    (NEWID(), @RoundId28, @TeamId27, @TeamId4, NULL, NULL, 0, 0), -- Wisła Kraków - Widzzew Łódź
    (NEWID(), @RoundId28, @TeamId11, @TeamId9, NULL, NULL, 0, 0), -- Radomiak Radom - Wisła Płock
    (NEWID(), @RoundId28, @TeamId2, @TeamId7, NULL, NULL, 0, 0), -- Legia Warszawa - Cracovia
    (NEWID(), @RoundId28, @TeamId6, @TeamId13, NULL, NULL, 0, 0), -- Pogoń Szczecin - GKS Katowice
    (NEWID(), @RoundId28, @TeamId5, @TeamId26, NULL, NULL, 0, 0), -- Raków Częstochowa - Wieczysta Kraków
    (NEWID(), @RoundId28, @TeamId20, @TeamId12, NULL, NULL, 0, 0), -- Śląsk Wrocław - Motor Lublin
    (NEWID(), @RoundId28, @TeamId16, @TeamId8, NULL, NULL, 0, 0), -- Piast Gliwice - Górnik Zabrze
    (NEWID(), @RoundId28, @TeamId1, @TeamId14, NULL, NULL, 0, 0), -- Jagiellonia Białystok - Zagłębie Lubin

 -- Kolejka 29, sezon 2026/2027
    (NEWID(), @RoundId29, @TeamId26, @TeamId20, NULL, NULL, 0, 0), -- Wieczysta Kraków - Śląsk Wrocław
    (NEWID(), @RoundId29, @TeamId7, @TeamId16, NULL, NULL, 0, 0), -- Cracovia - Piast Gliwice
    (NEWID(), @RoundId29, @TeamId11, @TeamId6, NULL, NULL, 0, 0), -- Radomiak Radom - Pogoń Szczecin
    (NEWID(), @RoundId29, @TeamId5, @TeamId27, NULL, NULL, 0, 0), -- Raków Częstochowa - Wisła Kraków
    (NEWID(), @RoundId29, @TeamId13, @TeamId15, NULL, NULL, 0, 0), -- GKS Katowice - Korona Kielce
    (NEWID(), @RoundId29, @TeamId8, @TeamId4, NULL, NULL, 0, 0), -- Górnik Zabrze - Widzew Łódź
    (NEWID(), @RoundId29, @TeamId14, @TeamId12, NULL, NULL, 0, 0), -- Zagłębie Lubin - Motor Lublin
    (NEWID(), @RoundId29, @TeamId9, @TeamId1, NULL, NULL, 0, 0), -- Wisła Płock - Jagiellonia Białystok
    (NEWID(), @RoundId29, @TeamId3, @TeamId2, NULL, NULL, 0, 0), -- Lech Poznań - Legia Warszawa

 -- Kolejka 30, sezon 2026/2027
    (NEWID(), @RoundId30, @TeamId12, @TeamId7, NULL, NULL, 0, 0), -- Motor Lublin - Cracovia
    (NEWID(), @RoundId30, @TeamId15, @TeamId14, NULL, NULL, 0, 0), -- Korona Kielce - Zagłębie Lubin
    (NEWID(), @RoundId30, @TeamId27, @TeamId3, NULL, NULL, 0, 0), -- Wisła Kraków - Lech Poznań
    (NEWID(), @RoundId30, @TeamId2, @TeamId8, NULL, NULL, 0, 0), -- Legia Warszawa - Górnik Zabrze
    (NEWID(), @RoundId30, @TeamId6, @TeamId5, NULL, NULL, 0, 0), -- Pogoń Szczecin - Raków Częstochowa
    (NEWID(), @RoundId30, @TeamId20, @TeamId11, NULL, NULL, 0, 0), -- Śląsk Wrocław - Radomiak Radom
    (NEWID(), @RoundId30, @TeamId4, @TeamId13, NULL, NULL, 0, 0), -- Widzew Łódź - GKS Katowice
    (NEWID(), @RoundId30, @TeamId16, @TeamId9, NULL, NULL, 0, 0), -- Piast Gliwice - Wisła Płock
    (NEWID(), @RoundId30, @TeamId1, @TeamId26, NULL, NULL, 0, 0), -- Jagiellonia Białystok - Wieczysta Kraków

 -- Kolejka 31, sezon 2026/2027
    (NEWID(), @RoundId31, @TeamId26, @TeamId8, NULL, NULL, 0, 0), -- Wieczysta Kraków - Górnik Zabrze
    (NEWID(), @RoundId31, @TeamId7, @TeamId27, NULL, NULL, 0, 0), -- Cracovia - Wisła Kraków
    (NEWID(), @RoundId31, @TeamId11, @TeamId15, NULL, NULL, 0, 0), -- Radomiak Radom - Korona Kielce
    (NEWID(), @RoundId31, @TeamId6, @TeamId4, NULL, NULL, 0, 0), -- Pogoń Szczecin - Widzew Łódź
    (NEWID(), @RoundId31, @TeamId5, @TeamId2, NULL, NULL, 0, 0), -- Raków Częstochowa - Legia Warszawa
    (NEWID(), @RoundId31, @TeamId13, @TeamId1, NULL, NULL, 0, 0), -- GKS Katowice - Jagiellonia Białystok
    (NEWID(), @RoundId31, @TeamId16, @TeamId20, NULL, NULL, 0, 0), -- Piast Gliwice - Śląsk Wrocław
    (NEWID(), @RoundId31, @TeamId14, @TeamId3, NULL, NULL, 0, 0), -- Zagłębie Lubin - Lech Poznań
    (NEWID(), @RoundId31, @TeamId9, @TeamId12, NULL, NULL, 0, 0), -- Wisła Płock - Motor Lublin

 -- Kolejka 32, sezon 2026/2027
    (NEWID(), @RoundId32, @TeamId12, @TeamId26, NULL, NULL, 0, 0), -- Motor Lublin - Wieczysta Kraków
    (NEWID(), @RoundId32, @TeamId15, @TeamId16, NULL, NULL, 0, 0), -- Korona Kielce - Piast Gliwice
    (NEWID(), @RoundId32, @TeamId27, @TeamId11, NULL, NULL, 0, 0), -- Wisła Kraków - Radomiak Radom
    (NEWID(), @RoundId32, @TeamId2, @TeamId9, NULL, NULL, 0, 0), -- Legia Warszawa - Wisła Płock
    (NEWID(), @RoundId32, @TeamId20, @TeamId6, NULL, NULL, 0, 0), -- Śląsk Wrocław - Pogoń Szczecin
    (NEWID(), @RoundId32, @TeamId4, @TeamId5, NULL, NULL, 0, 0), -- Widzew Łódź - Raków Częstochowa
    (NEWID(), @RoundId32, @TeamId1, @TeamId7, NULL, NULL, 0, 0), -- Jagiellonia Białystok - Cracovia
    (NEWID(), @RoundId32, @TeamId8, @TeamId14, NULL, NULL, 0, 0), -- Górnik Zabrze - Zagłębie Lubin
    (NEWID(), @RoundId32, @TeamId3, @TeamId13, NULL, NULL, 0, 0), -- Lech Poznań - GKS Katowice

 -- Kolejka 33, sezon 2026/2027
    (NEWID(), @RoundId33, @TeamId12, @TeamId27, NULL, NULL, 0, 0), -- Motor Lublin - Wisła Kraków
    (NEWID(), @RoundId33, @TeamId26, @TeamId2,  NULL, NULL, 0, 0), -- Wieczysta Kraków - Legia Warszawa
    (NEWID(), @RoundId33, @TeamId7,  @TeamId15, NULL, NULL, 0, 0), -- Cracovia - Korona Kielce
    (NEWID(), @RoundId33, @TeamId11, @TeamId1,  NULL, NULL, 0, 0), -- Radomiak Radom - Jagiellonia Białystok
    (NEWID(), @RoundId33, @TeamId6,  @TeamId3,  NULL, NULL, 0, 0), -- Pogoń Szczecin - Lech Poznań
    (NEWID(), @RoundId33, @TeamId13, @TeamId20, NULL, NULL, 0, 0), -- GKS Katowice - Śląsk Wrocław
    (NEWID(), @RoundId33, @TeamId16, @TeamId5,  NULL, NULL, 0, 0), -- Piast Gliwice - Raków Częstochowa
    (NEWID(), @RoundId33, @TeamId14, @TeamId4,  NULL, NULL, 0, 0), -- Zagłębie Lubin - Widzew Łódź
    (NEWID(), @RoundId33, @TeamId9,  @TeamId8,  NULL, NULL, 0, 0), -- Wisła Płock - Górnik Zabrze

 -- Kolejka 34, sezon 2026/2027
    (NEWID(), @RoundId34, @TeamId15, @TeamId26, NULL, NULL, 0, 0), -- Korona Kielce - Wieczysta Kraków
    (NEWID(), @RoundId34, @TeamId27, @TeamId14, NULL, NULL, 0, 0), -- Wisła Kraków - Zagłębie Lubin
    (NEWID(), @RoundId34, @TeamId2,  @TeamId13, NULL, NULL, 0, 0), -- Legia Warszawa - GKS Katowice
    (NEWID(), @RoundId34, @TeamId5,  @TeamId11, NULL, NULL, 0, 0), -- Raków Częstochowa - Radomiak Radom
    (NEWID(), @RoundId34, @TeamId20, @TeamId9,  NULL, NULL, 0, 0), -- Śląsk Wrocław - Wisła Płock
    (NEWID(), @RoundId34, @TeamId4,  @TeamId7,  NULL, NULL, 0, 0), -- Widzew Łódź - Cracovia
    (NEWID(), @RoundId34, @TeamId1,  @TeamId16, NULL, NULL, 0, 0), -- Jagiellonia Białystok - Piast Gliwice
    (NEWID(), @RoundId34, @TeamId8,  @TeamId6,  NULL, NULL, 0, 0), -- Górnik Zabrze - Pogoń Szczecin
    (NEWID(), @RoundId34, @TeamId3,  @TeamId12, NULL, NULL, 0, 0)  -- Lech Poznań - Motor Lublin

    COMMIT TRANSACTION
    PRINT '✅ SUKCES! Ekstraklasa 2026/2027'
    PRINT '   - max match rounds: 34'
    PRINT '   - matches: 306'
    PRINT '   - teams: 18'
    PRINT '   - Sezon: 2026/2027'
    PRINT '   - Status:'
    PRINT '     • played rounds 1-28:'
    PRINT '     • not played rounds 29-34:'
    PRINT '   - last update 20.08.2026'
    PRINT '//////'

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT '❌ Cannot insert! Ekstraklasa 2026/2027'
    PRINT 'Error: ' + ERROR_MESSAGE()
END CATCH
