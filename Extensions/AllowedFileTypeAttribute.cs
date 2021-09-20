﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWebsite
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FileTypeAttribute : ValidationAttribute, IClientValidatable
    {
        public IEnumerable<string> _ValidTypes { get; set; }

        public FileTypeAttribute(string validTypes)
        {
            _ValidTypes = validTypes.Split(',').Select(s => s.Trim().ToLower());

            //add the allowed file types
            string extratekst = $" (only {string.Join(", ", _ValidTypes)})";

            //the custom error message. EXT will be replaced in the javascript
            ErrorMessage = $"The file type \"EXT\" is not allowed{extratekst}.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var enumerableTest = value as System.Collections.IEnumerable;

            if (enumerableTest == null)
            {
                HttpPostedFileBase file_upload = value as HttpPostedFileBase;
                if (file_upload != null)
                {
                    if (!_ValidTypes.Any(e => file_upload.FileName.ToLower().EndsWith(e)))
                    {
                        return new ValidationResult(ErrorMessageString);
                    }
                }
            }
            else
            {
                IEnumerable<HttpPostedFileBase> files_upload = value as IEnumerable<HttpPostedFileBase>;
                if (files_upload != null)
                {
                    foreach (HttpPostedFileBase file in files_upload)
                    {
                        if (file != null && !_ValidTypes.Any(e => file.FileName.ToLower().EndsWith(e)))
                        {
                            return new ValidationResult(ErrorMessageString);
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "filetype",
                ErrorMessage = ErrorMessageString
            };
            rule.ValidationParameters.Add("validtypes", string.Join(",", _ValidTypes));
            yield return rule;
        }
    }
}