
-- ================================================================
-- LA LIGA 2022/2023 - 2025/2026 COMPLETE DATA SEED
-- Created: 07.12.2025
-- ================================================================

USE SportsDataDb;
GO

DECLARE 
    @CountryId UNIQUEIDENTIFIER, 
    @LeagueId UNIQUEIDENTIFIER,
    @CurrentDateTime DATETIME2 = GETDATE(),

    -- STADIUMS (2025/2026 current)
    @StadiumId1 UNIQUEIDENTIFIER = NEWID(),    -- Real Madrid
    @StadiumId2 UNIQUEIDENTIFIER = NEWID(),    -- FC Barcelona
    @StadiumId3 UNIQUEIDENTIFIER = NEWID(),    -- Atlético Madrid
    @StadiumId4 UNIQUEIDENTIFIER = NEWID(),    -- Valencia CF
    @StadiumId5 UNIQUEIDENTIFIER = NEWID(),    -- Real Sociedad
    @StadiumId6 UNIQUEIDENTIFIER = NEWID(),    -- Sevilla FC
    @StadiumId7 UNIQUEIDENTIFIER = NEWID(),    -- Villarreal CF
    @StadiumId8 UNIQUEIDENTIFIER = NEWID(),    -- Real Betis
    @StadiumId9 UNIQUEIDENTIFIER = NEWID(),    -- Athletic Bilbao
    @StadiumId10 UNIQUEIDENTIFIER = NEWID(),   -- CA Osasuna
    @StadiumId11 UNIQUEIDENTIFIER = NEWID(),   -- RC Celta de Vigo
    @StadiumId12 UNIQUEIDENTIFIER = NEWID(),   -- RCD Mallorca
    @StadiumId13 UNIQUEIDENTIFIER = NEWID(),   -- Real Valladolid
    @StadiumId14 UNIQUEIDENTIFIER = NEWID(),   -- Getafe CF
    @StadiumId15 UNIQUEIDENTIFIER = NEWID(),   -- Rayo Vallecano
    @StadiumId16 UNIQUEIDENTIFIER = NEWID(),   -- Girona FC
    @StadiumId17 UNIQUEIDENTIFIER = NEWID(),   -- Real Oviedo
    @StadiumId18 UNIQUEIDENTIFIER = NEWID(),   -- UD Las Palmas
    @StadiumId19 UNIQUEIDENTIFIER = NEWID(),   -- Deportivo Alavés
    @StadiumId20 UNIQUEIDENTIFIER = NEWID(),   -- Cádiz CF
    @StadiumId21 UNIQUEIDENTIFIER = NEWID(),   -- RCD Espanyol (historical)
    @StadiumId22 UNIQUEIDENTIFIER = NEWID(),   -- Real Sociedad II (historical)
    @StadiumId23 UNIQUEIDENTIFIER = NEWID(),   -- Elche CF (historical)
    @StadiumId24 UNIQUEIDENTIFIER = NEWID(),   -- Real Madrid Castilla (historical)
    @StadiumId25 UNIQUEIDENTIFIER = NEWID(),   -- Levante
    @StadiumId26 UNIQUEIDENTIFIER = NEWID(),   -- 
    @StadiumId27 UNIQUEIDENTIFIER = NEWID(),   -- 
    @StadiumId28 UNIQUEIDENTIFIER = NEWID(),   -- 

    -- TEAMS (fixed IDs for consistency)
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
    @TeamId28 UNIQUEIDENTIFIER = '00f250fc-6d15-4ab5-a26d-2db1a19879ef';  -- Granada

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
    -- INSERT STADIUMS
    -- ================================================================
    
    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.Stadium WHERE [Name] LIKE '%Santiago Bernabéu%')
    INSERT INTO dbo.Stadium (Id, [Name], Capacity)
    VALUES
        (@StadiumId1, 'Santiago Bernabéu', 81044),
        (@StadiumId2, 'Camp Nou', 99354),
        (@StadiumId3, 'Metropolitano', 68456),
        (@StadiumId4, 'Mestalla', 55000),
        (@StadiumId5, 'Anoeta', 39500),
        (@StadiumId6, 'Ramón Sánchez Pizjuán', 43883),
        (@StadiumId7, 'La Cerámica', 23500),
        (@StadiumId8, 'Estadio Benito Villamarín', 52887),
        (@StadiumId9, 'San Mamés', 53331),
        (@StadiumId10, 'Estadio El Sadar', 23576),
        (@StadiumId11, 'Estadio de Balaídos', 25900),
        (@StadiumId12, 'Estadio de Son Moix', 23500),
        (@StadiumId13, 'Estadio José Zorrilla', 30662),
        (@StadiumId14, 'Coliseum Alfonso Pérez', 17700),
        (@StadiumId15, 'Estadio de Vallecas', 14505),
        (@StadiumId16, 'Estadi de Montilivi', 17742),
        (@StadiumId17, 'Estadio Carlos Tartiere', 30500),
        (@StadiumId18, 'Estadio de Gran Canaria', 32392),
        (@StadiumId19, 'Estadio de Mendizorrotza', 19260),
        (@StadiumId20, 'Estadio Nuevo de Cádiz', 22000),
        (@StadiumId21, 'RCDE Stadium', 40500),
        (@StadiumId22, 'Anoeta (Anexo)', 6000),
        (@StadiumId23, 'Estadio Manuel Martínez Valero', 33000),
        (@StadiumId24, 'Estadio Alfredo Di Stéfano', 6000),
        (@StadiumId25, 'Estadio Ciudad de Valencia', 25354),
        (@StadiumId26, 'Estadio Municipal de Butarque', 12450),
        (@StadiumId27, 'Power Horse Stadium', 15274),
        (@StadiumId28, 'Nuevo Los Cármenes', 19336);



    -- ================================================================
    -- INSERT TEAMS
    -- ================================================================
    
    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.Team WHERE [Name] LIKE '%Real Madrid%' AND CountryId = @CountryId)
    INSERT INTO dbo.Team (Id, [Name], CountryId, StadiumId, ShortName)
    VALUES
        (@TeamId1, 'Real Madrid CF', @CountryId, @StadiumId1, 'RMA'),
        (@TeamId2, 'FC Barcelona', @CountryId, @StadiumId2, 'FCB'),
        (@TeamId3, 'Atlético Madrid', @CountryId, @StadiumId3, 'ATM'),
        (@TeamId4, 'Valencia CF', @CountryId, @StadiumId4, 'VAL'),
        (@TeamId5, 'Real Sociedad', @CountryId, @StadiumId5, 'RSO'),
        (@TeamId6, 'Sevilla FC', @CountryId, @StadiumId6, 'SEV'),
        (@TeamId7, 'Villarreal CF', @CountryId, @StadiumId7, 'VIL'),
        (@TeamId8, 'Real Betis', @CountryId, @StadiumId8, 'BET'),
        (@TeamId9, 'Athletic Club Bilbao', @CountryId, @StadiumId9, 'ATH'),
        (@TeamId10, 'Osasuna Pampeluna', @CountryId, @StadiumId10, 'OSA'),
        (@TeamId11, 'RC Celta de Vigo', @CountryId, @StadiumId11, 'CEL'),
        (@TeamId12, 'RCD Mallorca', @CountryId, @StadiumId12, 'MAL'),
        (@TeamId13, 'Real Valladolid CF', @CountryId, @StadiumId13, 'VAD'),
        (@TeamId14, 'Getafe CF', @CountryId, @StadiumId14, 'GET'),
        (@TeamId15, 'Rayo Vallecano', @CountryId, @StadiumId15, 'RAY'),
        (@TeamId16, 'Girona FC', @CountryId, @StadiumId16, 'GIR'),
        (@TeamId17, 'Real Oviedo', @CountryId, @StadiumId17, 'OVI'),
        (@TeamId18, 'UD Las Palmas', @CountryId, @StadiumId18, 'LPA'),
        (@TeamId19, 'Deportivo Alavés', @CountryId, @StadiumId19, 'ALA'),
        (@TeamId20, 'Cádiz CF', @CountryId, @StadiumId20, 'CAD'),
        (@TeamId21, 'RCD Espanyol', @CountryId, @StadiumId21, 'RCD'),
        (@TeamId22, 'Real Sociedad II', @CountryId, @StadiumId22, 'RS2'),
        (@TeamId23, 'Elche CF', @CountryId, @StadiumId23, 'ELC'),
        (@TeamId24, 'Real Madrid Castilla', @CountryId, @StadiumId24, 'RMC'),
        (@TeamId25, 'Levante UD', @CountryId, @StadiumId25, 'LEV'),
        (@TeamId26, 'CD Leganés', @CountryId, @StadiumId26, 'LEG'),
        (@TeamId27, 'UD Almería', @CountryId, @StadiumId27, 'ALM'),
        (@TeamId28, 'Granada CF', @CountryId, @StadiumId28, 'GRA');


    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.CompetitionMembership WHERE LeagueId = @LeagueId AND SeasonYear = '2025/2026')
    INSERT INTO dbo.CompetitionMembership (Id, TeamId, LeagueId, SeasonYear)
    VALUES
        -- 2025/2026 Season (20 teams)
        (NEWID(), @TeamId2, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId1, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId7, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId3, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId8, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId21, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId9, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId14, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId19, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId15, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId23, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId5, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId11, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId6, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId12, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId4, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId10, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId16, @LeagueId, '2025/2026'),--espanyol
        (NEWID(), @TeamId17, @LeagueId, '2025/2026'),
        (NEWID(), @TeamId25, @LeagueId, '2025/2026'),--elche

        -- 2024/2025 Season (20 teams)
        (NEWID(), @TeamId2, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId1, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId3, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId9, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId7, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId8, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId11, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId15, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId10, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId12, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId5, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId4, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId14, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId21, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId19, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId16, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId6, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId26, @LeagueId, '2024/2025'), -- leganes
        (NEWID(), @TeamId18, @LeagueId, '2024/2025'),
        (NEWID(), @TeamId13, @LeagueId, '2024/2025'),

        -- 2023/2024 Season (20 teams)
        (NEWID(), @TeamId1, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId2, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId16, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId3, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId9, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId5, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId8, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId7, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId4, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId19, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId10, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId14, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId11, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId6, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId12, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId18, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId15, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId20, @LeagueId, '2023/2024'),
        (NEWID(), @TeamId27, @LeagueId, '2023/2024'),--almeria
        (NEWID(), @TeamId28, @LeagueId, '2023/2024'),--granada

        -- 2022/2023 Season (20 teams)
        (NEWID(), @TeamId2, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId1, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId3, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId5, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId7, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId8, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId10, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId9, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId12, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId16, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId15, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId6, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId11, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId20, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId14, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId4, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId27, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId13, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId21, @LeagueId, '2022/2023'),
        (NEWID(), @TeamId23, @LeagueId, '2022/2023')

    COMMIT TRANSACTION
    PRINT 'La Liga stadiums and teams for 2022/2023 - 2025/2026 seasons inserted successfully.'
    
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT 'Error occurred while inserting La Liga stadiums and teams.'
    PRINT ERROR_MESSAGE()
END CATCH

GO
