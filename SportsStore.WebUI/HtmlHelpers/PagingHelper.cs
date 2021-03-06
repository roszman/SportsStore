﻿using System;
using System.Text;
using System.Web.Mvc;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.HtmlHelpers
{
    public static class PagingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            var result = new StringBuilder();
            for (var i = 1; i <= pagingInfo.TotalPages; i++)
            {
                var tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                // ReSharper disable SpecifyACultureInStringConversionExplicitly
                tag.InnerHtml = i.ToString();
                // ReSharper restore SpecifyACultureInStringConversionExplicitly
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                }
                result.Append(tag);
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}