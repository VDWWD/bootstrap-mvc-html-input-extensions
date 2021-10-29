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

        // This extension also needs the AllowedFileTypeAttribute.cs file

        public static MvcHtmlString BootstrapFileUploadFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var attributes = (IDictionary<string, object>)new RouteValueDictionary(FixHtmlAttributes(htmlAttributes));

            //add the correct textbox type and class
            attributes.Add("type", "file");

            //add a class if there is none
            if (!attributes.Any(x => x.Key.ToLower() == "class"))
            {
                attributes.Add("class", BootstrapHelper.FilePickerInput);
            }
            else
            {
                attributes["class"] = $"{BootstrapHelper.FilePickerInput} {attributes["class"]}";
            }

            //get the allowed extension accepts from the FileType attribute
            var member = (MemberExpression)expression.Body;
            if (member?.Member != null)
            {
                var filetypes = member.Member.GetCustomAttributes(typeof(FileTypeAttribute), false).FirstOrDefault();

                if (filetypes != null)
                {
                    attributes.Add("accept", "." + string.Join(",.", ((FileTypeAttribute)filetypes)._ValidTypes));
                }
            }

            //get the normal textbox html
            string textbox = htmlHelper.TextBoxFor(expression, attributes).ToHtmlString();

            //the file input needs to be wrapped with a div
            var outerDiv = new TagBuilder("div");
            outerDiv.Attributes.Add("class", BootstrapHelper.FilePickerControl);

            //and must contain an inner label to display properly
            var innerLabel = new TagBuilder("label");
            innerLabel.AddCssClass(BootstrapHelper.FilePickerLabel);

            //render the control
            var sb = new StringBuilder();
            sb.AppendLine(outerDiv.ToString(TagRenderMode.StartTag));
            sb.AppendLine(textbox);
            sb.AppendLine(innerLabel.ToString(TagRenderMode.StartTag));
            sb.AppendLine(innerLabel.ToString(TagRenderMode.EndTag));
            sb.AppendLine(outerDiv.ToString(TagRenderMode.EndTag));

            //return the newly created control as html
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}