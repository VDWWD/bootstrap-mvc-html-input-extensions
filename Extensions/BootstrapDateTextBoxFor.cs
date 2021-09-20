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
        public static MvcHtmlString BootstrapDateTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var attributes = (IDictionary<string, object>)new RouteValueDictionary(FixHtmlAttributes(htmlAttributes));

            //add a class if there is none
            if (!attributes.Any(x => x.Key.ToLower() == "class"))
            {
                attributes.Add("class", BootstrapHelper.DatePicker);
            }
            else
            {
                attributes["class"] =  $"{BootstrapHelper.DatePicker} {attributes["class"]}";
            }

            //add the correct textbox type and some other properties
            attributes.Add("placeholder", BootstrapHelper.DatePickerDateFormat);
            attributes.Add("maxlength", "10");
            attributes.Add("autocomplete", "off");

            return htmlHelper.TextBoxFor(expression, attributes);
        }
    }
}