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
    [HtmlTargetElement("bootstrap-input-password", Attributes = "asp-for")]
    public class BootstrapInputPasswordTagHelper : BaseTagHelper
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
                string icon = $"<i class=\"input-group-text {presentationAttr.Icon}\"></i>";
                output.Content.AppendHtml(icon);
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
                string icon = $"<i class=\"input-group-text {presentationAttr.Icon}\" id=\"reveal{id}Btn\"></i>";
                output.Content.AppendHtml(icon);
            }

            // Reveal password
            string reveal = $"<i class=\"input-group-text fa fa-eye\" onclick=\"toggleReveal(\"{id}\", \"reveal{id}Btn\")\"></i>";
            output.Content.AppendHtml(reveal);
        }
    }
}
