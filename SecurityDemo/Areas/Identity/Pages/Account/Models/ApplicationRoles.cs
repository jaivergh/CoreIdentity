using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityDemo.Areas.Identity.Pages.Account.Models
{
	public static class ApplicationRoles
	{
		public const string Admin = "admin";
	}

	public static class ApplicationPolicy
	{
		public const string BigCheese = "BigCheese";
	}

	public static class ApplicationClaims
	{
		public const string FavoriteCheese = "FavoriteCheese";
	}
}
