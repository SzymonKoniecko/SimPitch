-----LoggingService Initialization Script----
--DATABASES
IF DB_ID('LoggingDb') IS NULL
BEGIN
    CREATE DATABASE LoggingDb;
END
GO

USE LoggingDb;
GO
--TABLES
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='LogEntry' AND xtype='U')
BEGIN
    CREATE TABLE LogEntry (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        [Timestamp] DATETIME2 NOT NULL,
        [Message] NVARCHAR(MAX),
        [Level] NVARCHAR(50),
        StackTrace NVARCHAR(MAX),
        Source NVARCHAR(255),
        Context NVARCHAR(255)
    );
END
GO
----SportsDataDb Initialization Script----
--DATABASES
IF DB_ID('SportsDataDb') IS NULL
BEGIN
    CREATE DATABASE SportsDataDb;
END
GO

USE SportsDataDb;
GO
--TABLES
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Country' AND xtype='U')
BEGIN
    CREATE TABLE dbo.Country
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(255) NOT NULL,
        [Code] NVARCHAR(255) NOT NULL
    );
END
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='League' AND xtype='U')
BEGIN
    CREATE TABLE dbo.League
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(255) NOT NULL,
        CountryId UNIQUEIDENTIFIER NOT NULL,
        MaxRound INT NOT NULL
    );
END
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='LeagueStrength' AND xtype='U')
BEGIN
    CREATE TABLE dbo.LeagueStrength
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        LeagueId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES League(Id),
        SeasonYear NVARCHAR(255) NOT NULL,
        Strength FLOAT
    );
END
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Stadium' AND xtype='U')
BEGIN
    CREATE TABLE dbo.Stadium
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(255) NOT NULL,
        Capacity INT NOT NULL
    );
END
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Team' AND xtype='U')
BEGIN
    CREATE TABLE dbo.Team
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(255) NOT NULL,
        CountryId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Country(Id),
        StadiumId UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Stadium(Id),
        ShortName NVARCHAR(100) NULL
    );
END
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CompetitionMembership' AND xtype='U')
BEGIN
    CREATE TABLE dbo.CompetitionMembership (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        TeamId UNIQUEIDENTIFIER NOT NULL REFERENCES dbo.Team(Id),
        LeagueId UNIQUEIDENTIFIER NOT NULL REFERENCES dbo.League(Id),
        SeasonYear NVARCHAR(10) NOT NULL,
    );
END
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SeasonStats' AND xtype='U')
BEGIN
    CREATE TABLE dbo.SeasonStats
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        TeamId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Team(Id),
        SeasonYear NVARCHAR(255) NOT NULL,
        LeagueId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES League(Id),
        MatchesPlayed INT NOT NULL,
        Wins INT NOT NULL,
        Losses INT NOT NULL,
        Draws INT NOT NULL,
        GoalsFor INT NOT NULL,
        GoalsAgainst INT NOT NULL
    );
END
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='LeagueRound' AND xtype='U')
BEGIN
    CREATE TABLE dbo.LeagueRound
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        LeagueId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES League(Id),
        SeasonYear NVARCHAR(255) NOT NULL,
        Round INT NOT NULL
    );
END
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='MatchRound' AND xtype='U')
BEGIN
    CREATE TABLE dbo.MatchRound
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        RoundId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES LeagueRound(Id),
        HomeTeamId UNIQUEIDENTIFIER NOT NULL,
        AwayTeamId UNIQUEIDENTIFIER NOT NULL,
        HomeGoals INT NULL,
        AwayGoals INT NULL,
        IsDraw BIT NULL,
        IsPlayed BIT NOT NULL
    );
END

----SimulationDb Initialization Script----
--DATABASES
IF DB_ID('SimulationDb') IS NULL
BEGIN
    CREATE DATABASE SimulationDb;
END
GO

USE SimulationDb;
GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SimulationOverview' AND xtype='U')
BEGIN
    CREATE TABLE dbo.SimulationOverview
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        CreatedDate DATETIME2 NOT NULL,
        SimulationParams NVARCHAR(MAX) NULL,
        LeagueStrengthsJSON NVARCHAR(MAX) NULL,
        PriorLeagueStrength FLOAT NOT NULL,
    );
END
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='IterationResult' AND xtype='U')
BEGIN
    CREATE TABLE dbo.IterationResult
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        SimulationId UNIQUEIDENTIFIER NOT NULL,
        IterationIndex INT NOT NULL,
        StartDate DATETIME2 NOT NULL,
        ExecutionTime TIME NOT NULL,
        TeamStrengths NVARCHAR(MAX) NULL,
        SimulatedMatchRounds NVARCHAR(MAX) NULL,
    );
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SimulationState' AND xtype='U')
BEGIN
    CREATE TABLE dbo.SimulationState
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        SimulationId UNIQUEIDENTIFIER NOT NULL,
        LastCompletedIteration INT NOT NULL,
        ProgressPercent FLOAT NOT NULL,
        [State] NVARCHAR(MAX) NULL,
        UpdatedAt DATETIME2 NOT NULL
    );
END
----StatisticsDb Initialization Script----
--DATABASES
IF DB_ID('StatisticsDb') IS NULL
BEGIN
    CREATE DATABASE StatisticsDb;
END
GO

USE StatisticsDb;
GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Scoreboard' AND xtype='U')
BEGIN
    CREATE TABLE Scoreboard (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        SimulationId UNIQUEIDENTIFIER NOT NULL,
        IterationResultId UNIQUEIDENTIFIER NOT NULL,
        CreatedAt DATETIME2 NOT NULL
    );
END
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ScoreboardTeamStats' AND xtype='U')
BEGIN
    CREATE TABLE ScoreboardTeamStats (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        ScoreboardId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Scoreboard(Id),
        TeamId UNIQUEIDENTIFIER NOT NULL,
        Rank INT NOT NULL,
        Points INT NOT NULL,
        MatchPlayed INT NOT NULL,
        Wins INT NOT NULL,
        Losses INT NOT NULL,
        Draws INT NOT NULL,
        GoalsFor INT NOT NULL,
        GoalsAgainst INT NOT NULL
    );
END
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SimulationTeamStats' AND xtype='U')
BEGIN
    CREATE TABLE SimulationTeamStats(
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        SimulationId UNIQUEIDENTIFIER NOT NULL,
        TeamId UNIQUEIDENTIFIER NOT NULL,
        PositionProbbility NVARCHAR(MAX) NOT NULL,
        AverangePoints FLOAT NOT NULL,
        AverangeWins FLOAT NOT NULL,
        AverangeLosses FLOAT NOT NULL,
        AverangeDraws FLOAT NOT NULL,
        AverangeGoalsFor FLOAT NOT NULL,
        AverangeGoalsAgainst FLOAT NOT NULL,
    );
END
