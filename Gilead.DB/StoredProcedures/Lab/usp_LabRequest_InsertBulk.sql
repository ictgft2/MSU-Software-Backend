CREATE OR REPLACE FUNCTION public.usp_LabRequest_InsertBulk(jsonb)
RETURNS integer
LANGUAGE sql
VOLATILE
AS $function$
    WITH inserted AS (
        INSERT INTO public.LabRequests (
            Id, ConsultationNoteId, EncounterId, TestName, ClinicalIndication, Status, RequestedAt)
        SELECT
            item."Id", item."ConsultationNoteId", item."EncounterId", item."TestName",
            item."ClinicalIndication", item."Status", item."RequestedAt"
        FROM jsonb_to_recordset($1) AS item(
            "Id" uuid,
            "ConsultationNoteId" uuid,
            "EncounterId" uuid,
            "TestName" varchar(200),
            "ClinicalIndication" varchar(1000),
            "Status" varchar(30),
            "RequestedAt" timestamptz)
        RETURNING 1
    )
    SELECT count(*)::integer FROM inserted;
$function$;
