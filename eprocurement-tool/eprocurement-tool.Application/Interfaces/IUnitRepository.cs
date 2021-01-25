using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EGPS.Application.Models;
using EGPS.Domain.Entities;

namespace EGPS.Application.Interfaces
{
    public interface IUnitRepository: IRepository<Unit>
    {
        Task<PagedList<UnitsDTO>> GetUnits(UnitParameters parameters, Guid accountId);
        Task<bool> UserExistInUnit(Guid userId, Guid unitId);
        Task AddUnitMemeber(Guid userId, Guid unitId);
        void RemoveUserFromUnit(UnitMember unitMember);
        Task<UnitMember> GetUnitMember(Guid unitId, Guid userId);
        Task<PagedList<User>> GetMembers(Guid unitId, UnitMembersParameter parameters);
        Task<int> GetMembersCount(Guid unitId);
    }
}
