using EGPS.Domain.Enums;
using System;
using System.ComponentModel;
using System.Reflection;
using EGPS.Application.Models;
using System.Collections.Generic;

namespace EGPS.Application.Helpers
{
    public static class EnumExtension
    {
		public static string GetDescription(this Enum value)
		{
			FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
			if (fieldInfo == null) return null;
			var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
			return attribute.Description;
		}

		public static int ParseStringToEnum(this string str, Type value)
		{
			var isValid = Enum.TryParse(value, str, true, out var result);

			if(isValid)
			{
				return (int)result;
			}

			throw new ArgumentException($"{str} must be an enumerated type");
		}

		public static List<RoleResponse> GetRoles()
		{
			var result   = new List<RoleResponse>();
			var enumType = typeof(ERole);

			foreach (var roleName in Enum.GetNames(enumType))
			{
				var member = enumType.GetMember(roleName);
				
				var displayAttribute = member[0].GetCustomAttribute<DescriptionAttribute>();

				var role = (ERole) Enum.Parse(enumType, roleName, false);

				result.Add(new RoleResponse {Id = (int) role, Title = displayAttribute.Description});
			}
			result.RemoveAt(0);
			return result;
		}
	}

    public class EnumConverstion
    {
	    public static string GetStatus(EMilestoneStatus status)
	    {
		    try
		    {
			    return status.ToString();
		    }
		    catch (Exception e)
		    {
			    Console.WriteLine(e);

		    }

		    return "";
	    }
    }
}
