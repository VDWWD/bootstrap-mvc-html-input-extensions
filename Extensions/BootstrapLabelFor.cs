using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public static MvcHtmlString BootstrapLabelFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            //get the data from the model binding
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var descriptor = new AssociatedMetadataTypeTypeDescriptionProvider(metadata.ContainerType).GetTypeDescriptor(metadata.ContainerType).GetProperties().Find(metadata.PropertyName, true);
            var required = (new List<Attribute>(descriptor.Attributes.OfType<Attribute>())).OfType<RequiredAttribute>().FirstOrDefault();

            var attributes = (IDictionary<string, object>)new RouteValueDictionary(FixHtmlAttributes(htmlAttributes));

            //add a class if there is none
            if (!attributes.Any(x => x.Key.ToLower() == "class"))
            {
                attributes.Add("class", BootstrapHelper.Label);
            }

            //normal label html
            var label = htmlHelper.LabelFor(expression, attributes);

            //create the required asterix if the control has the Required attribute
            if (required != null)
            {
                var span = new TagBuilder("span");
                span.MergeAttribute("class", BootstrapHelper.LabelAsterix);

                //render the control
                var sb = new StringBuilder();
                sb.Append(span.ToString(TagRenderMode.StartTag));
                sb.Append("*");
                sb.Append(span.ToString(TagRenderMode.EndTag));

                return MvcHtmlString.Create(label.ToHtmlString() + " " + sb.ToString());
            }
            else
            {
                return label;
            }
        }
    }
}