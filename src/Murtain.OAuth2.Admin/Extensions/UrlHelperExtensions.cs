using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;

namespace IdentityAdmin.Extensions
{
    internal static class UrlHelperExtensions
    {
        public static string RelativeLink(this IUrlHelper urlHelper, string routeName)
        {
            return urlHelper.RelativeLink(routeName, null);
        }

        public static string RelativeLink(this IUrlHelper urlHelper, string routeName, object routeValues)
        {
            var absoluteUrl = urlHelper.Link(routeName, routeValues);
            var authority = "http://localhost:5000";
            var authorityIndex = absoluteUrl.IndexOf(authority, StringComparison.Ordinal);
            var relativePath = absoluteUrl.Substring(authorityIndex + authority.Length, absoluteUrl.Length - authorityIndex - authority.Length);
            return relativePath;
        }
    }
}
