using Microsoft.AspNetCore.Razor.TagHelpers;

namespace VirtualBazaar.TagHelpers;

[HtmlTargetElement("confirm-delete", TagStructure = TagStructure.WithoutEndTag)]
public class ConfirmDeleteTagHelper : TagHelper
{
    public string Message { get; set; } = "Are you sure you want to delete this item?";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "button";
        output.Attributes.SetAttribute("type", "submit");
        output.Attributes.SetAttribute("class", "btn btn-danger");
        output.Attributes.SetAttribute("onclick", $"return confirm('{Message}');");
        output.Content.SetContent("Delete");
    }
}