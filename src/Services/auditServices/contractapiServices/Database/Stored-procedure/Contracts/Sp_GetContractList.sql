SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE [dbo].[Sp_GetContractList]
    @companyId NVARCHAR(50) = NULL,
    @contractType NVARCHAR(100) = NULL,
    @pageSize INT = 50,
    @pageNumber INT = 1
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DECLARE @offset INT = (@pageNumber - 1) * @pageSize;

        SELECT
            c.ContractId,
            COALESCE(c.ContractName, 'Unknown Contract')                               AS ContractName,
            COALESCE(c.ContractType, 'Unknown')                                        AS ContractType,
            CAST(c.CompanyId AS NVARCHAR(50))                                          AS CompanyId,
            COALESCE(comp.CompanyName, 'Unknown Company')                              AS Company,
            COALESCE(
                STUFF((SELECT DISTINCT ', ' + COALESCE(svc.ServiceName, 'Unknown')
                       FROM ContractServices csvc
                       INNER JOIN Services svc ON csvc.ServiceId = svc.ServiceId
                       WHERE csvc.ContractId = c.ContractId
                       FOR XML PATH(''), TYPE).value('.','NVARCHAR(MAX)'), 1, 2, ''),
                ''
            )                                                                          AS Service,
            COALESCE(
                STUFF((SELECT DISTINCT ', ' + COALESCE(st.SiteName, 'Unknown')
                       FROM ContractSites cst
                       INNER JOIN Sites st ON cst.SiteId = st.SiteId
                       WHERE cst.ContractId = c.ContractId
                       FOR XML PATH(''), TYPE).value('.','NVARCHAR(MAX)'), 1, 2, ''),
                ''
            )                                                                          AS Sites,
            c.CreatedDate                                                              AS DateAdded,
            '10'                                                                       AS CurrentSecurity
        FROM Contracts c
        LEFT JOIN Companies comp ON c.CompanyId = comp.CompanyId
        WHERE c.IsActive = 1
          AND (@companyId     IS NULL OR CAST(c.CompanyId AS NVARCHAR(50)) = @companyId)
          AND (@contractType  IS NULL OR c.ContractType = @contractType)
        ORDER BY c.CreatedDate DESC, c.ContractId DESC
        OFFSET @offset ROWS
        FETCH NEXT @pageSize ROWS ONLY;

    END TRY
    BEGIN CATCH
        SELECT 'Error retrieving contract list: ' + ERROR_MESSAGE() AS errorMessage;
    END CATCH
END

