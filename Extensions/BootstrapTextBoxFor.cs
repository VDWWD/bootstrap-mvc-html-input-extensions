using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace DemoWebsite
{
    public static partial class HtmlHelperExtensions
    {
        public static MvcHtmlString BootstrapTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            //get the data from the model binding
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var modelValue = metadata.Model;
            var fieldNameLowered = fieldName.ToLower();

            //create the textbox
            var textbox = new TagBuilder("input");

            //are there attributes
            textbox.MergeAttributes(new RouteValueDictionary(FixHtmlAttributes(htmlAttributes)));

            //add the attributes if they are not already in htmlAttributes
            textbox.Attributes.Add("name", fullBindingName);
            if (!textbox.Attributes.Any(x => x.Key.ToLower() == "value") && modelValue != null)
            {
                textbox.Attributes.Add("value", modelValue.ToString());
            }
            if (!textbox.Attributes.Any(x => x.Key.ToLower() == "id"))
            {
                textbox.Attributes.Add("id", fieldId);
            }
            if (!textbox.Attributes.Any(x => x.Key.ToLower() == "autocomplete"))
            {
                textbox.Attributes.Add("autocomplete", "off");
            }
            if (!textbox.Attributes.Any(x => x.Key.ToLower() == "class"))
            {
                textbox.Attributes.Add("class", BootstrapHelper.DefaultClassName);
            }
            else
            {
                textbox.Attributes["class"] = $"{BootstrapHelper.DefaultClassName} {textbox.Attributes["class"]}";
            }

            //determine the textbox type based on it's name. You can add your own common names to get the correct type
            if (!textbox.Attributes.Any(x => x.Key.ToLower() == "type"))
            {
                string type = "text";

                if (fieldNameLowered.Contains("password"))
                {
                    type = "password";
                }
                else if (fieldNameLowered.Contains("e_mail") || fieldNameLowered.Contains("email"))
                {
                    type = "email";
                }
                else if (fieldNameLowered.Contains("phone") || fieldNameLowered.Contains("mobile") || fieldNameLowered.Contains("number") || fieldNameLowered.Contains("amount"))
                {
                    type = "tel";
                }
                else if (fieldNameLowered.Contains("search"))
                {
                    type = "search";
                }
                else if (fieldNameLowered.Contains("url") || fieldNameLowered.Contains("website"))
                {
                    type = "url";
                }

                textbox.Attributes.Add("type", type);
            }

            //find the maxlengt from the StringLength attribute
            if (!textbox.Attributes.Any(x => x.Key.ToLower() == "maxlength"))
            {
                var member = expression.Body as MemberExpression;
                var stringLength = member.Member.GetCustomAttributes(typeof(StringLengthAttribute), false).FirstOrDefault() as StringLengthAttribute;
                int maxLength = 50;

                if (stringLength != null)
                {
                    maxLength = stringLength.MaximumLength;
                }
                else
                {
                    //if there is no stringLength then use RangeAttribute. So if the range is [Range(5, 250)] then the maxlength will be 3 since the biggest number is three digits
                    var stringLengthInt = member.Member.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault() as RangeAttribute;

                    if (stringLengthInt != null)
                    {
                        maxLength = stringLengthInt.Maximum.ToString().Length;
                    }
                }

                textbox.Attributes.Add("maxlength", maxLength.ToString());
            }

            //add the validation
            textbox.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(fieldName));

            return MvcHtmlString.Create(textbox.ToString(TagRenderMode.SelfClosing));
        }
    }
}