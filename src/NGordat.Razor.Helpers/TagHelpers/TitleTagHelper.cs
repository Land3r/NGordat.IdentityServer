using Microsoft.AspNetCore.Razor.TagHelpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Razor.Helpers.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("title", Attributes = $"asp-site")]
    public class RazorTagHelper : TagHelper
    {
        public string? AspPage { get; set; }

        public string? AspSite { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!string.IsNullOrEmpty(AspPage))
            {
                output.Content.AppendFormat("{0} - {1}", AspSite, AspPage);
            }
            else
            {
                output.Content.Append(AspSite);
            }
        }
    }
}
