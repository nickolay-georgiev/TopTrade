namespace TopTrade.Common.TagHelpers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("div", Attributes = "custom-badge")]
    public class CustomBadgeTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var textContent = await output.GetChildContentAsync();
            var status = textContent.GetContent().Trim();

            if (status == "Pending")
            {
                output.Attributes.Add(
                    new TagHelperAttribute("class", "status badge badge-secondary badge-pill badge-sm"));
            }
            else if (status == "Canceled")
            {
                output.Attributes.Add(
                    new TagHelperAttribute("class", "status badge badge-danger badge-pill badge-sm"));
            }
            else if (status == "Completed")
            {
                output.Attributes.Add(
                    new TagHelperAttribute("class", "status badge badge-success badge-pill badge-sm"));
            }
        }
    }
}
