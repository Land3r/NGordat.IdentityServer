using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Razor.Helpers.TagHelpers
{
    public abstract class BaseTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression AspFor { get; set; }

        [HtmlAttributeName("asp-type")]
        public Type AspType { get; set; }

        public TAttribute GetPropertyAttribute<TAttribute>(Type type, string propertyName)
            where TAttribute : Attribute
        {
            string actualPropertyName = propertyName.Split('.').Last();

            var prop = type.GetProperty(actualPropertyName);
            if (Attribute.IsDefined(prop, typeof(TAttribute)))
            {
                return prop.GetCustomAttribute<TAttribute>();
            }
            return null;
        }

        public TagHelperAttributeList GetValidationAttributes()
        {
            IList<TagHelperAttribute> validationRules = new List<TagHelperAttribute>();

            var requiredAttr = GetPropertyAttribute<RequiredAttribute>(AspType, AspFor.Name);
            if (requiredAttr != null)
            {
                string errorMessage = $"The {0} field is required.";
                validationRules.Add(new TagHelperAttribute("data-val-required", errorMessage));
            }

            var emailAttr = GetPropertyAttribute<EmailAddressAttribute>(AspType, AspFor.Name);
            if (emailAttr != null)
            {
                string errorMessage = "The {0} field must be a valid email.";
                validationRules.Add(new TagHelperAttribute("data-val-email", errorMessage));
            }

            if (validationRules.Count > 0)
            {
                validationRules.Add(new TagHelperAttribute("data-val", "true"));
            }

            return new TagHelperAttributeList(validationRules);
        }
    }
}
