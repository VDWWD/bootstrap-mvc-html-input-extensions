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
        public static MvcHtmlString BootstrapSwitchFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression)
        {
            return BootstrapSwitchFor(htmlHelper, expression, null, false);
        }
        public static MvcHtmlString BootstrapSwitchFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, bool hide_label = false)
        {
            return BootstrapSwitchFor(htmlHelper, expression, null, hide_label);
        }
        public static MvcHtmlString BootstrapSwitchFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, object htmlAttributes = null, bool hide_label = false)
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
                attributes.Add("class", BootstrapHelper.SwitchInput);
            }
            else
            {
                attributes["class"] = $"{BootstrapHelper.SwitchInput} {attributes["class"]}";
            }

            //get the normal checkbox
            string checkbox = htmlHelper.CheckBoxFor(expression, attributes).ToHtmlString();

            //get the label text
            string labelText = metadata.DisplayName ?? metadata.PropertyName;

            //the switch needs to be wrapped with a div
            var outerDiv = new TagBuilder("div");
            outerDiv.Attributes.Add("class", string.Format(BootstrapHelper.SwitchControl, "switch"));

            //and must contain an inner label to display properly
            var innerLabel = new TagBuilder("label");
            innerLabel.Attributes.Add("class", BootstrapHelper.SwitchLabel);
            innerLabel.Attributes.Add("for", fieldId);

            //the switch needs content in the label to work properly
            if (hide_label)
            {
                labelText = "&nbsp;";
            }

            //render the control
            var sb = new StringBuilder();
            sb.AppendLine(outerDiv.ToString(TagRenderMode.StartTag));
            sb.AppendLine(checkbox);
            sb.AppendLine(innerLabel.ToString(TagRenderMode.StartTag));
            sb.Append(labelText);
            sb.AppendLine(innerLabel.ToString(TagRenderMode.EndTag));
            sb.AppendLine(outerDiv.ToString(TagRenderMode.EndTag));

            //return the newly created control as html
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}