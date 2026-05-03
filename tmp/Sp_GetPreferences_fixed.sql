SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID(N'[dbo].[Sp_GetPreferences]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[Sp_GetPreferences];
GO

CREATE PROCEDURE [dbo].[Sp_GetPreferences]
    @objectType NVARCHAR(50) = NULL,
    @objectName NVARCHAR(50) = NULL,
    @pageName   NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        @pageName   AS PageName,
        @objectType AS ObjectType,
        @objectName AS ObjectName,
        N'{"filters":{},"rowsPerPage":10,"columns":[],"showDefaultColumnsButton":true}' AS PreferenceDetail;
END
GO
