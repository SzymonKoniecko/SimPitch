-- ================================================================
-- ADD MISSING CONFERENCE LEAGUE TEAMS & STATS (REAL SHORT NAMES)
-- Season: 2024/2025 Context
-- Created: 08.12.2025
-- ================================================================

USE SportsDataDb;
GO

BEGIN TRANSACTION;
BEGIN TRY

    DECLARE @LeagueId UNIQUEIDENTIFIER;
    SELECT TOP 1 @LeagueId = Id FROM dbo.League WHERE [Name] = 'UEFA Conference League';

    IF @LeagueId IS NULL
    BEGIN
        SET @LeagueId = NEWID();
        DECLARE @EuropeId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Country WHERE Code = 'EUR');
        INSERT INTO dbo.League (Id, [Name], CountryId, [MaxRound]) VALUES (@LeagueId, 'UEFA Conference League', @EuropeId, 6);
    END

    -- 1. UTWÓRZ TABELĘ TYMCZASOWĄ (Z DODANYM RealShortName)
    CREATE TABLE #StatsData (
        TeamName NVARCHAR(100),
        RealShortName NVARCHAR(10),
        CountryCode NVARCHAR(10),
        CountryName NVARCHAR(100),
        StadiumName NVARCHAR(100),
        Capacity INT,
        M INT, W INT, D INT, L INT, GF INT, GA INT
    );

    INSERT INTO #StatsData (TeamName, RealShortName, CountryCode, CountryName, StadiumName, Capacity, M, W, D, L, GF, GA)
    VALUES
    ('Chelsea', 'CHE', 'EN', 'England', 'Stamford Bridge', 40341, 6, 6, 0, 0, 26, 5),
    ('Vitória de Guimarães', 'VSC', 'PT', 'Portugal', 'Estádio D. Afonso Henriques', 30000, 6, 4, 2, 0, 13, 6), -- Często VSC lub VIT
    ('Fiorentina', 'FIO', 'IT', 'Italy', 'Stadio Artemio Franchi', 43147, 6, 4, 1, 1, 18, 7),
    ('Rapid Wien', 'RAP', 'AT', 'Austria', 'Allianz Stadion', 28000, 6, 4, 1, 1, 11, 5),
    ('Djurgårdens IF', 'DJU', 'SE', 'Sweden', 'Tele2 Arena', 30000, 6, 4, 1, 1, 11, 7),
    ('Lugano', 'LUG', 'CH', 'Switzerland', 'Cornaredo Stadium', 6330, 6, 4, 1, 1, 11, 7),
    ('Legia Warsaw', 'LEG', 'PL', 'Poland', 'Stadion Wojska Polskiego', 31103, 6, 4, 0, 2, 13, 5),
    ('Cercle Brugge', 'CER', 'BE', 'Belgium', 'Jan Breydel Stadium', 29042, 6, 3, 2, 1, 14, 7),
    ('Jagiellonia Bialystok', 'JAG', 'PL', 'Poland', 'Stadion Miejski w Białymstoku', 22372, 6, 3, 2, 1, 10, 5),
    ('Shamrock Rovers', 'SHA', 'IE', 'Republic of Ireland', 'Tallaght Stadium', 10000, 6, 3, 2, 1, 12, 9), -- Czasem SHM
    ('APOEL', 'APO', 'CY', 'Cyprus', 'GSP Stadium', 22859, 6, 3, 2, 1, 8, 5),
    ('Pafos', 'PAF', 'CY', 'Cyprus', 'Stelios Kyriakides Stadium', 9394, 6, 3, 1, 2, 11, 7),
    ('Panathinaikos', 'PAO', 'GR', 'Greece', 'OAKA Spiros Louis', 69618, 6, 3, 1, 2, 10, 7),
    ('Olimpija Ljubljana', 'OLI', 'SI', 'Slovenia', 'Stožice Stadium', 16038, 6, 3, 1, 2, 7, 6),
    ('Real Betis', 'BET', 'ES', 'Spain', 'Benito Villamarín', 60721, 6, 3, 1, 2, 6, 5),
    ('1. FC Heidenheim', 'FCH', 'DE', 'Germany', 'Voith-Arena', 15000, 6, 3, 1, 2, 7, 7), -- FCH lub HEI
    ('Gent', 'GNT', 'BE', 'Belgium', 'Ghelamco Arena', 20000, 6, 3, 0, 3, 8, 8),
    ('Copenhagen', 'FCK', 'DK', 'Denmark', 'Parken Stadium', 38065, 6, 2, 2, 2, 8, 9), -- FCK standard
    ('Víkingur Reykjavík', 'VIK', 'IS', 'Iceland', 'Víkingsvöllur', 1449, 6, 2, 2, 2, 7, 8),
    ('Borac Banja Luka', 'BOR', 'BA', 'Bosnia and Herzegovina', 'Banja Luka City Stadium', 10030, 6, 2, 2, 2, 4, 7),
    ('Celje', 'CEL', 'SI', 'Slovenia', 'Stadion Z’dežele', 13059, 6, 2, 1, 3, 13, 13),
    ('Omonia', 'OMO', 'CY', 'Cyprus', 'GSP Stadium', 22859, 6, 2, 1, 3, 7, 7),
    ('Molde', 'MOL', 'NO', 'Norway', 'Aker Stadion', 11249, 6, 2, 1, 3, 10, 11),
    ('TSC', 'TSC', 'RS', 'Serbia', 'TSC Arena', 4500, 6, 2, 1, 3, 10, 13),
    ('Heart of Midlothian', 'HEA', 'SCT', 'Scotland', 'Tynecastle Park', 19852, 6, 2, 1, 3, 6, 9), -- Hearts = HEA/HRT
    ('İstanbul Başakşehir', 'IBFK', 'TR', 'Turkey', 'Fatih Terim Stadium', 17156, 6, 1, 3, 2, 9, 12), -- IBFK
    ('Mladá Boleslav', 'MLB', 'CZ', 'Czech Republic', 'Lokotrans Aréna', 5000, 6, 2, 0, 4, 7, 10),
    ('Astana', 'AST', 'KZ', 'Kazakhstan', 'Astana Arena', 30000, 6, 1, 2, 3, 4, 8),
    ('St. Gallen', 'FCSG', 'CH', 'Switzerland', 'Kybunpark', 19694, 6, 1, 2, 3, 10, 18), -- FCSG standard
    ('HJK', 'HJK', 'FI', 'Finland', 'Bolt Arena', 10770, 6, 1, 1, 4, 3, 9),
    ('Noah', 'NOA', 'AM', 'Armenia', 'Abovyan City Stadium', 3100, 6, 1, 1, 4, 6, 16),
    ('The New Saints', 'TNS', 'WLS', 'Wales', 'Park Hall', 2034, 6, 1, 0, 5, 5, 10), -- TNS standard
    ('Dinamo Minsk', 'DMN', 'BY', 'Belarus', 'Dinamo Stadium', 22000, 6, 1, 0, 5, 4, 13),
    ('Larne', 'LAR', 'NIR', 'Northern Ireland', 'Inver Park', 3000, 6, 1, 0, 5, 3, 12),
    ('LASK', 'LASK', 'AT', 'Austria', 'Raiffeisen Arena', 19080, 6, 0, 3, 3, 4, 14),
    ('Petrocub Hîncești', 'PET', 'MD', 'Moldova', 'Municipal Stadium Hîncești', 1200, 6, 0, 2, 4, 4, 13);

    -- 2. AUTOMATYCZNE DODAWANIE BRAKUJĄCYCH KRAJÓW
    INSERT INTO dbo.Country (Id, [Name], [Code])
    SELECT 
        NEWID(),
        CountryName,
        CountryCode
    FROM #StatsData
    WHERE CountryCode NOT IN (SELECT Code FROM dbo.Country)
    GROUP BY CountryName, CountryCode;

    -- 3. INSERT STADIUMS (MERGE)
    MERGE INTO dbo.Stadium AS Target
    USING #StatsData AS Source
    ON Target.[Name] = Source.StadiumName
    WHEN NOT MATCHED THEN
        INSERT (Id, [Name], Capacity)
        VALUES (NEWID(), Source.StadiumName, Source.Capacity);

    -- 4. INSERT TEAMS (MERGE) - TERAZ Z ShortName Z TABELI
    MERGE INTO dbo.Team AS Target
    USING (
        SELECT 
            s.TeamName, 
            s.RealShortName,
            s.StadiumName,
            st.Id AS StadId,
            c.Id AS CntryId
        FROM #StatsData s
        JOIN dbo.Stadium st ON s.StadiumName = st.[Name]
        JOIN dbo.Country c ON s.CountryCode = c.Code
    ) AS Source
    ON Target.[Name] = Source.TeamName
    WHEN NOT MATCHED THEN
        INSERT (Id, [Name], CountryId, StadiumId, ShortName)
        VALUES (NEWID(), Source.TeamName, Source.CntryId, Source.StadId, Source.RealShortName);

    -- 5. INSERT SEASON STATS (2024/2025)
    DECLARE @StatsSeason NVARCHAR(20) = '2024/2025';

    INSERT INTO dbo.SeasonStats (Id, TeamId, LeagueId, SeasonYear, MatchesPlayed, Wins, Losses, Draws, GoalsFor, GoalsAgainst)
    SELECT 
        NEWID(),
        t.Id,
        @LeagueId,
        @StatsSeason,
        s.M, 
        s.W, 
        s.L, -- L (Losses)
        s.D, -- D (Draws)
        s.GF, 
        s.GA
    FROM #StatsData s
    JOIN dbo.Team t ON s.TeamName = t.[Name]
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.SeasonStats ss 
        WHERE ss.TeamId = t.Id AND ss.LeagueId = @LeagueId AND ss.SeasonYear = @StatsSeason
    );

    -- 6. INSERT COMPETITION MEMBERSHIP (2024/2025)
    INSERT INTO dbo.CompetitionMembership (Id, TeamId, LeagueId, SeasonYear)
    SELECT 
        NEWID(),
        t.Id,
        @LeagueId,
        '2024/2025'
    FROM #StatsData s
    JOIN dbo.Team t ON s.TeamName = t.[Name]
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.CompetitionMembership cm 
        WHERE cm.TeamId = t.Id AND cm.LeagueId = @LeagueId AND cm.SeasonYear = '2024/2025'
    );

    DROP TABLE #StatsData;

    COMMIT TRANSACTION;
    PRINT '✅ Sukces! Dodano brakujące drużyny z poprawnymi skrótami (np. CHE, FCK, TNS) i ich statystyki.';

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT '❌ Błąd: ' + ERROR_MESSAGE();
END CATCH;
GO
