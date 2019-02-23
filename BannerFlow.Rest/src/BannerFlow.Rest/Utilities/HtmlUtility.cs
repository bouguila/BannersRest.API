using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BannerFlow.Rest.Utilities
{
    public static class HtmlUtility
    {
        public static bool IsValid(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            if (string.IsNullOrEmpty(html) || htmlDoc.ParseErrors.Any())
            {
                return false;
            }
            return true;
        }
    }
}
