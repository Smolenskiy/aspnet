using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using AccountSystem.Models;

namespace AccountSystem.TagHelpers
{
    public class PageTagHelper : TagHelper
    {
        public PageViewModel Model { get; set; }
        public string Action { get; set; }

        //  словарь, в котором каждой строке будет сопоставлен некоторый объект
        [HtmlAttributeName(DictionaryAttributePrefix = "rout-")]
        public Dictionary<string, object> RoutValues { get; set; } = new Dictionary<string, object>();

        private IUrlHelperFactory urlHelperFactory;
        public PageTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
       
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "div";

            // список ul
            TagBuilder tag = new TagBuilder("ul");
            tag.AddCssClass("pagination");

            // ссылки на текущую, предыдущую и следующую страницу
            TagBuilder currentItem = CreateTag(Model.PageNumber, urlHelper);

            if (Model.HasPreviousPage)
            {
                TagBuilder prevItem = CreateTag(Model.PageNumber - 1, urlHelper);
                tag.InnerHtml.AppendHtml(prevItem);
            }
            tag.InnerHtml.AppendHtml(currentItem);

            if (Model.HasNextPage)
            {
                TagBuilder nextItem = CreateTag(Model.PageNumber + 1, urlHelper);
                tag.InnerHtml.AppendHtml(nextItem);
            }
            output.Content.AppendHtml(tag);
        }

        TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
        {
            TagBuilder item = new TagBuilder("li");
            TagBuilder link = new TagBuilder("a");
            if (pageNumber == this.Model.PageNumber)
            {
                item.AddCssClass("active");
            }
            else
            {
                RoutValues["page"] = pageNumber;
                link.Attributes["href"] = urlHelper.Action(Action, new { page = pageNumber });
            }
            link.InnerHtml.Append(pageNumber.ToString());
            item.InnerHtml.AppendHtml(link);
            return item;
        }

    }
}
