using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Helpers
{
    public static class CustomToken
    {
        public static string GenerateToken()
        {
            //return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return Guid.NewGuid().ToString("N");
        }
        
    }
}
