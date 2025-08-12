USE SportsDataDb;

DECLARE 
    @CountryId UNIQUEIDENTIFIER, 
    @LeagueId UNIQUEIDENTIFIER,
    @CurrentDateTime DATETIME2 = GETDATE(),

    @StadiumId1 UNIQUEIDENTIFIER = NEWID(), -- Jagiellonia
    @StadiumId2 UNIQUEIDENTIFIER = NEWID(), -- Legia
    @StadiumId3 UNIQUEIDENTIFIER = NEWID(), -- Lech Poznań
    @StadiumId4 UNIQUEIDENTIFIER = NEWID(), -- Widzew Łódź
    @StadiumId5 UNIQUEIDENTIFIER = NEWID(), -- Raków Częstochowa
    @StadiumId6 UNIQUEIDENTIFIER = NEWID(), -- Pogoń Szczecin
    @StadiumId7 UNIQUEIDENTIFIER = NEWID(), -- Cracovia
    @StadiumId8 UNIQUEIDENTIFIER = NEWID(), -- Górnik Zabrze
    @StadiumId9 UNIQUEIDENTIFIER = NEWID(), -- Wisla Plock
    @StadiumId10 UNIQUEIDENTIFIER = NEWID(), -- Lechia Gdańsk
    @StadiumId11 UNIQUEIDENTIFIER = NEWID(), -- Radomiak Radom
    @StadiumId12 UNIQUEIDENTIFIER = NEWID(), -- Motor Lublin
    @StadiumId13 UNIQUEIDENTIFIER = NEWID(), -- Warta Poznań
    @StadiumId14 UNIQUEIDENTIFIER = NEWID(), -- Zagłębie Lubin
    @StadiumId15 UNIQUEIDENTIFIER = NEWID(), -- Korona Kielce
    @StadiumId16 UNIQUEIDENTIFIER = NEWID(), -- Miedź Legnica
    @StadiumId17 UNIQUEIDENTIFIER = NEWID(), -- Termalika
    @StadiumId18 UNIQUEIDENTIFIER = NEWID(), -- Arka
     
    @TeamId1 UNIQUEIDENTIFIER = NEWID(),    -- Jaga
    @TeamId2 UNIQUEIDENTIFIER = NEWID(),    -- Legia
    @TeamId3 UNIQUEIDENTIFIER = NEWID(),    -- Lech
    @TeamId4 UNIQUEIDENTIFIER = NEWID(),    -- Widzew Łódź
    @TeamId5 UNIQUEIDENTIFIER = NEWID(),    -- Rakow
    @TeamId6 UNIQUEIDENTIFIER = NEWID(),    -- Pogon
    @TeamId7 UNIQUEIDENTIFIER = NEWID(),    -- Cracovia
    @TeamId8 UNIQUEIDENTIFIER = NEWID(),    -- Górnik
    @TeamId9 UNIQUEIDENTIFIER = NEWID(),    -- Wisla Plock
    @TeamId10 UNIQUEIDENTIFIER = NEWID(),   -- Lechia
    @TeamId11 UNIQUEIDENTIFIER = NEWID(),   -- Radomiak
    @TeamId12 UNIQUEIDENTIFIER = NEWID(),   -- Motor Lublin
    @TeamId13 UNIQUEIDENTIFIER = NEWID(),   -- GKS
    @TeamId14 UNIQUEIDENTIFIER = NEWID(),   -- Zaglebie Lubin
    @TeamId15 UNIQUEIDENTIFIER = NEWID(),   -- Korona
    @TeamId16 UNIQUEIDENTIFIER = NEWID(),   -- Piast
    @TeamId17 UNIQUEIDENTIFIER = NEWID(),   -- Termalika
    @TeamId18 UNIQUEIDENTIFIER = NEWID()    -- Arka

SELECT 
    @CountryId = Id
FROM dbo.Country
WHERE [Code] = 'PL';

SELECT
    @LeagueId = Id
FROM dbo.League
WHERE [Name] = 'PKO BP Ekstraklasa' AND CountryId = @CountryId;

BEGIN TRANSACTION;

