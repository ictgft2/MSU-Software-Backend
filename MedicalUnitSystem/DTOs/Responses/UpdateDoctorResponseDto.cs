﻿namespace MedicalUnitSystem.DTOs.Responses
{
    public class UpdateDoctorResponseDto
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
    }
}
