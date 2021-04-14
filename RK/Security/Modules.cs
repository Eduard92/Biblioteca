using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RK.Security
{
    public class Module
    {
        public string slug { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }
    public class Menu
    {
        public string slug { get; set; }
        public string name { get; set; }
    }
    public class ShortCuts
    {
        public string name { get; set; }
        public string uri { get; set; }
        public string css { get; set; }
        public string icon { get; set; }
        public string extra{ get; set; }
        public string title { get; set; }
        public string controller { get; set; }

    }
}