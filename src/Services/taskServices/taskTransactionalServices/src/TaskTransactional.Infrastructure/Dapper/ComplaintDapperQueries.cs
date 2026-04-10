using Dapper;
using TaskTransactional.Application.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TaskTransactional.Infrastructure.Dapper;

public class ComplaintDapperQueries(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task<IEnumerable<ComplaintMainDto>> GetAllComplaintMainsAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<ComplaintMainDto>(
            @"SELECT CM_UNIT_CODE AS CmUnitCode, CM_GROUPID AS CmGroupId, CM_GROUP_NAME AS CmGroupName,
              CM_GROUP_DESC AS CmGroupDesc, CM_GROUP_SRC AS CmGroupSrc, CM_BEHALF_FLG AS CmBehalfFlg,
              CM_BEHALF_PIN AS CmBehalfPin, CM_REG_PIN AS CmRegPin, CM_SHIFT AS CmShift,
              CM_MAIL AS CmMail, CM_SUBMIT AS CmSubmit, CM_REG_DATE AS CmRegDate,
              CM_UPDATEDBY AS CmUpdatedBy, CM_UPDATEDON AS CmUpdatedOn
              FROM COMPL_MAIN");
    }

    public async Task<IEnumerable<ComplaintDetailDto>> GetTicketsByGroupIdAsync(decimal groupId)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<ComplaintDetailDto>(
            @"SELECT CD_TICKET_NUM AS CdTicketNum, CD_GROUPID AS CdGroupId, CD_TYPE AS CdType,
              CD_LOCATION AS CdLocation, CD_DEPARTMENT AS CdDepartment, CD_PROCESS AS CdProcess,
              CD_SUBJECT AS CdSubject, CD_DESCRIPTION AS CdDescription, CD_NCR AS CdNcr,
              CD_PICTUREPATH AS CdPicturePath, CD_FILEPATH AS CdFilePath,
              CD_TARGET_DATE AS CdTargetDate, CD_CLOSURE_DATE AS CdClosureDate
              FROM COMPL_DET WHERE CD_GROUPID = @GroupId",
            new { GroupId = groupId });
    }

    public async Task<IEnumerable<ComplaintHistoryDto>> GetHistoryByActionNumAsync(decimal actionNum)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<ComplaintHistoryDto>(
            @"SELECT CH_HISTORY_NUM AS ChHistoryNum, CH_ACTION_NUM AS ChActionNum, CH_SERIAL_NUM AS ChSerialNum,
              CH_FROM AS ChFrom, CH_TO AS ChTo, CH_ACTION_DATE AS ChActionDate, CH_ACTION_TYPE AS ChActionType,
              CH_REMARKS AS ChRemarks, CH_UPDATEDBY AS ChUpdatedBy, CH_UPDATEDON AS ChUpdatedOn,
              CH_FILEPATH AS ChFilePath
              FROM COMPL_HIST WHERE CH_ACTION_NUM = @ActionNum ORDER BY CH_SERIAL_NUM",
            new { ActionNum = actionNum });
    }

    public async Task<string?> GetComplaintStatusAsync(decimal ticketNum)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<string>(
            "SELECT dbo.fn_GetComplaintStatus(@TicketNum)",
            new { TicketNum = ticketNum });
    }
}
