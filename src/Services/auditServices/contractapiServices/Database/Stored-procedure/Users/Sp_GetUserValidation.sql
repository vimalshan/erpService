CREATE PROCEDURE [dbo].[Sp_GetUserValidation]
    @userId     NVARCHAR(255) = NULL,
    @veracityId NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT TOP 1
               CAST(u.IsActive AS BIT)        AS userIsActive,
               ''                           AS termsAcceptanceRedirectUrl,
               NULL                           AS policySubCode,
               CASE WHEN u.Role = 'admin' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS isDnvUser,
               COALESCE(u.Email, '')        AS userEmail,
               ''                           AS veracityId,
               'en'                         AS portalLanguage,
               CASE WHEN u.Role = 'admin' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS isAdmin
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
        SELECT CAST(0 AS BIT) AS userIsActive, '' AS termsAcceptanceRedirectUrl, NULL AS policySubCode,
               CAST(0 AS BIT) AS isDnvUser, '' AS userEmail, '' AS veracityId,
               'en' AS portalLanguage, CAST(0 AS BIT) AS isAdmin;
    END CATCH
END