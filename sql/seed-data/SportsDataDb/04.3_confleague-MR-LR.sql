-- ================================================================
-- UEFA CONFERENCE LEAGUE 2025/2026 - MATCH ROUNDS SEED (FIXED)
-- Description: Inserts all match results and fixtures for the league stage.
-- Created: 07.12.2025
-- ================================================================

USE SportsDataDb;
GO

BEGIN TRANSACTION;
BEGIN TRY

    DECLARE @LeagueId UNIQUEIDENTIFIER;
    DECLARE @SeasonYear NVARCHAR(20) = '2025/2026';
    
    SELECT TOP 1 @LeagueId = Id FROM dbo.League WHERE [Name] = 'UEFA Conference League';

    IF @LeagueId IS NULL
    BEGIN
        THROW 50000, 'League ''UEFA Conference League'' not found. Please run the initial seed scripts.', 1;
    END

    DECLARE @T_DynamoK UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Dynamo Kyiv');
    DECLARE @T_CrysPal UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Crystal Palace');
    DECLARE @T_Lausanne UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Lausanne-Sport');
    DECLARE @T_Breidablik UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Breiðablik');
    DECLARE @T_Noah UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Noah');
    DECLARE @T_Rijeka UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Rijeka');
    DECLARE @T_Zrinjski UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Zrinjski Mostar');
    DECLARE @T_Lincoln UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Lincoln Red Imps');
    DECLARE @T_Jagiellonia UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Jagiellonia Bialystok');
    DECLARE @T_Hamrun UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Hamrun Spartans');
    DECLARE @T_Lech UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Lech Poznan');
    DECLARE @T_Rapid UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Rapid Wien');
    DECLARE @T_KuPS UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'KuPS');
    DECLARE @T_Drita UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Drita');
    DECLARE @T_Omonia UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Omonia');
    DECLARE @T_Mainz UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Mainz 05');
    DECLARE @T_Rayo UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Rayo Vallecano');
    DECLARE @T_Shkendija UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Shkëndija');
    DECLARE @T_Aberdeen UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Aberdeen');
    DECLARE @T_Shakhtar UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Shakhtar Donetsk');
    DECLARE @T_SpartaP UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Sparta Prague');
    DECLARE @T_Shamrock UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Shamrock Rovers');
    DECLARE @T_Fiorentina UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Fiorentina');
    DECLARE @T_Sigma UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Sigma Olomouc');
    DECLARE @T_AEKLarnaca UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'AEK Larnaca');
    DECLARE @T_AZ UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'AZ Alkmaar');
    DECLARE @T_Legia UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Legia Warsaw');
    DECLARE @T_Samsunspor UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Samsunspor');
    DECLARE @T_Celje UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Celje');
    DECLARE @T_AEKAthens UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'AEK Athens');
    DECLARE @T_Rakow UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Raków Częstochowa');
    DECLARE @T_Craiova UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Universitatea Craiova');
    DECLARE @T_Shelbourne UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Shelbourne');
    DECLARE @T_Hacken UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'BK Häcken');
    DECLARE @T_SlovanB UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Slovan Bratislava');
    DECLARE @T_Strasbourg UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE Name = 'Strasbourg');

    DECLARE @RoundId1 UNIQUEIDENTIFIER, @RoundId2 UNIQUEIDENTIFIER, @RoundId3 UNIQUEIDENTIFIER,
            @RoundId4 UNIQUEIDENTIFIER, @RoundId5 UNIQUEIDENTIFIER, @RoundId6 UNIQUEIDENTIFIER;

    MERGE dbo.LeagueRound AS target
    USING (VALUES (1), (2), (3), (4), (5), (6)) AS source(RoundNum)
    ON target.LeagueId = @LeagueId AND target.SeasonYear = @SeasonYear AND target.Round = source.RoundNum
    WHEN NOT MATCHED THEN
        INSERT (Id, LeagueId, SeasonYear, Round) VALUES (NEWID(), @LeagueId, @SeasonYear, source.RoundNum);

    SELECT TOP 1 @RoundId1 = Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = @SeasonYear AND Round = 1;
    SELECT TOP 1 @RoundId2 = Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = @SeasonYear AND Round = 2;
    SELECT TOP 1 @RoundId3 = Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = @SeasonYear AND Round = 3;
    SELECT TOP 1 @RoundId4 = Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = @SeasonYear AND Round = 4;
    SELECT TOP 1 @RoundId5 = Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = @SeasonYear AND Round = 5;
    SELECT TOP 1 @RoundId6 = Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = @SeasonYear AND Round = 6;

    IF NOT EXISTS (SELECT 1 FROM dbo.MatchRound WHERE RoundId IN (@RoundId1, @RoundId2, @RoundId3, @RoundId4, @RoundId5, @RoundId6))
    BEGIN
        INSERT INTO dbo.MatchRound (Id, RoundId, HomeTeamId, AwayTeamId, HomeGoals, AwayGoals, IsDraw, IsPlayed)
        VALUES
        -- Matchday 1 (Played)
        (NEWID(), @RoundId1, @T_DynamoK, @T_CrysPal, 0, 2, 0, 1),
        (NEWID(), @RoundId1, @T_Lausanne, @T_Breidablik, 3, 0, 0, 1),
        (NEWID(), @RoundId1, @T_Noah, @T_Rijeka, 1, 0, 0, 1),
        (NEWID(), @RoundId1, @T_Zrinjski, @T_Lincoln, 5, 0, 0, 1),
        (NEWID(), @RoundId1, @T_Jagiellonia, @T_Hamrun, 1, 0, 0, 1),
        (NEWID(), @RoundId1, @T_Lech, @T_Rapid, 4, 1, 0, 1),
        (NEWID(), @RoundId1, @T_KuPS, @T_Drita, 1, 1, 1, 1),
        (NEWID(), @RoundId1, @T_Omonia, @T_Mainz, 0, 1, 0, 1),
        (NEWID(), @RoundId1, @T_Rayo, @T_Shkendija, 2, 0, 0, 1),
        (NEWID(), @RoundId1, @T_Aberdeen, @T_Shakhtar, 2, 3, 0, 1),
        (NEWID(), @RoundId1, @T_SpartaP, @T_Shamrock, 4, 1, 0, 1),
        (NEWID(), @RoundId1, @T_Fiorentina, @T_Sigma, 2, 0, 0, 1),
        (NEWID(), @RoundId1, @T_AEKLarnaca, @T_AZ, 4, 0, 0, 1),
        (NEWID(), @RoundId1, @T_Legia, @T_Samsunspor, 0, 1, 0, 1),
        (NEWID(), @RoundId1, @T_Celje, @T_AEKAthens, 3, 1, 0, 1),
        (NEWID(), @RoundId1, @T_Rakow, @T_Craiova, 2, 0, 0, 1),
        (NEWID(), @RoundId1, @T_Shelbourne, @T_Hacken, 0, 0, 1, 1),
        (NEWID(), @RoundId1, @T_SlovanB, @T_Strasbourg, 1, 2, 0, 1),

        -- Matchday 2 (Played)
        (NEWID(), @RoundId2, @T_AEKAthens, @T_Aberdeen, 6, 0, 0, 1),
        (NEWID(), @RoundId2, @T_Hacken, @T_Rayo, 2, 2, 1, 1),
        (NEWID(), @RoundId2, @T_Breidablik, @T_KuPS, 0, 0, 1, 1),
        (NEWID(), @RoundId2, @T_Shakhtar, @T_Legia, 1, 2, 0, 1),
        (NEWID(), @RoundId2, @T_Drita, @T_Omonia, 1, 1, 1, 1),
        (NEWID(), @RoundId2, @T_Rijeka, @T_SpartaP, 1, 0, 0, 1),
        (NEWID(), @RoundId2, @T_Shkendija, @T_Shelbourne, 1, 0, 0, 1),
        (NEWID(), @RoundId2, @T_Strasbourg, @T_Jagiellonia, 1, 1, 1, 1),
        (NEWID(), @RoundId2, @T_Rapid, @T_Fiorentina, 0, 3, 0, 1),
        (NEWID(), @RoundId2, @T_AZ, @T_SlovanB, 1, 0, 0, 1),
        (NEWID(), @RoundId2, @T_Mainz, @T_Zrinjski, 1, 0, 0, 1),
        (NEWID(), @RoundId2, @T_CrysPal, @T_AEKLarnaca, 0, 1, 0, 1),
        (NEWID(), @RoundId2, @T_Hamrun, @T_Lausanne, 0, 1, 0, 1),
        (NEWID(), @RoundId2, @T_Lincoln, @T_Lech, 2, 1, 0, 1),
        (NEWID(), @RoundId2, @T_Samsunspor, @T_DynamoK, 3, 0, 0, 1),
        (NEWID(), @RoundId2, @T_Shamrock, @T_Celje, 0, 2, 0, 1),
        (NEWID(), @RoundId2, @T_Sigma, @T_Rakow, 1, 1, 1, 1),
        (NEWID(), @RoundId2, @T_Craiova, @T_Noah, 1, 1, 1, 1),

        -- Matchday 3 (Played)
        (NEWID(), @RoundId3, @T_Mainz, @T_Fiorentina, 2, 1, 0, 1),
        (NEWID(), @RoundId3, @T_SpartaP, @T_Rakow, 0, 0, 1, 1),
        (NEWID(), @RoundId3, @T_AEKAthens, @T_Shamrock, 1, 1, 1, 1),
        (NEWID(), @RoundId3, @T_AEKLarnaca, @T_Aberdeen, 0, 0, 1, 1),
        (NEWID(), @RoundId3, @T_Shakhtar, @T_Breidablik, 2, 0, 0, 1),
        (NEWID(), @RoundId3, @T_Noah, @T_Sigma, 1, 2, 0, 1),
        (NEWID(), @RoundId3, @T_KuPS, @T_SlovanB, 3, 1, 0, 1),
        (NEWID(), @RoundId3, @T_Celje, @T_Legia, 2, 1, 0, 1),
        (NEWID(), @RoundId3, @T_Samsunspor, @T_Hamrun, 3, 0, 0, 1),
        (NEWID(), @RoundId3, @T_Hacken, @T_Strasbourg, 1, 2, 0, 1),
        (NEWID(), @RoundId3, @T_CrysPal, @T_AZ, 3, 1, 0, 1),
        (NEWID(), @RoundId3, @T_Lausanne, @T_Omonia, 1, 1, 1, 1),
        (NEWID(), @RoundId3, @T_DynamoK, @T_Zrinjski, 6, 0, 0, 1),
        (NEWID(), @RoundId3, @T_Shkendija, @T_Jagiellonia, 1, 1, 1, 1),
        (NEWID(), @RoundId3, @T_Lincoln, @T_Rijeka, 1, 1, 1, 1),
        (NEWID(), @RoundId3, @T_Rayo, @T_Lech, 3, 2, 0, 1),
        (NEWID(), @RoundId3, @T_Shelbourne, @T_Drita, 0, 1, 0, 1),
        (NEWID(), @RoundId3, @T_Rapid, @T_Craiova, 0, 1, 0, 1),

        -- Matchday 4 (Played)
        (NEWID(), @RoundId4, @T_AZ, @T_Shelbourne, 2, 0, 0, 1),
        (NEWID(), @RoundId4, @T_Hamrun, @T_Lincoln, 3, 1, 0, 1),
        (NEWID(), @RoundId4, @T_Zrinjski, @T_Hacken, 2, 1, 0, 1),
        (NEWID(), @RoundId4, @T_Lech, @T_Lausanne, 2, 0, 0, 1),
        (NEWID(), @RoundId4, @T_Omonia, @T_DynamoK, 2, 0, 0, 1),
        (NEWID(), @RoundId4, @T_Rakow, @T_Rapid, 4, 1, 0, 1),
        (NEWID(), @RoundId4, @T_Sigma, @T_Celje, 2, 1, 0, 1),
        (NEWID(), @RoundId4, @T_Craiova, @T_Mainz, 1, 0, 0, 1),
        (NEWID(), @RoundId4, @T_SlovanB, @T_Rayo, 2, 1, 0, 1),
        (NEWID(), @RoundId4, @T_Aberdeen, @T_Noah, 1, 1, 1, 1),
        (NEWID(), @RoundId4, @T_Fiorentina, @T_AEKAthens, 0, 1, 0, 1),
        (NEWID(), @RoundId4, @T_Breidablik, @T_Samsunspor, 2, 2, 1, 1),
        (NEWID(), @RoundId4, @T_Drita, @T_Shkendija, 1, 0, 0, 1),
        (NEWID(), @RoundId4, @T_Rijeka, @T_AEKLarnaca, 0, 0, 1, 1),
        (NEWID(), @RoundId4, @T_Jagiellonia, @T_KuPS, 1, 0, 0, 1),
        (NEWID(), @RoundId4, @T_Legia, @T_SpartaP, 0, 1, 0, 1),
        (NEWID(), @RoundId4, @T_Strasbourg, @T_CrysPal, 2, 1, 0, 1),
        (NEWID(), @RoundId4, @T_Shamrock, @T_Shakhtar, 1, 2, 0, 1),

        -- Matchday 5 (Not Played)
        (NEWID(), @RoundId5, @T_Fiorentina, @T_DynamoK, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Hacken, @T_AEKLarnaca, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Breidablik, @T_Shamrock, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Drita, @T_AZ, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Noah, @T_Legia, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Jagiellonia, @T_Rayo, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Shkendija, @T_SlovanB, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Samsunspor, @T_AEKAthens, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Craiova, @T_SpartaP, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Aberdeen, @T_Strasbourg, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Hamrun, @T_Shakhtar, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Rijeka, @T_Celje, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Lech, @T_Mainz, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_KuPS, @T_Lausanne, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Lincoln, @T_Sigma, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Rakow, @T_Zrinjski, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Shelbourne, @T_CrysPal, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId5, @T_Rapid, @T_Omonia, NULL, NULL, NULL, 0),

        -- Matchday 6 (Not Played)
        (NEWID(), @RoundId6, @T_Mainz, @T_Samsunspor, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_SpartaP, @T_Aberdeen, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_AEKAthens, @T_Craiova, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_AEKLarnaca, @T_Shkendija, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_AZ, @T_Jagiellonia, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_CrysPal, @T_KuPS, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_Shakhtar, @T_Rijeka, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_DynamoK, @T_Noah, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_Lausanne, @T_Fiorentina, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_Zrinjski, @T_Rapid, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_Legia, @T_Lincoln, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_Celje, @T_Shelbourne, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_Omonia, @T_Rakow, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_Strasbourg, @T_Breidablik, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_Rayo, @T_Drita, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_Shamrock, @T_Hamrun, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_Sigma, @T_Lech, NULL, NULL, NULL, 0),
        (NEWID(), @RoundId6, @T_SlovanB, @T_Hacken, NULL, NULL, NULL, 0);
    END
    
    COMMIT TRANSACTION;
    PRINT '✅ Sukces! Dodano wyniki i terminarz dla UEFA Conference League 2025/2026.';

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT '❌ Błąd podczas wstawiania danych meczowych:';
    PRINT ERROR_MESSAGE();
END CATCH;
GO
