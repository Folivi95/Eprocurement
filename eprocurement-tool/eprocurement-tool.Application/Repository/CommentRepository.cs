using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EGPS.Application.Repository
{
    public class CommentRepository: Repository<Comment>, ICommentRepository
    {
        public CommentRepository(EDMSDBContext context)
            : base(context) { }

        public async Task<PagedList<Comment>> GetComments(Commentparameters parameters)
        {
            var query = _context.Comments.Where(x => x.ParentId == null);

            query = query.Include(x => x.comments)
                         .Include(x => x.CreatedBy);

            if (!string.IsNullOrEmpty(parameters.Type) && Enum.IsDefined(typeof(CommentType), parameters.Type.ToUpper()))
            {
                var type = parameters.Type.ParseStringToEnum(typeof(CommentType));
                query = query.Where(x => x.Type == (CommentType)type);
            }

            if(parameters.ObjectId != null)
            {
                query = query.Where(x => x.ObjectId == parameters.ObjectId);
            }

            query = query.OrderByDescending(x => x.CreateAt);

            var comments = await PagedList<Comment>.Create(query, parameters.PageNumber, parameters.PageSize);

            return comments;
        }
    }
}
