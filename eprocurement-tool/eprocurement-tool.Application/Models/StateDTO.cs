using EGPS.Application.Common;

namespace EGPS.Application.Models
{
    public class StateDTO : AuditableModelDTO
    {
        public int Id { get; set; }

        public int CountryId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }
}
