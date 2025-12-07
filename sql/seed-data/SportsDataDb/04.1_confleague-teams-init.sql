-- ================================================================
-- UEFA CONFERENCE LEAGUE 2024/2025 SEED
-- Created: 07.12.2025
-- ================================================================

USE SportsDataDb;
GO

DECLARE @LeagueId UNIQUEIDENTIFIER;
DECLARE @SeasonYear NVARCHAR(20) = '2024/2025';

IF NOT EXISTS (SELECT 1 FROM dbo.League WHERE [Name] = 'UEFA Conference League')
BEGIN
    SET @LeagueId = NEWID();
    INSERT INTO dbo.League (Id, [Name], CountryId, [MaxRound]) 
    VALUES (@LeagueId, 'UEFA Conference League', SELECT Id FROM dbo.Country WHERE Code = 'EUR', 6);
END
ELSE
BEGIN
    SELECT @LeagueId = Id FROM dbo.League WHERE [Name] = 'UEFA Conference League';
END

DECLARE @CID_UA UNIQUEIDENTIFIER, @CID_ENG UNIQUEIDENTIFIER, @CID_CH UNIQUEIDENTIFIER, @CID_IS UNIQUEIDENTIFIER,
        @CID_AM UNIQUEIDENTIFIER, @CID_HR UNIQUEIDENTIFIER, @CID_BA UNIQUEIDENTIFIER, @CID_GI UNIQUEIDENTIFIER,
        @CID_PL UNIQUEIDENTIFIER, @CID_MT UNIQUEIDENTIFIER, @CID_AT UNIQUEIDENTIFIER, @CID_FI UNIQUEIDENTIFIER,
        @CID_XK UNIQUEIDENTIFIER, @CID_CY UNIQUEIDENTIFIER, @CID_DE UNIQUEIDENTIFIER, @CID_ES UNIQUEIDENTIFIER,
        @CID_MK UNIQUEIDENTIFIER, @CID_SCT UNIQUEIDENTIFIER, @CID_CZ UNIQUEIDENTIFIER, @CID_IE UNIQUEIDENTIFIER,
        @CID_IT UNIQUEIDENTIFIER, @CID_NL UNIQUEIDENTIFIER, @CID_TR UNIQUEIDENTIFIER, @CID_SI UNIQUEIDENTIFIER,
        @CID_GR UNIQUEIDENTIFIER, @CID_RO UNIQUEIDENTIFIER, @CID_SE UNIQUEIDENTIFIER, @CID_SK UNIQUEIDENTIFIER,
        @CID_FR UNIQUEIDENTIFIER;

SELECT @CID_UA = Id FROM dbo.Country WHERE Code = 'UA'; -- Ukraine
SELECT @CID_ENG = Id FROM dbo.Country WHERE Code = 'EN'; -- England
SELECT @CID_CH = Id FROM dbo.Country WHERE Code = 'CH'; -- Switzerland
SELECT @CID_IS = Id FROM dbo.Country WHERE Code = 'IS'; -- Iceland
SELECT @CID_AM = Id FROM dbo.Country WHERE Code = 'AM'; -- Armenia
SELECT @CID_HR = Id FROM dbo.Country WHERE Code = 'HR'; -- Croatia
SELECT @CID_BA = Id FROM dbo.Country WHERE Code = 'BA'; -- Bosnia
SELECT @CID_GI = Id FROM dbo.Country WHERE Code = 'GI'; -- Gibraltar
SELECT @CID_PL = Id FROM dbo.Country WHERE Code = 'PL'; -- Poland
SELECT @CID_MT = Id FROM dbo.Country WHERE Code = 'MT'; -- Malta
SELECT @CID_AT = Id FROM dbo.Country WHERE Code = 'AT'; -- Austria
SELECT @CID_FI = Id FROM dbo.Country WHERE Code = 'FI'; -- Finland
SELECT @CID_XK = Id FROM dbo.Country WHERE Code = 'XK'; -- Kosovo
SELECT @CID_CY = Id FROM dbo.Country WHERE Code = 'CY'; -- Cyprus
SELECT @CID_DE = Id FROM dbo.Country WHERE Code = 'DE'; -- Germany
SELECT @CID_ES = Id FROM dbo.Country WHERE Code = 'ES'; -- Spain
SELECT @CID_MK = Id FROM dbo.Country WHERE Code = 'MK'; -- North Macedonia
SELECT @CID_SCT = Id FROM dbo.Country WHERE Code = 'SCT'; -- Scotland
SELECT @CID_CZ = Id FROM dbo.Country WHERE Code = 'CZ'; -- Czech Republic
SELECT @CID_IE = Id FROM dbo.Country WHERE Code = 'IE'; -- Ireland
SELECT @CID_IT = Id FROM dbo.Country WHERE Code = 'IT'; -- Italy
SELECT @CID_NL = Id FROM dbo.Country WHERE Code = 'NL'; -- Netherlands
SELECT @CID_TR = Id FROM dbo.Country WHERE Code = 'TR'; -- Turkey
SELECT @CID_SI = Id FROM dbo.Country WHERE Code = 'SI'; -- Slovenia
SELECT @CID_GR = Id FROM dbo.Country WHERE Code = 'GR'; -- Greece
SELECT @CID_RO = Id FROM dbo.Country WHERE Code = 'RO'; -- Romania
SELECT @CID_SE = Id FROM dbo.Country WHERE Code = 'SE'; -- Sweden
SELECT @CID_SK = Id FROM dbo.Country WHERE Code = 'SK'; -- Slovakia
SELECT @CID_FR = Id FROM dbo.Country WHERE Code = 'FR'; -- France

