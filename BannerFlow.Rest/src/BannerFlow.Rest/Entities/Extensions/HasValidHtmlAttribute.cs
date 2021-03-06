﻿using BannerFlow.Rest.Entities.Models;
using BannerFlow.Rest.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BannerFlow.Rest.Entities.Extensions
{
    public class HasValidHtmlAttribute : ValidationAttribute
    {
        public HasValidHtmlAttribute()
        {
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            Banner banner = (Banner)validationContext.ObjectInstance;

            if (!HtmlUtility.IsValid(banner.Html))
            {
                return new ValidationResult("invalid HTML content", new List<string>(){"Html"});
            }

            return ValidationResult.Success;
        }
    }
}