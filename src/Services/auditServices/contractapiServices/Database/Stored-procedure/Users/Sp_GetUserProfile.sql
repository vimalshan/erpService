CREATE OR ALTER PROCEDURE [dbo].[Sp_GetUserProfile]
    @userId     NVARCHAR(255) = NULL,
    @veracityId NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT TOP 1
               COALESCE(u.FirstName, u.Username, '')  AS firstName,
               COALESCE(u.LastName, '')               AS lastName,
               COALESCE(NULLIF(LTRIM(RTRIM(CONCAT(u.FirstName, ' ', u.LastName))), ''), u.Username, '') AS displayName,
               ''                        AS country,
               ''                        AS countryCode,
               ''                        AS region,
               COALESCE(u.Email, '')     AS email,
               u.Phone                   AS phone,
               'English'                 AS communicationLanguage,
               COALESCE(u.Position, '')  AS jobTitle,
               COALESCE(u.Language, 'en') AS portalLanguage,
               ''                        AS veracityId
        FROM   Users u
        WHERE  u.IsActive = 1
          AND  (
               (@userId IS NOT NULL AND u.UserId = TRY_CAST(@userId AS INT))
            OR (@veracityId IS NOT NULL AND u.Email = @veracityId)
            OR (@userId IS NULL AND @veracityId IS NULL)
          )
        ORDER BY u.UserId;
    END TRY
    BEGIN CATCH
        SELECT '' AS firstName, '' AS lastName, '' AS displayName, '' AS country,
               '' AS countryCode, '' AS region, '' AS email, NULL AS phone,
               'English' AS communicationLanguage, '' AS jobTitle, 'en' AS portalLanguage, '' AS veracityId;
    END CATCH
END