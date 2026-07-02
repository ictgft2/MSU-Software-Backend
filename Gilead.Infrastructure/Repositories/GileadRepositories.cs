using System.Data;
using Dapper;
using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using Gilead.Domain.Entities;
using Gilead.Domain.Enums;
using Gilead.Infrastructure.Data;

namespace Gilead.Infrastructure.Repositories;

internal static class Db
{
    public static DynamicParameters Params(params (string Name, object? Value)[] values)
    {
        var parameters = new DynamicParameters();
        foreach (var (name, value) in values)
            parameters.Add(name, value);
        return parameters;
    }

    public static string? S<T>(T? value) where T : struct, Enum => value?.ToString();
    public static DateTime? D(DateOnly? value) => value?.ToDateTime(TimeOnly.MinValue);
    public static TimeSpan T(TimeOnly value) => value.ToTimeSpan();
}

public sealed class PatientRepository(SqlConnectionFactory factory) : IPatientRepository
{
    public async Task<Patient> InsertAsync(Patient patient, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleAsync<Patient>("usp_Patient_Insert", patient, commandType: CommandType.StoredProcedure);
    }

    public async Task<Patient?> GetByIdAsync(Guid patientId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<Patient>("usp_Patient_GetById", Db.Params(("PatientId", patientId)), commandType: CommandType.StoredProcedure);
    }

    public async Task<IReadOnlyList<Patient>> SearchAsync(string? name, string? phone, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<Patient>("usp_Patient_Search", Db.Params(("Name", name), ("Phone", phone)), commandType: CommandType.StoredProcedure);
        return rows.ToArray();
    }
}

public sealed class EncounterRepository(SqlConnectionFactory factory) : IEncounterRepository
{
    public async Task<Encounter> InsertAsync(Encounter encounter, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleAsync<Encounter>("usp_Encounter_Insert", ToParameters(encounter), commandType: CommandType.StoredProcedure);
    }

    public async Task<Encounter?> GetByIdAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<Encounter>("usp_Encounter_GetById", Db.Params(("EncounterId", encounterId)), commandType: CommandType.StoredProcedure);
    }

    public async Task<IReadOnlyList<Encounter>> GetListAsync(EncounterStatus? status, DateOnly? date, AdmissionType? type, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<Encounter>("usp_Encounter_GetList", Db.Params(("Status", Db.S(status)), ("Date", Db.D(date)), ("AdmissionType", Db.S(type))), commandType: CommandType.StoredProcedure);
        return rows.ToArray();
    }

    public async Task UpdateStatusAsync(Guid encounterId, EncounterStatus status, DateTimeOffset? dischargedAt, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync("usp_Encounter_UpdateStatus", Db.Params(("EncounterId", encounterId), ("Status", status.ToString()), ("DischargedAt", dischargedAt)), commandType: CommandType.StoredProcedure);
    }

    public async Task<EncounterDetail?> GetDetailAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        using var multi = await connection.QueryMultipleAsync("usp_Encounter_GetById", Db.Params(("EncounterId", encounterId)), commandType: CommandType.StoredProcedure);
        var encounter = await multi.ReadSingleOrDefaultAsync<Encounter>();
        if (encounter is null)
            return null;
        var patient = await multi.ReadSingleOrDefaultAsync<Patient>();
        var vitals = (await multi.ReadAsync<VitalSigns>()).ToArray();
        var consultation = await multi.ReadSingleOrDefaultAsync<ConsultationNote>();
        var prescriptions = (await multi.ReadAsync<Prescription>()).ToArray();
        var labRequests = (await multi.ReadAsync<LabRequest>()).ToArray();
        var labResults = (await multi.ReadAsync<LabResult>()).ToArray();
        var dressing = (await multi.ReadAsync<DressingOrder>()).ToArray();
        var contactTrace = await multi.ReadSingleOrDefaultAsync<ContactTrace>();
        return new EncounterDetail(encounter, patient, vitals, consultation, prescriptions, labRequests, labResults, dressing, contactTrace);
    }

    private static DynamicParameters ToParameters(Encounter encounter) => Db.Params(
        ("Id", encounter.Id),
        ("PatientId", encounter.PatientId),
        ("AdmissionType", encounter.AdmissionType.ToString()),
        ("Status", encounter.Status.ToString()),
        ("ArrivalMode", encounter.ArrivalMode.ToString()),
        ("ChiefComplaint", encounter.ChiefComplaint),
        ("RegisteredBy", encounter.RegisteredBy),
        ("AdmittedAt", encounter.AdmittedAt),
        ("DischargedAt", encounter.DischargedAt),
        ("CreatedAt", encounter.CreatedAt),
        ("UpdatedAt", encounter.UpdatedAt));
}

