using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EGPS.Application.Models;
using EGPS.Domain.Entities;

namespace EGPS.Application.Interfaces
{
    public interface ICommentRepository: IRepository<Comment>
    {
        Task<PagedList<Comment>> GetComments(Commentparameters parameters);
    }
}
