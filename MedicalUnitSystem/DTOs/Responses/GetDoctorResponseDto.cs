﻿namespace MedicalUnitSystem.DTOs.Responses
{
    public class GetDoctorResponseDto
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }
        public string? Phone { get; set; }
    }
}
