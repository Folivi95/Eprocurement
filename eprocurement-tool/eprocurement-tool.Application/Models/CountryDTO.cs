using EGPS.Application.Common;

namespace EGPS.Application.Models
{
    public class CountryDTO : AuditableModelDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
