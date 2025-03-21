﻿namespace MedicalUnitSystem.DTOs.Responses
{
    public class GetPatientResponseDto
    {
        public int PatientId { get; set; }
        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }
        public string? Phone { get; set; }

        public string MedicalHistory { get; set; }
    }
}
