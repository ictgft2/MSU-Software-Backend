CREATE OR ALTER PROCEDURE dbo.usp_Consultation_Insert
    @Id uniqueidentifier, @EncounterId uniqueidentifier, @DoctorId uniqueidentifier, @Diagnosis nvarchar(max), @ClinicalNotes nvarchar(max),
    @RequiresLab bit, @RequiresDressing bit, @IsReferral bit, @ReferralFacility nvarchar(250) = NULL, @ReferralReason nvarchar(1000) = NULL,
    @ConsultedAt datetimeoffset
AS
BEGIN
    INSERT dbo.ConsultationNotes VALUES (@Id, @EncounterId, @DoctorId, @Diagnosis, @ClinicalNotes, @RequiresLab, @RequiresDressing, @IsReferral, @ReferralFacility, @ReferralReason, @ConsultedAt);
END
