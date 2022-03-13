using Threax.AspNetCore.Halcyon.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace Test {

public class OutputCollectionResult 
{
    private HalEndpointClient client;

    public OutputCollectionResult(HalEndpointClient client) 
    {
        this.client = client;
    }

    private OutputCollection strongData = default(OutputCollection);
    public OutputCollection Data 
    {
        get
        {
            if(this.strongData == default(OutputCollection))
            {
                this.strongData = this.client.GetData<OutputCollection>();  
            }
            return this.strongData;
        }
    }

    private List<OutputResult> itemsStrong = null;
    public List<OutputResult> Items
    {
        get
        {
            if (this.itemsStrong == null) 
            {
                var embeds = this.client.GetEmbed("values");
                var clients = embeds.GetAllClients();
                this.itemsStrong = new List<OutputResult>(clients.Select(i => new OutputResult(i)));
            }
            return this.itemsStrong;
        }
    }

    public async Task Save(Input data) 
    {
        var result = await this.client.LoadLinkWithData("Save", data);
    }

    public bool CanSave 
    {
        get 
        {
            return this.client.HasLink("Save");
        }
    }

    public HalLink LinkForSave 
    {
        get 
        {
            return this.client.GetLink("Save");
        }
    }

    public async Task<HalEndpointDoc> GetSaveDocs(HalEndpointDocQuery query = null) 
    {
        var result = await this.client.LoadLinkDoc("Save", query);
        return result.GetData<HalEndpointDoc>();
    }

    public bool HasSaveDocs() {
        return this.client.HasLinkDoc("Save");
    }
}
}
namespace Test
{
    #pragma warning disable // Disable all warnings

    public partial class OutputCollection 
    {
        [Newtonsoft.Json.JsonProperty("offset", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Offset { get; set; }
    
        [Newtonsoft.Json.JsonProperty("limit", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Limit { get; set; }
    
        [Newtonsoft.Json.JsonProperty("total", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Total { get; set; }
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static OutputCollection FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<OutputCollection>(data);
        }
    
    }
    
    public partial class Input 
    {
        /// <summary>The number of pages (item number = Offset * Limit) into the collection to query.</summary>
        [Newtonsoft.Json.JsonProperty("offset", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Offset { get; set; }
    
        /// <summary>The limit of the number of items to return.</summary>
        [Newtonsoft.Json.JsonProperty("limit", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Limit { get; set; }
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static Input FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Input>(data);
        }
    
    }
}
