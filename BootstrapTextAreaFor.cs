using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace DemoWebsite
{
    public static partial class HtmlHelperExtensions
    {
        public static MvcHtmlString BootstrapTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var attributes = (IDictionary<string, object>)new RouteValueDictionary(FixHtmlAttributes(htmlAttributes));

            //get the data from the model binding
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var modelValue = metadata.Model;

            //find the maxlengt from the StringLength attribute
            int maxLength = 250;
            var member = (MemberExpression)expression.Body;

            if (member != null && member.Member != null)
            {
                var stringLength = member.Member.GetCustomAttributes(typeof(StringLengthAttribute), false).FirstOrDefault();

                if (stringLength != null)
                {
                    maxLength = ((StringLengthAttribute)stringLength).MaximumLength;
                }
            }

            //create the textarea
            var textarea = new TagBuilder("textarea");
            textarea.Attributes.Add("name", fullBindingName);
            textarea.Attributes.Add("id", fieldId);
            textarea.Attributes.Add("maxlength", maxLength.ToString());

            //add a class if there is none
            if (!attributes.Any(x => x.Key.ToLower() == "class"))
            {
                textarea.Attributes.Add("class", BootstrapHelper.DefaultClassName);
            }
            else
            {
                textarea.Attributes.Add("class", $"{BootstrapHelper.DefaultClassName} {attributes["class"]}");
            }

            //add disabled
            if (attributes.Any(x => x.Key.ToLower() == "disabled"))
            {
                textarea.Attributes.Add("disabled", "disabled");
            }

            //add the validation
            textarea.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(fieldName));

            //create the outer and inner tags
            var innerSpan = new TagBuilder("span");
            innerSpan.AddCssClass("textarea-remain");
            var remainSpan = new TagBuilder("span");
            remainSpan.AddCssClass("textarea-remain-value");

            //calculate the available length from the StringLength attribute
            int remain = maxLength;
            if (modelValue != null)
            {
                remain = maxLength - modelValue.ToString().Length;
            }

            //render the control
            var sb = new StringBuilder();
            sb.Append(textarea.ToString(TagRenderMode.StartTag));
            sb.Append(modelValue);
            sb.AppendLine(textarea.ToString(TagRenderMode.EndTag));

            sb.AppendLine(innerSpan.ToString(TagRenderMode.StartTag));
            sb.Append(remainSpan.ToString(TagRenderMode.StartTag));
            sb.Append(remain);
            sb.Append(remainSpan.ToString(TagRenderMode.EndTag));
            sb.AppendLine(" characters remaining.");
            sb.AppendLine(innerSpan.ToString(TagRenderMode.EndTag));

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}