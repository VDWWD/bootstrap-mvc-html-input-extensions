using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Routing;

namespace DemoWebsite
{
    public static partial class HtmlHelperExtensions
    {
        /// <summary>
        /// Properties in the html cannot contain a dash - so and underscore is used and they need to be replaced 
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <returns>IDictionary<string, object></returns>
        public static IDictionary<string, object> FixHtmlAttributes(object htmlAttributes)
        {
            var fixedHtmlAttributes = (IDictionary<string, object>)new RouteValueDictionary();

            if (htmlAttributes != null)
            {
                foreach (var item in new RouteValueDictionary(htmlAttributes))
                {
                    fixedHtmlAttributes.Add(item.Key.Replace("_", "-"), item.Value);
                }
            }

            return fixedHtmlAttributes;
        }
    }




    /// <summary>
    /// Sets the bootstrap version from the Global.asax file and creetes the class names accordingly. Or set it manually somewhere
    /// </summary>
    public class BootstrapHelper
    {
        public static BootstrapVersion Version { get; set; }

        public static string DefaultClassName { get; set; }
        public static string CheckRadioInput { get; set; }
        public static string CheckRadioControl { get; set; }
        public static string CheckRadioLabel { get; set; }
        public static string SwitchInput { get; set; }
        public static string SwitchControl { get; set; }
        public static string SwitchLabel { get; set; }
        public static string DatePicker { get; set; }
        public static string DatePickerDateFormat { get; set; }
        public static string FilePickerInput { get; set; }
        public static string FilePickerControl { get; set; }
        public static string FilePickerLabel { get; set; }
        public static string ToolTip { get; set; }
        public static string Validator { get; set; }
        public static string Label { get; set; }
        public static string LabelAsterix { get; set; }


        public static void Start(BootstrapVersion version)
        {
            Version = version;

            if (Version == BootstrapVersion.Version5)
            {
                DefaultClassName = "form-control";
                CheckRadioControl = "form-check";
                CheckRadioInput = "form-check-input";
                CheckRadioLabel = "form-check-label fw-bold";
                SwitchControl = "form-check form-switch";
                SwitchInput = CheckRadioInput;
                SwitchLabel = CheckRadioLabel;
                FilePickerControl = "";
                FilePickerInput = "form-control";
                FilePickerLabel = "";
                ToolTip = "badge bg-primary";
                Validator = "field-validation-valid fw-bold text-danger small";
                Label = "fw-bold";
                LabelAsterix = "fw-bold text-danger";
                DatePicker = $"{DefaultClassName} bootstrap-datepicker";
            }
            else
            {
                DefaultClassName = "form-control";
                CheckRadioControl = "custom-control custom-{0}";
                CheckRadioInput = "custom-control-input";
                CheckRadioLabel = "custom-control-label font-weight-bold";
                SwitchControl = CheckRadioControl;
                SwitchInput = CheckRadioInput;
                SwitchLabel = CheckRadioLabel;
                FilePickerControl = "custom-file";
                FilePickerInput = "custom-file-input";
                FilePickerLabel = "custom-file-label";
                ToolTip = "badge badge-primary";
                Validator = "field-validation-valid font-weight-bold text-danger small";
                Label = "font-weight-bold";
                LabelAsterix = "font-weight-bold text-danger";
                DatePicker = $"{DefaultClassName} bootstrap-datepicker";
            }

            //set the date format
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            DatePickerDateFormat = currentCulture.DateTimeFormat.ShortDatePattern;
        }


        public enum BootstrapVersion
        {
            Version4,
            Version5
        }
    }
}