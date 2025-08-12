USE SportsDataDb;

DECLARE 
    @CountryId UNIQUEIDENTIFIER, 
    @LeagueId UNIQUEIDENTIFIER,
    @CurrentDateTime DATETIME2 = GETDATE();

DECLARE @JagaId UNIQUEIDENTIFIER = NEWID(), @LegiaId UNIQUEIDENTIFIER = NEWID(),
        @LechId UNIQUEIDENTIFIER = NEWID(), @WidzewId UNIQUEIDENTIFIER = NEWID(),
        @RakowId UNIQUEIDENTIFIER = NEWID(), @PogonId UNIQUEIDENTIFIER = NEWID(),
        @CracoviaId UNIQUEIDENTIFIER = NEWID(), @GornikId UNIQUEIDENTIFIER = NEWID(),
        @WislaPlockId UNIQUEIDENTIFIER = NEWID(), @LechiaId UNIQUEIDENTIFIER = NEWID(),
        @RadomiakId UNIQUEIDENTIFIER = NEWID(), @MotorLublinId UNIQUEIDENTIFIER = NEWID(),
        @GKSKatowiceId UNIQUEIDENTIFIER = NEWID(), @ZaglebieLubinId UNIQUEIDENTIFIER = NEWID(),
        @KoronaKielceId UNIQUEIDENTIFIER = NEWID(), @PiastGliwiceId UNIQUEIDENTIFIER = NEWID(),
        @TermalicaId UNIQUEIDENTIFIER = NEWID(), @ArkaGdyniaId UNIQUEIDENTIFIER = NEWID();

