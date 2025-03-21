using MedicalUnitSystem.Helpers.Enums;

namespace MedicalUnitSystem.DTOs.Requests
{
    public class GetPaginatedDataRequestDto
    {
        public string searchTerm { get; set; }
        public string? sortColumn { get; set; } 
        public SortOrder sortOrder { get; set; }
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
}
