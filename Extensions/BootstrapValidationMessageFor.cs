using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace DemoWebsite
{
    public static partial class HtmlHelperExtensions
    {
        public static MvcHtmlString BootstrapValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var attributes = (IDictionary<string, object>)new RouteValueDictionary(FixHtmlAttributes(htmlAttributes));

            //get the data from the model binding
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);

            //create the span
            var validator = new TagBuilder("span");
            validator.MergeAttribute("data-valmsg-replace", "true");
            validator.MergeAttribute("data-valmsg-for", fieldId);

            //add a class if there is none
            if (!attributes.Any(x => x.Key.ToLower() == "class"))
            {
                validator.MergeAttribute("class", BootstrapHelper.Validator);
            }
            else
            {
                validator.MergeAttribute("class", $"{BootstrapHelper.Validator} {attributes["class"]}");
            }

            //render the control
            var sb = new StringBuilder();
            sb.Append(validator.ToString(TagRenderMode.StartTag));
            sb.Append(validator.ToString(TagRenderMode.EndTag));

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}