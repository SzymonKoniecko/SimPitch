USE SportsDataDb

DECLARE 
    @CountryId UNIQUEIDENTIFIER, 
    @LeagueId UNIQUEIDENTIFIER,
    @CurrentDateTime DATETIME2 = GETDATE(),

    @StadiumId1 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId2 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId3 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId4 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId5 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId6 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId7 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId8 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId9 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId10 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId11 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId12 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId13 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId14 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId15 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId16 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId17 UNIQUEIDENTIFIER = NEWID(),
    @StadiumId18 UNIQUEIDENTIFIER = NEWID(),
    
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
    @TeamId18 UNIQUEIDENTIFIER = 'e1b9c3d7-6f0a-4d2e-9b5c-3a7f8e1d0b6c'   -- Arka


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
    INSERT INTO dbo.Stadium (Id, [Name], Capacity, CreatedAt, UpdatedAt)
    VALUES
        (@StadiumId1, 'Municipal Stadium in Białystok', 22372, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId2, 'Marshal Józef Piłsudski Municipal Stadium of Legia Warsaw', 31103, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId3, 'Stadion Miejski Poznań (Municipal Stadium in Poznań)', 42717, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId4, 'Serce Łodzi - Stadion Widzewa Łódź', 18018, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId5, 'Raków Stadium in Częstochowa', 5000, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId6, 'Stadion Miejski im. Floriana Krygiera (Municipal Stadium in Szczecin)', 20915, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId7, 'Stadion Cracovii im. Józefa Piłsudskiego (Cracovia Stadium)', 15000, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId8, 'Arena Zabrze', 29444, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId9, 'Stadion Wisły Płock im. Kazimierza Górskiego', 15004, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId10, 'Stadion Energa Gdańsk (Energa Stadium)', 41715, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId11, 'Radom City Stadium', 15000, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId12, 'Motor Lublin Arena', 15247, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId13, 'Stadion Miejski w Katowicach', 15048, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId14, 'Lubin City Stadium', 16500, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId15, 'Korona Kielce Municipal Stadium', 15000, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId16, 'Stadion Miejski im. Piotra Wieczorka w Gliwicach', 9736, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId17, 'Stadion Sportowy BRUK-BET Termalica Nieciecza', 2262, @CurrentDateTime, @CurrentDateTime),
        (@StadiumId18, 'Stadion Miejski w Gdyni', 15139, @CurrentDateTime, @CurrentDateTime);

    INSERT INTO dbo.Team (Id, [Name], CountryId, StadiumId, LeagueId, LogoUrl, ShortName)
    VALUES
        (@TeamId1, 'Jagiellonia Białystok', @CountryId, @StadiumId1, @LeagueId, '/wikipedia/commons/thumb/e/e8/Jagiellonia_Białystok_Logo_1.png/250px-Jagiellonia_Białystok_Logo_1.png', 'JAG'),
        (@TeamId2, 'Legia Warsaw', @CountryId, @StadiumId2, @LeagueId, '/wikipedia/commons/thumb/f/fa/Legia_Warszawa.png/250px-Legia_Warszawa.png', 'LEG'),
        (@TeamId3, 'Lech Poznań', @CountryId, @StadiumId3, @LeagueId, '/wikipedia/commons/thumb/f/ff/Lech_Poznań_logo.png/250px-Lech_Poznań_logo.png', 'LEC'),
        (@TeamId4, 'Widzew Łódź', @CountryId, @StadiumId4, @LeagueId, '/wikipedia/commons/thumb/f/fd/Herb_Widzew_Łódź.png/250px-Herb_Widzew_Łódź.png', 'WID'),
        (@TeamId5, 'Raków Częstochowa', @CountryId, @StadiumId5, @LeagueId, '/wikipedia/commons/thumb/a/af/Rks_rakow_crest_ai.svg/250px-Rks_rakow_crest_ai.svg.png', 'RAK'),
        (@TeamId6, 'Pogoń Szczecin', @CountryId, @StadiumId6, @LeagueId, 'http://example.com/pogon.png', 'POG'),
        (@TeamId7, 'Cracovia', @CountryId, @StadiumId7, @LeagueId, 'arka.png', 'CRA'),
        (@TeamId8, 'Górnik Zabrze', @CountryId, @StadiumId8, @LeagueId, '/wikipedia/commons/thumb/d/d9/Gornik_Zabrze.png/250px-Gornik_Zabrze.png', 'GOR'),
        (@TeamId9, 'Wisła Płock', @CountryId, @StadiumId9, @LeagueId, '/wikipedia/commons/0/0e/Wisla_P%C5%82ock.png', 'WPŁ'),
        (@TeamId10, 'Lechia Gdańsk', @CountryId, @StadiumId10, @LeagueId, '/wikipedia/commons/thumb/2/2b/LechiaGdanskBadge2001.jpg/250px-LechiaGdanskBadge2001.jpg', 'LEH'),
        (@TeamId11, 'Radomiak Radom', @CountryId, @StadiumId11, @LeagueId, '/wikipedia/commons/thumb/2/2b/Herb_radomiaka_300dpi.png/250px-Herb_radomiaka_300dpi.png', 'RAD'),
        (@TeamId12, 'Motor Lublin', @CountryId, @StadiumId12, @LeagueId, '/wikipedia/commons/thumb/3/3d/Motor_Lublin_S.A._Oficjalny_Herb.png/250px-Motor_Lublin_S.A._Oficjalny_Herb.png', 'MOT'),
        (@TeamId13, 'GKS Katowice', @CountryId, @StadiumId13, @LeagueId, 'http://example.com/gks.png', 'GKS'),
        (@TeamId14, 'Zagłębie Lubin', @CountryId, @StadiumId14, @LeagueId, '/wikipedia/commons/thumb/e/e5/Staion_Zaglebie_Lubin.jpg/250px-Staion_Zaglebie_Lubin.jpg', 'ZAG'),
        (@TeamId15, 'Korona Kielce', @CountryId, @StadiumId15, @LeagueId, '/wikipedia/commons/thumb/2/2b/Stadion_Kielce_przed_meczem_Polska_-_Armenia.jpg/250px-Stadion_Kielce_przed_meczem_Polska_-_Armenia.jpg', 'KOR'),
        (@TeamId16, 'Piast Gliwice', @CountryId, @StadiumId16, @LeagueId, '/wikipedia/commons/thumb/f/f5/Stadion_Piasta_Gliwice_05.JPG/250px-Stadion_Piasta_Gliwice_05.JPG', 'PIA'),
        (@TeamId17, 'Bruk-Bet Termalica Nieciecza', @CountryId, @StadiumId17, @LeagueId, '/wikipedia/commons/thumb/8/84/KS_Nieciecza_herb.jpg/250px-KS_Nieciecza_herb.jpg', 'BBT'),
        (@TeamId18, 'Arka Gdynia', @CountryId, @StadiumId18, @LeagueId, 'http://example.com/arka.png', 'ARK');

    COMMIT TRANSACTION
    PRINT 'Ekstraklasa stadiums and teams for 2025/2026 season inserted successfully.'
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT 'Error occurred while inserting ekstraklasa stadiums and teams.'
END CATCH