BEGIN TRY
    -- Wstaw stadiony ekstraklasy (nazwy przetłumaczone na angielski)
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
        (@StadiumId18, 'Stadion Miejski w Gdyni', 15 139, @CurrentDateTime, @CurrentDateTime)

    -- Wstaw drużyny ekstraklasy na sezon 2025/2026
    INSERT INTO dbo.Team (Id, [Name], CountryId, StadiumId, LeagueId, LogoUrl, ShortName, Sport, CreatedAt, UpdatedAt)
    VALUES
        (@TeamId1, 'Jagiellonia Białystok', @CountryId, @StadiumId1, @LeagueId, '/wikipedia/commons/thumb/e/e8/Jagiellonia_Białystok_Logo_1.png/250px-Jagiellonia_Białystok_Logo_1.png', 'JAG', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId2, 'Legia Warsaw', @CountryId, @StadiumId2, @LeagueId, '/wikipedia/commons/thumb/f/fa/Legia_Warszawa.png/250px-Legia_Warszawa.png', 'LEG', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId3, 'Lech Poznań', @CountryId, @StadiumId3, @LeagueId, '/wikipedia/commons/thumb/f/ff/Lech_Poznań_logo.png/250px-Lech_Poznań_logo.png', 'LEC', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId4, 'Widzew Łódź', @CountryId, @StadiumId4, @LeagueId, '/wikipedia/commons/thumb/f/fd/Herb_Widzew_Łódź.png/250px-Herb_Widzew_Łódź.png', 'WID', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId5, 'Raków Częstochowa', @CountryId, @StadiumId5, @LeagueId, '/wikipedia/commons/thumb/a/af/Rks_rakow_crest_ai.svg/250px-Rks_rakow_crest_ai.svg.png', 'RAK', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId6, 'Pogoń Szczecin', @CountryId, @StadiumId6, @LeagueId, 'http://example.com/pogon.png', 'POG', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId7, 'Cracovia', @CountryId, @StadiumId7, @LeagueId, 'arka.png', 'CRA', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId8, 'Górnik Zabrze', @CountryId, @StadiumId8, @LeagueId, '/wikipedia/commons/thumb/d/d9/Gornik_Zabrze.png/250px-Gornik_Zabrze.png', 'GOR', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId9, 'Wisła Płock', @CountryId, @StadiumId9, @LeagueId, '/wikipedia/commons/0/0e/Wisla_P%C5%82ock.png', 'WPŁ', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId10, 'Lechia Gdańsk', @CountryId, @StadiumId10, @LeagueId, '/wikipedia/commons/thumb/2/2b/LechiaGdanskBadge2001.jpg/250px-LechiaGdanskBadge2001.jpg', 'LEH', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId11, 'Radomiak Radom', @CountryId, @StadiumId11, @LeagueId, '/wikipedia/commons/thumb/2/2b/Herb_radomiaka_300dpi.png/250px-Herb_radomiaka_300dpi.png', 'RAD', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId12, 'Motor Lublin', @CountryId, @StadiumId12, @LeagueId, '/wikipedia/commons/thumb/3/3d/Motor_Lublin_S.A._Oficjalny_Herb.png/250px-Motor_Lublin_S.A._Oficjalny_Herb.png', 'MOT', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId13, 'GKS Katowice', @CountryId, @StadiumId13, @LeagueId, 'http://example.com/gks.png', 'GKS', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId14, 'Zagłębie Lubin', @CountryId, @StadiumId14, @LeagueId, '/wikipedia/commons/thumb/e/e5/Staion_Zaglebie_Lubin.jpg/250px-Staion_Zaglebie_Lubin.jpg', 'ZAG', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId15, 'Korona Kielce', @CountryId, @StadiumId15, @LeagueId, '/wikipedia/commons/thumb/2/2b/Stadion_Kielce_przed_meczem_Polska_-_Armenia.jpg/250px-Stadion_Kielce_przed_meczem_Polska_-_Armenia.jpg', 'KOR', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId16, 'Piast Gliwice', @CountryId, @StadiumId16, @LeagueId, '/wikipedia/commons/thumb/f/f5/Stadion_Piasta_Gliwice_05.JPG/250px-Stadion_Piasta_Gliwice_05.JPG', 'PIA', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId17, 'Bruk-Bet Termalica Nieciecza', @CountryId, @StadiumId17, @LeagueId, '/wikipedia/commons/thumb/8/84/KS_Nieciecza_herb.jpg/250px-KS_Nieciecza_herb.jpg', 'BBT', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TeamId18, 'Arka Gdynia', @CountryId, @StadiumId18, @LeagueId, 
        '/wikipedia/commons/thumb/8/8f/Logo_of_Arka_Gdynia_at_graffiti_at_ulica_Jozefa_Bema_in_Gdynia_%28cropped%29.jpg/250px-Logo_of_Arka_Gdynia_at_graffiti_at_ulica_Jozefa_Bema_in_Gdynia_%28cropped%29.jpg', 'ARK', 'Football', @CurrentDateTime, @CurrentDateTime)

    COMMIT TRANSACTION;
    PRINT 'Ekstraklasa stadiums and teams for 2025/2026 season inserted successfully.';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error occurred while inserting ekstraklasa stadiums and teams.';
END CATCH;
