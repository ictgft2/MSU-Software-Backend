﻿namespace MedicalUnitSystem.DTOs.Responses
{
    public class GetVitalsResponseDto
    {
        public int PatientId { get; set; }
        public int HeartRate { get; set; }
        public int SystolicBloodPressure { get; set; }
        public int DiastolicBloodPressure { get; set; }
        public double Temperature { get; set; }
        public int RespiratoryRate { get; set; }
        public int OxygenSaturation { get; set; }
        public string Notes { get; set; }
    }
}
