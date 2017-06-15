using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Congressus.Web.Helpers
{
    public static class EnumExtensions
    {
        public static HtmlString EnumDisplayName(this HtmlHelper HtmlHelper,Type enumType, object enumValue)
        {
            var member = enumType.GetMember(enumValue.ToString());
            DisplayAttribute displayName = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayName != null)
            {
                return new HtmlString(displayName.Name);
            }

            return new HtmlString(enumValue.ToString());
        }
        
        public static HtmlString EnumDisplayName(this HtmlHelper HtmlHelper, Type enumType, int enumId)
        {
            var enumValueName = enumType.GetEnumName(enumId);
            var member = enumType.GetMember(enumValueName);
            DisplayAttribute displayName = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayName != null)
            {
                return new HtmlString(displayName.Name);
            }

            return new HtmlString(enumValueName);
        }


        public static MvcHtmlString EnumLabel(this HtmlHelper HtmlHelper, Type enumType, object enumValue)
        {
            //string value = enumValue.ToString();
            //var member = enumType.GetMember(enumValue.ToString());
            //DisplayAttribute displayName = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            //if (displayName != null)
            //{
            //    value = displayName.Name;
            //}
            var value = GetDisplayName(enumType, enumValue);
            var label = new TagBuilder("label");
            label.SetInnerText(value);

            return MvcHtmlString.Create(label.ToString(TagRenderMode.Normal));

        }

        public static MvcHtmlString EnumCheckBoxGroup(this HtmlHelper HtmlHelper,string group ,Type enumType)
        {
            var div = new TagBuilder("div");
            div.InnerHtml = "";
            var radios = new List<string>();
            foreach (var value in Enum.GetValues(enumType))
            {                
                var radio = "";
                radio += "<p>";
                radio += "<input type='radio' name='" + group + "' id='"+Enum.GetName(enumType,value)+"' value='"+ (int)value + "'/>";
                radio += "<label for='"+ Enum.GetName(enumType, value) + "'>"+GetDisplayName(enumType,value)+"</label>";
                radio += "</p>";
                radios.Add(radio);
            }
            radios.ForEach((radio) => 
            {
                div.InnerHtml += radio;
            });
            return MvcHtmlString.Create(div.ToString());
        }

        public static MvcHtmlString EnumCheckBoxGroup<T>(this HtmlHelper HtmlHelper, string group)
        {
            var enumType = typeof(T);
            return EnumCheckBoxGroup(HtmlHelper, group, enumType);
        }


        private static string GetDisplayName(Type enumType, object value)
        {
            var member = enumType.GetMember(value.ToString());
            DisplayAttribute displayName = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            if (displayName != null)
            {
                return displayName.Name;
            }
            return value.ToString();
        }


        private static string GetDisplayName<T>(T value)
        {
            var enumType = typeof(T);
            return GetDisplayName(enumType,value);
        }
    }
}