public sealed class VitalsRepository(SqlConnectionFactory factory) : IVitalsRepository
{
    public async Task<VitalSigns> InsertAsync(VitalSigns vitalSigns, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleAsync<VitalSigns>("usp_VitalSigns_Insert", vitalSigns, commandType: CommandType.StoredProcedure);
    }

    public async Task<IReadOnlyList<VitalSigns>> GetByEncounterAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        return (await connection.QueryAsync<VitalSigns>("usp_VitalSigns_GetByEncounter", Db.Params(("EncounterId", encounterId)), commandType: CommandType.StoredProcedure)).ToArray();
    }

    public async Task<VitalSigns?> GetLatestAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<VitalSigns>("usp_VitalSigns_GetLatest", Db.Params(("EncounterId", encounterId)), commandType: CommandType.StoredProcedure);
    }
}

public sealed class ConsultationRepository(SqlConnectionFactory factory) : IConsultationRepository
{
    public async Task<ConsultationNote?> GetByEncounterAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<ConsultationNote>("usp_Consultation_GetByEncounter", Db.Params(("EncounterId", encounterId)), commandType: CommandType.StoredProcedure);
    }

    public async Task CreateWithChildrenAsync(ConsultationNote note, IReadOnlyList<Prescription> prescriptions, IReadOnlyList<LabRequest> labRequests, DressingOrder? dressingOrder, EncounterStatus nextStatus, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            await connection.ExecuteAsync("usp_Consultation_Insert", note, transaction, commandType: CommandType.StoredProcedure);
            if (prescriptions.Count > 0)
                await connection.ExecuteAsync("usp_Prescription_InsertBulk", Bulk("Prescriptions", PrescriptionTable(prescriptions), "dbo.PrescriptionTvp"), transaction, commandType: CommandType.StoredProcedure);
            if (labRequests.Count > 0)
                await connection.ExecuteAsync("usp_LabRequest_InsertBulk", Bulk("LabRequests", LabRequestTable(labRequests), "dbo.LabRequestTvp"), transaction, commandType: CommandType.StoredProcedure);
            if (dressingOrder is not null)
                await connection.ExecuteAsync("usp_DressingOrder_Insert", dressingOrder, transaction, commandType: CommandType.StoredProcedure);
            await connection.ExecuteAsync("usp_Encounter_UpdateStatus", Db.Params(("EncounterId", note.EncounterId), ("Status", nextStatus.ToString()), ("DischargedAt", nextStatus == EncounterStatus.Referred ? DateTimeOffset.UtcNow : null)), transaction, commandType: CommandType.StoredProcedure);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static DynamicParameters Bulk(string parameterName, DataTable table, string typeName)
    {
        var parameters = new DynamicParameters();
        parameters.Add(parameterName, table.AsTableValuedParameter(typeName));
        return parameters;
    }

    private static DataTable PrescriptionTable(IEnumerable<Prescription> rows)
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(Guid));
        table.Columns.Add("ConsultationNoteId", typeof(Guid));
        table.Columns.Add("EncounterId", typeof(Guid));
        table.Columns.Add("DrugName", typeof(string));
        table.Columns.Add("Dosage", typeof(string));
        table.Columns.Add("Frequency", typeof(string));
        table.Columns.Add("Duration", typeof(string));
        table.Columns.Add("Route", typeof(string));
        table.Columns.Add("Instructions", typeof(string));
        table.Columns.Add("Status", typeof(string));
        table.Columns.Add("IssuedAt", typeof(DateTimeOffset));

        foreach (var r in rows)
            table.Rows.Add(r.Id, r.ConsultationNoteId, r.EncounterId, r.DrugName, r.Dosage, r.Frequency, r.Duration, r.Route.ToString(), r.Instructions ?? (object)DBNull.Value, r.Status.ToString(), r.IssuedAt);
        return table;
    }

    private static DataTable LabRequestTable(IEnumerable<LabRequest> rows)
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(Guid));
        table.Columns.Add("ConsultationNoteId", typeof(Guid));
        table.Columns.Add("EncounterId", typeof(Guid));
        table.Columns.Add("TestName", typeof(string));
        table.Columns.Add("ClinicalIndication", typeof(string));
        table.Columns.Add("Status", typeof(string));
        table.Columns.Add("RequestedAt", typeof(DateTimeOffset));

        foreach (var r in rows)
            table.Rows.Add(r.Id, r.ConsultationNoteId, r.EncounterId, r.TestName, r.ClinicalIndication, r.Status.ToString(), r.RequestedAt);
        return table;
    }
}

