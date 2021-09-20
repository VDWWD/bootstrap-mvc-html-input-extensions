using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace DemoWebsite
{
    public static partial class HtmlHelperExtensions
    {
        public static MvcHtmlString BootstrapRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object value, object htmlAttributes = null)
        {
            var attributes = (IDictionary<string, object>)new RouteValueDictionary(FixHtmlAttributes(htmlAttributes));

            //get the data from the model binding
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            //add a class if there is none
            if (!attributes.Any(x => x.Key.ToLower() == "class"))
            {
                attributes.Add("class", BootstrapHelper.CheckRadioInput);
            }
            else
            {
                attributes["class"] = $"{BootstrapHelper.CheckRadioInput} {attributes["class"]}";
            }

            //the id of the control
            string radioId = fieldId + value.ToString();

            //add the id 
            attributes.Add("id", radioId);

            //get the normal checkbox
            var checkbox = htmlHelper.RadioButtonFor(expression, value, attributes);

            //the checkbox needs to be wrapped with a div
            var outerDiv = new TagBuilder("div");
            outerDiv.Attributes.Add("class", string.Format(BootstrapHelper.CheckRadioControl, "radio"));

            //and must contain an inner label to display properly
            var innerLabel = new TagBuilder("label");
            innerLabel.Attributes.Add("class", BootstrapHelper.CheckRadioLabel);
            innerLabel.Attributes.Add("for", radioId);

            //render the control
            var sb = new StringBuilder();
            sb.AppendLine(outerDiv.ToString(TagRenderMode.StartTag));
            sb.AppendLine(checkbox.ToHtmlString());
            sb.AppendLine(innerLabel.ToString(TagRenderMode.StartTag));
            sb.Append(value.ToString().Replace("_", " "));
            sb.AppendLine(innerLabel.ToString(TagRenderMode.EndTag));
            sb.AppendLine(outerDiv.ToString(TagRenderMode.EndTag));

            //return the newly created control as html
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}