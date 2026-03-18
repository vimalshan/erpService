using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Stored Procedure: usp_PostGLEntry ──────────────────────────────────
            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE dbo.usp_PostGLEntry
    @p_AccountCode  VARCHAR(10),
    @p_DebitAmount  DECIMAL(19,0),
    @p_CreditAmount DECIMAL(19,0),
    @p_ReferenceID  BIGINT,
    @p_PostingDate  DATETIME2(3),
    @p_Remarks      VARCHAR(200),
    @p_PostedBy     BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF @p_DebitAmount <= 0 AND @p_CreditAmount <= 0
            THROW 50001, 'Either debit or credit amount must be greater than zero', 1;

        INSERT INTO dbo.GL_POSTING (
            ACCOUNT_CODE, POSTING_DATE, DEBIT_AMOUNT,
            CREDIT_AMOUNT, REFERENCE_ID, POSTING_REMARKS
        ) VALUES (
            @p_AccountCode, @p_PostingDate, @p_DebitAmount,
            @p_CreditAmount, @p_ReferenceID, @p_Remarks
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
");

            // ── View: vw_GLTrialBalance ────────────────────────────────────────────
            migrationBuilder.Sql(@"
CREATE OR ALTER VIEW dbo.vw_GLTrialBalance AS
SELECT
    gp.ACCOUNT_CODE,
    ma.MAIN_ACCOUNT_NAME,
    SUM(ISNULL(gp.DEBIT_AMOUNT, 0))                                     AS TotalDebit,
    SUM(ISNULL(gp.CREDIT_AMOUNT, 0))                                    AS TotalCredit,
    SUM(ISNULL(gp.DEBIT_AMOUNT, 0)) - SUM(ISNULL(gp.CREDIT_AMOUNT, 0)) AS Balance
FROM dbo.GL_POSTING gp
LEFT JOIN dbo.MAINACCOUNT_MASTER ma ON gp.ACCOUNT_CODE = ma.MAIN_ACCOUNT_CODE
GROUP BY gp.ACCOUNT_CODE, ma.MAIN_ACCOUNT_NAME;
");

            // ── View: vw_TransactionJournal ───────────────────────────────────────
            migrationBuilder.Sql(@"
CREATE OR ALTER VIEW dbo.vw_TransactionJournal AS
SELECT
    t.TRANSACTION_ID,
    t.TD_TRANSACTION_CODE,
    tm.TRANSACTION_NAME,
    t.TD_TRANSACTION_DATE,
    t.TD_MEMBER_NO,
    t.TD_AMOUNT,
    t.TD_TYPE_CODE,
    t.TD_REMARKS
FROM dbo.TRAN_DET t
LEFT JOIN dbo.TRANSACTION_MASTER tm ON t.TD_TRANSACTION_CODE = tm.TRANSACTION_CODE
WHERE t.TD_CANCEL_STATUS IS NULL;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.vw_TransactionJournal;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.vw_GLTrialBalance;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_PostGLEntry;");
        }
    }
}