public sealed class PrescriptionRepository(SqlConnectionFactory factory) : IPrescriptionRepository
{
    public async Task<Prescription?> GetByIdAsync(Guid id, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return await c.QuerySingleOrDefaultAsync<Prescription>("usp_Prescription_GetById", Db.Params(("Id", id)), commandType: CommandType.StoredProcedure); }
    public async Task<IReadOnlyList<Prescription>> GetWorklistAsync(PrescriptionStatus? status, DateOnly? date, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return (await c.QueryAsync<Prescription>("usp_Prescription_GetWorklist", Db.Params(("Status", Db.S(status)), ("Date", Db.D(date))), commandType: CommandType.StoredProcedure)).ToArray(); }
    public async Task UpdateStatusAsync(Guid id, PrescriptionStatus status, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); await c.ExecuteAsync("usp_Prescription_UpdateStatus", Db.Params(("Id", id), ("Status", status.ToString())), commandType: CommandType.StoredProcedure); }
    public async Task<bool> AllHandedOverForEncounterAsync(Guid encounterId, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return await c.QuerySingleAsync<bool>("usp_Prescription_AllHandedOverForEncounter", Db.Params(("EncounterId", encounterId)), commandType: CommandType.StoredProcedure); }
}

public sealed class DispensingRepository(SqlConnectionFactory factory) : IDispensingRepository
{
    public async Task<Dispensing> InsertAsync(Dispensing dispensing, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return await c.QuerySingleAsync<Dispensing>("usp_Dispensing_Insert", dispensing, commandType: CommandType.StoredProcedure); }
    public async Task<Dispensing?> GetByIdAsync(Guid id, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return await c.QuerySingleOrDefaultAsync<Dispensing>("usp_Dispensing_GetById", Db.Params(("Id", id)), commandType: CommandType.StoredProcedure); }
}

public sealed class DrugHandoverRepository(SqlConnectionFactory factory) : IDrugHandoverRepository
{
    public async Task<DrugHandover> InsertAsync(DrugHandover handover, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return await c.QuerySingleAsync<DrugHandover>("usp_DrugHandover_Insert", handover, commandType: CommandType.StoredProcedure); }
    public async Task<IReadOnlyList<DrugHandover>> GetWorklistAsync(string? status, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return (await c.QueryAsync<DrugHandover>("usp_DrugHandover_GetWorklist", Db.Params(("Status", status)), commandType: CommandType.StoredProcedure)).ToArray(); }
    public async Task<DrugHandover?> GetByIdAsync(Guid id, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return await c.QuerySingleOrDefaultAsync<DrugHandover>("usp_DrugHandover_GetById", Db.Params(("Id", id)), commandType: CommandType.StoredProcedure); }
    public async Task ConfirmAsync(DrugHandover handover, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); await c.ExecuteAsync("usp_DrugHandover_Confirm", handover, commandType: CommandType.StoredProcedure); }
}

public sealed class LabRepository(SqlConnectionFactory factory) : ILabRepository
{
    public async Task<IReadOnlyList<LabRequest>> GetRequestsAsync(LabRequestStatus? status, DateOnly? date, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return (await c.QueryAsync<LabRequest>("usp_LabRequest_GetWorklist", Db.Params(("Status", Db.S(status)), ("Date", Db.D(date))), commandType: CommandType.StoredProcedure)).ToArray(); }
    public async Task<LabRequest?> GetRequestAsync(Guid requestId, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return await c.QuerySingleOrDefaultAsync<LabRequest>("usp_LabRequest_GetById", Db.Params(("RequestId", requestId)), commandType: CommandType.StoredProcedure); }
    public async Task<LabResult> InsertResultAsync(LabResult result, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return await c.QuerySingleAsync<LabResult>("usp_LabResult_Insert", result, commandType: CommandType.StoredProcedure); }
    public async Task<IReadOnlyList<LabResult>> GetResultsByEncounterAsync(Guid encounterId, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return (await c.QueryAsync<LabResult>("usp_LabResult_GetByEncounter", Db.Params(("EncounterId", encounterId)), commandType: CommandType.StoredProcedure)).ToArray(); }
}

