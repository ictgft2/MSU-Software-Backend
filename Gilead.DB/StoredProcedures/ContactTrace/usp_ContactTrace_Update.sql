CREATE OR REPLACE FUNCTION public.usp_ContactTrace_Update(
    uuid, uuid, uuid,
    varchar(200), varchar(40), varchar(100), varchar(500), varchar(500), text, varchar(250), timestamptz)
RETURNS SETOF public.ContactTraces
LANGUAGE sql
VOLATILE
AS $function$
    UPDATE public.ContactTraces
    SET 
        RecordedBy = $3,
        NextOfKinName = $4,
        NextOfKinPhone = $5,
        NextOfKinRelationship = $6,
        ResidentialAddress = $7,
        WorkplaceAddress = $8,
        DischargeNotes = $9,
        ReferralDestination = $10,
        RecordedAt = $11
    WHERE Id = $1 AND EncounterId = $2
    RETURNING *;
$function$;
