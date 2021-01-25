using EGPS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IContractService
    {
        DateTime ConvertDuration(int duration, EDurationType type);

    }
}
