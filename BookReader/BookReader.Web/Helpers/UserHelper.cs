using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BookReader.Web.Helpers
{
	public static class UserHelper
	{
		public static Int32 GetCurrentUserId(HttpContext context)
		{
			var id = Int32.Parse(context.User.Claims.Where(x => x.Type == ClaimTypes.Sid).First().Value);
			return id;
		}

		public static string GetCurrentUserRole(HttpContext context)
		{
			var role = context.User.Claims.Where(x => x.Type == ClaimTypes.Role).First().Value;
			return role;
		}
	}
}
