using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkOrderService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── fn_GetTaskCompletionPercentage ──────────────────────────
            migrationBuilder.Sql("""
                CREATE FUNCTION dbo.fn_GetTaskCompletionPercentage (@p_WorkOrderID BIGINT)
                RETURNS INT
                AS BEGIN
                    DECLARE @Percentage INT = 0;
                    DECLARE @Total INT = (SELECT COUNT(*) FROM dbo.WORK_TASK WHERE WORK_ORDER_ID = @p_WorkOrderID);
                    IF @Total > 0
                    BEGIN
                        DECLARE @Completed INT = (SELECT COUNT(*) FROM dbo.WORK_TASK WHERE WORK_ORDER_ID = @p_WorkOrderID AND TASK_STATUS = 'C');
                        SET @Percentage = CAST((@Completed * 100.0) / @Total AS INT);
                    END
                    RETURN @Percentage;
                END;
                """);

            // ── usp_CreateWorkOrder ─────────────────────────────────────
            migrationBuilder.Sql("""
                CREATE PROCEDURE dbo.usp_CreateWorkOrder
                    @p_WorkOrderName VARCHAR(200),
                    @p_Description VARCHAR(500),
                    @p_DueDate DATE,
                    @p_AssignedTo BIGINT,
                    @p_CreatedBy BIGINT,
                    @p_WorkOrderID BIGINT OUTPUT
                AS BEGIN
                    SET NOCOUNT ON;
                    BEGIN TRY
                        BEGIN TRANSACTION;
                        INSERT INTO dbo.WORK_ORDER (
                            WORK_ORDER_NAME, WORK_ORDER_DESCRIPTION, DUE_DATE,
                            ASSIGNED_TO, WORK_ORDER_STATUS, CREATED_BY, CREATED_ON
                        )
                        VALUES (
                            @p_WorkOrderName, @p_Description, @p_DueDate,
                            @p_AssignedTo, 'O', @p_CreatedBy, GETDATE()
                        );
                        SET @p_WorkOrderID = SCOPE_IDENTITY();
                        COMMIT TRANSACTION;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION;
                        THROW;
                    END CATCH
                END;
                """);

            // ── usp_AssignTaskToWorkOrder ────────────────────────────────
            migrationBuilder.Sql("""
                CREATE PROCEDURE dbo.usp_AssignTaskToWorkOrder
                    @p_WorkOrderID BIGINT,
                    @p_TaskName VARCHAR(100),
                    @p_AssignedTo BIGINT,
                    @p_EstimatedHours INT,
                    @p_CreatedBy BIGINT,
                    @p_TaskID BIGINT OUTPUT
                AS BEGIN
                    SET NOCOUNT ON;
                    BEGIN TRY
                        BEGIN TRANSACTION;
                        INSERT INTO dbo.WORK_TASK (
                            WORK_ORDER_ID, TASK_NAME, ASSIGNED_TO,
                            ESTIMATED_HOURS, TASK_STATUS, CREATED_BY, CREATED_ON
                        )
                        VALUES (
                            @p_WorkOrderID, @p_TaskName, @p_AssignedTo,
                            @p_EstimatedHours, 'O', @p_CreatedBy, GETDATE()
                        );
                        SET @p_TaskID = SCOPE_IDENTITY();
                        COMMIT TRANSACTION;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION;
                        THROW;
                    END CATCH
                END;
                """);

            // ── usp_CompleteTask ─────────────────────────────────────────
            migrationBuilder.Sql("""
                CREATE PROCEDURE dbo.usp_CompleteTask
                    @p_TaskID BIGINT,
                    @p_ActualHours INT,
                    @p_CompletionRemarks VARCHAR(500),
                    @p_CompletedBy BIGINT
                AS BEGIN
                    SET NOCOUNT ON;
                    BEGIN TRY
                        BEGIN TRANSACTION;
                        UPDATE dbo.WORK_TASK
                        SET
                            TASK_STATUS = 'C',
                            ACTUAL_HOURS = @p_ActualHours,
                            COMPLETION_REMARKS = @p_CompletionRemarks,
                            COMPLETED_BY = @p_CompletedBy,
                            COMPLETED_ON = GETDATE(),
                            UPDATED_BY = @p_CompletedBy,
                            UPDATED_ON = GETDATE()
                        WHERE TASK_ID = @p_TaskID;
                        COMMIT TRANSACTION;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION;
                        THROW;
                    END CATCH
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_CompleteTask;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_AssignTaskToWorkOrder;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_CreateWorkOrder;");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS dbo.fn_GetTaskCompletionPercentage;");
        }
    }
}