public sealed class DressingRepository(SqlConnectionFactory factory) : IDressingRepository
{
    public async Task<IReadOnlyList<DressingOrder>> GetWorklistAsync(DressingOrderStatus? status, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return (await c.QueryAsync<DressingOrder>("usp_DressingOrder_GetWorklist", Db.Params(("Status", Db.S(status))), commandType: CommandType.StoredProcedure)).ToArray(); }
    public async Task<DressingOrder?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return await c.QuerySingleOrDefaultAsync<DressingOrder>("usp_DressingOrder_GetById", Db.Params(("OrderId", orderId)), commandType: CommandType.StoredProcedure); }
    public async Task CompleteAsync(Guid orderId, Guid performedBy, string? procedureNotes, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); await c.ExecuteAsync("usp_DressingOrder_Complete", Db.Params(("OrderId", orderId), ("PerformedBy", performedBy), ("ProcedureNotes", procedureNotes)), commandType: CommandType.StoredProcedure); }
}

public sealed class ContactTraceRepository(SqlConnectionFactory factory) : IContactTraceRepository
{
    public async Task<ContactTrace> InsertAsync(ContactTrace contactTrace, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return await c.QuerySingleAsync<ContactTrace>("usp_ContactTrace_Insert", contactTrace, commandType: CommandType.StoredProcedure); }
    public async Task<ContactTrace?> GetByEncounterAsync(Guid encounterId, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return await c.QuerySingleOrDefaultAsync<ContactTrace>("usp_ContactTrace_GetByEncounter", Db.Params(("EncounterId", encounterId)), commandType: CommandType.StoredProcedure); }
    public async Task<ContactTrace> UpdateAsync(ContactTrace contactTrace, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return await c.QuerySingleAsync<ContactTrace>("usp_ContactTrace_Update", contactTrace, commandType: CommandType.StoredProcedure); }
}

public sealed class RegisterRepository(SqlConnectionFactory factory) : IRegisterRepository
{
    public async Task<IReadOnlyList<DrugRegisterEntry>> GetDrugsAsync(DateOnly? date, int page, int limit, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return (await c.QueryAsync<DrugRegisterEntry>("usp_Register_GetDrugs", Db.Params(("Date", Db.D(date)), ("Page", page), ("Limit", limit)), commandType: CommandType.StoredProcedure)).ToArray(); }
    public async Task<IReadOnlyList<DrugRegisterEntry>> ExportDrugsAsync(DateOnly? date, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return (await c.QueryAsync<DrugRegisterEntry>("usp_Register_ExportDrugs", Db.Params(("Date", Db.D(date))), commandType: CommandType.StoredProcedure)).ToArray(); }
}

public sealed class ServiceWindowRepository(SqlConnectionFactory factory) : IServiceWindowRepository
{
    public async Task<ServiceTimeWindow> InsertAsync(ServiceTimeWindow window, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return await c.QuerySingleAsync<ServiceTimeWindow>("usp_ServiceWindow_Insert", Db.Params(("Id", window.Id), ("Date", window.Date.ToDateTime(TimeOnly.MinValue)), ("ColdCaseOpenTime", Db.T(window.ColdCaseOpenTime)), ("ColdCaseCloseTime", Db.T(window.ColdCaseCloseTime)), ("CreatedBy", window.CreatedBy), ("CreatedAt", window.CreatedAt)), commandType: CommandType.StoredProcedure); }
    public async Task<ServiceTimeWindow?> GetCurrentAsync(DateOnly date, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return await c.QuerySingleOrDefaultAsync<ServiceTimeWindow>("usp_ServiceWindow_GetCurrent", Db.Params(("Date", date.ToDateTime(TimeOnly.MinValue))), commandType: CommandType.StoredProcedure); }
    public async Task<ServiceTimeWindow> UpdateAsync(Guid windowId, TimeOnly openTime, TimeOnly closeTime, CancellationToken cancellationToken) { await using var c = await factory.CreateOpenConnectionAsync(cancellationToken); return await c.QuerySingleAsync<ServiceTimeWindow>("usp_ServiceWindow_Update", Db.Params(("WindowId", windowId), ("ColdCaseOpenTime", Db.T(openTime)), ("ColdCaseCloseTime", Db.T(closeTime))), commandType: CommandType.StoredProcedure); }
}
