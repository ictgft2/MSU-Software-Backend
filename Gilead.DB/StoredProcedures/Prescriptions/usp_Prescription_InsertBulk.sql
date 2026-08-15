CREATE OR REPLACE FUNCTION public.usp_Prescription_InsertBulk(jsonb)
RETURNS integer
LANGUAGE sql
VOLATILE
AS $function$
    WITH inserted AS (
        INSERT INTO public.Prescriptions (
            Id, ConsultationNoteId, EncounterId, DrugName, Dosage, Frequency, Duration, Route, Instructions, Status, IssuedAt)
        SELECT
            item."Id", item."ConsultationNoteId", item."EncounterId", item."DrugName", item."Dosage",
            item."Frequency", item."Duration", item."Route", item."Instructions", item."Status", item."IssuedAt"
        FROM jsonb_to_recordset($1) AS item(
            "Id" uuid,
            "ConsultationNoteId" uuid,
            "EncounterId" uuid,
            "DrugName" varchar(200),
            "Dosage" varchar(100),
            "Frequency" varchar(100),
            "Duration" varchar(100),
            "Route" varchar(30),
            "Instructions" varchar(1000),
            "Status" varchar(30),
            "IssuedAt" timestamptz)
        RETURNING 1
    )
    SELECT count(*)::integer FROM inserted;
$function$;
