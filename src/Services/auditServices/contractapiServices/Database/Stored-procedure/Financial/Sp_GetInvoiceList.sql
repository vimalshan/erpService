CREATE PROCEDURE [dbo].[Sp_GetInvoiceList]
    @pageSize      INT           = 50,
    @pageNumber    INT           = 1,
    @status        NVARCHAR(50)  = NULL,
    @companyFilter NVARCHAR(255) = NULL,
    @startDate     DATE          = NULL,
    @endDate       DATE          = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @offset INT = (@pageNumber - 1) * @pageSize;

        SELECT CONCAT(FORMAT(i.Amount,'N2'),' ',COALESCE(i.Currency,'USD')) AS amount,
               ''                                                            AS billingAddress,
               COALESCE(c.CompanyName, '')                                   AS company,
               ''                                                            AS contactPerson,
               CONVERT(NVARCHAR(23), i.DueDate, 126)                        AS dueDate,
               COALESCE(i.InvoiceNumber, '')                                AS invoice,
               CONVERT(NVARCHAR(23), i.InvoiceDate, 126)                    AS issueDate,
               NULL                                                         AS originalInvoice,
               CONVERT(NVARCHAR(23), i.PlannedPaymentDate, 126)             AS plannedPaymentDate,
               NULL                                                         AS referenceNumber,
               COALESCE(i.Status, 'Unknown')                               AS status,
               ''                                                           AS reportingCountry,
               ''                                                           AS projectNumber,
               ''                                                           AS accountDNVId
        FROM   Invoices i
        LEFT JOIN Companies c ON i.CompanyId = c.CompanyId
        WHERE  i.IsActive = 1
          AND  (@status        IS NULL OR i.Status LIKE @status)
          AND  (@companyFilter IS NULL OR c.CompanyName LIKE '%' + @companyFilter + '%')
          AND  (@startDate     IS NULL OR i.InvoiceDate >= @startDate)
          AND  (@endDate       IS NULL OR i.InvoiceDate <= @endDate)
        ORDER  BY i.InvoiceDate DESC, i.InvoiceId DESC
        OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;
    END TRY
    BEGIN CATCH
        SELECT '' AS amount, '' AS billingAddress, '' AS company, '' AS contactPerson,
               NULL AS dueDate, '' AS invoice, NULL AS issueDate, NULL AS originalInvoice,
               NULL AS plannedPaymentDate, NULL AS referenceNumber, '' AS status,
               '' AS reportingCountry, '' AS projectNumber, '' AS accountDNVId;
    END CATCH
END