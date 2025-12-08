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
    @TeamId10 UNIQUEIDENTIFIER = '4f5e6d7c-8b9a-0f1e-2d3c-4b5a6f7e8d9c',  -- CA Osasuna
    @TeamId11 UNIQUEIDENTIFIER = '3e4d5c6b-7a8f-9e0d-1c2b-3a4f5e6d7c8b',  -- RC Celta de Vigo
    @TeamId12 UNIQUEIDENTIFIER = '2d3c4b5a-6f7e-8d9c-0b1a-2f3e4d5c6b7a',  -- RCD Mallorca
    @TeamId13 UNIQUEIDENTIFIER = '1c2b3a4f-5e6d-7c8b-9a0f-1e2d3c4b5a6f',  -- Real Valladolid
    @TeamId14 UNIQUEIDENTIFIER = '0b1a2f3e-4d5c-6b7a-8f9e-0d1c2b3a4f5e',  -- Getafe CF
    @TeamId15 UNIQUEIDENTIFIER = 'f9e8d7c6-b5a4-3291-807f-6e5d4c3b2a19',  -- Rayo Vallecano
    @TeamId16 UNIQUEIDENTIFIER = 'e8d7c6b5-a4f3-2190-9f8e-7d6c5b4a3918',  -- Girona FC
    @TeamId17 UNIQUEIDENTIFIER = 'd7c6b5a4-f3e2-1908-8f7e-6d5c4b3a2917',  -- Real Oviedo
    @TeamId18 UNIQUEIDENTIFIER = 'c6b5a4f3-e2d1-0897-7f6e-5d4c3b2a1916',  -- UD Las Palmas
    @TeamId19 UNIQUEIDENTIFIER = 'b5a4f3e2-d1c0-0796-6f5e-4d3c2b1a0815',  -- Deportivo Alavés
    @TeamId20 UNIQUEIDENTIFIER = 'a4f3e2d1-c0b9-0695-5f4e-3d2c1b0a0f14',  -- Cádiz CF
    @TeamId21 UNIQUEIDENTIFIER = '9f4e3d2c-b1a0-0594-4e5d-2c1b0a9f8e7d',  -- RCD Espanyol
    @TeamId22 UNIQUEIDENTIFIER = '8e3d2c1b-a09f-0483-3d4c-1b0a9f8e7d6c',  -- Real Sociedad II
    @TeamId23 UNIQUEIDENTIFIER = '7d2c1b0a-9f8e-0372-2c3b-0a9f8e7d6c5b',  -- Elche CF
    @TeamId24 UNIQUEIDENTIFIER = '6c1b0a9f-8e7d-0261-1b2a-9f8e7d6c5b4a',  -- Real Madrid Castilla
    @TeamId25 UNIQUEIDENTIFIER = '0dd8d49f-1546-4938-af28-4b6243f3f424',  -- Levante
    @TeamId26 UNIQUEIDENTIFIER = '053ed585-c601-4628-b650-f01c3e31142b',  -- Leganes
    @TeamId27 UNIQUEIDENTIFIER = '4083f6b1-b562-401f-83a4-f21cc24ea347',  -- Almeria
    @TeamId28 UNIQUEIDENTIFIER = '00f250fc-6d15-4ab5-a26d-2db1a19879ef',  -- Granada

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
        (NEWID(), @RoundId1, @TeamId16, @TeamId15, 1, 3, 0, 1),  -- Girona 1-3 Rayo Vallecano
        (NEWID(), @RoundId1, @TeamId7, @TeamId17, 2, 0, 0, 1),   -- Villarreal 2-0 R. Oviedo
        (NEWID(), @RoundId1, @TeamId12, @TeamId2, 0, 3, 0, 1),   -- Mallorca 0-3 FC Barcelona
        (NEWID(), @RoundId1, @TeamId19, @TeamId25, 2, 1, 0, 1),  -- Alavés 2-1 Levante
        (NEWID(), @RoundId1, @TeamId4, @TeamId5, 1, 1, 1, 1),    -- Valencia 1-1 Real Sociedad (DRAW)
        (NEWID(), @RoundId1, @TeamId11, @TeamId14, 0, 2, 0, 1),  -- Celta de Vigo 0-2 Getafe CF
        (NEWID(), @RoundId1, @TeamId9, @TeamId6, 3, 2, 0, 1),    -- Athletic Bilbao 3-2 Sevilla
        (NEWID(), @RoundId1, @TeamId21, @TeamId3, 2, 1, 0, 1),   -- Espanyol Barcelona 2-1 Atlético Madryt
        (NEWID(), @RoundId1, @TeamId23, @TeamId8, 1, 1, 1, 1),   -- Elche CF 1-1 Betis Sevilla (DRAW)
        (NEWID(), @RoundId1, @TeamId1, @TeamId10, 1, 0, 0, 1),   -- Real Madryt 1-0 Osasuna

        -- KOLEJKA 2 (@RoundId2)
        (NEWID(), @RoundId2, @TeamId8, @TeamId19, 1, 0, 0, 1),   -- Betis Sevilla 1-0 Deportivo Alavés
        (NEWID(), @RoundId2, @TeamId12, @TeamId11, 1, 1, 1, 1),  -- Mallorca 1-1 Celta de Vigo (DRAW)
        (NEWID(), @RoundId2, @TeamId3, @TeamId23, 1, 1, 1, 1),   -- Atlético Madryt 1-1 Elche CF (DRAW)
        (NEWID(), @RoundId2, @TeamId25, @TeamId2, 2, 3, 0, 1),   -- Levante 2-3 FC Barcelona
        (NEWID(), @RoundId2, @TeamId10, @TeamId4, 1, 0, 0, 1),   -- Osasuna 1-0 Valencia
        (NEWID(), @RoundId2, @TeamId5, @TeamId21, 2, 2, 1, 1),   -- Real Sociedad 2-2 Espanyol Barcelona (DRAW)
        (NEWID(), @RoundId2, @TeamId7, @TeamId16, 5, 0, 0, 1),   -- Villarreal 5-0 Girona FC
        (NEWID(), @RoundId2, @TeamId17, @TeamId1, 0, 3, 0, 1),   -- R. Oviedo 0-3 Real Madryt
        (NEWID(), @RoundId2, @TeamId9, @TeamId15, 1, 0, 0, 1),   -- Athletic Bilbao 1-0 Rayo Vallecano
        (NEWID(), @RoundId2, @TeamId6, @TeamId14, 1, 2, 0, 1),   -- Sevilla 1-2 Getafe CF

        -- KOLEJKA 3 (@RoundId3)
        (NEWID(), @RoundId3, @TeamId23, @TeamId25, 2, 0, 0, 1),   -- Elche CF 2-0 Levante
        (NEWID(), @RoundId3, @TeamId4, @TeamId14, 3, 0, 0, 1),    -- Valencia 3-0 Getafe CF
        (NEWID(), @RoundId3, @TeamId19, @TeamId3, 1, 1, 1, 1),    -- Alavés 1-1 Atlético Madryt (DRAW)
        (NEWID(), @RoundId3, @TeamId17, @TeamId5, 1, 0, 0, 1),    -- R. Oviedo 1-0 Real Sociedad (UPSET!)
        (NEWID(), @RoundId3, @TeamId16, @TeamId6, 0, 2, 0, 1),    -- Girona FC 0-2 Sevilla
        (NEWID(), @RoundId3, @TeamId1, @TeamId12, 2, 1, 0, 1),    -- Real Madryt 2-1 Mallorca
        (NEWID(), @RoundId3, @TeamId11, @TeamId7, 1, 1, 1, 1),    -- Celta de Vigo 1-1 Villarreal (DRAW)
        (NEWID(), @RoundId3, @TeamId8, @TeamId9, 1, 2, 0, 1),     -- Betis Sevilla 1-2 Athletic Bilbao
        (NEWID(), @RoundId3, @TeamId21, @TeamId10, 1, 0, 0, 1),   -- Espanyol Barcelona 1-0 Osasuna
        (NEWID(), @RoundId3, @TeamId15, @TeamId2, 1, 1, 1, 1),    -- Rayo Vallecano 1-1 FC Barcelona (DRAW)

        -- KOLEJKA 4 (@RoundId4)
        (NEWID(), @RoundId4, @TeamId6, @TeamId23, 2, 2, 1, 1),   -- Sevilla 2-2 Elche CF (DRAW)
        (NEWID(), @RoundId4, @TeamId14, @TeamId17, 2, 0, 0, 1),  -- Getafe CF 2-0 R. Oviedo
        (NEWID(), @RoundId4, @TeamId5, @TeamId1, 1, 2, 0, 1),    -- Real Sociedad 1-2 Real Madryt
        (NEWID(), @RoundId4, @TeamId9, @TeamId19, 0, 1, 0, 1),   -- Athletic Bilbao 0-1 Alaves
        (NEWID(), @RoundId4, @TeamId3, @TeamId7, 2, 0, 0, 1),    -- Atlético Madryt 2-0 Villarreal
        (NEWID(), @RoundId4, @TeamId11, @TeamId16, 1, 1, 1, 1),  -- Celta de Vigo 1-1 Girona FC (DRAW)
        (NEWID(), @RoundId4, @TeamId25, @TeamId8, 2, 2, 1, 1),   -- Levante 2-2 Betis Sevilla (DRAW)
        (NEWID(), @RoundId4, @TeamId10, @TeamId15, 2, 0, 0, 1),  -- Osasuna 2-0 Rayo Vallecano
        (NEWID(), @RoundId4, @TeamId2, @TeamId4, 6, 0, 0, 1),    -- FC Barcelona 6-0 Valencia 
        (NEWID(), @RoundId4, @TeamId21, @TeamId12, 3, 2, 0, 1),  -- Espanyol Barcelona 3-2 Mallorca

        -- KOLEJKA 5 (@RoundId5)
        (NEWID(), @RoundId5, @TeamId8, @TeamId5, 3, 1, 0, 1),    -- Betis Sevilla 3-1 Real Sociedad
        (NEWID(), @RoundId5, @TeamId16, @TeamId25, 0, 4, 0, 1),  -- Girona FC 0-4 Levante
        (NEWID(), @RoundId5, @TeamId1, @TeamId21, 2, 0, 0, 1),   -- Real Madryt 2-0 Espanyol Barcelona
        (NEWID(), @RoundId5, @TeamId7, @TeamId10, 2, 1, 0, 1),   -- Villarreal 2-1 Osasuna
        (NEWID(), @RoundId5, @TeamId19, @TeamId6, 1, 2, 0, 1),   -- Alaves 1-2 Sevilla
        (NEWID(), @RoundId5, @TeamId4, @TeamId9, 2, 0, 0, 1),    -- Valencia 2-0 Athletic Bilbao
        (NEWID(), @RoundId5, @TeamId15, @TeamId11, 1, 1, 1, 1),  -- Rayo Vallecano 1-1 Celta de Vigo (DRAW)
        (NEWID(), @RoundId5, @TeamId12, @TeamId3, 1, 1, 1, 1),   -- Mallorca 1-1 Atlético Madryt (DRAW)
        (NEWID(), @RoundId5, @TeamId23, @TeamId17, 1, 0, 0, 1),  -- Elche CF 1-0 R. Oviedo
        (NEWID(), @RoundId5, @TeamId2, @TeamId14, 3, 0, 0, 1),   -- FC Barcelona 3-0 Getafe CF

        -- KOLEJKA 6 (@RoundId6)
        (NEWID(), @RoundId6, @TeamId11, @TeamId8, 1, 1, 1, 1),   -- Celta de Vigo 1-1 Betis Sevilla (DRAW)
        (NEWID(), @RoundId6, @TeamId9, @TeamId16, 1, 1, 1, 1),   -- Athletic Bilbao 1-1 Girona FC (DRAW)
        (NEWID(), @RoundId6, @TeamId21, @TeamId4, 2, 2, 1, 1),   -- Espanyol Barcelona 2-2 Valencia (DRAW)
        (NEWID(), @RoundId6, @TeamId25, @TeamId1, 1, 4, 0, 1),   -- Levante 1-4 Real Madryt
        (NEWID(), @RoundId6, @TeamId6, @TeamId7, 1, 2, 0, 1),    -- Sevilla 1-2 Villarreal
        (NEWID(), @RoundId6, @TeamId14, @TeamId19, 1, 1, 1, 1),  -- Getafe CF 1-1 Alaves (DRAW)
        (NEWID(), @RoundId6, @TeamId5, @TeamId12, 1, 0, 0, 1),   -- Real Sociedad 1-0 Mallorca
        (NEWID(), @RoundId6, @TeamId3, @TeamId15, 3, 2, 0, 1),   -- Atlético Madryt 3-2 Rayo Vallecano
        (NEWID(), @RoundId6, @TeamId10, @TeamId23, 1, 1, 1, 1),  -- Osasuna 1-1 Elche CF (DRAW)
        (NEWID(), @RoundId6, @TeamId17, @TeamId2, 1, 3, 0, 1),   -- R. Oviedo 1-3 FC Barcelona

        -- KOLEJKA 7 (@RoundId7)
        (NEWID(), @RoundId7, @TeamId16, @TeamId21, 0, 0, 1, 1),   -- Girona FC 0-0 Espanyol Barcelona (DRAW)
        (NEWID(), @RoundId7, @TeamId14, @TeamId25, 1, 1, 1, 1),   -- Getafe CF 1-1 Levante (DRAW)
        (NEWID(), @RoundId7, @TeamId3, @TeamId1, 5, 2, 0, 1),    -- Atlético Madryt 5-2 Real Madryt
        (NEWID(), @RoundId7, @TeamId12, @TeamId19, 1, 0, 0, 1),  -- Mallorca 1-0 Alaves
        (NEWID(), @RoundId7, @TeamId7, @TeamId9, 1, 0, 0, 1),    -- Villarreal 1-0 Athletic Bilbao
        (NEWID(), @RoundId7, @TeamId15, @TeamId6, 0, 1, 0, 1),   -- Rayo Vallecano 0-1 Sevilla
        (NEWID(), @RoundId7, @TeamId23, @TeamId11, 2, 1, 0, 1),  -- Elche CF 2-1 Celta de Vigo
        (NEWID(), @RoundId7, @TeamId2, @TeamId5, 2, 1, 0, 1),    -- FC Barcelona 2-1 Real Sociedad
        (NEWID(), @RoundId7, @TeamId8, @TeamId10, 2, 0, 0, 1),   -- Betis Sevilla 2-0 Osasuna
        (NEWID(), @RoundId7, @TeamId4, @TeamId17, 1, 2, 0, 1),   -- Valencia 1-2 R. Oviedo

        -- KOLEJKA 8 (@RoundId8)
        (NEWID(), @RoundId8, @TeamId10, @TeamId14, 2, 1, 0, 1),   -- Osasuna 2-1 Getafe CF
        (NEWID(), @RoundId8, @TeamId17, @TeamId25, 0, 2, 0, 1),   -- R. Oviedo 0-2 Levante
        (NEWID(), @RoundId8, @TeamId16, @TeamId4, 2, 1, 0, 1),    -- Girona FC 2-1 Valencia
        (NEWID(), @RoundId8, @TeamId9, @TeamId12, 2, 1, 0, 1),    -- Athletic Bilbao 2-1 Mallorca
        (NEWID(), @RoundId8, @TeamId1, @TeamId7, 3, 1, 0, 1),     -- Real Madryt 3-1 Villarreal
        (NEWID(), @RoundId8, @TeamId19, @TeamId23, 3, 1, 0, 1),   -- Alaves 3-1 Elche CF
        (NEWID(), @RoundId8, @TeamId6, @TeamId2, 4, 1, 0, 1),     -- Sevilla 4-1 FC Barcelona
        (NEWID(), @RoundId8, @TeamId21, @TeamId8, 1, 2, 0, 1),    -- Espanyol Barcelona 1-2 Betis Sevilla
        (NEWID(), @RoundId8, @TeamId5, @TeamId15, 0, 1, 0, 1),    -- Real Sociedad 0-1 Rayo Vallecano
        (NEWID(), @RoundId8, @TeamId11, @TeamId3, 1, 1, 1, 1),    -- Celta de Vigo 1-1 Atlético Madryt (DRAW)

        -- KOLEJKA 9 (@RoundId9)
        (NEWID(), @RoundId9, @TeamId17, @TeamId21, 0, 2, 0, 1),   -- R. Oviedo 0-2 Espanyol Barcelona
        (NEWID(), @RoundId9, @TeamId6, @TeamId12, 1, 3, 0, 1),    -- Sevilla 1-3 Mallorca (UPSET!)
        (NEWID(), @RoundId9, @TeamId2, @TeamId16, 2, 1, 0, 1),    -- FC Barcelona 2-1 Girona FC
        (NEWID(), @RoundId9, @TeamId7, @TeamId8, 2, 2, 1, 1),     -- Villarreal 2-2 Betis Sevilla (DRAW)
        (NEWID(), @RoundId9, @TeamId3, @TeamId10, 1, 0, 0, 1),    -- Atlético Madryt 1-0 Osasuna
        (NEWID(), @RoundId9, @TeamId23, @TeamId9, 0, 0, 1, 1),    -- Elche CF 0-0 Athletic Bilbao (DRAW)
        (NEWID(), @RoundId9, @TeamId11, @TeamId5, 1, 1, 1, 1),    -- Celta de Vigo 1-1 Real Sociedad (DRAW)
        (NEWID(), @RoundId9, @TeamId25, @TeamId15, 0, 3, 0, 1),   -- Levante 0-3 Rayo Vallecano
        (NEWID(), @RoundId9, @TeamId14, @TeamId1, 0, 1, 0, 1),    -- Getafe CF 0-1 Real Madryt
        (NEWID(), @RoundId9, @TeamId19, @TeamId4, 0, 0, 1, 1),    -- Alaves 0-0 Valencia (DRAW)

        -- KOLEJKA 10 (@RoundId10)
        (NEWID(), @RoundId10, @TeamId5, @TeamId6, 2, 1, 0, 1),    -- Real Sociedad 2-1 Sevilla
        (NEWID(), @RoundId10, @TeamId16, @TeamId17, 3, 3, 1, 1),  -- Girona FC 3-3 R. Oviedo (DRAW)
        (NEWID(), @RoundId10, @TeamId21, @TeamId23, 1, 0, 0, 1),  -- Espanyol Barcelona 1-0 Elche CF
        (NEWID(), @RoundId10, @TeamId9, @TeamId14, 0, 1, 0, 1),   -- Athletic Bilbao 0-1 Getafe CF
        (NEWID(), @RoundId10, @TeamId4, @TeamId7, 0, 2, 0, 1),    -- Valencia 0-2 Villarreal
        (NEWID(), @RoundId10, @TeamId12, @TeamId25, 1, 1, 1, 1),  -- Mallorca 1-1 Levante (DRAW)
        (NEWID(), @RoundId10, @TeamId1, @TeamId2, 2, 1, 0, 1),    -- Real Madryt 2-1 FC Barcelona
        (NEWID(), @RoundId10, @TeamId10, @TeamId11, 2, 3, 0, 1),  -- Osasuna 2-3 Celta de Vigo
        (NEWID(), @RoundId10, @TeamId15, @TeamId19, 1, 0, 0, 1),  -- Rayo Vallecano 1-0 Alaves
        (NEWID(), @RoundId10, @TeamId8, @TeamId3, 0, 2, 0, 1),    -- Betis Sevilla 0-2 Atlético Madryt

        -- KOLEJKA 11 (@RoundId11) - Rozegrane
        (NEWID(), @RoundId11, @TeamId14, @TeamId16, 2, 1, 0, 1),  -- Getafe CF 2-1 Girona FC
        (NEWID(), @RoundId11, @TeamId7, @TeamId15, 4, 0, 0, 1),   -- Villarreal 4-0 Rayo Vallecano
        (NEWID(), @RoundId11, @TeamId3, @TeamId6, 3, 0, 0, 1),    -- Atlético Madryt 3-0 Sevilla
        (NEWID(), @RoundId11, @TeamId5, @TeamId9, 3, 2, 0, 1),    -- Real Sociedad 3-2 Athletic Bilbao
        (NEWID(), @RoundId11, @TeamId1, @TeamId4, 4, 0, 0, 1),    -- Real Madryt 4-0 Valencia
        (NEWID(), @RoundId11, @TeamId25, @TeamId11, 1, 2, 0, 1),  -- Levante 1-2 Celta de Vigo
        (NEWID(), @RoundId11, @TeamId19, @TeamId21, 2, 1, 0, 1),  -- Alaves 2-1 Espanyol Barcelona
        (NEWID(), @RoundId11, @TeamId2, @TeamId23, 3, 1, 0, 1),   -- FC Barcelona 3-1 Elche CF
        (NEWID(), @RoundId11, @TeamId8, @TeamId12, 3, 0, 0, 1),   -- Betis Sevilla 3-0 Mallorca
        (NEWID(), @RoundId11, @TeamId17, @TeamId10, 0, 0, 1, 1),  -- R. Oviedo 0-0 Osasuna (DRAW)

        -- KOLEJKA 12 (@RoundId12) - Rozegrane
        (NEWID(), @RoundId12, @TeamId23, @TeamId5, 1, 1, 1, 1),   -- Elche CF 1-1 Real Sociedad (DRAW)
        (NEWID(), @RoundId12, @TeamId16, @TeamId19, 1, 0, 0, 1),  -- Girona FC 1-0 Alaves
        (NEWID(), @RoundId12, @TeamId6, @TeamId10, 1, 0, 0, 1),   -- Sevilla 1-0 Osasuna
        (NEWID(), @RoundId12, @TeamId3, @TeamId25, 3, 1, 0, 1),   -- Atlético Madryt 3-1 Levante
        (NEWID(), @RoundId12, @TeamId21, @TeamId7, 0, 2, 0, 1),   -- Espanyol Barcelona 0-2 Villarreal
        (NEWID(), @RoundId12, @TeamId9, @TeamId17, 1, 0, 0, 1),   -- Athletic Bilbao 1-0 R. Oviedo
        (NEWID(), @RoundId12, @TeamId15, @TeamId1, 0, 0, 1, 1),   -- Rayo Vallecano 0-0 Real Madryt (DRAW)
        (NEWID(), @RoundId12, @TeamId12, @TeamId14, 1, 0, 0, 1),  -- Mallorca 1-0 Getafe CF
        (NEWID(), @RoundId12, @TeamId4, @TeamId8, 1, 1, 1, 1),    -- Valencia 1-1 Betis Sevilla (DRAW)
        (NEWID(), @RoundId12, @TeamId11, @TeamId2, 2, 4, 0, 1),   -- Celta de Vigo 2-4 FC Barcelona

        -- KOLEJKA 13 (@RoundId13) - Rozegrane
        (NEWID(), @RoundId13, @TeamId4, @TeamId25, 1, 0, 0, 1),   -- Valencia 1-0 Levante
        (NEWID(), @RoundId13, @TeamId19, @TeamId11, 0, 1, 0, 1),  -- Alaves 0-1 Celta de Vigo
        (NEWID(), @RoundId13, @TeamId2, @TeamId9, 4, 0, 0, 1),    -- FC Barcelona 4-0 Athletic Bilbao
        (NEWID(), @RoundId13, @TeamId10, @TeamId5, 1, 3, 0, 1),   -- Osasuna 1-3 Real Sociedad
        (NEWID(), @RoundId13, @TeamId7, @TeamId12, 2, 1, 0, 1),   -- Villarreal 2-1 Mallorca
        (NEWID(), @RoundId13, @TeamId17, @TeamId15, 0, 0, 1, 1),  -- R. Oviedo 0-0 Rayo Vallecano (DRAW)
        (NEWID(), @RoundId13, @TeamId8, @TeamId16, 1, 1, 1, 1),   -- Betis Sevilla 1-1 Girona FC (DRAW)
        (NEWID(), @RoundId13, @TeamId14, @TeamId3, 0, 1, 0, 1),   -- Getafe CF 0-1 Atlético Madryt
        (NEWID(), @RoundId13, @TeamId23, @TeamId1, 2, 2, 1, 1),   -- Elche CF 2-2 Real Madryt (DRAW - UPSET!)
        (NEWID(), @RoundId13, @TeamId21, @TeamId6, 2, 1, 0, 1),   -- Espanyol Barcelona 2-1 Sevilla

        -- KOLEJKA 14 (@RoundId14) - Rozegrane
        (NEWID(), @RoundId14, @TeamId14, @TeamId23, 1, 0, 0, 1),  -- Getafe CF 1-0 Elche CF
        (NEWID(), @RoundId14, @TeamId12, @TeamId10, 2, 2, 1, 1),  -- Mallorca 2-2 Osasuna (DRAW)
        (NEWID(), @RoundId14, @TeamId2, @TeamId19, 3, 1, 0, 1),   -- FC Barcelona 3-1 Alaves
        (NEWID(), @RoundId14, @TeamId25, @TeamId9, 0, 2, 0, 1),   -- Levante 0-2 Athletic Bilbao
        (NEWID(), @RoundId14, @TeamId3, @TeamId17, 2, 0, 0, 1),   -- Atlético Madryt 2-0 R. Oviedo
        (NEWID(), @RoundId14, @TeamId5, @TeamId7, 2, 3, 0, 1),    -- Real Sociedad 2-3 Villarreal (AWAY THRILLER!)
        (NEWID(), @RoundId14, @TeamId6, @TeamId8, 0, 2, 0, 1),    -- Sevilla 0-2 Betis Sevilla (DERBY!)
        (NEWID(), @RoundId14, @TeamId11, @TeamId21, 0, 1, 0, 1),  -- Celta de Vigo 0-1 Espanyol Barcelona
        (NEWID(), @RoundId14, @TeamId16, @TeamId1, 1, 1, 1, 1),   -- Girona FC 1-1 Real Madryt (COMEBACK DRAW!)
        (NEWID(), @RoundId14, @TeamId15, @TeamId4, 1, 1, 1, 1),   -- Rayo Vallecano 1-1 Valencia (DRAW)

        -- KOLEJKA 15 (@RoundId15) - Rozegrane
        (NEWID(), @RoundId15, @TeamId17, @TeamId12, 0, 0, 1, 1),  -- R. Oviedo 0-0 Mallorca (DRAW)
        (NEWID(), @RoundId15, @TeamId7, @TeamId14, 2, 0, 0, 1),   -- Villarreal 2-0 Getafe CF
        (NEWID(), @RoundId15, @TeamId19, @TeamId5, 1, 0, 0, 1),   -- Alaves 1-0 Real Sociedad
        (NEWID(), @RoundId15, @TeamId8, @TeamId2, 3, 5, 0, 1),    -- Betis Sevilla 3-5 FC Barcelona (OFFENSIVE EXPLOSION!)
        (NEWID(), @RoundId15, @TeamId9, @TeamId3, 1, 0, 0, 1),    -- Athletic Bilbao 1-0 Atlético Madryt
        (NEWID(), @RoundId15, @TeamId23, @TeamId16, 3, 0, 0, 1),  -- Elche CF 3-0 Girona FC
        (NEWID(), @RoundId15, @TeamId4, @TeamId6, 1, 1, 1, 1),    -- Valencia 1-1 Sevilla (DRAW)
        (NEWID(), @RoundId15, @TeamId21, @TeamId15, 1, 0, 0, 1),  -- Espanyol Barcelona 1-0 Rayo Vallecano
        (NEWID(), @RoundId15, @TeamId1, @TeamId11, 0, 2, 0, 1),   -- Real Madryt 0-2 Celta de Vigo (HUGE UPSET!)
        (NEWID(), @RoundId15, @TeamId10, @TeamId25, 2, 0, 0, 1),  -- Osasuna 2-0 Levante

        -- KOLEJKA 16 (@RoundId16) - Rozegrane
        (NEWID(), @RoundId16, @TeamId5, @TeamId16, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId16, @TeamId3, @TeamId4, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId16, @TeamId12, @TeamId23, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId16, @TeamId2, @TeamId10, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId16, @TeamId14, @TeamId21, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId16, @TeamId6, @TeamId17, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId16, @TeamId11, @TeamId9, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId16, @TeamId25, @TeamId7, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId16, @TeamId19, @TeamId1, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId16, @TeamId15, @TeamId8, NULL, NULL, NULL, 0),

        -- KOLEJKA 17 (@RoundId17) - Nierozegrane (NULL)
        (NEWID(), @RoundId17, @TeamId1, @TeamId6, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId17, @TeamId4, @TeamId12, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId17, @TeamId10, @TeamId19, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId17, @TeamId9, @TeamId21, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId17, @TeamId17, @TeamId11, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId17, @TeamId8, @TeamId14, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId17, @TeamId16, @TeamId3, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId17, @TeamId7, @TeamId2, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId17, @TeamId25, @TeamId5, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId17, @TeamId23, @TeamId15, NULL, NULL, NULL, 0),

        -- KOLEJKA 18 (@RoundId18) - Nierozegrane (NULL)
        (NEWID(), @RoundId18, @TeamId15, @TeamId14, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId18, @TeamId11, @TeamId4, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId18, @TeamId10, @TeamId9, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId18, @TeamId23, @TeamId7, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId18, @TeamId21, @TeamId2, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId18, @TeamId6, @TeamId25, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId18, @TeamId1, @TeamId8, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId18, @TeamId12, @TeamId16, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId18, @TeamId19, @TeamId17, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId18, @TeamId5, @TeamId3, NULL, NULL, NULL, 0),

        -- KOLEJKA 19 (@RoundId19) - Nierozegrane (NULL)
        (NEWID(), @RoundId19, @TeamId2, @TeamId3, 3, 1, 0, 1),     -- FC Barcelona 3-1 Atlético Madryt (PLAYED)
        (NEWID(), @RoundId19, @TeamId9, @TeamId1, 0, 3, 0, 1),     -- Athletic Bilbao 0-3 Real Madryt (PLAYED)
        (NEWID(), @RoundId19, @TeamId14, @TeamId5, NULL, NULL, NULL, 0), -- Getafe CF vs Real Sociedad (FUTURE)
        (NEWID(), @RoundId19, @TeamId6, @TeamId11, NULL, NULL, NULL, 0), -- Sevilla vs Celta de Vigo (FUTURE)
        (NEWID(), @RoundId19, @TeamId4, @TeamId23, NULL, NULL, NULL, 0), -- Valencia vs Elche CF (FUTURE)
        (NEWID(), @RoundId19, @TeamId16, @TeamId10, NULL, NULL, NULL, 0), -- Girona FC vs Osasuna (FUTURE)
        (NEWID(), @RoundId19, @TeamId15, @TeamId12, NULL, NULL, NULL, 0), -- Rayo Vallecano vs Mallorca (FUTURE)
        (NEWID(), @RoundId19, @TeamId17, @TeamId8, NULL, NULL, NULL, 0),  -- R. Oviedo vs Betis Sevilla (FUTURE)
        (NEWID(), @RoundId19, @TeamId25, @TeamId21, NULL, NULL, NULL, 0), -- Levante vs Espanyol Barcelona (FUTURE)
        (NEWID(), @RoundId19, @TeamId7, @TeamId19, NULL, NULL, NULL, 0),  -- Villarreal vs Alaves (FUTURE)

        -- KOLEJKA 20 (@RoundId20) - Nierozegrane (NULL)
        (NEWID(), @RoundId20, @TeamId1, @TeamId25, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId20, @TeamId11, @TeamId15, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId20, @TeamId8, @TeamId7, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId20, @TeamId21, @TeamId16, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId20, @TeamId5, @TeamId2, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId20, @TeamId10, @TeamId17, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId20, @TeamId23, @TeamId6, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId20, @TeamId3, @TeamId19, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId20, @TeamId14, @TeamId4, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId20, @TeamId12, @TeamId9, NULL, NULL, NULL, 0),

        -- KOLEJKA 21 (@RoundId21) - Nierozegrane (NULL)
        (NEWID(), @RoundId21, @TeamId3, @TeamId12, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId21, @TeamId4, @TeamId21, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId21, @TeamId7, @TeamId1, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId21, @TeamId16, @TeamId14, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId21, @TeamId25, @TeamId23, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId21, @TeamId19, @TeamId8, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId21, @TeamId6, @TeamId9, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId21, @TeamId15, @TeamId10, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId21, @TeamId2, @TeamId17, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId21, @TeamId5, @TeamId11, NULL, NULL, NULL, 0),

        -- KOLEJKA 22 (@RoundId22) - Nierozegrane (NULL)
        (NEWID(), @RoundId22, @TeamId8, @TeamId4, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId22, @TeamId23, @TeamId2, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId22, @TeamId1, @TeamId15, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId22, @TeamId25, @TeamId3, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId22, @TeamId12, @TeamId6, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId22, @TeamId14, @TeamId11, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId22, @TeamId9, @TeamId5, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId22, @TeamId17, @TeamId16, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId22, @TeamId10, @TeamId7, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId22, @TeamId21, @TeamId19, NULL, NULL, NULL, 0),

        -- KOLEJKA 23 (@RoundId23) - Nierozegrane (NULL)
        (NEWID(), @RoundId23, @TeamId19, @TeamId14, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId23, @TeamId15, @TeamId17, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId23, @TeamId3, @TeamId8, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId23, @TeamId2, @TeamId12, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId23, @TeamId9, @TeamId25, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId23, @TeamId7, @TeamId21, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId23, @TeamId5, @TeamId23, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId23, @TeamId6, @TeamId16, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId23, @TeamId11, @TeamId10, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId23, @TeamId4, @TeamId1, NULL, NULL, NULL, 0),

        -- KOLEJKA 24 (@RoundId24) - Nierozegrane (NULL)
        (NEWID(), @RoundId24, @TeamId23, @TeamId10, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId24, @TeamId14, @TeamId7, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId24, @TeamId17, @TeamId9, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId24, @TeamId16, @TeamId2, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId24, @TeamId21, @TeamId11, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId24, @TeamId1, @TeamId5, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId24, @TeamId25, @TeamId4, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId24, @TeamId15, @TeamId3, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId24, @TeamId12, @TeamId8, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId24, @TeamId6, @TeamId19, NULL, NULL, NULL, 0),

        -- KOLEJKA 25 (@RoundId25) - Nierozegrane (NULL)
        (NEWID(), @RoundId25, @TeamId10, @TeamId1, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId25, @TeamId3, @TeamId21, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId25, @TeamId9, @TeamId23, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId25, @TeamId7, @TeamId4, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId25, @TeamId14, @TeamId6, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId25, @TeamId11, @TeamId12, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId25, @TeamId5, @TeamId17, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId25, @TeamId19, @TeamId16, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId25, @TeamId2, @TeamId25, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId25, @TeamId8, @TeamId15, NULL, NULL, NULL, 0),

        -- KOLEJKA 26 (@RoundId26) - Nierozegrane (NULL)
        (NEWID(), @RoundId26, @TeamId15, @TeamId9, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId26, @TeamId2, @TeamId7, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId26, @TeamId12, @TeamId5, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId26, @TeamId23, @TeamId21, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId26, @TeamId16, @TeamId11, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId26, @TeamId25, @TeamId19, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId26, @TeamId4, @TeamId10, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId26, @TeamId1, @TeamId14, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId26, @TeamId8, @TeamId6, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId26, @TeamId17, @TeamId3, NULL, NULL, NULL, 0),

        -- KOLEJKA 27 (@RoundId27) - Nierozegrane (NULL)
        (NEWID(), @RoundId27, @TeamId10, @TeamId12, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId27, @TeamId3, @TeamId5, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId27, @TeamId4, @TeamId19, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId27, @TeamId25, @TeamId16, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId27, @TeamId14, @TeamId8, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId27, @TeamId21, @TeamId17, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId27, @TeamId11, @TeamId1, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId27, @TeamId6, @TeamId15, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId27, @TeamId7, @TeamId23, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId27, @TeamId9, @TeamId2, NULL, NULL, NULL, 0),

        -- KOLEJKA 28 (@RoundId28) - Nierozegrane (NULL)
        (NEWID(), @RoundId28, @TeamId1, @TeamId23, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId28, @TeamId8, @TeamId11, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId28, @TeamId17, @TeamId4, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId28, @TeamId5, @TeamId10, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId28, @TeamId19, @TeamId7, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId28, @TeamId16, @TeamId9, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId28, @TeamId12, @TeamId21, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId28, @TeamId3, @TeamId14, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId28, @TeamId2, @TeamId6, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId28, @TeamId15, @TeamId25, NULL, NULL, NULL, 0),

        -- KOLEJKA 29 (@RoundId29) - Nierozegrane (NULL)
        (NEWID(), @RoundId29, @TeamId6, @TeamId4, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId29, @TeamId25, @TeamId17, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId29, @TeamId2, @TeamId15, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId29, @TeamId11, @TeamId19, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId29, @TeamId1, @TeamId3, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId29, @TeamId10, @TeamId16, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId29, @TeamId7, @TeamId5, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId29, @TeamId23, @TeamId12, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId29, @TeamId9, @TeamId8, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId29, @TeamId21, @TeamId14, NULL, NULL, NULL, 0),


        -- KOLEJKA 30 (@RoundId30) - Nierozegrane (NULL)
        (NEWID(), @RoundId30, @TeamId4, @TeamId11, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId30, @TeamId8, @TeamId21, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId30, @TeamId15, @TeamId23, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId30, @TeamId12, @TeamId1, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId30, @TeamId5, @TeamId25, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId30, @TeamId16, @TeamId7, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId30, @TeamId19, @TeamId10, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId30, @TeamId17, @TeamId6, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId30, @TeamId14, @TeamId9, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId30, @TeamId3, @TeamId2, NULL, NULL, NULL, 0),
            
            -- KOLEJKA 31 (@RoundId31) - Nierozegrane (NULL)
        (NEWID(), @RoundId31, @TeamId6, @TeamId3, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId31, @TeamId1, @TeamId16, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId31, @TeamId2, @TeamId21, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId31, @TeamId25, @TeamId14, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId31, @TeamId11, @TeamId17, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId31, @TeamId12, @TeamId15, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId31, @TeamId5, @TeamId19, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId31, @TeamId10, @TeamId8, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId31, @TeamId23, @TeamId4, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId31, @TeamId9, @TeamId7, NULL, NULL, NULL, 0),

        -- KOLEJKA 32 (@RoundId32) - Nierozegrane (NULL)
        (NEWID(), @RoundId32, @TeamId21, @TeamId25, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId32, @TeamId10, @TeamId6, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId32, @TeamId4, @TeamId16, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId32, @TeamId8, @TeamId1, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId32, @TeamId14, @TeamId2, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId32, @TeamId17, @TeamId23, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId32, @TeamId19, @TeamId12, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId32, @TeamId15, @TeamId5, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId32, @TeamId7, @TeamId11, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId32, @TeamId3, @TeamId9, NULL, NULL, NULL, 0),

        -- KOLEJKA 33 (@RoundId33) - Nierozegrane (NULL)
        (NEWID(), @RoundId33, @TeamId1, @TeamId19, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId33, @TeamId25, @TeamId6, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId33, @TeamId2, @TeamId11, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId33, @TeamId17, @TeamId7, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId33, @TeamId5, @TeamId14, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId33, @TeamId9, @TeamId10, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId33, @TeamId16, @TeamId8, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId33, @TeamId15, @TeamId21, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId33, @TeamId12, @TeamId4, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId33, @TeamId23, @TeamId3, NULL, NULL, NULL, 0),

        -- KOLEJKA 34 (@RoundId34) - Nierozegrane (NULL)
        (NEWID(), @RoundId34, @TeamId21, @TeamId1, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId34, @TeamId6, @TeamId5, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId34, @TeamId7, @TeamId25, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId34, @TeamId14, @TeamId15, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId34, @TeamId8, @TeamId17, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId34, @TeamId4, @TeamId3, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId34, @TeamId16, @TeamId12, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId34, @TeamId10, @TeamId2, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId34, @TeamId19, @TeamId9, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId34, @TeamId11, @TeamId23, NULL, NULL, NULL, 0),

        -- KOLEJKA 35 (@RoundId35) - Nierozegrane (NULL)
        (NEWID(), @RoundId35, @TeamId3, @TeamId11, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId35, @TeamId6, @TeamId21, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId35, @TeamId2, @TeamId1, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId35, @TeamId15, @TeamId16, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId35, @TeamId23, @TeamId19, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId35, @TeamId12, @TeamId7, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId35, @TeamId25, @TeamId10, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId35, @TeamId5, @TeamId8, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId35, @TeamId9, @TeamId4, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId35, @TeamId17, @TeamId14, NULL, NULL, NULL, 0),

        -- KOLEJKA 36 (@RoundId36) - Nierozegrane (NULL)
        (NEWID(), @RoundId36, @TeamId21, @TeamId9, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId36, @TeamId7, @TeamId6, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId36, @TeamId10, @TeamId3, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId36, @TeamId1, @TeamId17, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId36, @TeamId19, @TeamId2, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId36, @TeamId4, @TeamId15, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId36, @TeamId14, @TeamId12, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId36, @TeamId16, @TeamId5, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId36, @TeamId11, @TeamId25, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId36, @TeamId8, @TeamId23, NULL, NULL, NULL, 0),

        -- KOLEJKA 37 (@RoundId37) - Nierozegrane (NULL)
        (NEWID(), @RoundId37, @TeamId17, @TeamId19, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId37, @TeamId25, @TeamId12, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId37, @TeamId3, @TeamId16, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId37, @TeamId15, @TeamId7, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId37, @TeamId5, @TeamId4, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId37, @TeamId2, @TeamId8, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId37, @TeamId6, @TeamId1, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId37, @TeamId9, @TeamId11, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId37, @TeamId10, @TeamId21, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId37, @TeamId23, @TeamId14, NULL, NULL, NULL, 0),

        -- KOLEJKA 38 (@RoundId38) - Nierozegrane (NULL) - Ostatnia runda
        (NEWID(), @RoundId38, @TeamId8, @TeamId25, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId38, @TeamId14, @TeamId10, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId38, @TeamId1, @TeamId9, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId38, @TeamId7, @TeamId3, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId38, @TeamId11, @TeamId6, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId38, @TeamId16, @TeamId23, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId38, @TeamId4, @TeamId2, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId38, @TeamId19, @TeamId15, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId38, @TeamId21, @TeamId5, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId38, @TeamId12, @TeamId17, NULL, NULL, NULL, 0);
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
