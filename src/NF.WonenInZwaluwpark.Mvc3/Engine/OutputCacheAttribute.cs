using System;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

namespace NF.WonenInZwaluwpark.Mvc3.Engine
{
    public class OutputCacheAttribute : Attribute
    {
        public int Duration { get; set; }
        public string VaryByParam { get; set; }
        public string CacheProfile { get; set; }
    }
}