-- =============================================
-- Sp_GetOverviewFinancialStatus
-- Returns counts and percentages of invoices grouped by Status.
-- (Rewritten: original file had multiple syntax errors; the SP was
-- not referenced in code but is now valid SQL for completeness.)
-- =============================================
CREATE PROCEDURE [dbo].[Sp_GetOverviewFinancialStatus]
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        ;WITH StatusData AS
        (
            SELECT
                COALESCE(NULLIF(LTRIM(RTRIM(i.Status)), ''), 'Unknown') AS FinancialStatus,
                COUNT(*) AS FinancialCount
            FROM dbo.Invoices i
            WHERE i.IsActive = 1
            GROUP BY COALESCE(NULLIF(LTRIM(RTRIM(i.Status)), ''), 'Unknown')
        ),
        TotalCount AS
        (
            SELECT CAST(SUM(FinancialCount) AS FLOAT) AS Total FROM StatusData
        ),
        Template AS
        (
                       SELECT 'Pending'        AS StatusName, 1 AS SortOrder
            UNION ALL  SELECT 'Overdue'        AS StatusName, 2 AS SortOrder
            UNION ALL  SELECT 'Paid'           AS StatusName, 3 AS SortOrder
            UNION ALL  SELECT 'Partially Paid' AS StatusName, 4 AS SortOrder
        )
        SELECT
            CAST(1 AS BIT) AS isSuccess,
            'Successfully retrieved the data.' AS message,
            (
                SELECT
                    t.StatusName                                                                                          AS financialStatus,
                    COALESCE(s.FinancialCount, 0)                                                                          AS financialCount,
                    CASE
                        WHEN tc.Total > 0 AND s.FinancialCount IS NOT NULL
                        THEN ROUND((CAST(s.FinancialCount AS FLOAT) / tc.Total) * 100.0, 1)
                        ELSE 0.0
                    END                                                                                                    AS financialPercentage
                FROM Template t
                CROSS JOIN TotalCount tc
                LEFT JOIN StatusData s ON s.FinancialStatus = t.StatusName
                ORDER BY t.SortOrder
                FOR JSON PATH
            ) AS data,
            CAST(NULL AS NVARCHAR(50)) AS errorCode
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;
    END TRY
    BEGIN CATCH
        SELECT
            CAST(0 AS BIT)                AS isSuccess,
            ERROR_MESSAGE()               AS message,
            'DATABASE_ERROR'              AS errorCode,
            JSON_QUERY('[]')              AS data
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;
    END CATCH
END
