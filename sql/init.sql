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
        MaxRound INT NOT NULL,
        Strength FLOAT NOT NULL
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
        LeagueId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES League(Id),
        LogoUrl NVARCHAR(1024) NULL,
        ShortName NVARCHAR(100) NULL
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

----StatisticsDb Initialization Script----
--DATABASES
IF DB_ID('StatisticsDb') IS NULL
BEGIN
    CREATE DATABASE StatisticsDb;
END
GO

USE StatisticsDb;
GO