-- Pobierz Id kraju Polska oraz ligi PKO BP Ekstraklasa
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
    -- Wstawianie drużyn (jeśli jeszcze nie istnieją)
    -- W rzeczywistości, zamiast newid(), pobrałbyś id z tabeli.
    -- Ten skrypt zakłada, że tabela jest pusta, więc używamy NEWID()
    INSERT INTO dbo.Team (Id, [Name], CountryId, StadiumId, LeagueId, LogoUrl, ShortName, Sport, CreatedAt, UpdatedAt)
    VALUES
        (@JagaId, 'Jagiellonia Białystok', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/e/e8/Jagiellonia_Bia%C5%82ystok_Logo_1.png', 'JAG', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@LegiaId, 'Legia Warsaw', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/f/fa/Legia_Warszawa.png', 'LEG', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@LechId, 'Lech Poznań', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/f/ff/Lech_Pozna%C5%84_logo.png', 'LEC', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@WidzewId, 'Widzew Łódź', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/f/fd/Herb_Widzew_%C5%81%C3%B3d%C5%BA.png', 'WID', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@RakowId, 'Raków Częstochowa', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/a/af/Rks_rakow_crest_ai.svg', 'RAK', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@PogonId, 'Pogoń Szczecin', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/a/a9/Pogo%C5%84_Szczecin_logo.svg', 'POG', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@CracoviaId, 'Cracovia', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/5/5e/Cracovia_logo.png', 'CRA', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@GornikId, 'Górnik Zabrze', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/d/d9/Gornik_Zabrze.png', 'GOR', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@WislaPlockId, 'Wisła Płock', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/0/0e/Wisla_P%C5%82ock.png', 'WPŁ', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@LechiaId, 'Lechia Gdańsk', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/thumb/1/15/Lechia_Gda%C5%84sk_Logo.svg/1024px-Lechia_Gda%C5%84sk_Logo.svg.png', 'LEH', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@RadomiakId, 'Radomiak Radom', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/2/2b/Herb_radomiaka_300dpi.png', 'RAD', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@MotorLublinId, 'Motor Lublin', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/3/3d/Motor_Lublin_S.A._Oficjalny_Herb.png', 'MOT', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@GKSKatowiceId, 'GKS Katowice', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/3/38/GKS_Katowice.png', 'GKS', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@ZaglebieLubinId, 'Zagłębie Lubin', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/e/e4/Zaglebie_Lubin.png', 'ZAG', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@KoronaKielceId, 'Korona Kielce', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/b/b3/Korona_Kielce_logo.png', 'KOR', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@PiastGliwiceId, 'Piast Gliwice', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/b/bb/Piast_Gliwice.png', 'PIA', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@TermalicaId, 'Bruk-Bet Termalica Nieciecza', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/8/84/KS_Nieciecza_herb.jpg', 'BBT', 'Football', @CurrentDateTime, @CurrentDateTime),
        (@ArkaGdyniaId, 'Arka Gdynia', @CountryId, NEWID(), @LeagueId, 'https://upload.wikimedia.org/wikipedia/commons/8/8f/Logo_of_Arka_Gdynia_at_graffiti_at_ulica_Jozefa_Bema_in_Gdynia_%28cropped%29.jpg', 'ARK', 'Football', @CurrentDateTime, @CurrentDateTime);

    -- RUNDA 1
    DECLARE @Round1JSON NVARCHAR(MAX) = (
        SELECT 
            (SELECT @JagaId AS HomeTeamId, @LechId AS AwayTeamId, 2 AS HomeGoals, 1 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @LegiaId AS HomeTeamId, @RakowId AS AwayTeamId, 3 AS HomeGoals, 0 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @WidzewId AS HomeTeamId, @PogonId AS AwayTeamId, 1 AS HomeGoals, 1 AS AwayGoals, 1 AS IsDraw FOR JSON PATH),
            (SELECT @CracoviaId AS HomeTeamId, @GornikId AS AwayTeamId, 0 AS HomeGoals, 2 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @WislaPlockId AS HomeTeamId, @LechiaId AS AwayTeamId, 2 AS HomeGoals, 2 AS AwayGoals, 1 AS IsDraw FOR JSON PATH),
            (SELECT @RadomiakId AS HomeTeamId, @MotorLublinId AS AwayTeamId, 1 AS HomeGoals, 0 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @GKSKatowiceId AS HomeTeamId, @ZaglebieLubinId AS AwayTeamId, 0 AS HomeGoals, 0 AS AwayGoals, 1 AS IsDraw FOR JSON PATH),
            (SELECT @KoronaKielceId AS HomeTeamId, @PiastGliwiceId AS AwayTeamId, 3 AS HomeGoals, 1 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @TermalicaId AS HomeTeamId, @ArkaGdyniaId AS AwayTeamId, 1 AS HomeGoals, 3 AS AwayGoals, 0 AS IsDraw FOR JSON PATH)
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    );

    INSERT INTO dbo.RoundOfFootballLeague (Id, LeagueId, SeasonYear, RoundOf, DataJSON)
    VALUES (NEWID(), @LeagueId, '2025/2026', 'Runda 1', @Round1JSON);

    -- RUNDA 2
    DECLARE @Round2JSON NVARCHAR(MAX) = (
        SELECT 
            (SELECT @LechId AS HomeTeamId, @LegiaId AS AwayTeamId, 1 AS HomeGoals, 0 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @RakowId AS HomeTeamId, @JagaId AS AwayTeamId, 2 AS HomeGoals, 2 AS AwayGoals, 1 AS IsDraw FOR JSON PATH),
            (SELECT @PogonId AS HomeTeamId, @WislaPlockId AS AwayTeamId, 3 AS HomeGoals, 1 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @GornikId AS HomeTeamId, @WidzewId AS AwayTeamId, 0 AS HomeGoals, 1 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @LechiaId AS HomeTeamId, @CracoviaId AS AwayTeamId, 4 AS HomeGoals, 0 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @MotorLublinId AS HomeTeamId, @GKSKatowiceId AS AwayTeamId, 1 AS HomeGoals, 1 AS AwayGoals, 1 AS IsDraw FOR JSON PATH),
            (SELECT @ZaglebieLubinId AS HomeTeamId, @KoronaKielceId AS AwayTeamId, 2 AS HomeGoals, 0 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @PiastGliwiceId AS HomeTeamId, @TermalicaId AS AwayTeamId, 1 AS HomeGoals, 2 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @ArkaGdyniaId AS HomeTeamId, @RadomiakId AS AwayTeamId, 0 AS HomeGoals, 0 AS AwayGoals, 1 AS IsDraw FOR JSON PATH)
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    );

    INSERT INTO dbo.RoundOfFootballLeague (Id, LeagueId, SeasonYear, RoundOf, DataJSON)
    VALUES (NEWID(), @LeagueId, '2025/2026', 'Runda 2', @Round2JSON);

    -- RUNDA 3
    DECLARE @Round3JSON NVARCHAR(MAX) = (
        SELECT 
            (SELECT @JagaId AS HomeTeamId, @PogonId AS AwayTeamId, 1 AS HomeGoals, 2 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @LegiaId AS HomeTeamId, @WidzewId AS AwayTeamId, 2 AS HomeGoals, 1 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @LechId AS HomeTeamId, @GornikId AS AwayTeamId, 3 AS HomeGoals, 3 AS AwayGoals, 1 AS IsDraw FOR JSON PATH),
            (SELECT @RakowId AS HomeTeamId, @LechiaId AS AwayTeamId, 0 AS HomeGoals, 1 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @CracoviaId AS HomeTeamId, @MotorLublinId AS AwayTeamId, 2 AS HomeGoals, 0 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @WislaPlockId AS HomeTeamId, @ZaglebieLubinId AS AwayTeamId, 1 AS HomeGoals, 1 AS AwayGoals, 1 AS IsDraw FOR JSON PATH),
            (SELECT @RadomiakId AS HomeTeamId, @PiastGliwiceId AS AwayTeamId, 0 AS HomeGoals, 0 AS AwayGoals, 1 AS IsDraw FOR JSON PATH),
            (SELECT @GKSKatowiceId AS HomeTeamId, @ArkaGdyniaId AS AwayTeamId, 1 AS HomeGoals, 2 AS AwayGoals, 0 AS IsDraw FOR JSON PATH),
            (SELECT @KoronaKielceId AS HomeTeamId, @TermalicaId AS AwayTeamId, 3 AS HomeGoals, 0 AS AwayGoals, 0 AS IsDraw FOR JSON PATH)
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    );

    INSERT INTO dbo.RoundOfFootballLeague (Id, LeagueId, SeasonYear, RoundOf, DataJSON)
    VALUES (NEWID(), @LeagueId, '2025/2026', 'Runda 3', @Round3JSON);

    COMMIT TRANSACTION;
    PRINT 'Results for the first 3 rounds of PKO BP Ekstraklasa (2025/2026) inserted successfully.';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error occurred while inserting match results.';
    PRINT ERROR_MESSAGE();
END CATCH;