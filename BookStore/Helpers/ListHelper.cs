using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Helpers
{
    public static class ListHelper
    {
        public static IHtmlString CreateList(this HtmlHelper html, string[] items)
        {
            TagBuilder ul = new TagBuilder("ul");
			ul.AddCssClass("list-group");
	        StringBuilder ulHtml = new StringBuilder("");
            foreach (string item in items)
            {
                TagBuilder li = new TagBuilder("li");
				li.AddCssClass("list-group-item");
                li.SetInnerText(item);
	            ulHtml.Append(li);
            }
			ul.InnerHtml = ulHtml.ToString();

			return new HtmlString(ul.ToString());
        }
    }
}