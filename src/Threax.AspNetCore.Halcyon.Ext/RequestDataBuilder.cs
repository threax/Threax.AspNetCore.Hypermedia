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

        private Dictionary<String, Object> data = new Dictionary<string, object>();

        /// <summary>
        /// Get the dictionary that backs the data. Use this to pass the data onward, don't modify the dictionary yourself.
        /// </summary>
        public Dictionary<String,Object> Data { get => data; }

        public void AppendItem(String name, Object value)
        {
            Object current;
            if(!data.TryGetValue(name, out current))
            {
                data[name] = value;
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
            data[name] = itemList;
        }
    }
}
