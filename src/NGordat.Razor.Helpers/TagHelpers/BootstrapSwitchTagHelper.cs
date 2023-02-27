using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NGordat.Razor.Helpers.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NGordat.Razor.Helpers.TagHelpers
{
    [HtmlTargetElement("bootstrap-switch", Attributes = "asp-for")]
    public class BootstrapSwitchTagHelper : BaseTagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var typeOfModel = AspFor.ModelExplorer.ModelType;
            var presentationAttr = GetPropertyAttribute<PresentationAttribute>(AspType, AspFor.Name);
            string dataType = "checkbox";
            string id = AspFor.Name;

            if (presentationAttr != null)
            {
                dataType = presentationAttr.DataType;
            }

            // Main div tag (input-group).
            output.TagName = "div";
            output.Attributes.Add("class", "input-group form-check form-switch");

            // Start icon, if any.
            //if (!string.IsNullOrEmpty(presentationAttr.Icon) && presentationAttr?.IconPosition == IconPosition.Start)
            //{
            //    string icon = $"<i class=\"input-group-text {presentationAttr.Icon}\"></i>";
            //    output.Content.AppendHtml(icon);
            //}

            // Floating div (form-floating).

            // Input
            string input = $"<input type=\"{dataType}\" class=\"form-control form-check-input\" role=\"switch\" id=\"{id}\" name=\"{id}\"";

            if (AspFor.Model.ToString() == "true")
            {
                input += " checked";
            }

            var validationAttrs = GetValidationAttributes();
            if (validationAttrs != null && validationAttrs.Count > 0)
            {
                foreach (var validationAttr in validationAttrs)
                {
                    input += $" {validationAttr.Name}=\"{validationAttr.Value}\"";
                }
            }

            input += "/>";
            output.Content.AppendHtml(input);

            // Label
            string label = $"<label for=\"{id}\" class=\"form-check-label\">{AspFor.Name}</label>";
            output.Content.AppendHtml(label);
        }
    }
}
