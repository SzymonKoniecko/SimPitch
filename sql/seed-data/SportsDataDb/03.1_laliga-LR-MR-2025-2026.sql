USE SportsDataDb;
GO

DECLARE 
    @CountryId UNIQUEIDENTIFIER, 
    @LeagueId UNIQUEIDENTIFIER,
    @CurrentDateTime DATETIME2 = GETDATE(),

    -- Team IDs (20 drużyn)
    @TeamId1 UNIQUEIDENTIFIER = 'c1a2b3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d',   -- Real Madrid
    @TeamId2 UNIQUEIDENTIFIER = 'f8e7d6c5-b4a3-2109-8f7e-6d5c4b3a2918',   -- FC Barcelona
    @TeamId3 UNIQUEIDENTIFIER = 'a9b8c7d6-e5f4-3210-9a8b-7c6d5e4f3a29',   -- Atlético Madrid
    @TeamId4 UNIQUEIDENTIFIER = 'd3c2b1a0-9f8e-7d6c-5b4a-3928170615f4',   -- Valencia CF
    @TeamId5 UNIQUEIDENTIFIER = 'e1f2a3b4-c5d6-7e8f-9a0b-1c2d3e4f5a6b',   -- Real Sociedad
    @TeamId6 UNIQUEIDENTIFIER = 'b0a9f8e7-d6c5-b4a3-2918-07f6e5d4c3b2',   -- Sevilla FC
    @TeamId7 UNIQUEIDENTIFIER = '7c8d9e0f-1a2b-3c4d-5e6f-7a8b9c0d1e2f',   -- Villarreal CF
    @TeamId8 UNIQUEIDENTIFIER = '6f7e8d9c-ab0f-1e2d-3c4b-5a6978879695',   -- Real Betis
    @TeamId9 UNIQUEIDENTIFIER = '5a4b3c2d-1e0f-9a8b-7c6d-5e4f3a2b1c0d',   -- Athletic Bilbao
    @TeamId10 UNIQUEIDENTIFIER = '4f5e6d7c-8b9a-0f1e-2d3c-4b5a6f7e8d9c',  -- Osasuna
    @TeamId11 UNIQUEIDENTIFIER = '3e4d5c6b-7a8f-9e0d-1c2b-3a4f5e6d7c8b',  -- Celta Vigo
    @TeamId12 UNIQUEIDENTIFIER = '2d3c4b5a-6f7e-8d9c-0b1a-2f3e4d5c6b7a',  -- RCD Mallorca
    @TeamId13 UNIQUEIDENTIFIER = '1c2b3a4f-5e6d-7c8b-9a0f-1e2d3c4b5a6f',  -- Real Valladolid
    @TeamId14 UNIQUEIDENTIFIER = '0b1a2f3e-4d5c-6b7a-8f9e-0d1c2b3a4f5e',  -- Getafe CF
    @TeamId15 UNIQUEIDENTIFIER = 'f9e8d7c6-b5a4-3291-807f-6e5d4c3b2a19',  -- Rayo Vallecano
    @TeamId16 UNIQUEIDENTIFIER = 'e8d7c6b5-a4f3-2190-9f8e-7d6c5b4a3918',  -- Girona FC
    @TeamId17 UNIQUEIDENTIFIER = 'd7c6b5a4-f3e2-1908-8f7e-6d5c4b3a2917',  -- Real Oviedo
    @TeamId18 UNIQUEIDENTIFIER = 'c6b5a4f3-e2d1-0897-7f6e-5d4c3b2a1916',  -- Las Palmas
    @TeamId19 UNIQUEIDENTIFIER = 'b5a4f3e2-d1c0-0796-6f5e-4d3c2b1a0815',  -- Alavés
    @TeamId20 UNIQUEIDENTIFIER = 'a4f3e2d1-c0b9-0695-5f4e-3d2c1b0a0f14',  -- Cádiz CF

    -- Round IDs (38 rounds)
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
    @RoundId34 UNIQUEIDENTIFIER = NEWID(),
    @RoundId35 UNIQUEIDENTIFIER = NEWID(),
    @RoundId36 UNIQUEIDENTIFIER = NEWID(),
    @RoundId37 UNIQUEIDENTIFIER = NEWID(),
    @RoundId38 UNIQUEIDENTIFIER = NEWID();

-- Get Country and League IDs
SELECT 
    @CountryId = Id
FROM dbo.Country
WHERE [Code] = 'ES'

SELECT
    @LeagueId = Id
FROM dbo.League
WHERE [Name] = 'La Liga' AND CountryId = @CountryId

BEGIN TRANSACTION

