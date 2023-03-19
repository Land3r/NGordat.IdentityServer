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
    [HtmlTargetElement("bootstrap-input", Attributes = "asp-for")]
    public class BootstrapInputTagHelper : BaseTagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var typeOfModel = AspFor.ModelExplorer.ModelType;
            var presentationAttr = GetPropertyAttribute<PresentationAttribute>(AspType, AspFor.Name);
            string dataType = "text";
            string id = AspFor.Name;

            if (presentationAttr != null)
            {
                dataType = presentationAttr.DataType;
            }

            // Main div tag (input-group).
            output.TagName = "div";
            output.Attributes.Add("class", "input-group");

            // Start icon, if any.
            if (!string.IsNullOrEmpty(presentationAttr.Icon) && presentationAttr?.IconPosition == IconPosition.Start)
            {
                string iconPre = $"<span class=\"input-group-text\">";
                iconPre += $"<i class=\" {presentationAttr.Icon}\"></i>";
                iconPre += $"</span>";
                output.Content.AppendHtml(iconPre);
            }

            // Floating div (form-floating).
            string floating = "<div class=\"form-floating\">";
            output.Content.AppendHtml(floating);

            // Input
            string input = $"<input type=\"{dataType}\" class=\"form-control\" id=\"{id}\" name=\"{id}\" placeholder=\"{AspFor.Name}\" value=\"{AspFor.Model}\"";

            var validationAttrs = GetValidationAttributes();
            if (validationAttrs != null && validationAttrs.Count > 0)
            {
                foreach (var validationAttr in validationAttrs)
                {
                    input += $" {validationAttr.Name}=\"{validationAttr.Value}\"";
                }
            }

            if (presentationAttr?.Autofocus == true)
            {
                input += $" autofocus";
            }

            input += "/>";
            output.Content.AppendHtml(input);

            // Label
            string label = $"<label for=\"{id}\">{AspFor.Name}</label>";
            output.Content.AppendHtml(label);

            // End of floating div (form-floating).
            output.Content.AppendHtml("</div>");

            // End icon, if any.
            if (!string.IsNullOrEmpty(presentationAttr.Icon) && presentationAttr?.IconPosition == IconPosition.End)
            {
                string iconPost = $"<span class=\"input-group-text\">";
                iconPost += $"<i class=\" {presentationAttr.Icon}\"></i>";
                iconPost += $"</span>";
                output.Content.AppendHtml(iconPost);
            }
        }
    }
}
