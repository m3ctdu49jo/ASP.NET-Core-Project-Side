using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ShoppingMall.Web.TagHelpers
{
    public class StyledInputTagHelper : TagHelper
    {
        public string AspFor { get; set; } = string.Empty;
        public string Type { get; set; } = "text";
        public bool Required { get; set; } = false;
        public string Value { get; set; } = string.Empty;
        public string Placeholder { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "input";
            output.TagMode = TagMode.SelfClosing;
            output.Attributes.SetAttribute("type", Type);
            output.Attributes.SetAttribute("name", AspFor);
            output.Attributes.SetAttribute("id", AspFor);
            output.Attributes.SetAttribute("class", "px-2 py-1 rounded-3 border-secondary border-1");
            if (!string.IsNullOrEmpty(Value))
                output.Attributes.SetAttribute("value", Value);
            if (!string.IsNullOrEmpty(Placeholder))
                output.Attributes.SetAttribute("placeholder", Placeholder);
            if (Required)
                output.Attributes.SetAttribute("required", "required");
        }
    }
}
