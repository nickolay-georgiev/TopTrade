namespace TopTrade.Common.TagHelpers
{
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("span", Attributes = "custom-color")]
    public class CustomColorTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var textContent = await output.GetChildContentAsync();
            var textContentAsString = textContent.GetContent().Trim();

            var pattern = new Regex("[$%()]");
            textContentAsString = pattern.Replace(textContentAsString, string.Empty);

            decimal amount;
            var isDecimal = decimal.TryParse(textContentAsString, out amount);

            if (isDecimal)
            {
                if (amount < 0)
                {
                    output.Attributes.Add(new TagHelperAttribute("class", "text-danger"));
                }
                else
                {
                    output.Attributes.Add(new TagHelperAttribute("class", "text-success"));
                }
            }
            else
            {
                if (textContentAsString.Equals("BUY"))
                {
                    output.Attributes.Add(new TagHelperAttribute("class", "text-success"));
                }
                else if (textContentAsString.Equals("SELL"))
                {
                    output.Attributes.Add(new TagHelperAttribute("class", "text-danger"));
                }
            }
        }
    }
}
