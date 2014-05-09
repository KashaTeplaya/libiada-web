﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace LibiadaWeb.Helpers
{
    public static class CheckBoxListHelper
    {
        public static List<MvcHtmlString> CheckBoxList(this HtmlHelper helper, string name,
                                                       IEnumerable<SelectListItem> listInfo)
        {
            return helper.CheckBoxList(name, listInfo, null);
        }

        public static List<MvcHtmlString> CheckBoxList(this HtmlHelper helper, string name,
                                                       IEnumerable<SelectListItem> listInfo, object htmlAttributes)
        {
            return helper.CheckBoxList(name, listInfo, new RouteValueDictionary(htmlAttributes));
        }

        public static List<MvcHtmlString> CheckBoxList(this HtmlHelper helper, string name,
                                                       IEnumerable<SelectListItem> listInfo,
                                                       IDictionary<string, object> htmlAttributes)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("The argument must have a value", "name");
            if (listInfo == null)
                throw new ArgumentNullException("listInfo");

            List<MvcHtmlString> result = new List<MvcHtmlString>();

            foreach (SelectListItem info in listInfo)
            {
                result.Add(helper.InputElement(info, name, "checkbox", htmlAttributes));
            }

            return result;
        } 
    }
}