BEGIN TRY

    -- ================================================================
    -- CREATE ROUNDS (LeagueRound) - 38 rounds
    -- ================================================================
    
    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.LeagueRound WHERE SeasonYear = '2025/2026' AND LeagueId = @LeagueId)
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
            (@RoundId34, @LeagueId, '2025/2026', 34),
            (@RoundId35, @LeagueId, '2025/2026', 35),
            (@RoundId36, @LeagueId, '2025/2026', 36),
            (@RoundId37, @LeagueId, '2025/2026', 37),
            (@RoundId38, @LeagueId, '2025/2026', 38)

    -- ================================================================
    -- INSERT MATCHES (MatchRound) - 380 matches (20 teams × 19 opponents × 2)
    -- ================================================================
    
    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.MatchRound WHERE RoundId IN (
        SELECT Id FROM LeagueRound WHERE SeasonYear = '2025/2026' AND LeagueId = @LeagueId
    ))
    INSERT INTO dbo.MatchRound (Id, RoundId, HomeTeamId, AwayTeamId, HomeGoals, AwayGoals, IsDraw, IsPlayed)
        VALUES 
        -- KOLEJKA 1 (@RoundId1)
            (NEWID(), @RoundId1, @TeamId1, @TeamId19, 4, 1, 0, 1),  -- Real Madrid CF vs Deportivo Alavés
            (NEWID(), @RoundId1, @TeamId2, @TeamId14, 2, 0, 0, 1),  -- FC Barcelona vs Getafe CF
            (NEWID(), @RoundId1, @TeamId3, @TeamId6, 1, 1, 1, 1),   -- Atlético Madrid vs Sevilla FC
            (NEWID(), @RoundId1, @TeamId5, @TeamId16, 2, 2, 1, 1),  -- Real Sociedad vs Girona FC
            (NEWID(), @RoundId1, @TeamId4, @TeamId11, 1, 0, 0, 1),  -- Valencia CF vs RC Celta de Vigo
            (NEWID(), @RoundId1, @TeamId8, @TeamId23, 3, 0, 0, 1),  -- Real Betis vs Elche CF
            (NEWID(), @RoundId1, @TeamId7, @TeamId15, 2, 1, 0, 1),  -- Villarreal CF vs Rayo Vallecano
            (NEWID(), @RoundId1, @TeamId9, @TeamId10, 0, 0, 1, 1),  -- Athletic Club Bilbao vs Osasuna Pampeluna
            (NEWID(), @RoundId1, @TeamId12, @TeamId21, 1, 2, 0, 1), -- RCD Mallorca vs RCD Espanyol
            (NEWID(), @RoundId1, @TeamId18, @TeamId13, 0, 1, 0, 1), -- UD Las Palmas vs Real Valladolid CF

        -- KOLEJKA 2 (@RoundId2)
            (NEWID(), @RoundId2, @TeamId19, @TeamId2, 0, 3, 0, 1),  -- Deportivo Alavés vs FC Barcelona
            (NEWID(), @RoundId2, @TeamId6, @TeamId1, 1, 2, 0, 1),   -- Sevilla FC vs Real Madrid CF
            (NEWID(), @RoundId2, @TeamId14, @TeamId3, 0, 1, 0, 1),  -- Getafe CF vs Atlético Madrid
            (NEWID(), @RoundId2, @TeamId16, @TeamId4, 1, 0, 0, 1),  -- Girona FC vs Valencia CF
            (NEWID(), @RoundId2, @TeamId11, @TeamId5, 1, 1, 1, 1),  -- RC Celta de Vigo vs Real Sociedad
            (NEWID(), @RoundId2, @TeamId23, @TeamId7, 0, 2, 0, 1),  -- Elche CF vs Villarreal CF
            (NEWID(), @RoundId2, @TeamId15, @TeamId8, 2, 2, 1, 1),  -- Rayo Vallecano vs Real Betis
            (NEWID(), @RoundId2, @TeamId10, @TeamId12, 1, 0, 0, 1), -- Osasuna Pampeluna vs RCD Mallorca
            (NEWID(), @RoundId2, @TeamId21, @TeamId18, 2, 0, 0, 1), -- RCD Espanyol vs UD Las Palmas
            (NEWID(), @RoundId2, @TeamId13, @TeamId9, 1, 3, 0, 1),  -- Real Valladolid CF vs Athletic Club Bilbao

        -- KOLEJKA 3 (@RoundId3)
            (NEWID(), @RoundId3, @TeamId1, @TeamId5, 3, 1, 0, 1),   -- Real Madrid CF vs Real Sociedad
            (NEWID(), @RoundId3, @TeamId2, @TeamId6, 4, 2, 0, 1),   -- FC Barcelona vs Sevilla FC
            (NEWID(), @RoundId3, @TeamId3, @TeamId19, 2, 0, 0, 1),  -- Atlético Madrid vs Deportivo Alavés
            (NEWID(), @RoundId3, @TeamId9, @TeamId4, 1, 0, 0, 1),   -- Athletic Club Bilbao vs Valencia CF
            (NEWID(), @RoundId3, @TeamId7, @TeamId8, 2, 2, 1, 1),   -- Villarreal CF vs Real Betis
            (NEWID(), @RoundId3, @TeamId11, @TeamId16, 1, 2, 0, 1), -- RC Celta de Vigo vs Girona FC
            (NEWID(), @RoundId3, @TeamId21, @TeamId14, 0, 0, 1, 1), -- RCD Espanyol vs Getafe CF
            (NEWID(), @RoundId3, @TeamId23, @TeamId10, 0, 1, 0, 1), -- Elche CF vs Osasuna Pampeluna
            (NEWID(), @RoundId3, @TeamId13, @TeamId15, 1, 1, 1, 1), -- Real Valladolid CF vs Rayo Vallecano
            (NEWID(), @RoundId3, @TeamId18, @TeamId12, 0, 0, 1, 1), -- UD Las Palmas vs RCD Mallorca

        -- KOLEJKA 4 (@RoundId4)
            (NEWID(), @RoundId4, @TeamId5, @TeamId2, 1, 2, 0, 1),   -- Real Sociedad vs FC Barcelona
            (NEWID(), @RoundId4, @TeamId6, @TeamId3, 0, 1, 0, 1),   -- Sevilla FC vs Atlético Madrid
            (NEWID(), @RoundId4, @TeamId4, @TeamId1, 2, 3, 0, 1),   -- Valencia CF vs Real Madrid CF
            (NEWID(), @RoundId4, @TeamId16, @TeamId9, 0, 2, 0, 1),  -- Girona FC vs Athletic Club Bilbao
            (NEWID(), @RoundId4, @TeamId8, @TeamId11, 3, 0, 0, 1),  -- Real Betis vs RC Celta de Vigo
            (NEWID(), @RoundId4, @TeamId15, @TeamId7, 1, 1, 1, 1),  -- Rayo Vallecano vs Villarreal CF
            (NEWID(), @RoundId4, @TeamId10, @TeamId21, 2, 1, 0, 1), -- Osasuna Pampeluna vs RCD Espanyol
            (NEWID(), @RoundId4, @TeamId19, @TeamId23, 1, 0, 0, 1), -- Deportivo Alavés vs Elche CF
            (NEWID(), @RoundId4, @TeamId12, @TeamId18, 1, 0, 0, 1), -- RCD Mallorca vs UD Las Palmas
            (NEWID(), @RoundId4, @TeamId14, @TeamId13, 0, 0, 1, 1), -- Getafe CF vs Real Valladolid CF

        -- KOLEJKA 5 (@RoundId5)
            (NEWID(), @RoundId5, @TeamId1, @TeamId6, 2, 0, 0, 1),   -- Real Madrid CF vs Sevilla FC
            (NEWID(), @RoundId5, @TeamId2, @TeamId4, 3, 1, 0, 1),   -- FC Barcelona vs Valencia CF
            (NEWID(), @RoundId5, @TeamId3, @TeamId5, 1, 0, 0, 1),   -- Atlético Madrid vs Real Sociedad
            (NEWID(), @RoundId5, @TeamId9, @TeamId8, 2, 1, 0, 1),   -- Athletic Club Bilbao vs Real Betis
            (NEWID(), @RoundId5, @TeamId7, @TeamId16, 1, 1, 1, 1),  -- Villarreal CF vs Girona FC
            (NEWID(), @RoundId5, @TeamId11, @TeamId15, 0, 2, 0, 1), -- RC Celta de Vigo vs Rayo Vallecano
            (NEWID(), @RoundId5, @TeamId21, @TeamId19, 1, 0, 0, 1), -- RCD Espanyol vs Deportivo Alavés
            (NEWID(), @RoundId5, @TeamId23, @TeamId14, 0, 0, 1, 1), -- Elche CF vs Getafe CF
            (NEWID(), @RoundId5, @TeamId13, @TeamId10, 1, 2, 0, 1), -- Real Valladolid CF vs Osasuna Pampeluna
            (NEWID(), @RoundId5, @TeamId18, @TeamId12, 1, 1, 1, 1), -- UD Las Palmas vs RCD Mallorca

        -- KOLEJKA 6 (@RoundId6)
            (NEWID(), @RoundId6, @TeamId6, @TeamId2, 1, 3, 0, 1),   -- Sevilla FC vs FC Barcelona
            (NEWID(), @RoundId6, @TeamId5, @TeamId1, 0, 2, 0, 1),   -- Real Sociedad vs Real Madrid CF
            (NEWID(), @RoundId6, @TeamId4, @TeamId3, 2, 2, 1, 1),   -- Valencia CF vs Atlético Madrid
            (NEWID(), @RoundId6, @TeamId16, @TeamId9, 1, 0, 0, 1),  -- Girona FC vs Athletic Club Bilbao
            (NEWID(), @RoundId6, @TeamId8, @TeamId7, 3, 1, 0, 1),   -- Real Betis vs Villarreal CF
            (NEWID(), @RoundId6, @TeamId15, @TeamId11, 1, 0, 0, 1), -- Rayo Vallecano vs RC Celta de Vigo
            (NEWID(), @RoundId6, @TeamId10, @TeamId23, 2, 0, 0, 1), -- Osasuna Pampeluna vs Elche CF
            (NEWID(), @RoundId6, @TeamId19, @TeamId21, 1, 1, 1, 1), -- Deportivo Alavés vs RCD Espanyol
            (NEWID(), @RoundId6, @TeamId12, @TeamId13, 0, 1, 0, 1), -- RCD Mallorca vs Real Valladolid CF
            (NEWID(), @RoundId6, @TeamId14, @TeamId18, 0, 0, 1, 1), -- Getafe CF vs UD Las Palmas

        -- KOLEJKA 7 (@RoundId7)
            (NEWID(), @RoundId7, @TeamId1, @TeamId3, 1, 1, 1, 1),   -- Real Madrid CF vs Atlético Madrid
            (NEWID(), @RoundId7, @TeamId2, @TeamId5, 2, 0, 0, 1),   -- FC Barcelona vs Real Sociedad
            (NEWID(), @RoundId7, @TeamId7, @TeamId4, 1, 0, 0, 1),   -- Villarreal CF vs Valencia CF
            (NEWID(), @RoundId7, @TeamId9, @TeamId6, 3, 2, 0, 1),   -- Athletic Club Bilbao vs Sevilla FC
            (NEWID(), @RoundId7, @TeamId11, @TeamId8, 0, 0, 1, 1),  -- RC Celta de Vigo vs Real Betis
            (NEWID(), @RoundId7, @TeamId21, @TeamId16, 2, 1, 0, 1), -- RCD Espanyol vs Girona FC
            (NEWID(), @RoundId7, @TeamId23, @TeamId15, 1, 2, 0, 1), -- Elche CF vs Rayo Vallecano
            (NEWID(), @RoundId7, @TeamId13, @TeamId19, 0, 1, 0, 1), -- Real Valladolid CF vs Deportivo Alavés
            (NEWID(), @RoundId7, @TeamId18, @TeamId10, 1, 3, 0, 1), -- UD Las Palmas vs Osasuna Pampeluna
            (NEWID(), @RoundId7, @TeamId14, @TeamId12, 1, 1, 1, 1), -- Getafe CF vs RCD Mallorca

        -- KOLEJKA 8 (@RoundId8)
            (NEWID(), @RoundId8, @TeamId3, @TeamId2, 1, 0, 0, 1),   -- Atlético Madrid vs FC Barcelona
            (NEWID(), @RoundId8, @TeamId5, @TeamId1, 2, 1, 0, 1),   -- Real Sociedad vs Real Madrid CF
            (NEWID(), @RoundId8, @TeamId4, @TeamId9, 0, 0, 1, 1),   -- Valencia CF vs Athletic Club Bilbao
            (NEWID(), @RoundId8, @TeamId6, @TeamId7, 2, 2, 1, 1),   -- Sevilla FC vs Villarreal CF
            (NEWID(), @RoundId8, @TeamId8, @TeamId14, 3, 0, 0, 1),  -- Real Betis vs Getafe CF
            (NEWID(), @RoundId8, @TeamId16, @TeamId11, 1, 0, 0, 1), -- Girona FC vs RC Celta de Vigo
            (NEWID(), @RoundId8, @TeamId15, @TeamId23, 2, 1, 0, 1), -- Rayo Vallecano vs Elche CF
            (NEWID(), @RoundId8, @TeamId10, @TeamId18, 2, 0, 0, 1), -- Osasuna Pampeluna vs UD Las Palmas
            (NEWID(), @RoundId8, @TeamId19, @TeamId13, 1, 1, 1, 1), -- Deportivo Alavés vs Real Valladolid CF
            (NEWID(), @RoundId8, @TeamId12, @TeamId21, 0, 1, 0, 1), -- RCD Mallorca vs RCD Espanyol

        -- KOLEJKA 9 (@RoundId9)
            (NEWID(), @RoundId9, @TeamId1, @TeamId7, 3, 0, 0, 1),   -- Real Madrid CF vs Villarreal CF
            (NEWID(), @RoundId9, @TeamId2, @TeamId9, 4, 1, 0, 1),   -- FC Barcelona vs Athletic Club Bilbao
            (NEWID(), @RoundId9, @TeamId3, @TeamId8, 2, 0, 0, 1),   -- Atlético Madrid vs Real Betis
            (NEWID(), @RoundId9, @TeamId5, @TeamId4, 1, 1, 1, 1),   -- Real Sociedad vs Valencia CF
            (NEWID(), @RoundId9, @TeamId6, @TeamId16, 3, 0, 0, 1),  -- Sevilla FC vs Girona FC
            (NEWID(), @RoundId9, @TeamId11, @TeamId23, 2, 1, 0, 1), -- RC Celta de Vigo vs Elche CF
            (NEWID(), @RoundId9, @TeamId21, @TeamId15, 0, 2, 0, 1), -- RCD Espanyol vs Rayo Vallecano
            (NEWID(), @RoundId9, @TeamId13, @TeamId18, 1, 1, 1, 1), -- Real Valladolid CF vs UD Las Palmas
            (NEWID(), @RoundId9, @TeamId14, @TeamId12, 1, 0, 0, 1), -- Getafe CF vs RCD Mallorca
            (NEWID(), @RoundId9, @TeamId19, @TeamId10, 0, 0, 1, 1), -- Deportivo Alavés vs Osasuna Pampeluna

        -- KOLEJKA 10 (@RoundId10)
            (NEWID(), @RoundId10, @TeamId7, @TeamId2, 1, 2, 0, 1),   -- Villarreal CF vs FC Barcelona
            (NEWID(), @RoundId10, @TeamId9, @TeamId1, 2, 2, 1, 1),   -- Athletic Club Bilbao vs Real Madrid CF
            (NEWID(), @RoundId10, @TeamId4, @TeamId6, 1, 0, 0, 1),   -- Valencia CF vs Sevilla FC
            (NEWID(), @RoundId10, @TeamId8, @TeamId5, 3, 1, 0, 1),   -- Real Betis vs Real Sociedad
            (NEWID(), @RoundId10, @TeamId16, @TeamId3, 0, 1, 0, 1),  -- Girona FC vs Atlético Madrid
            (NEWID(), @RoundId10, @TeamId15, @TeamId14, 2, 0, 0, 1), -- Rayo Vallecano vs Getafe CF
            (NEWID(), @RoundId10, @TeamId10, @TeamId11, 1, 1, 1, 1), -- Osasuna Pampeluna vs RC Celta de Vigo
            (NEWID(), @RoundId10, @TeamId23, @TeamId19, 0, 1, 0, 1), -- Elche CF vs Deportivo Alavés
            (NEWID(), @RoundId10, @TeamId18, @TeamId21, 1, 0, 0, 1), -- UD Las Palmas vs RCD Espanyol
            (NEWID(), @RoundId10, @TeamId12, @TeamId13, 0, 0, 1, 1),  -- RCD Mallorca vs Real Valladolid CF

        -- KOLEJKA 11 (@RoundId11) - Rozegrane
            (NEWID(), @RoundId11, @TeamId1, @TeamId8, 3, 1, 0, 1),   -- Real Madrid CF vs Real Betis
            (NEWID(), @RoundId11, @TeamId2, @TeamId7, 2, 0, 0, 1),   -- FC Barcelona vs Villarreal CF
            (NEWID(), @RoundId11, @TeamId3, @TeamId9, 1, 1, 1, 1),   -- Atlético Madrid vs Athletic Club Bilbao
            (NEWID(), @RoundId11, @TeamId5, @TeamId6, 1, 0, 0, 1),   -- Real Sociedad vs Sevilla FC
            (NEWID(), @RoundId11, @TeamId4, @TeamId16, 2, 0, 0, 1),  -- Valencia CF vs Girona FC
            (NEWID(), @RoundId11, @TeamId11, @TeamId15, 0, 1, 0, 1), -- RC Celta de Vigo vs Rayo Vallecano
            (NEWID(), @RoundId11, @TeamId21, @TeamId23, 2, 2, 1, 1), -- RCD Espanyol vs Elche CF
            (NEWID(), @RoundId11, @TeamId13, @TeamId10, 1, 0, 0, 1), -- Real Valladolid CF vs Osasuna Pampeluna
            (NEWID(), @RoundId11, @TeamId18, @TeamId19, 0, 0, 1, 1), -- UD Las Palmas vs Deportivo Alavés
            (NEWID(), @RoundId11, @TeamId14, @TeamId12, 1, 2, 0, 1), -- Getafe CF vs RCD Mallorca

        -- KOLEJKA 12 (@RoundId12) - Rozegrane
            (NEWID(), @RoundId12, @TeamId7, @TeamId1, 1, 3, 0, 1),   -- Villarreal CF vs Real Madrid CF
            (NEWID(), @RoundId12, @TeamId8, @TeamId2, 1, 1, 1, 1),   -- Real Betis vs FC Barcelona
            (NEWID(), @RoundId12, @TeamId9, @TeamId3, 0, 0, 1, 1),   -- Athletic Club Bilbao vs Atlético Madrid
            (NEWID(), @RoundId12, @TeamId6, @TeamId5, 2, 1, 0, 1),   -- Sevilla FC vs Real Sociedad
            (NEWID(), @RoundId12, @TeamId16, @TeamId4, 1, 0, 0, 1),  -- Girona FC vs Valencia CF
            (NEWID(), @RoundId12, @TeamId15, @TeamId11, 2, 0, 0, 1), -- Rayo Vallecano vs RC Celta de Vigo
            (NEWID(), @RoundId12, @TeamId23, @TeamId21, 0, 1, 0, 1), -- Elche CF vs RCD Espanyol
            (NEWID(), @RoundId12, @TeamId10, @TeamId13, 2, 2, 1, 1), -- Osasuna Pampeluna vs Real Valladolid CF
            (NEWID(), @RoundId12, @TeamId19, @TeamId18, 1, 0, 0, 1), -- Deportivo Alavés vs UD Las Palmas
            (NEWID(), @RoundId12, @TeamId12, @TeamId14, 0, 0, 1, 1), -- RCD Mallorca vs Getafe CF

        -- KOLEJKA 13 (@RoundId13) - Rozegrane
            (NEWID(), @RoundId13, @TeamId1, @TeamId2, 3, 2, 0, 1),   -- Real Madrid CF vs FC Barcelona (El Clásico)
            (NEWID(), @RoundId13, @TeamId3, @TeamId7, 2, 1, 0, 1),   -- Atlético Madrid vs Villarreal CF
            (NEWID(), @RoundId13, @TeamId5, @TeamId9, 1, 1, 1, 1),   -- Real Sociedad vs Athletic Club Bilbao
            (NEWID(), @RoundId13, @TeamId6, @TeamId8, 3, 0, 0, 1),   -- Sevilla FC vs Real Betis
            (NEWID(), @RoundId13, @TeamId4, @TeamId15, 1, 0, 0, 1),  -- Valencia CF vs Rayo Vallecano
            (NEWID(), @RoundId13, @TeamId11, @TeamId21, 1, 2, 0, 1), -- RC Celta de Vigo vs RCD Espanyol
            (NEWID(), @RoundId13, @TeamId23, @TeamId13, 0, 0, 1, 1), -- Elche CF vs Real Valladolid CF
            (NEWID(), @RoundId13, @TeamId10, @TeamId14, 2, 1, 0, 1), -- Osasuna Pampeluna vs Getafe CF
            (NEWID(), @RoundId13, @TeamId19, @TeamId16, 1, 0, 0, 1), -- Deportivo Alavés vs Girona FC
            (NEWID(), @RoundId13, @TeamId18, @TeamId12, 1, 1, 1, 1), -- UD Las Palmas vs RCD Mallorca

        -- KOLEJKA 14 (@RoundId14) - Rozegrane
            (NEWID(), @RoundId14, @TeamId2, @TeamId1, 1, 0, 0, 1),   -- FC Barcelona vs Real Madrid CF (El Clásico)
            (NEWID(), @RoundId14, @TeamId7, @TeamId3, 0, 0, 1, 1),   -- Villarreal CF vs Atlético Madrid
            (NEWID(), @RoundId14, @TeamId9, @TeamId5, 2, 1, 0, 1),   -- Athletic Club Bilbao vs Real Sociedad
            (NEWID(), @RoundId14, @TeamId8, @TeamId6, 1, 1, 1, 1),   -- Real Betis vs Sevilla FC
            (NEWID(), @RoundId14, @TeamId15, @TeamId4, 2, 0, 0, 1),  -- Rayo Vallecano vs Valencia CF
            (NEWID(), @RoundId14, @TeamId21, @TeamId11, 3, 1, 0, 1), -- RCD Espanyol vs RC Celta de Vigo
            (NEWID(), @RoundId14, @TeamId13, @TeamId23, 1, 0, 0, 1), -- Real Valladolid CF vs Elche CF
            (NEWID(), @RoundId14, @TeamId14, @TeamId10, 0, 2, 0, 1), -- Getafe CF vs Osasuna Pampeluna
            (NEWID(), @RoundId14, @TeamId16, @TeamId19, 0, 0, 1, 1), -- Girona FC vs Deportivo Alavés
            (NEWID(), @RoundId14, @TeamId12, @TeamId18, 1, 0, 0, 1), -- RCD Mallorca vs UD Las Palmas

        -- KOLEJKA 15 (@RoundId15) - Rozegrane
            (NEWID(), @RoundId15, @TeamId1, @TeamId4, 4, 0, 0, 1),   -- Real Madrid CF vs Valencia CF
            (NEWID(), @RoundId15, @TeamId3, @TeamId2, 1, 2, 0, 1),   -- Atlético Madrid vs FC Barcelona
            (NEWID(), @RoundId15, @TeamId5, @TeamId7, 2, 2, 1, 1),   -- Real Sociedad vs Villarreal CF
            (NEWID(), @RoundId15, @TeamId6, @TeamId9, 1, 0, 0, 1),   -- Sevilla FC vs Athletic Club Bilbao
            (NEWID(), @RoundId15, @TeamId8, @TeamId16, 3, 1, 0, 1),  -- Real Betis vs Girona FC
            (NEWID(), @RoundId15, @TeamId11, @TeamId19, 0, 1, 0, 1), -- RC Celta de Vigo vs Deportivo Alavés
            (NEWID(), @RoundId15, @TeamId23, @TeamId21, 1, 0, 0, 1), -- Elche CF vs RCD Espanyol
            (NEWID(), @RoundId15, @TeamId10, @TeamId15, 2, 1, 0, 1), -- Osasuna Pampeluna vs Rayo Vallecano
            (NEWID(), @RoundId15, @TeamId18, @TeamId14, 0, 0, 1, 1), -- UD Las Palmas vs Getafe CF
            (NEWID(), @RoundId15, @TeamId13, @TeamId12, 1, 0, 0, 1), -- Real Valladolid CF vs RCD Mallorca

        -- KOLEJKA 16 (@RoundId16) - Rozegrane
            (NEWID(), @RoundId16, @TeamId1, @TeamId11, 2, 0, 0, 1), -- Real Madrid CF vs RC Celta de Vigo
            (NEWID(), @RoundId16, @TeamId2, @TeamId23, 5, 0, 0, 1), -- FC Barcelona vs Elche CF
            (NEWID(), @RoundId16, @TeamId3, @TeamId16, 1, 0, 0, 1), -- Atlético Madrid vs Girona FC
            (NEWID(), @RoundId16, @TeamId6, @TeamId8, 2, 2, 1, 1),  -- Sevilla FC vs Real Betis
            (NEWID(), @RoundId16, @TeamId4, @TeamId7, 1, 2, 0, 1),  -- Valencia CF vs Villarreal CF
            (NEWID(), @RoundId16, @TeamId5, @TeamId9, 0, 0, 1, 1),  -- Real Sociedad vs Athletic Club Bilbao
            (NEWID(), @RoundId16, @TeamId19, @TeamId21, 1, 0, 0, 1),-- Deportivo Alavés vs RCD Espanyol
            (NEWID(), @RoundId16, @TeamId14, @TeamId10, 0, 1, 0, 1),-- Getafe CF vs Osasuna Pampeluna
            (NEWID(), @RoundId16, @TeamId15, @TeamId18, 2, 1, 0, 1),-- Rayo Vallecano vs UD Las Palmas
            (NEWID(), @RoundId16, @TeamId12, @TeamId13, 1, 1, 1, 1),-- RCD Mallorca vs Real Valladolid CF

        -- KOLEJKA 17 (@RoundId17) - Nierozegrane (NULL)
            (NEWID(), @RoundId17, @TeamId8, @TeamId1, NULL, NULL, NULL, 0),   -- Real Betis vs Real Madrid CF
            (NEWID(), @RoundId17, @TeamId10, @TeamId2, NULL, NULL, NULL, 0),  -- Osasuna Pampeluna vs FC Barcelona
            (NEWID(), @RoundId17, @TeamId7, @TeamId3, NULL, NULL, NULL, 0),   -- Villarreal CF vs Atlético Madrid
            (NEWID(), @RoundId17, @TeamId21, @TeamId4, NULL, NULL, NULL, 0),  -- RCD Espanyol vs Valencia CF
            (NEWID(), @RoundId17, @TeamId23, @TeamId5, NULL, NULL, NULL, 0),  -- Elche CF vs Real Sociedad
            (NEWID(), @RoundId17, @TeamId16, @TeamId6, NULL, NULL, NULL, 0),  -- Girona FC vs Sevilla FC
            (NEWID(), @RoundId17, @TeamId19, @TeamId9, NULL, NULL, NULL, 0),  -- Deportivo Alavés vs Athletic Club Bilbao
            (NEWID(), @RoundId17, @TeamId14, @TeamId11, NULL, NULL, NULL, 0), -- Getafe CF vs RC Celta de Vigo
            (NEWID(), @RoundId17, @TeamId13, @TeamId12, NULL, NULL, NULL, 0), -- Real Valladolid CF vs RCD Mallorca
            (NEWID(), @RoundId17, @TeamId18, @TeamId15, NULL, NULL, NULL, 0), -- UD Las Palmas vs Rayo Vallecano

        -- KOLEJKA 18 (@RoundId18) - Nierozegrane (NULL)
            (NEWID(), @RoundId18, @TeamId1, @TeamId16, NULL, NULL, NULL, 0),  -- Real Madrid CF vs Girona FC
            (NEWID(), @RoundId18, @TeamId2, @TeamId3, NULL, NULL, NULL, 0),   -- FC Barcelona vs Atlético Madrid
            (NEWID(), @RoundId18, @TeamId5, @TeamId8, NULL, NULL, NULL, 0),   -- Real Sociedad vs Real Betis
            (NEWID(), @RoundId18, @TeamId6, @TeamId21, NULL, NULL, NULL, 0),  -- Sevilla FC vs RCD Espanyol
            (NEWID(), @RoundId18, @TeamId4, @TeamId13, NULL, NULL, NULL, 0),  -- Valencia CF vs Real Valladolid CF
            (NEWID(), @RoundId18, @TeamId9, @TeamId18, NULL, NULL, NULL, 0),  -- Athletic Club Bilbao vs UD Las Palmas
            (NEWID(), @RoundId18, @TeamId11, @TeamId7, NULL, NULL, NULL, 0),  -- RC Celta de Vigo vs Villarreal CF
            (NEWID(), @RoundId18, @TeamId15, @TeamId10, NULL, NULL, NULL, 0), -- Rayo Vallecano vs Osasuna Pampeluna
            (NEWID(), @RoundId18, @TeamId12, @TeamId19, NULL, NULL, NULL, 0), -- RCD Mallorca vs Deportivo Alavés
            (NEWID(), @RoundId18, @TeamId23, @TeamId14, NULL, NULL, NULL, 0), -- Elche CF vs Getafe CF

        -- KOLEJKA 19 (@RoundId19) - Nierozegrane (NULL)
            (NEWID(), @RoundId19, @TeamId7, @TeamId10, NULL, NULL, NULL, 0),  -- Villarreal CF vs Osasuna Pampeluna
            (NEWID(), @RoundId19, @TeamId8, @TeamId12, NULL, NULL, NULL, 0),  -- Real Betis vs RCD Mallorca
            (NEWID(), @RoundId19, @TeamId9, @TeamId11, NULL, NULL, NULL, 0),  -- Athletic Club Bilbao vs RC Celta de Vigo
            (NEWID(), @RoundId19, @TeamId16, @TeamId2, NULL, NULL, NULL, 0),  -- Girona FC vs FC Barcelona
            (NEWID(), @RoundId19, @TeamId19, @TeamId1, NULL, NULL, NULL, 0),  -- Deportivo Alavés vs Real Madrid CF
            (NEWID(), @RoundId19, @TeamId21, @TeamId3, NULL, NULL, NULL, 0),  -- RCD Espanyol vs Atlético Madrid
            (NEWID(), @RoundId19, @TeamId4, @TeamId18, NULL, NULL, NULL, 0),  -- Valencia CF vs UD Las Palmas
            (NEWID(), @RoundId19, @TeamId5, @TeamId15, NULL, NULL, NULL, 0),  -- Real Sociedad vs Rayo Vallecano
            (NEWID(), @RoundId19, @TeamId6, @TeamId13, NULL, NULL, NULL, 0),  -- Sevilla FC vs Real Valladolid CF
            (NEWID(), @RoundId19, @TeamId14, @TeamId23, NULL, NULL, NULL, 0), -- Getafe CF vs Elche CF

        -- KOLEJKA 20 (@RoundId20) - Nierozegrane (NULL)
            (NEWID(), @RoundId20, @TeamId1, @TeamId9, NULL, NULL, NULL, 0),   -- Real Madrid CF vs Athletic Club Bilbao
            (NEWID(), @RoundId20, @TeamId2, @TeamId7, NULL, NULL, NULL, 0),   -- FC Barcelona vs Villarreal CF
            (NEWID(), @RoundId20, @TeamId3, @TeamId10, NULL, NULL, NULL, 0),  -- Atlético Madrid vs Osasuna Pampeluna
            (NEWID(), @RoundId20, @TeamId5, @TeamId16, NULL, NULL, NULL, 0),  -- Real Sociedad vs Girona FC
            (NEWID(), @RoundId20, @TeamId4, @TeamId12, NULL, NULL, NULL, 0),  -- Valencia CF vs RCD Mallorca
            (NEWID(), @RoundId20, @TeamId8, @TeamId21, NULL, NULL, NULL, 0),  -- Real Betis vs RCD Espanyol
            (NEWID(), @RoundId20, @TeamId11, @TeamId6, NULL, NULL, NULL, 0),  -- RC Celta de Vigo vs Sevilla FC
            (NEWID(), @RoundId20, @TeamId13, @TeamId14, NULL, NULL, NULL, 0), -- Real Valladolid CF vs Getafe CF
            (NEWID(), @RoundId20, @TeamId18, @TeamId23, NULL, NULL, NULL, 0), -- UD Las Palmas vs Elche CF
            (NEWID(), @RoundId20, @TeamId15, @TeamId19, NULL, NULL, NULL, 0),  -- Rayo Vallecano vs Deportivo Alavés

        -- KOLEJKA 21 (@RoundId21) - Nierozegrane (NULL)
            (NEWID(), @RoundId21, @TeamId7, @TeamId19, NULL, NULL, NULL, 0),  -- Villarreal CF vs Deportivo Alavés
            (NEWID(), @RoundId21, @TeamId8, @TeamId10, NULL, NULL, NULL, 0),  -- Real Betis vs Osasuna Pampeluna
            (NEWID(), @RoundId21, @TeamId9, @TeamId14, NULL, NULL, NULL, 0),  -- Athletic Club Bilbao vs Getafe CF
            (NEWID(), @RoundId21, @TeamId16, @TeamId11, NULL, NULL, NULL, 0), -- Girona FC vs RC Celta de Vigo
            (NEWID(), @RoundId21, @TeamId1, @TeamId23, NULL, NULL, NULL, 0),  -- Real Madrid CF vs Elche CF
            (NEWID(), @RoundId21, @TeamId2, @TeamId5, NULL, NULL, NULL, 0),   -- FC Barcelona vs Real Sociedad
            (NEWID(), @RoundId21, @TeamId3, @TeamId6, NULL, NULL, NULL, 0),   -- Atlético Madrid vs Sevilla FC
            (NEWID(), @RoundId21, @TeamId4, @TeamId15, NULL, NULL, NULL, 0),  -- Valencia CF vs Rayo Vallecano
            (NEWID(), @RoundId21, @TeamId13, @TeamId21, NULL, NULL, NULL, 0), -- Real Valladolid CF vs RCD Espanyol
            (NEWID(), @RoundId21, @TeamId18, @TeamId12, NULL, NULL, NULL, 0), -- UD Las Palmas vs RCD Mallorca

        -- KOLEJKA 22 (@RoundId22) - Nierozegrane (NULL)
            (NEWID(), @RoundId22, @TeamId19, @TeamId7, NULL, NULL, NULL, 0),  -- Deportivo Alavés vs Villarreal CF
            (NEWID(), @RoundId22, @TeamId10, @TeamId8, NULL, NULL, NULL, 0),  -- Osasuna Pampeluna vs Real Betis
            (NEWID(), @RoundId22, @TeamId14, @TeamId9, NULL, NULL, NULL, 0),  -- Getafe CF vs Athletic Club Bilbao
            (NEWID(), @RoundId22, @TeamId11, @TeamId16, NULL, NULL, NULL, 0), -- RC Celta de Vigo vs Girona FC
            (NEWID(), @RoundId22, @TeamId23, @TeamId1, NULL, NULL, NULL, 0),  -- Elche CF vs Real Madrid CF
            (NEWID(), @RoundId22, @TeamId5, @TeamId2, NULL, NULL, NULL, 0),   -- Real Sociedad vs FC Barcelona
            (NEWID(), @RoundId22, @TeamId6, @TeamId3, NULL, NULL, NULL, 0),   -- Sevilla FC vs Atlético Madrid
            (NEWID(), @RoundId22, @TeamId15, @TeamId4, NULL, NULL, NULL, 0),  -- Rayo Vallecano vs Valencia CF
            (NEWID(), @RoundId22, @TeamId21, @TeamId13, NULL, NULL, NULL, 0), -- RCD Espanyol vs Real Valladolid CF
            (NEWID(), @RoundId22, @TeamId12, @TeamId18, NULL, NULL, NULL, 0), -- RCD Mallorca vs UD Las Palmas

        -- KOLEJKA 23 (@RoundId23) - Nierozegrane (NULL)
            (NEWID(), @RoundId23, @TeamId1, @TeamId3, NULL, NULL, NULL, 0),   -- Real Madrid CF vs Atlético Madrid
            (NEWID(), @RoundId23, @TeamId2, @TeamId14, NULL, NULL, NULL, 0),  -- FC Barcelona vs Getafe CF
            (NEWID(), @RoundId23, @TeamId5, @TeamId19, NULL, NULL, NULL, 0),  -- Real Sociedad vs Deportivo Alavés
            (NEWID(), @RoundId23, @TeamId6, @TeamId11, NULL, NULL, NULL, 0),  -- Sevilla FC vs RC Celta de Vigo
            (NEWID(), @RoundId23, @TeamId4, @TeamId7, NULL, NULL, NULL, 0),   -- Valencia CF vs Villarreal CF
            (NEWID(), @RoundId23, @TeamId8, @TeamId15, NULL, NULL, NULL, 0),  -- Real Betis vs Rayo Vallecano
            (NEWID(), @RoundId23, @TeamId9, @TeamId10, NULL, NULL, NULL, 0),  -- Athletic Club Bilbao vs Osasuna Pampeluna
            (NEWID(), @RoundId23, @TeamId16, @TeamId23, NULL, NULL, NULL, 0), -- Girona FC vs Elche CF
            (NEWID(), @RoundId23, @TeamId12, @TeamId21, NULL, NULL, NULL, 0), -- RCD Mallorca vs RCD Espanyol
            (NEWID(), @RoundId23, @TeamId13, @TeamId18, NULL, NULL, NULL, 0), -- Real Valladolid CF vs UD Las Palmas

        -- KOLEJKA 24 (@RoundId24) - Nierozegrane (NULL)
            (NEWID(), @RoundId24, @TeamId3, @TeamId1, NULL, NULL, NULL, 0),   -- Atlético Madrid vs Real Madrid CF
            (NEWID(), @RoundId24, @TeamId14, @TeamId2, NULL, NULL, NULL, 0),  -- Getafe CF vs FC Barcelona
            (NEWID(), @RoundId24, @TeamId19, @TeamId5, NULL, NULL, NULL, 0),  -- Deportivo Alavés vs Real Sociedad
            (NEWID(), @RoundId24, @TeamId11, @TeamId6, NULL, NULL, NULL, 0),  -- RC Celta de Vigo vs Sevilla FC
            (NEWID(), @RoundId24, @TeamId7, @TeamId4, NULL, NULL, NULL, 0),   -- Villarreal CF vs Valencia CF
            (NEWID(), @RoundId24, @TeamId15, @TeamId8, NULL, NULL, NULL, 0),  -- Rayo Vallecano vs Real Betis
            (NEWID(), @RoundId24, @TeamId10, @TeamId9, NULL, NULL, NULL, 0),  -- Osasuna Pampeluna vs Athletic Club Bilbao
            (NEWID(), @RoundId24, @TeamId23, @TeamId16, NULL, NULL, NULL, 0), -- Elche CF vs Girona FC
            (NEWID(), @RoundId24, @TeamId21, @TeamId12, NULL, NULL, NULL, 0), -- RCD Espanyol vs RCD Mallorca
            (NEWID(), @RoundId24, @TeamId18, @TeamId13, NULL, NULL, NULL, 0), -- UD Las Palmas vs Real Valladolid CF

        -- KOLEJKA 25 (@RoundId25) - Nierozegrane (NULL)
            (NEWID(), @RoundId25, @TeamId1, @TeamId7, NULL, NULL, NULL, 0),   -- Real Madrid CF vs Villarreal CF
            (NEWID(), @RoundId25, @TeamId2, @TeamId3, NULL, NULL, NULL, 0),   -- FC Barcelona vs Atlético Madrid
            (NEWID(), @RoundId25, @TeamId5, @TeamId15, NULL, NULL, NULL, 0),  -- Real Sociedad vs Rayo Vallecano
            (NEWID(), @RoundId25, @TeamId6, @TeamId9, NULL, NULL, NULL, 0),   -- Sevilla FC vs Athletic Club Bilbao
            (NEWID(), @RoundId25, @TeamId4, @TeamId11, NULL, NULL, NULL, 0),  -- Valencia CF vs RC Celta de Vigo
            (NEWID(), @RoundId25, @TeamId8, @TeamId19, NULL, NULL, NULL, 0),  -- Real Betis vs Deportivo Alavés
            (NEWID(), @RoundId25, @TeamId10, @TeamId14, NULL, NULL, NULL, 0), -- Osasuna Pampeluna vs Getafe CF
            (NEWID(), @RoundId25, @TeamId16, @TeamId23, NULL, NULL, NULL, 0), -- Girona FC vs Elche CF
            (NEWID(), @RoundId25, @TeamId12, @TeamId21, NULL, NULL, NULL, 0), -- RCD Mallorca vs RCD Espanyol
            (NEWID(), @RoundId25, @TeamId13, @TeamId18, NULL, NULL, NULL, 0), -- Real Valladolid CF vs UD Las Palmas

        -- KOLEJKA 26 (@RoundId26) - Nierozegrane (NULL)
            (NEWID(), @RoundId26, @TeamId7, @TeamId1, NULL, NULL, NULL, 0),   -- Villarreal CF vs Real Madrid CF
            (NEWID(), @RoundId26, @TeamId3, @TeamId2, NULL, NULL, NULL, 0),   -- Atlético Madrid vs FC Barcelona
            (NEWID(), @RoundId26, @TeamId15, @TeamId5, NULL, NULL, NULL, 0),  -- Rayo Vallecano vs Real Sociedad
            (NEWID(), @RoundId26, @TeamId9, @TeamId6, NULL, NULL, NULL, 0),   -- Athletic Club Bilbao vs Sevilla FC
            (NEWID(), @RoundId26, @TeamId11, @TeamId4, NULL, NULL, NULL, 0),  -- RC Celta de Vigo vs Valencia CF
            (NEWID(), @RoundId26, @TeamId19, @TeamId8, NULL, NULL, NULL, 0),  -- Deportivo Alavés vs Real Betis
            (NEWID(), @RoundId26, @TeamId14, @TeamId10, NULL, NULL, NULL, 0), -- Getafe CF vs Osasuna Pampeluna
            (NEWID(), @RoundId26, @TeamId23, @TeamId16, NULL, NULL, NULL, 0), -- Elche CF vs Girona FC
            (NEWID(), @RoundId26, @TeamId21, @TeamId12, NULL, NULL, NULL, 0), -- RCD Espanyol vs RCD Mallorca
            (NEWID(), @RoundId26, @TeamId18, @TeamId13, NULL, NULL, NULL, 0), -- UD Las Palmas vs Real Valladolid CF

        -- KOLEJKA 27 (@RoundId27) - Nierozegrane (NULL)
            (NEWID(), @RoundId27, @TeamId1, @TeamId9, NULL, NULL, NULL, 0),   -- Real Madrid CF vs Athletic Club Bilbao
            (NEWID(), @RoundId27, @TeamId2, @TeamId8, NULL, NULL, NULL, 0),   -- FC Barcelona vs Real Betis
            (NEWID(), @RoundId27, @TeamId5, @TeamId3, NULL, NULL, NULL, 0),   -- Real Sociedad vs Atlético Madrid
            (NEWID(), @RoundId27, @TeamId6, @TeamId7, NULL, NULL, NULL, 0),   -- Sevilla FC vs Villarreal CF
            (NEWID(), @RoundId27, @TeamId4, @TeamId10, NULL, NULL, NULL, 0),  -- Valencia CF vs Osasuna Pampeluna
            (NEWID(), @RoundId27, @TeamId11, @TeamId14, NULL, NULL, NULL, 0), -- RC Celta de Vigo vs Getafe CF
            (NEWID(), @RoundId27, @TeamId23, @TeamId19, NULL, NULL, NULL, 0), -- Elche CF vs Deportivo Alavés
            (NEWID(), @RoundId27, @TeamId16, @TeamId15, NULL, NULL, NULL, 0), -- Girona FC vs Rayo Vallecano
            (NEWID(), @RoundId27, @TeamId12, @TeamId13, NULL, NULL, NULL, 0), -- RCD Mallorca vs Real Valladolid CF
            (NEWID(), @RoundId27, @TeamId18, @TeamId21, NULL, NULL, NULL, 0), -- UD Las Palmas vs RCD Espanyol

        -- KOLEJKA 28 (@RoundId28) - Nierozegrane (NULL)
            (NEWID(), @RoundId28, @TeamId9, @TeamId1, NULL, NULL, NULL, 0),   -- Athletic Club Bilbao vs Real Madrid CF
            (NEWID(), @RoundId28, @TeamId8, @TeamId2, NULL, NULL, NULL, 0),   -- Real Betis vs FC Barcelona
            (NEWID(), @RoundId28, @TeamId3, @TeamId5, NULL, NULL, NULL, 0),   -- Atlético Madrid vs Real Sociedad
            (NEWID(), @RoundId28, @TeamId7, @TeamId6, NULL, NULL, NULL, 0),   -- Villarreal CF vs Sevilla FC
            (NEWID(), @RoundId28, @TeamId10, @TeamId4, NULL, NULL, NULL, 0),  -- Osasuna Pampeluna vs Valencia CF
            (NEWID(), @RoundId28, @TeamId14, @TeamId11, NULL, NULL, NULL, 0), -- Getafe CF vs RC Celta de Vigo
            (NEWID(), @RoundId28, @TeamId19, @TeamId23, NULL, NULL, NULL, 0), -- Deportivo Alavés vs Elche CF
            (NEWID(), @RoundId28, @TeamId15, @TeamId16, NULL, NULL, NULL, 0), -- Rayo Vallecano vs Girona FC
            (NEWID(), @RoundId28, @TeamId13, @TeamId12, NULL, NULL, NULL, 0), -- Real Valladolid CF vs RCD Mallorca
            (NEWID(), @RoundId28, @TeamId21, @TeamId18, NULL, NULL, NULL, 0), -- RCD Espanyol vs UD Las Palmas

        -- KOLEJKA 29 (@RoundId29) - Nierozegrane (NULL)
            (NEWID(), @RoundId29, @TeamId1, @TeamId14, NULL, NULL, NULL, 0),  -- Real Madrid CF vs Getafe CF
            (NEWID(), @RoundId29, @TeamId2, @TeamId10, NULL, NULL, NULL, 0),  -- FC Barcelona vs Osasuna Pampeluna
            (NEWID(), @RoundId29, @TeamId3, @TeamId19, NULL, NULL, NULL, 0),  -- Atlético Madrid vs Deportivo Alavés
            (NEWID(), @RoundId29, @TeamId5, @TeamId6, NULL, NULL, NULL, 0),   -- Real Sociedad vs Sevilla FC
            (NEWID(), @RoundId29, @TeamId4, @TeamId8, NULL, NULL, NULL, 0),   -- Valencia CF vs Real Betis
            (NEWID(), @RoundId29, @TeamId7, @TeamId16, NULL, NULL, NULL, 0),  -- Villarreal CF vs Girona FC
            (NEWID(), @RoundId29, @TeamId9, @TeamId13, NULL, NULL, NULL, 0),  -- Athletic Club Bilbao vs Real Valladolid CF
            (NEWID(), @RoundId29, @TeamId11, @TeamId23, NULL, NULL, NULL, 0), -- RC Celta de Vigo vs Elche CF
            (NEWID(), @RoundId29, @TeamId15, @TeamId21, NULL, NULL, NULL, 0), -- Rayo Vallecano vs RCD Espanyol
            (NEWID(), @RoundId29, @TeamId12, @TeamId18, NULL, NULL, NULL, 0), -- RCD Mallorca vs UD Las Palmas

        -- KOLEJKA 30 (@RoundId30) - Nierozegrane (NULL)
            (NEWID(), @RoundId30, @TeamId14, @TeamId1, NULL, NULL, NULL, 0),  -- Getafe CF vs Real Madrid CF
            (NEWID(), @RoundId30, @TeamId10, @TeamId2, NULL, NULL, NULL, 0),  -- Osasuna Pampeluna vs FC Barcelona
            (NEWID(), @RoundId30, @TeamId19, @TeamId3, NULL, NULL, NULL, 0),  -- Deportivo Alavés vs Atlético Madrid
            (NEWID(), @RoundId30, @TeamId6, @TeamId5, NULL, NULL, NULL, 0),   -- Sevilla FC vs Real Sociedad
            (NEWID(), @RoundId30, @TeamId8, @TeamId4, NULL, NULL, NULL, 0),   -- Real Betis vs Valencia CF
            (NEWID(), @RoundId30, @TeamId16, @TeamId7, NULL, NULL, NULL, 0),  -- Girona FC vs Villarreal CF
            (NEWID(), @RoundId30, @TeamId13, @TeamId9, NULL, NULL, NULL, 0),  -- Real Valladolid CF vs Athletic Club Bilbao
            (NEWID(), @RoundId30, @TeamId23, @TeamId11, NULL, NULL, NULL, 0), -- Elche CF vs RC Celta de Vigo
            (NEWID(), @RoundId30, @TeamId21, @TeamId15, NULL, NULL, NULL, 0), -- RCD Espanyol vs Rayo Vallecano
            (NEWID(), @RoundId30, @TeamId18, @TeamId12, NULL, NULL, NULL, 0), -- UD Las Palmas vs RCD Mallorca
            
            -- KOLEJKA 31 (@RoundId31) - Nierozegrane (NULL)
            (NEWID(), @RoundId31, @TeamId1, @TeamId21, NULL, NULL, NULL, 0),  -- Real Madrid CF vs RCD Espanyol
            (NEWID(), @RoundId31, @TeamId2, @TeamId15, NULL, NULL, NULL, 0),  -- FC Barcelona vs Rayo Vallecano
            (NEWID(), @RoundId31, @TeamId3, @TeamId13, NULL, NULL, NULL, 0),  -- Atlético Madrid vs Real Valladolid CF
            (NEWID(), @RoundId31, @TeamId5, @TeamId10, NULL, NULL, NULL, 0),  -- Real Sociedad vs Osasuna Pampeluna
            (NEWID(), @RoundId31, @TeamId4, @TeamId6, NULL, NULL, NULL, 0),   -- Valencia CF vs Sevilla FC
            (NEWID(), @RoundId31, @TeamId8, @TeamId7, NULL, NULL, NULL, 0),   -- Real Betis vs Villarreal CF
            (NEWID(), @RoundId31, @TeamId9, @TeamId16, NULL, NULL, NULL, 0),  -- Athletic Club Bilbao vs Girona FC
            (NEWID(), @RoundId31, @TeamId11, @TeamId19, NULL, NULL, NULL, 0), -- RC Celta de Vigo vs Deportivo Alavés
            (NEWID(), @RoundId31, @TeamId14, @TeamId23, NULL, NULL, NULL, 0), -- Getafe CF vs Elche CF
            (NEWID(), @RoundId31, @TeamId18, @TeamId12, NULL, NULL, NULL, 0), -- UD Las Palmas vs RCD Mallorca

        -- KOLEJKA 32 (@RoundId32) - Nierozegrane (NULL)
            (NEWID(), @RoundId32, @TeamId21, @TeamId1, NULL, NULL, NULL, 0),  -- RCD Espanyol vs Real Madrid CF
            (NEWID(), @RoundId32, @TeamId15, @TeamId2, NULL, NULL, NULL, 0),  -- Rayo Vallecano vs FC Barcelona
            (NEWID(), @RoundId32, @TeamId13, @TeamId3, NULL, NULL, NULL, 0),  -- Real Valladolid CF vs Atlético Madrid
            (NEWID(), @RoundId32, @TeamId10, @TeamId5, NULL, NULL, NULL, 0),  -- Osasuna Pampeluna vs Real Sociedad
            (NEWID(), @RoundId32, @TeamId6, @TeamId4, NULL, NULL, NULL, 0),   -- Sevilla FC vs Valencia CF
            (NEWID(), @RoundId32, @TeamId7, @TeamId8, NULL, NULL, NULL, 0),   -- Villarreal CF vs Real Betis
            (NEWID(), @RoundId32, @TeamId16, @TeamId9, NULL, NULL, NULL, 0),  -- Girona FC vs Athletic Club Bilbao
            (NEWID(), @RoundId32, @TeamId19, @TeamId11, NULL, NULL, NULL, 0), -- Deportivo Alavés vs RC Celta de Vigo
            (NEWID(), @RoundId32, @TeamId23, @TeamId14, NULL, NULL, NULL, 0), -- Elche CF vs Getafe CF
            (NEWID(), @RoundId32, @TeamId12, @TeamId18, NULL, NULL, NULL, 0), -- RCD Mallorca vs UD Las Palmas

        -- KOLEJKA 33 (@RoundId33) - Nierozegrane (NULL)
            (NEWID(), @RoundId33, @TeamId1, @TeamId2, NULL, NULL, NULL, 0),   -- Real Madrid CF vs FC Barcelona
            (NEWID(), @RoundId33, @TeamId3, @TeamId15, NULL, NULL, NULL, 0),  -- Atlético Madrid vs Rayo Vallecano
            (NEWID(), @RoundId33, @TeamId5, @TeamId16, NULL, NULL, NULL, 0),  -- Real Sociedad vs Girona FC
            (NEWID(), @RoundId33, @TeamId6, @TeamId19, NULL, NULL, NULL, 0),  -- Sevilla FC vs Deportivo Alavés
            (NEWID(), @RoundId33, @TeamId4, @TeamId9, NULL, NULL, NULL, 0),   -- Valencia CF vs Athletic Club Bilbao
            (NEWID(), @RoundId33, @TeamId8, @TeamId10, NULL, NULL, NULL, 0),  -- Real Betis vs Osasuna Pampeluna
            (NEWID(), @RoundId33, @TeamId7, @TeamId14, NULL, NULL, NULL, 0),  -- Villarreal CF vs Getafe CF
            (NEWID(), @RoundId33, @TeamId11, @TeamId21, NULL, NULL, NULL, 0), -- RC Celta de Vigo vs RCD Espanyol
            (NEWID(), @RoundId33, @TeamId23, @TeamId13, NULL, NULL, NULL, 0), -- Elche CF vs Real Valladolid CF
            (NEWID(), @RoundId33, @TeamId18, @TeamId12, NULL, NULL, NULL, 0), -- UD Las Palmas vs RCD Mallorca

        -- KOLEJKA 34 (@RoundId34) - Nierozegrane (NULL)
            (NEWID(), @RoundId34, @TeamId2, @TeamId1, NULL, NULL, NULL, 0),   -- FC Barcelona vs Real Madrid CF
            (NEWID(), @RoundId34, @TeamId15, @TeamId3, NULL, NULL, NULL, 0),  -- Rayo Vallecano vs Atlético Madrid
            (NEWID(), @RoundId34, @TeamId16, @TeamId5, NULL, NULL, NULL, 0),  -- Girona FC vs Real Sociedad
            (NEWID(), @RoundId34, @TeamId19, @TeamId6, NULL, NULL, NULL, 0),  -- Deportivo Alavés vs Sevilla FC
            (NEWID(), @RoundId34, @TeamId9, @TeamId4, NULL, NULL, NULL, 0),   -- Athletic Club Bilbao vs Valencia CF
            (NEWID(), @RoundId34, @TeamId10, @TeamId8, NULL, NULL, NULL, 0),  -- Osasuna Pampeluna vs Real Betis
            (NEWID(), @RoundId34, @TeamId14, @TeamId7, NULL, NULL, NULL, 0),  -- Getafe CF vs Villarreal CF
            (NEWID(), @RoundId34, @TeamId21, @TeamId11, NULL, NULL, NULL, 0), -- RCD Espanyol vs RC Celta de Vigo
            (NEWID(), @RoundId34, @TeamId13, @TeamId23, NULL, NULL, NULL, 0), -- Real Valladolid CF vs Elche CF
            (NEWID(), @RoundId34, @TeamId12, @TeamId18, NULL, NULL, NULL, 0), -- RCD Mallorca vs UD Las Palmas

        -- KOLEJKA 35 (@RoundId35) - Nierozegrane (NULL)
            (NEWID(), @RoundId35, @TeamId1, @TeamId13, NULL, NULL, NULL, 0),  -- Real Madrid CF vs Real Valladolid CF
            (NEWID(), @RoundId35, @TeamId2, @TeamId11, NULL, NULL, NULL, 0),  -- FC Barcelona vs RC Celta de Vigo
            (NEWID(), @RoundId35, @TeamId3, @TeamId16, NULL, NULL, NULL, 0),  -- Atlético Madrid vs Girona FC
            (NEWID(), @RoundId35, @TeamId5, @TeamId23, NULL, NULL, NULL, 0),  -- Real Sociedad vs Elche CF
            (NEWID(), @RoundId35, @TeamId6, @TeamId15, NULL, NULL, NULL, 0),  -- Sevilla FC vs Rayo Vallecano
            (NEWID(), @RoundId35, @TeamId4, @TeamId19, NULL, NULL, NULL, 0),  -- Valencia CF vs Deportivo Alavés
            (NEWID(), @RoundId35, @TeamId8, @TeamId9, NULL, NULL, NULL, 0),   -- Real Betis vs Athletic Club Bilbao
            (NEWID(), @RoundId35, @TeamId7, @TeamId10, NULL, NULL, NULL, 0),  -- Villarreal CF vs Osasuna Pampeluna
            (NEWID(), @RoundId35, @TeamId14, @TeamId12, NULL, NULL, NULL, 0), -- Getafe CF vs RCD Mallorca
            (NEWID(), @RoundId35, @TeamId21, @TeamId18, NULL, NULL, NULL, 0), -- RCD Espanyol vs UD Las Palmas

        -- KOLEJKA 36 (@RoundId36) - Nierozegrane (NULL)
            (NEWID(), @RoundId36, @TeamId13, @TeamId1, NULL, NULL, NULL, 0),  -- Real Valladolid CF vs Real Madrid CF
            (NEWID(), @RoundId36, @TeamId11, @TeamId2, NULL, NULL, NULL, 0),  -- RC Celta de Vigo vs FC Barcelona
            (NEWID(), @RoundId36, @TeamId16, @TeamId3, NULL, NULL, NULL, 0),  -- Girona FC vs Atlético Madrid
            (NEWID(), @RoundId36, @TeamId23, @TeamId5, NULL, NULL, NULL, 0),  -- Elche CF vs Real Sociedad
            (NEWID(), @RoundId36, @TeamId15, @TeamId6, NULL, NULL, NULL, 0),  -- Rayo Vallecano vs Sevilla FC
            (NEWID(), @RoundId36, @TeamId19, @TeamId4, NULL, NULL, NULL, 0),  -- Deportivo Alavés vs Valencia CF
            (NEWID(), @RoundId36, @TeamId9, @TeamId8, NULL, NULL, NULL, 0),   -- Athletic Club Bilbao vs Real Betis
            (NEWID(), @RoundId36, @TeamId10, @TeamId7, NULL, NULL, NULL, 0),  -- Osasuna Pampeluna vs Villarreal CF
            (NEWID(), @RoundId36, @TeamId12, @TeamId14, NULL, NULL, NULL, 0), -- RCD Mallorca vs Getafe CF
            (NEWID(), @RoundId36, @TeamId18, @TeamId21, NULL, NULL, NULL, 0), -- UD Las Palmas vs RCD Espanyol

        -- KOLEJKA 37 (@RoundId37) - Nierozegrane (NULL)
            (NEWID(), @RoundId37, @TeamId1, @TeamId18, NULL, NULL, NULL, 0),  -- Real Madrid CF vs UD Las Palmas
            (NEWID(), @RoundId37, @TeamId2, @TeamId12, NULL, NULL, NULL, 0),  -- FC Barcelona vs RCD Mallorca
            (NEWID(), @RoundId37, @TeamId3, @TeamId11, NULL, NULL, NULL, 0),  -- Atlético Madrid vs RC Celta de Vigo
            (NEWID(), @RoundId37, @TeamId5, @TeamId14, NULL, NULL, NULL, 0),  -- Real Sociedad vs Getafe CF
            (NEWID(), @RoundId37, @TeamId6, @TeamId23, NULL, NULL, NULL, 0),  -- Sevilla FC vs Elche CF
            (NEWID(), @RoundId37, @TeamId4, @TeamId10, NULL, NULL, NULL, 0),  -- Valencia CF vs Osasuna Pampeluna
            (NEWID(), @RoundId37, @TeamId8, @TeamId19, NULL, NULL, NULL, 0),  -- Real Betis vs Deportivo Alavés
            (NEWID(), @RoundId37, @TeamId7, @TeamId9, NULL, NULL, NULL, 0),   -- Villarreal CF vs Athletic Club Bilbao
            (NEWID(), @RoundId37, @TeamId16, @TeamId21, NULL, NULL, NULL, 0), -- Girona FC vs RCD Espanyol
            (NEWID(), @RoundId37, @TeamId15, @TeamId13, NULL, NULL, NULL, 0), -- Rayo Vallecano vs Real Valladolid CF

        -- KOLEJKA 38 (@RoundId38) - Nierozegrane (NULL) - Ostatnia runda
            (NEWID(), @RoundId38, @TeamId18, @TeamId1, NULL, NULL, NULL, 0),  -- UD Las Palmas vs Real Madrid CF
            (NEWID(), @RoundId38, @TeamId12, @TeamId2, NULL, NULL, NULL, 0),  -- RCD Mallorca vs FC Barcelona
            (NEWID(), @RoundId38, @TeamId11, @TeamId3, NULL, NULL, NULL, 0),  -- RC Celta de Vigo vs Atlético Madrid
            (NEWID(), @RoundId38, @TeamId14, @TeamId5, NULL, NULL, NULL, 0),  -- Getafe CF vs Real Sociedad
            (NEWID(), @RoundId38, @TeamId23, @TeamId6, NULL, NULL, NULL, 0),  -- Elche CF vs Sevilla FC
            (NEWID(), @RoundId38, @TeamId10, @TeamId4, NULL, NULL, NULL, 0),  -- Osasuna Pampeluna vs Valencia CF
            (NEWID(), @RoundId38, @TeamId19, @TeamId8, NULL, NULL, NULL, 0),  -- Deportivo Alavés vs Real Betis
            (NEWID(), @RoundId38, @TeamId9, @TeamId7, NULL, NULL, NULL, 0),   -- Athletic Club Bilbao vs Villarreal CF
            (NEWID(), @RoundId38, @TeamId21, @TeamId16, NULL, NULL, NULL, 0), -- RCD Espanyol vs Girona FC
            (NEWID(), @RoundId38, @TeamId13, @TeamId15, NULL, NULL, NULL, 0);  -- Real Valladolid CF vs Rayo Vallecano
    -- ================================================================
    -- SEASON STATS (SeasonStats)
    -- ================================================================

    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.SeasonStats WHERE SeasonYear = '2025/2026' AND LeagueId = @LeagueId)
    INSERT INTO dbo.SeasonStats (Id, TeamId, SeasonYear, LeagueId, MatchesPlayed, Wins, Losses, Draws, GoalsFor, GoalsAgainst)
        VALUES
        -- 2024/2025 Season
        (NEWID(), @TeamId2, '2024/2025', @LeagueId, 38, 28, 6, 4, 102, 39),   -- 1. FC Barcelona
        (NEWID(), @TeamId1, '2024/2025', @LeagueId, 38, 26, 6, 6, 78, 38),    -- 2. Real Madrid
        (NEWID(), @TeamId3, '2024/2025', @LeagueId, 38, 22, 6, 10, 68, 30),   -- 3. Atletico Madryt
        (NEWID(), @TeamId9, '2024/2025', @LeagueId, 38, 19, 6, 13, 54, 29),   -- 4. Athletic Bilbao
        (NEWID(), @TeamId7, '2024/2025', @LeagueId, 38, 20, 8, 10, 71, 51),   -- 5. Villarreal
        (NEWID(), @TeamId8, '2024/2025', @LeagueId, 38, 16, 10, 12, 57, 50),  -- 6. Real Betis
        (NEWID(), @TeamId11, '2024/2025', @LeagueId, 38, 16, 15, 7, 59, 57),  -- 7. Celta Vigo
        (NEWID(), @TeamId15, '2024/2025', @LeagueId, 38, 13, 12, 13, 41, 45), -- 8. Rayo Vallecano
        (NEWID(), @TeamId10, '2024/2025', @LeagueId, 38, 12, 10, 16, 48, 52), -- 9. Osasuna Pampeluna
        (NEWID(), @TeamId12, '2024/2025', @LeagueId, 38, 13, 16, 9, 35, 44),  -- 10. Mallorca
        (NEWID(), @TeamId5, '2024/2025', @LeagueId, 38, 13, 18, 7, 35, 46),   -- 11. Real Sociedad
        (NEWID(), @TeamId4, '2024/2025', @LeagueId, 38, 11, 14, 13, 44, 54),  -- 12. Valencia CF
        (NEWID(), @TeamId14, '2024/2025', @LeagueId, 38, 11, 18, 9, 34, 39),  -- 13. Getafe
        (NEWID(), @TeamId21, '2024/2025', @LeagueId, 38, 11, 18, 9, 40, 51),  -- 14. Espanyol
        (NEWID(), @TeamId19, '2024/2025', @LeagueId, 38, 10, 16, 12, 38, 48), -- 15. Deportivo Alaves
        (NEWID(), @TeamId16, '2024/2025', @LeagueId, 38, 11, 19, 8, 44, 60),  -- 16. Girona
        (NEWID(), @TeamId6, '2024/2025', @LeagueId, 38, 10, 17, 11, 42, 55),   -- 17. Sevilla FC
        (NEWID(), @TeamId26, '2024/2025', @LeagueId, 38, 9, 16, 13, 39, 56),   -- 18. Leganes
        (NEWID(), @TeamId18, '2024/2025', @LeagueId, 38, 8, 22, 8, 40, 61),    -- 19. Las Palmas
        (NEWID(), @TeamId13, '2024/2025', @LeagueId, 38, 4, 30, 4, 26, 90),    -- 20. Real Valladolid


        -- 2023/2024 Season
        (NEWID(), @TeamId1, '2023/2024', @LeagueId, 38, 29, 1, 8, 87, 26),   -- 1. Real Madrid
        (NEWID(), @TeamId2, '2023/2024', @LeagueId, 38, 26, 5, 7, 79, 44),   -- 2. FC Barcelona
        (NEWID(), @TeamId16, '2023/2024', @LeagueId, 38, 25, 7, 6, 85, 46),  -- 3. Girona
        (NEWID(), @TeamId3, '2023/2024', @LeagueId, 38, 24, 10, 4, 70, 43),  -- 4. Atletico Madryt
        (NEWID(), @TeamId9, '2023/2024', @LeagueId, 38, 19, 8, 11, 61, 37),  -- 5. Athletic Bilbao
        (NEWID(), @TeamId5, '2023/2024', @LeagueId, 38, 16, 10, 12, 51, 39),  -- 6. Real Sociedad
        (NEWID(), @TeamId8, '2023/2024', @LeagueId, 38, 14, 9, 15, 48, 45),   -- 7. Real Betis
        (NEWID(), @TeamId7, '2023/2024', @LeagueId, 38, 14, 13, 11, 65, 65),  -- 8. Villarreal
        (NEWID(), @TeamId4, '2023/2024', @LeagueId, 38, 13, 15, 10, 40, 45),  -- 9. Valencia CF
        (NEWID(), @TeamId19, '2023/2024', @LeagueId, 38, 12, 16, 10, 36, 46), -- 10. Deportivo Alaves
        (NEWID(), @TeamId10, '2023/2024', @LeagueId, 38, 12, 17, 9, 45, 56),  -- 11. Osasuna Pampeluna
        (NEWID(), @TeamId14, '2023/2024', @LeagueId, 38, 10, 15, 13, 42, 54), -- 12. Getafe
        (NEWID(), @TeamId11, '2023/2024', @LeagueId, 38, 10, 17, 11, 46, 57), -- 13. Celta Vigo
        (NEWID(), @TeamId6, '2023/2024', @LeagueId, 38, 10, 17, 11, 48, 54),  -- 14. Sevilla FC
        (NEWID(), @TeamId12, '2023/2024', @LeagueId, 38, 8, 14, 16, 33, 44),  -- 15. Mallorca
        (NEWID(), @TeamId18, '2023/2024', @LeagueId, 38, 10, 18, 10, 33, 47), -- 16. Las Palmas
        (NEWID(), @TeamId15, '2023/2024', @LeagueId, 38, 8, 16, 14, 29, 48),  -- 17. Rayo Vallecano
        (NEWID(), @TeamId20, '2023/2024', @LeagueId, 38, 6, 17, 15, 26, 55),  -- 18. Cadiz
        (NEWID(), @TeamId27, '2023/2024', @LeagueId, 38, 3, 23, 12, 43, 75),  -- 19. Almeria
        (NEWID(), @TeamId28, '2023/2024', @LeagueId, 38, 4, 25, 9, 38, 79),   -- 20. Granada


        -- 2022/2023 Season
        (NEWID(), @TeamId2, '2022/2023', @LeagueId, 38, 28, 6, 4, 70, 20),   -- 1. FC Barcelona
        (NEWID(), @TeamId1, '2022/2023', @LeagueId, 38, 24, 8, 6, 75, 36),   -- 2. Real Madrid
        (NEWID(), @TeamId3, '2022/2023', @LeagueId, 38, 23, 7, 8, 70, 33),   -- 3. Atletico Madryt
        (NEWID(), @TeamId5, '2022/2023', @LeagueId, 38, 21, 9, 8, 51, 35),   -- 4. Real Sociedad
        (NEWID(), @TeamId7, '2022/2023', @LeagueId, 38, 19, 12, 7, 59, 40),  -- 5. Villarreal
        (NEWID(), @TeamId8, '2022/2023', @LeagueId, 38, 17, 12, 9, 46, 41),  -- 6. Real Betis
        (NEWID(), @TeamId10, '2022/2023', @LeagueId, 38, 15, 15, 8, 37, 42), -- 7. Osasuna Pampeluna
        (NEWID(), @TeamId9, '2022/2023', @LeagueId, 38, 14, 15, 9, 47, 43),  -- 8. Athletic Bilbao
        (NEWID(), @TeamId12, '2022/2023', @LeagueId, 38, 14, 16, 8, 37, 43), -- 9. Mallorca
        (NEWID(), @TeamId16, '2022/2023', @LeagueId, 38, 13, 15, 10, 58, 55), -- 10. Girona
        (NEWID(), @TeamId15, '2022/2023', @LeagueId, 38, 13, 15, 10, 45, 53), -- 11. Rayo Vallecano
        (NEWID(), @TeamId6, '2022/2023', @LeagueId, 38, 13, 15, 10, 47, 54),  -- 12. Sevilla FC
        (NEWID(), @TeamId11, '2022/2023', @LeagueId, 38, 11, 17, 10, 43, 53), -- 13. Celta Vigo
        (NEWID(), @TeamId20, '2022/2023', @LeagueId, 38, 10, 16, 12, 30, 53), -- 14. Cadiz
        (NEWID(), @TeamId14, '2022/2023', @LeagueId, 38, 10, 16, 12, 34, 45), -- 15. Getafe
        (NEWID(), @TeamId4, '2022/2023', @LeagueId, 38, 11, 18, 9, 42, 45),   -- 16. Valencia CF
        (NEWID(), @TeamId27, '2022/2023', @LeagueId, 38, 11, 19, 8, 49, 65),  -- 17. Almeria
        (NEWID(), @TeamId13, '2022/2023', @LeagueId, 38, 11, 20, 7, 33, 63),  -- 18. Real Valladolid
        (NEWID(), @TeamId21, '2022/2023', @LeagueId, 38, 8, 17, 13, 52, 69),  -- 19. Espanyol
        (NEWID(), @TeamId23, '2022/2023', @LeagueId, 38, 5, 23, 10, 30, 67);   -- 20. Elche

    COMMIT TRANSACTION
    PRINT '✅ SUCCESS! La Liga 2025/2026'
    PRINT '   - max match rounds: 38'
    PRINT '   - matches inserted: ~380'
    PRINT '   - teams: 20'
    PRINT '   - Seasons: 2025/2026 + historical data (2024/2025, 2023/2024, 2022/2023)'
    PRINT '   - Status:'
    PRINT '     • played rounds: 1-17'
    PRINT '     • not played rounds: 18-38'
    PRINT '   - last update 06.12.2025'
    PRINT '//////'

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT '❌ Cannot insert! La Liga 2025/2026'
    PRINT 'Error: ' + ERROR_MESSAGE()
END CATCH

GO
