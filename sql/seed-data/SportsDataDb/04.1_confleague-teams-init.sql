-- ================================================================
-- UEFA CONFERENCE LEAGUE 2024/2025 SEED (FIXED FOREIGN KEY)
-- Created: 07.12.2025
-- ================================================================

USE SportsDataDb;
GO

DECLARE @LeagueId UNIQUEIDENTIFIER;
DECLARE @SeasonYear NVARCHAR(20) = '2024/2025';
DECLARE @EuropeId UNIQUEIDENTIFIER;

-- 1. POBRANIE ID EUROPY
SELECT TOP 1 @EuropeId = Id FROM dbo.Country WHERE Code = 'EUR';

-- 2. UTWORZENIE LIGI
IF NOT EXISTS (SELECT 1 FROM dbo.League WHERE [Name] = 'UEFA Conference League')
BEGIN
    SET @LeagueId = NEWID();
    INSERT INTO dbo.League (Id, [Name], CountryId, [MaxRound]) 
    VALUES (@LeagueId, 'UEFA Conference League', @EuropeId, 6);
END
ELSE
BEGIN
    SELECT @LeagueId = Id FROM dbo.League WHERE [Name] = 'UEFA Conference League';
END

-- ZMIENNE KRAJÓW (BEZPIECZNE POBIERANIE)
DECLARE @CID_UA UNIQUEIDENTIFIER, @CID_ENG UNIQUEIDENTIFIER, @CID_CH UNIQUEIDENTIFIER, @CID_IS UNIQUEIDENTIFIER,
        @CID_AM UNIQUEIDENTIFIER, @CID_HR UNIQUEIDENTIFIER, @CID_BA UNIQUEIDENTIFIER, @CID_GI UNIQUEIDENTIFIER,
        @CID_PL UNIQUEIDENTIFIER, @CID_MT UNIQUEIDENTIFIER, @CID_AT UNIQUEIDENTIFIER, @CID_FI UNIQUEIDENTIFIER,
        @CID_XK UNIQUEIDENTIFIER, @CID_CY UNIQUEIDENTIFIER, @CID_DE UNIQUEIDENTIFIER, @CID_ES UNIQUEIDENTIFIER,
        @CID_MK UNIQUEIDENTIFIER, @CID_SCT UNIQUEIDENTIFIER, @CID_CZ UNIQUEIDENTIFIER, @CID_IE UNIQUEIDENTIFIER,
        @CID_IT UNIQUEIDENTIFIER, @CID_NL UNIQUEIDENTIFIER, @CID_TR UNIQUEIDENTIFIER, @CID_SI UNIQUEIDENTIFIER,
        @CID_GR UNIQUEIDENTIFIER, @CID_RO UNIQUEIDENTIFIER, @CID_SE UNIQUEIDENTIFIER, @CID_SK UNIQUEIDENTIFIER,
        @CID_FR UNIQUEIDENTIFIER;

SELECT @CID_UA = Id FROM dbo.Country WHERE Code = 'UA'; 
SELECT @CID_ENG = Id FROM dbo.Country WHERE Code = 'EN'; 
SELECT @CID_CH = Id FROM dbo.Country WHERE Code = 'CH'; 
SELECT @CID_IS = Id FROM dbo.Country WHERE Code = 'IS'; 
SELECT @CID_AM = Id FROM dbo.Country WHERE Code = 'AM'; 
SELECT @CID_HR = Id FROM dbo.Country WHERE Code = 'HR'; 
SELECT @CID_BA = Id FROM dbo.Country WHERE Code = 'BA'; 
SELECT @CID_GI = Id FROM dbo.Country WHERE Code = 'GI'; 
SELECT @CID_PL = Id FROM dbo.Country WHERE Code = 'PL'; 
SELECT @CID_MT = Id FROM dbo.Country WHERE Code = 'MT'; 
SELECT @CID_AT = Id FROM dbo.Country WHERE Code = 'AT'; 
SELECT @CID_FI = Id FROM dbo.Country WHERE Code = 'FI'; 
SELECT @CID_XK = Id FROM dbo.Country WHERE Code = 'XK'; 
SELECT @CID_CY = Id FROM dbo.Country WHERE Code = 'CY'; 
SELECT @CID_DE = Id FROM dbo.Country WHERE Code = 'DE'; 
SELECT @CID_ES = Id FROM dbo.Country WHERE Code = 'ES'; 
SELECT @CID_MK = Id FROM dbo.Country WHERE Code = 'MK'; 
SELECT @CID_SCT = Id FROM dbo.Country WHERE Code = 'SCT'; 
SELECT @CID_CZ = Id FROM dbo.Country WHERE Code = 'CZ'; 
SELECT @CID_IE = Id FROM dbo.Country WHERE Code = 'IE'; 
SELECT @CID_IT = Id FROM dbo.Country WHERE Code = 'IT'; 
SELECT @CID_NL = Id FROM dbo.Country WHERE Code = 'NL'; 
SELECT @CID_TR = Id FROM dbo.Country WHERE Code = 'TR'; 
SELECT @CID_SI = Id FROM dbo.Country WHERE Code = 'SI'; 
SELECT @CID_GR = Id FROM dbo.Country WHERE Code = 'GR'; 
SELECT @CID_RO = Id FROM dbo.Country WHERE Code = 'RO'; 
SELECT @CID_SE = Id FROM dbo.Country WHERE Code = 'SE'; 
SELECT @CID_SK = Id FROM dbo.Country WHERE Code = 'SK'; 
SELECT @CID_FR = Id FROM dbo.Country WHERE Code = 'FR'; 

