using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ShoppingMall.Web.TagHelpers
{
    [HtmlTargetElement("styled-input")]
    public class StyledInputTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression AspFor { get; set; }
        public string Type { get; set; } = "text";
        public bool Required { get; set; } = false;
        public string Value { get; set; } = string.Empty;
        public string Placeholder { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "input";
            output.TagMode = TagMode.SelfClosing;
            output.Attributes.SetAttribute("type", Type);

            if (AspFor != null)
            {
                output.Attributes.SetAttribute("name", AspFor.Name);
                output.Attributes.SetAttribute("id", AspFor.Name);
                if (Value == string.Empty && AspFor.Model != null)
                    output.Attributes.SetAttribute("value", AspFor.Model.ToString());
            }
            else
            {
                output.Attributes.SetAttribute("name", "");
                output.Attributes.SetAttribute("id", "");
            }

            output.Attributes.SetAttribute("class", "px-2 py-2 rounded-0 rounded-top border-0 border-bottom border-secondary bg-light");
            if (!string.IsNullOrEmpty(Value))
                output.Attributes.SetAttribute("value", Value);
            if (!string.IsNullOrEmpty(Placeholder))
                output.Attributes.SetAttribute("placeholder", Placeholder);
            if (Required)
                output.Attributes.SetAttribute("required", "required");
        }
    }
}
