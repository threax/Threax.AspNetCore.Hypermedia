using Threax.AspNetCore.Halcyon.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace Test {

public class EntryPointInjector 
{
    private string url;
    private IHttpClientFactory fetcher;
    private Task<EntryPointResult> instanceTask = default(Task<EntryPointResult>);

    public EntryPointInjector(string url, IHttpClientFactory fetcher)
    {
        this.url = url;
        this.fetcher = fetcher;
    }

    public Task<EntryPointResult> Load()
    {
        if (this.instanceTask == default(Task<EntryPointResult>))
        {
            this.instanceTask = EntryPointResult.Load(this.url, this.fetcher);
        }
        return this.instanceTask;
    }
}

public class EntryPointResult 
{
    private HalEndpointClient client;

    public static async Task<EntryPointResult> Load(string url, IHttpClientFactory fetcher)
    {
        var result = await HalEndpointClient.Load(new HalLink(url, "GET"), fetcher);
        return new EntryPointResult(result);
    }

    public EntryPointResult(HalEndpointClient client) 
    {
        this.client = client;
    }

    private EntryPoint strongData = default(EntryPoint);
    public EntryPoint Data 
    {
        get
        {
            if(this.strongData == default(EntryPoint))
            {
                this.strongData = this.client.GetData<EntryPoint>();  
            }
            return this.strongData;
        }
    }

    public async Task<EntryPointResult> Refresh() 
    {
        var result = await this.client.LoadLink("self");
        return new EntryPointResult(result);

    }

    public bool CanRefresh 
    {
        get 
        {
            return this.client.HasLink("self");
        }
    }

    public HalLink LinkForRefresh 
    {
        get 
        {
            return this.client.GetLink("self");
        }
    }

    public async Task<HalEndpointDoc> GetRefreshDocs(HalEndpointDocQuery query = null) 
    {
        var result = await this.client.LoadLinkDoc("self", query);
        return result.GetData<HalEndpointDoc>();
    }

    public bool HasRefreshDocs() {
        return this.client.HasLinkDoc("self");
    }
}
}
namespace Test
{
    #pragma warning disable // Disable all warnings

    public partial class EntryPoint 
    {
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static EntryPoint FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<EntryPoint>(data);
        }
    
    }
}
