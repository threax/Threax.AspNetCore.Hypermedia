using Threax.AspNetCore.Halcyon.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace Test {

public class OutputResult 
{
    private HalEndpointClient client;

    public OutputResult(HalEndpointClient client) 
    {
        this.client = client;
    }

    private Output strongData = default(Output);
    public Output Data 
    {
        get
        {
            if(this.strongData == default(Output))
            {
                this.strongData = this.client.GetData<Output>();  
            }
            return this.strongData;
        }
    }

    private List<EmbeddedObjectResult> embedsStrong = null;
    public List<EmbeddedObjectResult> Embeds
    {
        get
        {
            if (this.embedsStrong == null) 
            {
                var embeds = this.client.GetEmbed("embeds");
                var clients = embeds.GetAllClients();
                this.embedsStrong = new List<EmbeddedObjectResult>(clients.Select(i => new EmbeddedObjectResult(i)));
            }
            return this.embedsStrong;
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

    public partial class EmbeddedObject 
    {
        [Newtonsoft.Json.JsonProperty("value", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Value { get; set; }
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static EmbeddedObject FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<EmbeddedObject>(data);
        }
    
    }
    
    public partial class Output 
    {
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static Output FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Output>(data);
        }
    
    }
    
    public partial class Input 
    {
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
