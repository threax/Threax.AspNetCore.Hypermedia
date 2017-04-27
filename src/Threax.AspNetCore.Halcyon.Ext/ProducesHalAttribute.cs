using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class ProducesHalAttribute : ProducesAttribute
    {
        public const String MediaType = "application/json+halcyon";

        public ProducesHalAttribute()
            :base(MediaType)
        {
            
        }
    }
}
