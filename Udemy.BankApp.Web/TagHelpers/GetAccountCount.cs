using Microsoft.AspNetCore.Razor.TagHelpers;
using Udemy.BankApp.Web.Data.Context;

namespace Udemy.BankApp.Web.TagHelpers
{
    [HtmlTargetElement("getAccountCount")]
    public class GetAccountCount : TagHelper
    {
        public int ApplicationUserId { get; set; }
        private readonly BankContext _bankContext;

        public GetAccountCount(BankContext bankContext)
        {
            _bankContext = bankContext;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var count = _bankContext.Accounts.Count(x => x.ApplicationUserId == ApplicationUserId);
            var html = $"<span class='badge bg-danger' > {count}</span>";
            output.Content.SetHtmlContent(html);
        }
    }
}
