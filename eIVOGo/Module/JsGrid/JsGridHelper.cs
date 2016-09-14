using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;

namespace eIVOGo.Module.JsGrid
{
    public class JsGridHelper
    {
        public static JsGridField[] EnumJsGridFields(Object item)
        {
            Type type = item.GetType();
            var items = type.GetProperties(BindingFlags.Public | BindingFlags.Instance
                | BindingFlags.GetProperty | BindingFlags.SetProperty);
            return items.Select(p => new JsGridField
                {
                    align = "left",
                    name = p.Name,
                    title = p.Name,
                    type = "text",
                    width = 80
                }).ToArray();
        }

        public static String JsGridFieldArray(Object item)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(JsGridHelper.EnumJsGridFields(item));
        }
    }

    public class JsGridField
    {
        public String type { get; set; }
        public String name { get; set; }
        public String title { get; set; }
        public String align { get; set; }
        public int width { get; set; }
    }

}