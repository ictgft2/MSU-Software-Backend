CREATE OR REPLACE FUNCTION public.usp_ContactTrace_Insert(
    uuid, uuid, uuid,
    varchar(200), varchar(40), varchar(100), varchar(500), varchar(500), text, varchar(250), timestamptz)
RETURNS SETOF public.ContactTraces
LANGUAGE sql
VOLATILE
AS $function$
    INSERT INTO public.ContactTraces (
        Id, EncounterId, RecordedBy,
        NextOfKinName, NextOfKinPhone, NextOfKinRelationship,
        ResidentialAddress, WorkplaceAddress, DischargeNotes, ReferralDestination, RecordedAt)
    VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11)
    RETURNING *;
$function$;
