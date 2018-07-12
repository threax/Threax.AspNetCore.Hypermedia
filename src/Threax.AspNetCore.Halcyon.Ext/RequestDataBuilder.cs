using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class RequestDataBuilder
    {
        class QueryList : List<Object>
        {

        }

        private Dictionary<String, Object> query = new Dictionary<string, object>();

        internal Dictionary<String,Object> Data { get => query; }

        public void AppendItem(String name, Object value)
        {
            Object current;
            if(!query.TryGetValue(name, out current))
            {
                query[name] = value;
                return;
            }

            var itemList = current as QueryList; 
            if(itemList != null)
            {
                itemList.Add(value);
                return;
            }

            itemList = new QueryList();
            itemList.Add(current);
            itemList.Add(value);
            query[name] = itemList;
        }
    }
}
