
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
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Team' AND xtype='U')
BEGIN
    CREATE TABLE dbo.Team
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(255) NOT NULL,
        CityId UNIQUEIDENTIFIER NOT NULL,
        CountryId UNIQUEIDENTIFIER NOT NULL,
        StadiumId UNIQUEIDENTIFIER NOT NULL,
        LeagueId UNIQUEIDENTIFIER NOT NULL,
        LogoUrl NVARCHAR(1024) NOT NULL,
        ShortName NVARCHAR(100) NOT NULL
    );
END