-- ================================================================
-- EKSTRAKLASA 2026/2027 - MATCH ROUNDS (fixtures + results)
-- Description: Inserts MatchRound rows for the 2026/2027 season.
--   Rounds 1-7 mostly played (results as of this seed); some matches
--   within rounds 2-4 were postponed and have no result yet, so they
--   are inserted as unplayed even though they belong to that round.
--   Rounds 8-34 are unplayed fixtures (no result yet).
-- Created: 10.09.2026
-- ================================================================

USE SportsDataDb

DECLARE
    @LeagueId UNIQUEIDENTIFIER,

    @T_Jag  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Jagiellonia Białystok'),
    @T_Leg  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Legia Warsaw'),
    @T_Lech UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Lech Poznań'),
    @T_Wid  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Widzew Łódź'),
    @T_Rak  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Raków Częstochowa'),
    @T_Pog  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Pogoń Szczecin'),
    @T_Cra  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Cracovia'),
    @T_Gor  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Górnik Zabrze'),
    @T_WPl  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Wisła Płock'),
    @T_Rad  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Radomiak Radom'),
    @T_Mot  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Motor Lublin'),
    @T_GKS  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'GKS Katowice'),
    @T_Zag  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Zagłębie Lubin'),
    @T_Kor  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Korona Kielce'),
    @T_Pia  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Piast Gliwice'),
    @T_Sla  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Śląsk Wrocław'),
    @T_WKr  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Wisła Kraków'),
    @T_Wcz  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Team WHERE [Name] = 'Wieczysta Kraków');

SELECT @LeagueId = Id
FROM dbo.League
WHERE [Name] = 'PKO BP Ekstraklasa'

DECLARE
    @R1  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 1),
    @R2  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 2),
    @R3  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 3),
    @R4  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 4),
    @R5  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 5),
    @R6  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 6),
    @R7  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 7),
    @R8  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 8),
    @R9  UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 9),
    @R10 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 10),
    @R11 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 11),
    @R12 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 12),
    @R13 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 13),
    @R14 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 14),
    @R15 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 15),
    @R16 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 16),
    @R17 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 17),
    @R18 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 18),
    @R19 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 19),
    @R20 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 20),
    @R21 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 21),
    @R22 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 22),
    @R23 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 23),
    @R24 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 24),
    @R25 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 25),
    @R26 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 26),
    @R27 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 27),
    @R28 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 28),
    @R29 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 29),
    @R30 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 30),
    @R31 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 31),
    @R32 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 32),
    @R33 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 33),
    @R34 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027' AND Round = 34);

BEGIN TRANSACTION

