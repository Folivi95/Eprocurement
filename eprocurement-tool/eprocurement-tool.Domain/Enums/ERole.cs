using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public  enum ERole
    {
        [Description("Vendor")] 
        Vendor =0,
        
        [Description("Procurement Officer")] 
        PROCUREMENTOFFICER = 1,

        [Description("Permanent Secretary")] 
        PERMANENTSECRETARY = 2,

        [Description("Commissioner")] 
        COMMISSIONER = 3,

        [Description("Accountant")] 
        ACCOUNTANT = 4,

        [Description("Audit/store personnel")] 
        AUDITSTOREPERSONNEL = 5,

        [Description("Bureau of Public Procurement")]
        BUREAUOFPUBLICPROCUREMENT = 6,
        
        [Description("Executive")]
        EXECUTIVE = 7
    }
}
