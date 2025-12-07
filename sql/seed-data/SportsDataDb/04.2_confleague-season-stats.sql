-- ================================================================
-- ADD MISSING CONFERENCE LEAGUE TEAMS & STATS (FIXED & SAFE)
-- Season: 2024/2025 Context
-- Created: 07.12.2025
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

    -- 1. UTWÓRZ TABELĘ TYMCZASOWĄ Z NOWYMI DRUŻYNAMI I STATYSTYKAMI
    CREATE TABLE #StatsData (
        TeamName NVARCHAR(100),
        CountryCode NVARCHAR(10),
        StadiumName NVARCHAR(100),
        Capacity INT,
        M INT, W INT, D INT, L INT, GF INT, GA INT
    );

    -- Wstawianie danych (W, D, L - Zwróć uwagę na kolejność!)
    INSERT INTO #StatsData (TeamName, CountryCode, StadiumName, Capacity, M, W, D, L, GF, GA)
    VALUES
    ('Chelsea', 'EN', 'Stamford Bridge', 40341, 6, 6, 0, 0, 26, 5),
    ('Vitória de Guimarães', 'PT', 'Estádio D. Afonso Henriques', 30000, 6, 4, 2, 0, 13, 6),
    ('Fiorentina', 'IT', 'Stadio Artemio Franchi', 43147, 6, 4, 1, 1, 18, 7),
    ('Rapid Wien', 'AT', 'Allianz Stadion', 28000, 6, 4, 1, 1, 11, 5),
    ('Djurgårdens IF', 'SE', 'Tele2 Arena', 30000, 6, 4, 1, 1, 11, 7),
    ('Lugano', 'CH', 'Cornaredo Stadium', 6330, 6, 4, 1, 1, 11, 7),
    ('Legia Warsaw', 'PL', 'Stadion Wojska Polskiego', 31103, 6, 4, 0, 2, 13, 5),
    ('Cercle Brugge', 'BE', 'Jan Breydel Stadium', 29042, 6, 3, 2, 1, 14, 7),
    ('Jagiellonia Bialystok', 'PL', 'Stadion Miejski w Białymstoku', 22372, 6, 3, 2, 1, 10, 5),
    ('Shamrock Rovers', 'IE', 'Tallaght Stadium', 10000, 6, 3, 2, 1, 12, 9),
    ('APOEL', 'CY', 'GSP Stadium', 22859, 6, 3, 2, 1, 8, 5),
    ('Pafos', 'CY', 'Stelios Kyriakides Stadium', 9394, 6, 3, 1, 2, 11, 7),
    ('Panathinaikos', 'GR', 'OAKA Spiros Louis', 69618, 6, 3, 1, 2, 10, 7),
    ('Olimpija Ljubljana', 'SI', 'Stožice Stadium', 16038, 6, 3, 1, 2, 7, 6),
    ('Real Betis', 'ES', 'Benito Villamarín', 60721, 6, 3, 1, 2, 6, 5),
    ('1. FC Heidenheim', 'DE', 'Voith-Arena', 15000, 6, 3, 1, 2, 7, 7),
    ('Gent', 'BE', 'Ghelamco Arena', 20000, 6, 3, 0, 3, 8, 8),
    ('Copenhagen', 'DK', 'Parken Stadium', 38065, 6, 2, 2, 2, 8, 9),
    ('Víkingur Reykjavík', 'IS', 'Víkingsvöllur', 1449, 6, 2, 2, 2, 7, 8),
    ('Borac Banja Luka', 'BA', 'Banja Luka City Stadium', 10030, 6, 2, 2, 2, 4, 7),
    ('Celje', 'SI', 'Stadion Z’dežele', 13059, 6, 2, 1, 3, 13, 13),
    ('Omonia', 'CY', 'GSP Stadium', 22859, 6, 2, 1, 3, 7, 7),
    ('Molde', 'NO', 'Aker Stadion', 11249, 6, 2, 1, 3, 10, 11),
    ('TSC', 'RS', 'TSC Arena', 4500, 6, 2, 1, 3, 10, 13),
    ('Heart of Midlothian', 'SCT', 'Tynecastle Park', 19852, 6, 2, 1, 3, 6, 9),
    ('İstanbul Başakşehir', 'TR', 'Fatih Terim Stadium', 17156, 6, 1, 3, 2, 9, 12),
    ('Mladá Boleslav', 'CZ', 'Lokotrans Aréna', 5000, 6, 2, 0, 4, 7, 10),
    ('Astana', 'KZ', 'Astana Arena', 30000, 6, 1, 2, 3, 4, 8),
    ('St. Gallen', 'CH', 'Kybunpark', 19694, 6, 1, 2, 3, 10, 18),
    ('HJK', 'FI', 'Bolt Arena', 10770, 6, 1, 1, 4, 3, 9),
    ('Noah', 'AM', 'Abovyan City Stadium', 3100, 6, 1, 1, 4, 6, 16),
    ('The New Saints', 'WLS', 'Park Hall', 2034, 6, 1, 0, 5, 5, 10),
    ('Dinamo Minsk', 'BY', 'Dinamo Stadium', 22000, 6, 1, 0, 5, 4, 13),
    ('Larne', 'NIR', 'Inver Park', 3000, 6, 1, 0, 5, 3, 12),
    ('LASK', 'AT', 'Raiffeisen Arena', 19080, 6, 0, 3, 3, 4, 14),
    ('Petrocub Hîncești', 'MD', 'Municipal Stadium Hîncești', 1200, 6, 0, 2, 4, 4, 13);


    -- 2. INSERT STADIUMS (MERGE)
    MERGE INTO dbo.Stadium AS Target
    USING #StatsData AS Source
    ON Target.[Name] = Source.StadiumName
    WHEN NOT MATCHED THEN
        INSERT (Id, [Name], Capacity)
        VALUES (NEWID(), Source.StadiumName, Source.Capacity);


    -- 3. INSERT TEAMS (MERGE)
    MERGE INTO dbo.Team AS Target
    USING (
        SELECT 
            s.TeamName, 
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
        VALUES (NEWID(), Source.TeamName, Source.CntryId, Source.StadId, LEFT(Source.TeamName, 3));


    -- 4. INSERT SEASON STATS
    -- Uwaga: W danych wejściowych masz W, D, L.
    -- W tabeli SeasonStats kolumny to często MatchesPlayed, Wins, Losses, Draws.
    -- Musimy zmapować to poprawnie: D -> Draws, L -> Losses.
    
    DECLARE @StatsSeason NVARCHAR(20) = '2023/2024'; -- Statystyki "bazowe" do symulacji

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


    -- 5. INSERT COMPETITION MEMBERSHIP (2024/2025)
    -- Rejestrujemy drużyny w lidze na sezon 24/25
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
    PRINT '✅ Sukces! Dodano brakujące drużyny (np. Chelsea, Betis) i ich statystyki.';


END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT '❌ Błąd: ' + ERROR_MESSAGE();
END CATCH;
GO
