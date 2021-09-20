using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace DemoWebsite
{
    public static partial class HtmlHelperExtensions
    {
        public static MvcHtmlString BootstrapToolTip(this HtmlHelper htmlHelper, string toolTipText, object htmlAttributes = null)
        {
            var attributes = (IDictionary<string, object>)new RouteValueDictionary(FixHtmlAttributes(htmlAttributes));

            //replace all characters that will break the javascript
            toolTipText = toolTipText.Replace("’", "&acute;");
            toolTipText = toolTipText.Replace("’", "&acute;");
            toolTipText = toolTipText.Replace("\"", "&quot;");
            toolTipText = toolTipText.Replace("“", "&quot;");
            toolTipText = toolTipText.Replace("”", "&quot;");

            //create the tooltip
            var tooltip = new TagBuilder("span");
            tooltip.MergeAttribute("data-toggle", "tooltip");
            tooltip.MergeAttribute("title", toolTipText);

            //add a class if there is none
            if (!attributes.Any(x => x.Key.ToLower() == "class"))
            {
                tooltip.MergeAttribute("class", BootstrapHelper.ToolTip);
            }
            else
            {
                tooltip.MergeAttribute("class", $"{BootstrapHelper.ToolTip} {attributes["class"]}");
            }

            //render the control
            var sb = new StringBuilder();
            sb.Append(tooltip.ToString(TagRenderMode.StartTag));
            sb.Append("?");
            sb.Append(tooltip.ToString(TagRenderMode.EndTag));

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}