BEGIN TRY

    IF NOT EXISTS (SELECT 1 FROM SportsDataDb.dbo.MatchRound WHERE RoundId IN (
        SELECT Id FROM dbo.LeagueRound WHERE LeagueId = @LeagueId AND SeasonYear = '2026/2027'
    ))
    INSERT INTO dbo.MatchRound (Id, RoundId, HomeTeamId, AwayTeamId, HomeGoals, AwayGoals, IsDraw, IsPlayed)
        VALUES
        -- Kolejka 1 (25-26 lipca) - rozegrana
        (NEWID(), @R1, @T_Gor, @T_Sla, 2, 1, 0, 1),
        (NEWID(), @R1, @T_Jag, @T_Kor, 1, 0, 0, 1),
        (NEWID(), @R1, @T_Lech, @T_Cra, 0, 0, 1, 1),
        (NEWID(), @R1, @T_Pog, @T_Leg, 0, 1, 0, 1),
        (NEWID(), @R1, @T_Rad, @T_Wcz, 2, 1, 0, 1),
        (NEWID(), @R1, @T_Rak, @T_WPl, 1, 2, 0, 1),
        (NEWID(), @R1, @T_Wid, @T_Mot, 2, 2, 1, 1),
        (NEWID(), @R1, @T_WKr, @T_GKS, 2, 1, 0, 1),
        (NEWID(), @R1, @T_Zag, @T_Pia, 2, 0, 0, 1),

        -- Kolejka 2 (1-2 sierpnia) - Korona-Górnik przełożony na 15.09, bez wyniku
        (NEWID(), @R2, @T_Cra, @T_Pog, 0, 2, 0, 1),
        (NEWID(), @R2, @T_GKS, @T_Rad, 3, 1, 0, 1),
        (NEWID(), @R2, @T_Kor, @T_Gor, NULL, NULL, NULL, 0),
        (NEWID(), @R2, @T_Leg, @T_Zag, 3, 1, 0, 1),
        (NEWID(), @R2, @T_Mot, @T_Jag, 1, 2, 0, 1),
        (NEWID(), @R2, @T_Pia, @T_WKr, 4, 3, 0, 1),
        (NEWID(), @R2, @T_Sla, @T_Rak, 2, 1, 0, 1),
        (NEWID(), @R2, @T_Wcz, @T_Lech, 1, 2, 0, 1),
        (NEWID(), @R2, @T_WPl, @T_Wid, 0, 0, 1, 1),

        -- Kolejka 3 (8-9 sierpnia) - GKS-Wieczysta przełożony na 12.10, Raków-Zagłębie przełożony na 15.09
        (NEWID(), @R3, @T_GKS, @T_Wcz, NULL, NULL, NULL, 0),
        (NEWID(), @R3, @T_Jag, @T_Wid, 0, 2, 0, 1),
        (NEWID(), @R3, @T_Kor, @T_Leg, 1, 1, 1, 1),
        (NEWID(), @R3, @T_Lech, @T_Pia, 3, 0, 0, 1),
        (NEWID(), @R3, @T_Pog, @T_Mot, 3, 1, 0, 1),
        (NEWID(), @R3, @T_Rad, @T_Gor, 1, 3, 0, 1),
        (NEWID(), @R3, @T_Rak, @T_Zag, NULL, NULL, NULL, 0),
        (NEWID(), @R3, @T_Sla, @T_Cra, 0, 0, 1, 1),
        (NEWID(), @R3, @T_WKr, @T_WPl, 2, 1, 0, 1),

        -- Kolejka 4 (15-16 sierpnia) - Jagiellonia-Pogoń przełożony na 16.12, Wisła Płock-Lech przełożony na 17.12
        (NEWID(), @R4, @T_Cra, @T_Rak, 2, 1, 0, 1),
        (NEWID(), @R4, @T_Gor, @T_WKr, 2, 1, 0, 1),
        (NEWID(), @R4, @T_Jag, @T_Pog, NULL, NULL, NULL, 0),
        (NEWID(), @R4, @T_Leg, @T_Rad, 5, 0, 0, 1),
        (NEWID(), @R4, @T_Mot, @T_GKS, 1, 0, 0, 1),
        (NEWID(), @R4, @T_Pia, @T_Wcz, 3, 4, 0, 1),
        (NEWID(), @R4, @T_Wid, @T_Kor, 1, 2, 0, 1),
        (NEWID(), @R4, @T_WPl, @T_Lech, NULL, NULL, NULL, 0),
        (NEWID(), @R4, @T_Zag, @T_Sla, 2, 1, 0, 1),

        -- Kolejka 5 (22-23 sierpnia) - rozegrana
        (NEWID(), @R5, @T_Cra, @T_Wcz, 3, 2, 0, 1),
        (NEWID(), @R5, @T_GKS, @T_WPl, 5, 0, 0, 1),
        (NEWID(), @R5, @T_Kor, @T_Mot, 1, 1, 1, 1),
        (NEWID(), @R5, @T_Lech, @T_Jag, 2, 1, 0, 1),
        (NEWID(), @R5, @T_Pia, @T_Leg, 1, 1, 1, 1),
        (NEWID(), @R5, @T_Pog, @T_WKr, 2, 2, 1, 1),
        (NEWID(), @R5, @T_Rad, @T_Zag, 0, 0, 1, 1),
        (NEWID(), @R5, @T_Rak, @T_Gor, 1, 2, 0, 1),
        (NEWID(), @R5, @T_Sla, @T_Wid, 3, 3, 1, 1),

        -- Kolejka 6 (29-30 sierpnia) - rozegrana
        (NEWID(), @R6, @T_Gor, @T_GKS, 2, 3, 0, 1),
        (NEWID(), @R6, @T_Leg, @T_Sla, 1, 1, 1, 1),
        (NEWID(), @R6, @T_Mot, @T_Pia, 0, 2, 0, 1),
        (NEWID(), @R6, @T_Rad, @T_Cra, 2, 1, 0, 1),
        (NEWID(), @R6, @T_Rak, @T_Jag, 2, 5, 0, 1),
        (NEWID(), @R6, @T_Wid, @T_Lech, 2, 3, 0, 1),
        (NEWID(), @R6, @T_WKr, @T_Wcz, 2, 0, 0, 1),
        (NEWID(), @R6, @T_WPl, @T_Kor, 0, 2, 0, 1),
        (NEWID(), @R6, @T_Zag, @T_Pog, 0, 0, 1, 1),

        -- Kolejka 7 (5-6 września) - rozegrana
        (NEWID(), @R7, @T_Cra, @T_Gor, 0, 1, 0, 1),
        (NEWID(), @R7, @T_Jag, @T_Sla, 2, 1, 0, 1),
        (NEWID(), @R7, @T_Kor, @T_WKr, 1, 1, 1, 1),
        (NEWID(), @R7, @T_Lech, @T_Rak, 1, 0, 0, 1),
        (NEWID(), @R7, @T_Mot, @T_Leg, 2, 3, 0, 1),
        (NEWID(), @R7, @T_Pia, @T_GKS, 3, 0, 0, 1),
        (NEWID(), @R7, @T_Pog, @T_WPl, 1, 1, 1, 1),
        (NEWID(), @R7, @T_Wid, @T_Rad, 0, 0, 1, 1),
        (NEWID(), @R7, @T_Wcz, @T_Zag, 1, 1, 1, 1),

        -- Kolejka 8 (12-13 września) - nierozegrana
        (NEWID(), @R8, @T_Gor, @T_Lech, NULL, NULL, NULL, 0),
        (NEWID(), @R8, @T_Leg, @T_Wid, NULL, NULL, NULL, 0),
        (NEWID(), @R8, @T_Pog, @T_Wcz, NULL, NULL, NULL, 0),
        (NEWID(), @R8, @T_Rad, @T_Pia, NULL, NULL, NULL, 0),
        (NEWID(), @R8, @T_Rak, @T_Mot, NULL, NULL, NULL, 0),
        (NEWID(), @R8, @T_Sla, @T_Kor, NULL, NULL, NULL, 0),
        (NEWID(), @R8, @T_WKr, @T_Jag, NULL, NULL, NULL, 0),
        (NEWID(), @R8, @T_WPl, @T_Cra, NULL, NULL, NULL, 0),
        (NEWID(), @R8, @T_Zag, @T_GKS, NULL, NULL, NULL, 0),

        -- Kolejka 9 (19-20 września) - nierozegrana
        (NEWID(), @R9, @T_GKS, @T_Cra, NULL, NULL, NULL, 0),
        (NEWID(), @R9, @T_Jag, @T_Leg, NULL, NULL, NULL, 0),
        (NEWID(), @R9, @T_Kor, @T_Rak, NULL, NULL, NULL, 0),
        (NEWID(), @R9, @T_Lech, @T_Rad, NULL, NULL, NULL, 0),
        (NEWID(), @R9, @T_Mot, @T_Gor, NULL, NULL, NULL, 0),
        (NEWID(), @R9, @T_Pia, @T_Pog, NULL, NULL, NULL, 0),
        (NEWID(), @R9, @T_Wid, @T_Wcz, NULL, NULL, NULL, 0),
        (NEWID(), @R9, @T_WKr, @T_Sla, NULL, NULL, NULL, 0),
        (NEWID(), @R9, @T_Zag, @T_WPl, NULL, NULL, NULL, 0),

        -- Kolejka 10 (10-11 października) - nierozegrana
        (NEWID(), @R10, @T_Cra, @T_Zag, NULL, NULL, NULL, 0),
        (NEWID(), @R10, @T_Jag, @T_Gor, NULL, NULL, NULL, 0),
        (NEWID(), @R10, @T_Leg, @T_WKr, NULL, NULL, NULL, 0),
        (NEWID(), @R10, @T_Pia, @T_Wid, NULL, NULL, NULL, 0),
        (NEWID(), @R10, @T_Pog, @T_Kor, NULL, NULL, NULL, 0),
        (NEWID(), @R10, @T_Rad, @T_Mot, NULL, NULL, NULL, 0),
        (NEWID(), @R10, @T_Rak, @T_GKS, NULL, NULL, NULL, 0),
        (NEWID(), @R10, @T_Sla, @T_Lech, NULL, NULL, NULL, 0),
        (NEWID(), @R10, @T_Wcz, @T_WPl, NULL, NULL, NULL, 0),

        -- Kolejka 11 (17-18 października) - nierozegrana
        (NEWID(), @R11, @T_Cra, @T_Leg, NULL, NULL, NULL, 0),
        (NEWID(), @R11, @T_GKS, @T_Pog, NULL, NULL, NULL, 0),
        (NEWID(), @R11, @T_Gor, @T_Pia, NULL, NULL, NULL, 0),
        (NEWID(), @R11, @T_Lech, @T_Kor, NULL, NULL, NULL, 0),
        (NEWID(), @R11, @T_Mot, @T_Sla, NULL, NULL, NULL, 0),
        (NEWID(), @R11, @T_Wid, @T_WKr, NULL, NULL, NULL, 0),
        (NEWID(), @R11, @T_Wcz, @T_Rak, NULL, NULL, NULL, 0),
        (NEWID(), @R11, @T_WPl, @T_Rad, NULL, NULL, NULL, 0),
        (NEWID(), @R11, @T_Zag, @T_Jag, NULL, NULL, NULL, 0),

        -- Kolejka 12 (24-25 października) - nierozegrana
        (NEWID(), @R12, @T_Jag, @T_WPl, NULL, NULL, NULL, 0),
        (NEWID(), @R12, @T_Kor, @T_GKS, NULL, NULL, NULL, 0),
        (NEWID(), @R12, @T_Leg, @T_Lech, NULL, NULL, NULL, 0),
        (NEWID(), @R12, @T_Mot, @T_Zag, NULL, NULL, NULL, 0),
        (NEWID(), @R12, @T_Pia, @T_Cra, NULL, NULL, NULL, 0),
        (NEWID(), @R12, @T_Pog, @T_Rad, NULL, NULL, NULL, 0),
        (NEWID(), @R12, @T_Sla, @T_Wcz, NULL, NULL, NULL, 0),
        (NEWID(), @R12, @T_Wid, @T_Gor, NULL, NULL, NULL, 0),
        (NEWID(), @R12, @T_WKr, @T_Rak, NULL, NULL, NULL, 0),

        -- Kolejka 13 (31 października-1 listopada) - nierozegrana
        (NEWID(), @R13, @T_Cra, @T_Mot, NULL, NULL, NULL, 0),
        (NEWID(), @R13, @T_GKS, @T_Wid, NULL, NULL, NULL, 0),
        (NEWID(), @R13, @T_Gor, @T_Leg, NULL, NULL, NULL, 0),
        (NEWID(), @R13, @T_Lech, @T_WKr, NULL, NULL, NULL, 0),
        (NEWID(), @R13, @T_Rad, @T_Sla, NULL, NULL, NULL, 0),
        (NEWID(), @R13, @T_Rak, @T_Pog, NULL, NULL, NULL, 0),
        (NEWID(), @R13, @T_Wcz, @T_Jag, NULL, NULL, NULL, 0),
        (NEWID(), @R13, @T_WPl, @T_Pia, NULL, NULL, NULL, 0),
        (NEWID(), @R13, @T_Zag, @T_Kor, NULL, NULL, NULL, 0),

        -- Kolejka 14 (7-8 listopada) - nierozegrana
        (NEWID(), @R14, @T_Gor, @T_Wcz, NULL, NULL, NULL, 0),
        (NEWID(), @R14, @T_Jag, @T_GKS, NULL, NULL, NULL, 0),
        (NEWID(), @R14, @T_Kor, @T_Rad, NULL, NULL, NULL, 0),
        (NEWID(), @R14, @T_Lech, @T_Zag, NULL, NULL, NULL, 0),
        (NEWID(), @R14, @T_Leg, @T_Rak, NULL, NULL, NULL, 0),
        (NEWID(), @R14, @T_Mot, @T_WPl, NULL, NULL, NULL, 0),
        (NEWID(), @R14, @T_Sla, @T_Pia, NULL, NULL, NULL, 0),
        (NEWID(), @R14, @T_Wid, @T_Pog, NULL, NULL, NULL, 0),
        (NEWID(), @R14, @T_WKr, @T_Cra, NULL, NULL, NULL, 0),

        -- Kolejka 15 (21-22 listopada) - nierozegrana
        (NEWID(), @R15, @T_Cra, @T_Jag, NULL, NULL, NULL, 0),
        (NEWID(), @R15, @T_GKS, @T_Lech, NULL, NULL, NULL, 0),
        (NEWID(), @R15, @T_Pia, @T_Kor, NULL, NULL, NULL, 0),
        (NEWID(), @R15, @T_Pog, @T_Sla, NULL, NULL, NULL, 0),
        (NEWID(), @R15, @T_Rad, @T_WKr, NULL, NULL, NULL, 0),
        (NEWID(), @R15, @T_Rak, @T_Wid, NULL, NULL, NULL, 0),
        (NEWID(), @R15, @T_Wcz, @T_Mot, NULL, NULL, NULL, 0),
        (NEWID(), @R15, @T_WPl, @T_Leg, NULL, NULL, NULL, 0),
        (NEWID(), @R15, @T_Zag, @T_Gor, NULL, NULL, NULL, 0),

        -- Kolejka 16 (28-29 listopada) - nierozegrana
        (NEWID(), @R16, @T_Gor, @T_WPl, NULL, NULL, NULL, 0),
        (NEWID(), @R16, @T_Jag, @T_Rad, NULL, NULL, NULL, 0),
        (NEWID(), @R16, @T_Kor, @T_Cra, NULL, NULL, NULL, 0),
        (NEWID(), @R16, @T_Lech, @T_Pog, NULL, NULL, NULL, 0),
        (NEWID(), @R16, @T_Leg, @T_Wcz, NULL, NULL, NULL, 0),
        (NEWID(), @R16, @T_Rak, @T_Pia, NULL, NULL, NULL, 0),
        (NEWID(), @R16, @T_Sla, @T_GKS, NULL, NULL, NULL, 0),
        (NEWID(), @R16, @T_Wid, @T_Zag, NULL, NULL, NULL, 0),
        (NEWID(), @R16, @T_WKr, @T_Mot, NULL, NULL, NULL, 0),

        -- Kolejka 17 (5-6 grudnia) - nierozegrana
        (NEWID(), @R17, @T_Cra, @T_Wid, NULL, NULL, NULL, 0),
        (NEWID(), @R17, @T_GKS, @T_Leg, NULL, NULL, NULL, 0),
        (NEWID(), @R17, @T_Mot, @T_Lech, NULL, NULL, NULL, 0),
        (NEWID(), @R17, @T_Pia, @T_Jag, NULL, NULL, NULL, 0),
        (NEWID(), @R17, @T_Pog, @T_Gor, NULL, NULL, NULL, 0),
        (NEWID(), @R17, @T_Rad, @T_Rak, NULL, NULL, NULL, 0),
        (NEWID(), @R17, @T_Wcz, @T_Kor, NULL, NULL, NULL, 0),
        (NEWID(), @R17, @T_WPl, @T_Sla, NULL, NULL, NULL, 0),
        (NEWID(), @R17, @T_Zag, @T_WKr, NULL, NULL, NULL, 0),

        -- Kolejka 18 (12-13 grudnia) - nierozegrana
        (NEWID(), @R18, @T_Cra, @T_Lech, NULL, NULL, NULL, 0),
        (NEWID(), @R18, @T_GKS, @T_WKr, NULL, NULL, NULL, 0),
        (NEWID(), @R18, @T_Kor, @T_Jag, NULL, NULL, NULL, 0),
        (NEWID(), @R18, @T_Leg, @T_Pog, NULL, NULL, NULL, 0),
        (NEWID(), @R18, @T_Mot, @T_Wid, NULL, NULL, NULL, 0),
        (NEWID(), @R18, @T_Pia, @T_Zag, NULL, NULL, NULL, 0),
        (NEWID(), @R18, @T_Sla, @T_Gor, NULL, NULL, NULL, 0),
        (NEWID(), @R18, @T_Wcz, @T_Rad, NULL, NULL, NULL, 0),
        (NEWID(), @R18, @T_WPl, @T_Rak, NULL, NULL, NULL, 0),

        -- Kolejka 19 (30-31 stycznia) - nierozegrana
        (NEWID(), @R19, @T_Gor, @T_Kor, NULL, NULL, NULL, 0),
        (NEWID(), @R19, @T_Jag, @T_Mot, NULL, NULL, NULL, 0),
        (NEWID(), @R19, @T_Lech, @T_Wcz, NULL, NULL, NULL, 0),
        (NEWID(), @R19, @T_Pog, @T_Cra, NULL, NULL, NULL, 0),
        (NEWID(), @R19, @T_Rad, @T_GKS, NULL, NULL, NULL, 0),
        (NEWID(), @R19, @T_Rak, @T_Sla, NULL, NULL, NULL, 0),
        (NEWID(), @R19, @T_Wid, @T_WPl, NULL, NULL, NULL, 0),
        (NEWID(), @R19, @T_WKr, @T_Pia, NULL, NULL, NULL, 0),
        (NEWID(), @R19, @T_Zag, @T_Leg, NULL, NULL, NULL, 0),

        -- Kolejka 20 (6-7 lutego) - nierozegrana
        (NEWID(), @R20, @T_Cra, @T_Sla, NULL, NULL, NULL, 0),
        (NEWID(), @R20, @T_Gor, @T_Rad, NULL, NULL, NULL, 0),
        (NEWID(), @R20, @T_Leg, @T_Kor, NULL, NULL, NULL, 0),
        (NEWID(), @R20, @T_Mot, @T_Pog, NULL, NULL, NULL, 0),
        (NEWID(), @R20, @T_Pia, @T_Lech, NULL, NULL, NULL, 0),
        (NEWID(), @R20, @T_Wid, @T_Jag, NULL, NULL, NULL, 0),
        (NEWID(), @R20, @T_Wcz, @T_GKS, NULL, NULL, NULL, 0),
        (NEWID(), @R20, @T_WPl, @T_WKr, NULL, NULL, NULL, 0),
        (NEWID(), @R20, @T_Zag, @T_Rak, NULL, NULL, NULL, 0),

        -- Kolejka 21 (13-14 lutego) - nierozegrana
        (NEWID(), @R21, @T_GKS, @T_Mot, NULL, NULL, NULL, 0),
        (NEWID(), @R21, @T_Kor, @T_Wid, NULL, NULL, NULL, 0),
        (NEWID(), @R21, @T_Lech, @T_WPl, NULL, NULL, NULL, 0),
        (NEWID(), @R21, @T_Pog, @T_Jag, NULL, NULL, NULL, 0),
        (NEWID(), @R21, @T_Rad, @T_Leg, NULL, NULL, NULL, 0),
        (NEWID(), @R21, @T_Rak, @T_Cra, NULL, NULL, NULL, 0),
        (NEWID(), @R21, @T_Sla, @T_Zag, NULL, NULL, NULL, 0),
        (NEWID(), @R21, @T_Wcz, @T_Pia, NULL, NULL, NULL, 0),
        (NEWID(), @R21, @T_WKr, @T_Gor, NULL, NULL, NULL, 0),

        -- Kolejka 22 (20-21 lutego) - nierozegrana
        (NEWID(), @R22, @T_Gor, @T_Rak, NULL, NULL, NULL, 0),
        (NEWID(), @R22, @T_Jag, @T_Lech, NULL, NULL, NULL, 0),
        (NEWID(), @R22, @T_Leg, @T_Pia, NULL, NULL, NULL, 0),
        (NEWID(), @R22, @T_Mot, @T_Kor, NULL, NULL, NULL, 0),
        (NEWID(), @R22, @T_Wid, @T_Sla, NULL, NULL, NULL, 0),
        (NEWID(), @R22, @T_Wcz, @T_Cra, NULL, NULL, NULL, 0),
        (NEWID(), @R22, @T_WKr, @T_Pog, NULL, NULL, NULL, 0),
        (NEWID(), @R22, @T_WPl, @T_GKS, NULL, NULL, NULL, 0),
        (NEWID(), @R22, @T_Zag, @T_Rad, NULL, NULL, NULL, 0),

        -- Kolejka 23 (27-28 lutego) - nierozegrana
        (NEWID(), @R23, @T_Cra, @T_Rad, NULL, NULL, NULL, 0),
        (NEWID(), @R23, @T_GKS, @T_Gor, NULL, NULL, NULL, 0),
        (NEWID(), @R23, @T_Jag, @T_Rak, NULL, NULL, NULL, 0),
        (NEWID(), @R23, @T_Kor, @T_WPl, NULL, NULL, NULL, 0),
        (NEWID(), @R23, @T_Lech, @T_Wid, NULL, NULL, NULL, 0),
        (NEWID(), @R23, @T_Pia, @T_Mot, NULL, NULL, NULL, 0),
        (NEWID(), @R23, @T_Pog, @T_Zag, NULL, NULL, NULL, 0),
        (NEWID(), @R23, @T_Sla, @T_Leg, NULL, NULL, NULL, 0),
        (NEWID(), @R23, @T_Wcz, @T_WKr, NULL, NULL, NULL, 0),

        -- Kolejka 24 (6-7 marca) - nierozegrana
        (NEWID(), @R24, @T_GKS, @T_Pia, NULL, NULL, NULL, 0),
        (NEWID(), @R24, @T_Gor, @T_Cra, NULL, NULL, NULL, 0),
        (NEWID(), @R24, @T_Leg, @T_Mot, NULL, NULL, NULL, 0),
        (NEWID(), @R24, @T_Rad, @T_Wid, NULL, NULL, NULL, 0),
        (NEWID(), @R24, @T_Rak, @T_Lech, NULL, NULL, NULL, 0),
        (NEWID(), @R24, @T_Sla, @T_Jag, NULL, NULL, NULL, 0),
        (NEWID(), @R24, @T_WKr, @T_Kor, NULL, NULL, NULL, 0),
        (NEWID(), @R24, @T_WPl, @T_Pog, NULL, NULL, NULL, 0),
        (NEWID(), @R24, @T_Zag, @T_Wcz, NULL, NULL, NULL, 0),

        -- Kolejka 25 (13-14 marca) - nierozegrana
        (NEWID(), @R25, @T_Cra, @T_WPl, NULL, NULL, NULL, 0),
        (NEWID(), @R25, @T_GKS, @T_Zag, NULL, NULL, NULL, 0),
        (NEWID(), @R25, @T_Jag, @T_WKr, NULL, NULL, NULL, 0),
        (NEWID(), @R25, @T_Kor, @T_Sla, NULL, NULL, NULL, 0),
        (NEWID(), @R25, @T_Lech, @T_Gor, NULL, NULL, NULL, 0),
        (NEWID(), @R25, @T_Mot, @T_Rak, NULL, NULL, NULL, 0),
        (NEWID(), @R25, @T_Pia, @T_Rad, NULL, NULL, NULL, 0),
        (NEWID(), @R25, @T_Wid, @T_Leg, NULL, NULL, NULL, 0),
        (NEWID(), @R25, @T_Wcz, @T_Pog, NULL, NULL, NULL, 0),

        -- Kolejka 26 (20-21 marca) - nierozegrana
        (NEWID(), @R26, @T_Cra, @T_GKS, NULL, NULL, NULL, 0),
        (NEWID(), @R26, @T_Gor, @T_Mot, NULL, NULL, NULL, 0),
        (NEWID(), @R26, @T_Leg, @T_Jag, NULL, NULL, NULL, 0),
        (NEWID(), @R26, @T_Pog, @T_Pia, NULL, NULL, NULL, 0),
        (NEWID(), @R26, @T_Rad, @T_Lech, NULL, NULL, NULL, 0),
        (NEWID(), @R26, @T_Rak, @T_Kor, NULL, NULL, NULL, 0),
        (NEWID(), @R26, @T_Sla, @T_WKr, NULL, NULL, NULL, 0),
        (NEWID(), @R26, @T_Wcz, @T_Wid, NULL, NULL, NULL, 0),
        (NEWID(), @R26, @T_WPl, @T_Zag, NULL, NULL, NULL, 0),

        -- Kolejka 27 (3-4 kwietnia) - nierozegrana
        (NEWID(), @R27, @T_GKS, @T_Rak, NULL, NULL, NULL, 0),
        (NEWID(), @R27, @T_Gor, @T_Jag, NULL, NULL, NULL, 0),
        (NEWID(), @R27, @T_Kor, @T_Pog, NULL, NULL, NULL, 0),
        (NEWID(), @R27, @T_Lech, @T_Sla, NULL, NULL, NULL, 0),
        (NEWID(), @R27, @T_Mot, @T_Rad, NULL, NULL, NULL, 0),
        (NEWID(), @R27, @T_Wid, @T_Pia, NULL, NULL, NULL, 0),
        (NEWID(), @R27, @T_WKr, @T_Leg, NULL, NULL, NULL, 0),
        (NEWID(), @R27, @T_WPl, @T_Wcz, NULL, NULL, NULL, 0),
        (NEWID(), @R27, @T_Zag, @T_Cra, NULL, NULL, NULL, 0),

        -- Kolejka 28 (10-11 kwietnia) - nierozegrana
        (NEWID(), @R28, @T_Jag, @T_Zag, NULL, NULL, NULL, 0),
        (NEWID(), @R28, @T_Kor, @T_Lech, NULL, NULL, NULL, 0),
        (NEWID(), @R28, @T_Leg, @T_Cra, NULL, NULL, NULL, 0),
        (NEWID(), @R28, @T_Pia, @T_Gor, NULL, NULL, NULL, 0),
        (NEWID(), @R28, @T_Pog, @T_GKS, NULL, NULL, NULL, 0),
        (NEWID(), @R28, @T_Rad, @T_WPl, NULL, NULL, NULL, 0),
        (NEWID(), @R28, @T_Rak, @T_Wcz, NULL, NULL, NULL, 0),
        (NEWID(), @R28, @T_Sla, @T_Mot, NULL, NULL, NULL, 0),
        (NEWID(), @R28, @T_WKr, @T_Wid, NULL, NULL, NULL, 0),

        -- Kolejka 29 (17-18 kwietnia) - nierozegrana
        (NEWID(), @R29, @T_Cra, @T_Pia, NULL, NULL, NULL, 0),
        (NEWID(), @R29, @T_GKS, @T_Kor, NULL, NULL, NULL, 0),
        (NEWID(), @R29, @T_Gor, @T_Wid, NULL, NULL, NULL, 0),
        (NEWID(), @R29, @T_Lech, @T_Leg, NULL, NULL, NULL, 0),
        (NEWID(), @R29, @T_Rad, @T_Pog, NULL, NULL, NULL, 0),
        (NEWID(), @R29, @T_Rak, @T_WKr, NULL, NULL, NULL, 0),
        (NEWID(), @R29, @T_Wcz, @T_Sla, NULL, NULL, NULL, 0),
        (NEWID(), @R29, @T_WPl, @T_Jag, NULL, NULL, NULL, 0),
        (NEWID(), @R29, @T_Zag, @T_Mot, NULL, NULL, NULL, 0),

        -- Kolejka 30 (24-25 kwietnia) - nierozegrana
        (NEWID(), @R30, @T_Jag, @T_Wcz, NULL, NULL, NULL, 0),
        (NEWID(), @R30, @T_Kor, @T_Zag, NULL, NULL, NULL, 0),
        (NEWID(), @R30, @T_Leg, @T_Gor, NULL, NULL, NULL, 0),
        (NEWID(), @R30, @T_Mot, @T_Cra, NULL, NULL, NULL, 0),
        (NEWID(), @R30, @T_Pia, @T_WPl, NULL, NULL, NULL, 0),
        (NEWID(), @R30, @T_Pog, @T_Rak, NULL, NULL, NULL, 0),
        (NEWID(), @R30, @T_Sla, @T_Rad, NULL, NULL, NULL, 0),
        (NEWID(), @R30, @T_Wid, @T_GKS, NULL, NULL, NULL, 0),
        (NEWID(), @R30, @T_WKr, @T_Lech, NULL, NULL, NULL, 0),

        -- Kolejka 31 (1-2 maja) - nierozegrana
        (NEWID(), @R31, @T_Cra, @T_WKr, NULL, NULL, NULL, 0),
        (NEWID(), @R31, @T_GKS, @T_Jag, NULL, NULL, NULL, 0),
        (NEWID(), @R31, @T_Pia, @T_Sla, NULL, NULL, NULL, 0),
        (NEWID(), @R31, @T_Pog, @T_Wid, NULL, NULL, NULL, 0),
        (NEWID(), @R31, @T_Rad, @T_Kor, NULL, NULL, NULL, 0),
        (NEWID(), @R31, @T_Rak, @T_Leg, NULL, NULL, NULL, 0),
        (NEWID(), @R31, @T_Wcz, @T_Gor, NULL, NULL, NULL, 0),
        (NEWID(), @R31, @T_WPl, @T_Mot, NULL, NULL, NULL, 0),
        (NEWID(), @R31, @T_Zag, @T_Lech, NULL, NULL, NULL, 0),

        -- Kolejka 32 (8-9 maja) - nierozegrana
        (NEWID(), @R32, @T_Gor, @T_Zag, NULL, NULL, NULL, 0),
        (NEWID(), @R32, @T_Jag, @T_Cra, NULL, NULL, NULL, 0),
        (NEWID(), @R32, @T_Kor, @T_Pia, NULL, NULL, NULL, 0),
        (NEWID(), @R32, @T_Lech, @T_GKS, NULL, NULL, NULL, 0),
        (NEWID(), @R32, @T_Leg, @T_WPl, NULL, NULL, NULL, 0),
        (NEWID(), @R32, @T_Mot, @T_Wcz, NULL, NULL, NULL, 0),
        (NEWID(), @R32, @T_Sla, @T_Pog, NULL, NULL, NULL, 0),
        (NEWID(), @R32, @T_Wid, @T_Rak, NULL, NULL, NULL, 0),
        (NEWID(), @R32, @T_WKr, @T_Rad, NULL, NULL, NULL, 0),

        -- Kolejka 33 (15-16 maja) - nierozegrana
        (NEWID(), @R33, @T_Cra, @T_Kor, NULL, NULL, NULL, 0),
        (NEWID(), @R33, @T_GKS, @T_Sla, NULL, NULL, NULL, 0),
        (NEWID(), @R33, @T_Mot, @T_WKr, NULL, NULL, NULL, 0),
        (NEWID(), @R33, @T_Pia, @T_Rak, NULL, NULL, NULL, 0),
        (NEWID(), @R33, @T_Pog, @T_Lech, NULL, NULL, NULL, 0),
        (NEWID(), @R33, @T_Rad, @T_Jag, NULL, NULL, NULL, 0),
        (NEWID(), @R33, @T_Wcz, @T_Leg, NULL, NULL, NULL, 0),
        (NEWID(), @R33, @T_WPl, @T_Gor, NULL, NULL, NULL, 0),
        (NEWID(), @R33, @T_Zag, @T_Wid, NULL, NULL, NULL, 0),

        -- Kolejka 34 (22-23 maja) - nierozegrana
        (NEWID(), @R34, @T_Gor, @T_Pog, NULL, NULL, NULL, 0),
        (NEWID(), @R34, @T_Jag, @T_Pia, NULL, NULL, NULL, 0),
        (NEWID(), @R34, @T_Kor, @T_Wcz, NULL, NULL, NULL, 0),
        (NEWID(), @R34, @T_Lech, @T_Mot, NULL, NULL, NULL, 0),
        (NEWID(), @R34, @T_Leg, @T_GKS, NULL, NULL, NULL, 0),
        (NEWID(), @R34, @T_Rak, @T_Rad, NULL, NULL, NULL, 0),
        (NEWID(), @R34, @T_Sla, @T_WPl, NULL, NULL, NULL, 0),
        (NEWID(), @R34, @T_Wid, @T_Cra, NULL, NULL, NULL, 0),
        (NEWID(), @R34, @T_WKr, @T_Zag, NULL, NULL, NULL, 0)

    COMMIT TRANSACTION
    PRINT '✅ SUKCES! Ekstraklasa 2026/2027 - fixtures inserted'
    PRINT '   - rounds 1-7: mostly played (a few postponed matches left unplayed)'
    PRINT '   - rounds 8-34: fixtures not yet played'

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT '❌ Cannot insert! Ekstraklasa 2026/2027 fixtures'
    PRINT 'Error: ' + ERROR_MESSAGE()
END CATCH
