-- ================================================================
-- INSERT MISSING COUNTRIES FOR CONFERENCE LEAGUE
-- Created: 07.12.2025
-- ================================================================

USE SportsDataDb;
GO

BEGIN TRANSACTION
BEGIN TRY

    DECLARE @Countries TABLE (
        Name NVARCHAR(100),
        Code NVARCHAR(10) -- ISO Code (2-letter or 3-letter usually)
    );

    INSERT INTO @Countries (Name, Code)
    VALUES
    ('Ukraine', 'UA'),
    ('England', 'EN'),
    ('Switzerland', 'CH'),
    ('Iceland', 'IS'),
    ('Armenia', 'AM'),
    ('Croatia', 'HR'),
    ('Bosnia and Herzegovina', 'BA'),
    ('Gibraltar', 'GI'),
    ('Poland', 'PL'),
    ('Malta', 'MT'),
    ('Austria', 'AT'),
    ('Finland', 'FI'),
    ('Kosovo', 'XK'), -- XK is a common temporary code for Kosovo
    ('Cyprus', 'CY'),
    ('Germany', 'DE'),
    ('Spain', 'ES'),
    ('North Macedonia', 'MK'),
    ('Scotland', 'SCT'), -- Often GB-SCT or just SCT in sports DBs
    ('Czech Republic', 'CZ'),
    ('Republic of Ireland', 'IE'),
    ('Italy', 'IT'),
    ('Netherlands', 'NL'),
    ('Turkey', 'TR'),
    ('Slovenia', 'SI'),
    ('Greece', 'GR'),
    ('Romania', 'RO'),
    ('Sweden', 'SE'),
    ('Slovakia', 'SK'),
    ('France', 'FR');

    INSERT INTO dbo.Country (Id, [Name], [Code])
    SELECT 
        NEWID(),
        src.Name,
        src.Code
    FROM @Countries src
    WHERE NOT EXISTS (
        SELECT 1 
        FROM dbo.Country c 
        WHERE c.Code = src.Code OR c.Name = src.Name
    );

    DECLARE @AddedCount INT = @@ROWCOUNT;

    COMMIT TRANSACTION;
    PRINT '✅ Sukces! Dodano ' + CAST(@AddedCount AS NVARCHAR(10)) + ' nowych krajów.';

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT '❌ Błąd podczas dodawania krajów:';
    PRINT ERROR_MESSAGE();
END CATCH;
GO

