using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookReader.Web.Helpers
{
	public static class SelectListHelper
	{
		public static IList<SelectListItem> ToSelectListItem<T>(this List<T> Items,
			Func<T, string> text, Func<T, string> value, string selectedValue = null)
		{
			List<SelectListItem> items = new List<SelectListItem>();

			foreach (var item in Items)
			{
				items.Add(new SelectListItem
				{
					Text = text(item),
					Value = value(item),
					Selected = selectedValue == value(item) ? true : false
				});
			}

			return items
				.OrderBy(l => l.Text)
				.ToList();
		}

	}
}
