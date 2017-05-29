using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Congressus.Web.Helpers
{
    public static class ComboAjaxHelper
    {
        public static MvcHtmlString Combo(this AjaxHelper ajaxHelper, string name, IEnumerable<SelectListItem> list ,AjaxOptions ajaxOptions, object htmlAttributes)
        {
            var tag = new TagBuilder("select");
            tag.MergeAttribute("name", name);
            tag.InnerHtml = GenerateOptionsTags(list);
            tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            tag.MergeAttributes((ajaxOptions ?? new AjaxOptions()).ToUnobtrusiveHtmlAttributes());

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        private static string GenerateOptionsTags(IEnumerable<SelectListItem> list)
        {
            string optionTags = "";
            foreach (var item in list)
            {
                optionTags += "<option value ='" + item.Value + "'>" + item.Text + "</option>";
            }
            return optionTags;
        }
    }
}