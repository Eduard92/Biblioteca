using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

namespace RK.Models
{
    public class SettingModel
    {

        public Dictionary<string, string> Sections { get; set; }
        public Dictionary<string, List<FormsModel>> Settings { get; set; }
        public List<DataLayer.settings> list_settings { get; set; }

        public static string GetTitle(string slug = null)
        {
            string title = "";
            Dictionary<string, string> titles = new Dictionary<string, string>{ 
                {"email","Correo electrónico"},
                {"integration","Integración"},
                {"general","General"},
                {"users","Usuarios"},
                {"settings","Confiuraciones"},
                {"files","Archivos"},
                {"comments","Comentarios"},
                {"viaticos","Viáticos"},
                 {"applications","Aplicaciones"},
           };

            if (titles.ContainsKey(slug))
            {
                title = titles[slug];
            }
            else
            {
                title = "S/D";
            }

            return title;
        }
    }
    public class FormsModel
    {
        public string module { get; set; }
        public string slug { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public string value { get; set; }
        public string options { get; set; }
        public string form_control { get; set; }

        public string[] values { get; set; }

        public static string[] FormatOptions(string options)
        {
            string[] options_array = null;



            if (options.Substring(0, 5) != "func:")
            {
                options_array = options == null ? null : options.ToString().Split("|".ToCharArray());



            }
            return options_array;
        }
        public static string BuildForm(FormsModel setting, int index)
        {
            string form_ctrl = "";
            string options_html = "";
            //string[] options = null;



            switch (setting.type)
            {
                case "password":
                    form_ctrl = "<input type='password' name='Settings[" + setting.module + "][" + index + "].value' class='form-control' value='" + setting.value + "'/>";
                    break;
                case "text":
                    form_ctrl = "<input type='text' name='Settings[" + setting.module + "][" + index + "].value' class='form-control' value='" + setting.value + "'/>";
                    break;
                case "textarea":
                    form_ctrl = "<textarea  name='Settings[" + setting.module + "][" + index + "].value' class='form-control'>" + setting.value + "</textarea>";
                    break;
                case "select":
                    if (FormatOptions(setting.options) != null)
                    {
                        foreach (string option in FormatOptions(setting.options))
                        {
                            string[] values = option.Split("=".ToCharArray());

                            options_html += "<option value='" + values[0] + "' " + (values[0] == setting.value ? "selected" : "") + ">" + values[1] + "</option>";
                        }
                    }
                    form_ctrl = "<select class='form-control' name='Settings[" + setting.module + "][" + index + "].value'>" + options_html + "</select>";
                    break;
                case "radio":
                    if (FormatOptions(setting.options) != null)
                    {
                        foreach (string option in FormatOptions(setting.options))
                        {
                            string[] values = option.Split("=".ToCharArray());
                            form_ctrl += "<label><input type='radio' " + (setting.value == values[0] ? "checked" : "") + " value='" + values[0] + "' name='Settings[" + setting.module + "][" + index + "].value'>" + values[1] + "</label> ";
                            //options_html += "<option value='" + values[0] + "'>" + values[1] + "</option>";
                        }
                    }
                    break;
                case "checkbox":
                    string[] values_checkbox = setting.value == null ? null : setting.value.Split(",".ToCharArray());


                    if (FormatOptions(setting.options) != null)
                    {
                        foreach (string option in FormatOptions(setting.options))
                        {
                            string[] values = option.Split("=".ToCharArray());
                            form_ctrl += "<label><input " + (values_checkbox.Contains(values[0]) == true ? "checked" : "") + " type='checkbox' value='" + values[0] + "' name='Settings[" + setting.module + "][" + index + "].values'>" + values[1] + "</label> ";
                            //options_html += "<option value='" + values[0] + "'>" + values[1] + "</option>";
                        }
                    }
                    break;

            }
            return form_ctrl;
        }
    }
}