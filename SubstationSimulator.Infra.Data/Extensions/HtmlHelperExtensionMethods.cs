using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Infra.Data.Extensions
{
    public static class HtmlHelperExtensionMethods
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string[] controllers, string action = null)
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeAction = routeData.Values["action"].ToString();
            var routeController = routeData.Values["controller"].ToString();

            foreach (var controller in controllers)
            {
                if (string.IsNullOrEmpty(action))
                {
                    if (controller == routeController)
                    {
                        return "nav-link active";
                    }
                }
                else
                {
                    if (controller == routeController && action == routeAction)
                    {
                        return "nav-link active";
                    }
                }
            }

            return "nav-link";
        }

        public static string IsOpen(this IHtmlHelper htmlHelper, string[] controllers)
        {
            var routeData = htmlHelper.ViewContext.RouteData;
            var routeController = routeData.Values["controller"].ToString();

            foreach (var controller in controllers)
            {
                if (controller == routeController)
                {
                    return "nav-item has-treeview menu-open";
                }
            }

            return "nav-item has-treeview";
        }
    }
}
