using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EGPS.Application.Helpers
{
    public static class ModelStateExtension
    {
        public static Dictionary<string, string[]> Error(this ModelStateDictionary modelState)
        {
            var errorList = modelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                                   );
            return errorList;
        }

        public static Dictionary<string, string[]> FilterError(this Dictionary<string, string[]> errorDictionary)
        {
            var errors = new Dictionary<string, string[]>();
            foreach (var error in errorDictionary)
            {
                if (error.Value != null && error.Value.Length > 0)
                {
                    errors.Add(error.Key, error.Value);
                }
            }

            return errors;
        }
    }
}