BEGIN TRANSACTION
BEGIN TRY

    CREATE TABLE #TempTeams (
        TeamName NVARCHAR(100),
        ShortName NVARCHAR(10),
        CountryId UNIQUEIDENTIFIER,
        StadiumName NVARCHAR(100),
        StadiumCap INT,
        TeamId UNIQUEIDENTIFIER DEFAULT NEWID(),
        StadiumId UNIQUEIDENTIFIER DEFAULT NEWID()
    );

    INSERT INTO #TempTeams (TeamName, ShortName, CountryId, StadiumName, StadiumCap)
    VALUES
    -- UKRAINE
    ('Dynamo Kyiv', 'DYN', @CID_UA, 'Valeriy Lobanovskyi Dynamo Stadium', 16873),
    ('Shakhtar Donetsk', 'SHK', @CID_UA, 'Veltins-Arena', 62271),

    -- ENGLAND
    ('Crystal Palace', 'CRY', @CID_ENG, 'Selhurst Park', 25486),

    -- SWITZERLAND
    ('Lausanne-Sport', 'LAU', @CID_CH, 'Stade de la Tuilière', 12544),

    -- ICELAND
    ('Breiðablik', 'BRE', @CID_IS, 'Kópavogsvöllur', 3009),

    -- ARMENIA
    ('Noah', 'NOA', @CID_AM, 'Abovyan City Stadium', 3100),

    -- CROATIA
    ('Rijeka', 'RIJ', @CID_HR, 'Stadion Rujevica', 8279),

    -- BOSNIA
    ('Zrinjski Mostar', 'ZRI', @CID_BA, 'Stadion pod Bijelim Brijegom', 9000),

    -- GIBRALTAR
    ('Lincoln Red Imps', 'LIN', @CID_GI, 'Victoria Stadium', 5000),

    -- POLAND
    ('Jagiellonia Bialystok', 'JAG', @CID_PL, 'Municipal Stadium in Białystok', 22372),
    ('Lech Poznan', 'LPO', @CID_PL, 'Stadion Miejski Poznań (Municipal Stadium in Poznań)', 42837),
    ('Legia Warsaw', 'LEG', @CID_PL, 'Marshal Józef Piłsudski Municipal Stadium of Legia Warsaw', 31103),
    ('Rakow Czestochowa', 'RAK', @CID_PL, 'Raków Stadium in Częstochowa', 5500),

    -- MALTA
    ('Hamrun Spartans', 'HAM', @CID_MT, 'Victor Tedesco Stadium', 1962),

    -- AUSTRIA
    ('Rapid Wien', 'RAP', @CID_AT, 'Allianz Stadion', 28000),

    -- FINLAND
    ('KuPS', 'KUP', @CID_FI, 'Väre Areena', 5000),

    -- KOSOVO
    ('Drita', 'DRI', @CID_XK, 'Gjilan City Stadium', 15000),

    -- CYPRUS
    ('Omonia', 'OMO', @CID_CY, 'GSP Stadium', 22859),
    ('AEK Larnaca', 'AEK', @CID_CY, 'AEK Arena', 7400),

    -- GERMANY
    ('Mainz 05', 'MNZ', @CID_DE, 'Mewa Arena', 33305),

    -- SPAIN
    ('Rayo Vallecano', 'RAY', @CID_ES, 'Estadio de Vallecas', 14708),

    -- NORTH MACEDONIA
    ('Shkëndija', 'SHK', @CID_MK, 'Ecolog Arena', 15000),

    -- SCOTLAND
    ('Aberdeen', 'ABE', @CID_SCT, 'Pittodrie Stadium', 20866),

    -- CZECH REPUBLIC
    ('Sparta Prague', 'SPA', @CID_CZ, 'epet ARENA', 18887),
    ('Sigma Olomouc', 'SIG', @CID_CZ, 'Andrův stadion', 12483),

    -- IRELAND
    ('Shamrock Rovers', 'SHA', @CID_IE, 'Tallaght Stadium', 10000),
    ('Shelbourne', 'SHE', @CID_IE, 'Tolka Park', 5700),

    -- ITALY
    ('Fiorentina', 'FIO', @CID_IT, 'Stadio Artemio Franchi', 43147),

    -- NETHERLANDS
    ('AZ Alkmaar', 'AZ', @CID_NL, 'AFAS Stadion', 19478), -- W inpucie AZ

    -- TURKEY
    ('Samsunspor', 'SAM', @CID_TR, 'Samsun 19 Mayıs Stadium', 33919),

    -- SLOVENIA
    ('Celje', 'CEL', @CID_SI, 'Stadion Z’dežele', 13059),

    -- GREECE
    ('AEK Athens', 'ATH', @CID_GR, 'OPAP Arena', 31100),

    -- ROMANIA
    ('Universitatea Craiova', 'CRA', @CID_RO, 'Ion Oblemenco', 30983),

    -- SWEDEN
    ('BK Häcken', 'HAC', @CID_SE, 'Bravida Arena', 6500),

    -- SLOVAKIA
    ('Slovan Bratislava', 'SLO', @CID_SK, 'Tehelné pole', 22500),

    -- FRANCE
    ('Strasbourg', 'STR', @CID_FR, 'Stade de la Meinau', 26109);

    -- 2. INSERT STADIUMS
    MERGE INTO dbo.Stadium AS Target
    USING #TempTeams AS Source
    ON Target.[Name] = Source.StadiumName
    WHEN NOT MATCHED THEN
        INSERT (Id, [Name], Capacity)
        VALUES (Source.StadiumId, Source.StadiumName, Source.StadiumCap);

    UPDATE t
    SET t.StadiumId = s.Id
    FROM #TempTeams t
    JOIN dbo.Stadium s ON t.StadiumName = s.[Name];

    -- 3. INSERT TEAMS
    MERGE INTO dbo.Team AS Target
    USING #TempTeams AS Source
    ON Target.[ShortName] = Source.ShortName
    WHEN NOT MATCHED THEN
        INSERT (Id, [Name], CountryId, StadiumId, ShortName)
        VALUES (Source.TeamId, Source.TeamName, Source.CountryId, Source.StadiumId, Source.ShortName);

    UPDATE t
    SET t.TeamId = tm.Id
    FROM #TempTeams t
    JOIN dbo.Team tm ON t.TeamName = tm.[Name];

    -- 4. INSERT COMPETITION MEMBERSHIP
    INSERT INTO dbo.CompetitionMembership (Id, TeamId, LeagueId, SeasonYear)
    SELECT 
        NEWID(),
        t.TeamId,
        @LeagueId,
        @SeasonYear
    FROM #TempTeams t
    WHERE NOT EXISTS (
        SELECT 1 
        FROM dbo.CompetitionMembership cm 
        WHERE cm.TeamId = t.TeamId 
          AND cm.LeagueId = @LeagueId 
          AND cm.SeasonYear = @SeasonYear
    );

    DROP TABLE #TempTeams;

    COMMIT TRANSACTION;
    PRINT '✅ Sukces! Added teams, stadium and memberships for conference league 2025/2026.';

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT '❌ Błąd podczas wykonywania skryptu:';
    PRINT ERROR_MESSAGE();
END CATCH;
GO

-- WALIDACJA
SELECT 
    l.[Name] AS League,
    cm.SeasonYear,
    t.[Name] AS Team,
    c.Code AS Country,
    s.[Name] AS Stadium
FROM dbo.CompetitionMembership cm
JOIN dbo.League l ON cm.LeagueId = l.Id
JOIN dbo.Team t ON cm.TeamId = t.Id
LEFT JOIN dbo.Country c ON t.CountryId = c.Id
LEFT JOIN dbo.Stadium s ON t.StadiumId = s.Id
WHERE l.[Name] = 'UEFA Conference League'
ORDER BY t.[Name];