BEGIN TRANSACTION
BEGIN TRY

    -- TABELA TYMCZASOWA
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
    ('Dynamo Kyiv', 'DYN', @CID_UA, 'Valeriy Lobanovskyi Dynamo Stadium', 16873),
    ('Shakhtar Donetsk', 'SHK', @CID_UA, 'Veltins-Arena', 62271),
    ('Crystal Palace', 'CRY', @CID_ENG, 'Selhurst Park', 25486),
    ('Lausanne-Sport', 'LAU', @CID_CH, 'Stade de la Tuilière', 12544),
    ('Breiðablik', 'BRE', @CID_IS, 'Kópavogsvöllur', 3009),
    ('Noah', 'NOA', @CID_AM, 'Abovyan City Stadium', 3100),
    ('Rijeka', 'RIJ', @CID_HR, 'Stadion Rujevica', 8279),
    ('Zrinjski Mostar', 'ZRI', @CID_BA, 'Stadion pod Bijelim Brijegom', 9000),
    ('Lincoln Red Imps', 'LIN', @CID_GI, 'Victoria Stadium', 5000),
    ('Jagiellonia Bialystok', 'JAG', @CID_PL, 'Municipal Stadium in Białystok', 22372),
    ('Lech Poznan', 'LPO', @CID_PL, 'Stadion Miejski Poznań (Municipal Stadium in Poznań)', 42837),
    ('Legia Warsaw', 'LEG', @CID_PL, 'Marshal Józef Piłsudski Municipal Stadium of Legia Warsaw', 31103),
    ('Rakow Czestochowa', 'RAK', @CID_PL, 'Raków Stadium in Częstochowa', 5500),
    ('Hamrun Spartans', 'HAM', @CID_MT, 'Victor Tedesco Stadium', 1962),
    ('Rapid Wien', 'RAP', @CID_AT, 'Allianz Stadion', 28000),
    ('KuPS', 'KUP', @CID_FI, 'Väre Areena', 5000),
    ('Drita', 'DRI', @CID_XK, 'Gjilan City Stadium', 15000),
    ('Omonia', 'OMO', @CID_CY, 'GSP Stadium', 22859),
    ('AEK Larnaca', 'AEK', @CID_CY, 'AEK Arena', 7400),
    ('Mainz 05', 'MNZ', @CID_DE, 'Mewa Arena', 33305),
    ('Rayo Vallecano', 'RAY', @CID_ES, 'Estadio de Vallecas', 14708),
    ('Shkëndija', 'SHK', @CID_MK, 'Ecolog Arena', 15000),
    ('Aberdeen', 'ABE', @CID_SCT, 'Pittodrie Stadium', 20866),
    ('Sparta Prague', 'SPA', @CID_CZ, 'epet ARENA', 18887),
    ('Sigma Olomouc', 'SIG', @CID_CZ, 'Andrův stadion', 12483),
    ('Shamrock Rovers', 'SHA', @CID_IE, 'Tallaght Stadium', 10000),
    ('Shelbourne', 'SHE', @CID_IE, 'Tolka Park', 5700),
    ('Fiorentina', 'FIO', @CID_IT, 'Stadio Artemio Franchi', 43147),
    ('AZ Alkmaar', 'AZ', @CID_NL, 'AFAS Stadion', 19478),
    ('Samsunspor', 'SAM', @CID_TR, 'Samsun 19 Mayıs Stadium', 33919),
    ('Celje', 'CEL', @CID_SI, 'Stadion Z’dežele', 13059),
    ('AEK Athens', 'ATH', @CID_GR, 'OPAP Arena', 31100),
    ('Universitatea Craiova', 'CRA', @CID_RO, 'Ion Oblemenco', 30983),
    ('BK Häcken', 'HAC', @CID_SE, 'Bravida Arena', 6500),
    ('Slovan Bratislava', 'SLO', @CID_SK, 'Tehelné pole', 22500),
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
    ON Target.[Name] = Source.TeamName
    WHEN NOT MATCHED THEN
        INSERT (Id, [Name], CountryId, StadiumId, ShortName)
        VALUES (Source.TeamId, Source.TeamName, Source.CountryId, Source.StadiumId, Source.ShortName);

    -- [CRITICAL FIX]
    UPDATE t
    SET t.TeamId = tm.Id
    FROM #TempTeams t
    JOIN dbo.Team tm ON (t.ShortName = tm.ShortName OR t.TeamName = tm.[Name]);

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
    )
    -- [CRITICAL SAFETY]
    AND EXISTS (
        SELECT 1 FROM dbo.Team WHERE Id = t.TeamId
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
