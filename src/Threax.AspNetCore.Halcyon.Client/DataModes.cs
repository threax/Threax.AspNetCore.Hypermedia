using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Client
{
    public static class DataModes
    {
        public const String NoData = null;
        public const String Query = "query";
        public const String Body = "body";
        public const String Form = "form";
        public const String QueryAndBody = "queryandbody";
        public const String QueryAndForm = "queryandform";
    }
}
