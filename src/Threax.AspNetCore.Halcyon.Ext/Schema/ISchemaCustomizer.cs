using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface ISchemaCustomizer
    {
        Task Customize(SchemaCustomizerArgs args);
    }
}
