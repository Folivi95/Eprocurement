using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Models;
using EGPS.Domain.Enums;

namespace EGPS.Application.Helpers
{
    public static class ProcurmentGroupExtension
    {
        public static int GetCount(this List<ProcurementGroup> groups, string category, EProcurementPlanStatus status)
        {
            int count = 0;
            foreach (var group in groups)
            {
                if (group.Category.ToUpper() == category.ToUpper() && group.Status == status)
                {
                    count = group.Count;
                    break;
                }
            }

            return count;
        }

        public static int GetTotalCount(this List<ProcurementGroup> groups, string category)
        {
            int count = 0;
            foreach (var group in groups)
            {
                if (group.Category.ToUpper() == category.ToUpper())
                {
                    count += group.Count;
                }
            }

            return count;
        }
    }
}
