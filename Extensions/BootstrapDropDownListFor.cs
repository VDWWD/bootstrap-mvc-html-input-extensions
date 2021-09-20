using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace DemoWebsite
{
    public static partial class HtmlHelperExtensions
    {
        public static MvcHtmlString BootstrapDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes = null)
        {
            return BootstrapDropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
        }
        public static MvcHtmlString BootstrapDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes = null)
        {
            var attributes = (IDictionary<string, object>)new RouteValueDictionary(FixHtmlAttributes(htmlAttributes));

            //add a class if there is none
            if (!attributes.Any(x => x.Key.ToLower() == "class"))
            {
                attributes.Add("class", BootstrapHelper.DefaultClassName);
            }
            else
            {
                attributes["class"] = $"{BootstrapHelper.DefaultClassName} {attributes["class"]}";
            }

            return htmlHelper.DropDownListFor(expression, selectList, optionLabel, attributes);
        }
    }
}