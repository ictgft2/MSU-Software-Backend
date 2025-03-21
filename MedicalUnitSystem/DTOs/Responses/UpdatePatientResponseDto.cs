﻿namespace MedicalUnitSystem.DTOs.Responses
{
    public class UpdatePatientResponseDto
    {
        public int PatientId { get; set; }
        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }
        public string? Phone { get; set; }
    }
}
