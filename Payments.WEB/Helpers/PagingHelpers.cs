using Payments.WEB.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Payments.WEB.Helpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
        PageInfo pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            if (pageInfo.TotalPages == 1)
            {
                return MvcHtmlString.Create(result.ToString());
            }
            else
            {
                TagBuilder nav = new TagBuilder("nav");
                nav.AddCssClass("p-3");
                //first page button
                TagBuilder first = new TagBuilder("a");
                first.AddCssClass("btn");

                if (pageInfo.PageNumber == 1)
                {
                    first.AddCssClass(" disabled");
                }
                else
                {
                    first.MergeAttribute("href", pageUrl(1));
                }
                TagBuilder firstIcon = new TagBuilder("i");
                firstIcon.AddCssClass("fa fa-angle-double-left");
                firstIcon.MergeAttribute("aria-hidden", "true");
                first.InnerHtml += firstIcon.ToString();
                nav.InnerHtml += first.ToString();

                //previous page button
                TagBuilder prev = new TagBuilder("a");
                prev.AddCssClass("btn");

                if (pageInfo.PageNumber == 1)
                {
                    prev.AddCssClass("disabled");
                }
                else
                {
                    prev.MergeAttribute("href", pageUrl(pageInfo.PageNumber - 1));
                }
                TagBuilder prevIcon = new TagBuilder("i");
                prevIcon.AddCssClass("fa fa-angle-left");
                prevIcon.MergeAttribute("aria-hidden", "true");
                prev.InnerHtml += prevIcon.ToString();
                nav.InnerHtml += prev.ToString();

                // page number buttons
                for (int i = 1; i <= pageInfo.TotalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.MergeAttribute("href", pageUrl(i));
                    tag.InnerHtml = i.ToString();

                    if (i == pageInfo.PageNumber)
                    {
                        tag.AddCssClass("selected");
                        tag.AddCssClass("btn-dark");
                    }
                    tag.AddCssClass("btn");
                    nav.InnerHtml += tag.ToString();
                }

                // next page button
                TagBuilder next = new TagBuilder("a");
                next.AddCssClass("btn");
                if (pageInfo.PageNumber == pageInfo.TotalPages)
                {
                    next.AddCssClass("disabled");
                }
                else
                {
                    next.MergeAttribute("href", pageUrl(pageInfo.PageNumber + 1));

                }
                TagBuilder nextIcon = new TagBuilder("i");
                nextIcon.AddCssClass("fa fa-angle-right");
                nextIcon.MergeAttribute("aria-hidden", "true");
                next.InnerHtml += nextIcon.ToString();
                nav.InnerHtml += next.ToString();

                // last page button
                TagBuilder last = new TagBuilder("a");
                last.AddCssClass("btn");
               
                if (pageInfo.PageNumber == pageInfo.TotalPages)
                {
                    last.AddCssClass(" disabled");
                }
                else
                {
                    last.MergeAttribute("href", pageUrl(pageInfo.TotalPages));
                }
                TagBuilder lastIcon = new TagBuilder("i");
                lastIcon.AddCssClass("fa fa-angle-double-right");
                lastIcon.MergeAttribute("aria-hidden", "true");
                last.InnerHtml += lastIcon.ToString();
                nav.InnerHtml += last.ToString();

                result.Append(nav.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}