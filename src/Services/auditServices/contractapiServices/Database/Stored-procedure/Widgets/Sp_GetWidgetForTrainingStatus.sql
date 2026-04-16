CREATE PROCEDURE [dbo].[Sp_GetWidgetForTrainingStatus]
    @userId NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT COALESCE(t.TrainingName, '')  AS trainingName,
               COALESCE(ut.Status, '')       AS trainingStatus,
               CONVERT(NVARCHAR(10), COALESCE(ut.DueDate, t.DueDate), 23) AS trainingDueDate,
               ''                           AS trainingLocation
        FROM   UserTrainings ut
        INNER JOIN Trainings t ON ut.TrainingId = t.TrainingId
        WHERE  (
               @userId IS NULL
            OR ut.UserId = TRY_CAST(@userId AS INT)
        )
        ORDER BY ut.DueDate;
    END TRY
    BEGIN CATCH
        SELECT '' AS trainingName, '' AS trainingStatus, NULL AS trainingDueDate, '' AS trainingLocation;
    END CATCH
END


