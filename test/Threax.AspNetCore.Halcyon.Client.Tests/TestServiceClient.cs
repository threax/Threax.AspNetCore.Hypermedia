using Threax.AspNetCore.Halcyon.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace TestHalcyonApi.ServiceClient
{

    public class EntryPointsInjector
    {
        private string url;
        private IHttpClientFactory fetcher;
        private EntryPointsResult instance = default(EntryPointsResult);

        public EntryPointsInjector(string url, IHttpClientFactory fetcher)
        {
            this.url = url;
            this.fetcher = fetcher;
        }

        public async Task<EntryPointsResult> Load()
        {
            if (this.instance == default(EntryPointsResult))
            {
                this.instance = await EntryPointsResult.Load(this.url, this.fetcher);
            }
            return this.instance;
        }
    }

    public class EntryPointsResult
    {
        private HalEndpointClient client;

        public static async Task<EntryPointsResult> Load(string url, IHttpClientFactory fetcher)
        {
            var result = await HalEndpointClient.Load(new HalLink(url, "GET"), fetcher);
            return new EntryPointsResult(result);
        }

        public EntryPointsResult(HalEndpointClient client)
        {
            this.client = client;
        }

        private EntryPoints strongData = default(EntryPoints);
        public EntryPoints Data
        {
            get
            {
                if (this.strongData == default(EntryPoints))
                {
                    this.strongData = this.client.GetData<EntryPoints>();
                }
                return this.strongData;
            }
        }

        public async Task<EntryPointsResult> Refresh()
        {
            var result = await this.client.LoadLink("self");
            return new EntryPointsResult(result);

        }

        public bool CanRefresh()
        {
            return this.client.HasLink("self");
        }

        public async Task<HalEndpointDoc> GetRefreshDocs()
        {
            var result = await this.client.LoadLinkDoc("self");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasRefreshDocs()
        {
            return this.client.HasLinkDoc("self");
        }

        public async Task<ThingyCollectionViewResult> ListThingies(PagedCollectionQuery query)
        {
            var result = await this.client.LoadLinkWithQuery("listThingies", query);
            return new ThingyCollectionViewResult(result);

        }

        public bool CanListThingies()
        {
            return this.client.HasLink("listThingies");
        }

        public async Task<HalEndpointDoc> GetListThingiesDocs()
        {
            var result = await this.client.LoadLinkDoc("listThingies");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasListThingiesDocs()
        {
            return this.client.HasLinkDoc("listThingies");
        }

        public async Task<ThingyViewResult> AddThingy(ThingyView data)
        {
            var result = await this.client.LoadLinkWithBody("addThingy", data);
            return new ThingyViewResult(result);

        }

        public bool CanAddThingy()
        {
            return this.client.HasLink("addThingy");
        }

        public async Task<HalEndpointDoc> GetAddThingyDocs()
        {
            var result = await this.client.LoadLinkDoc("addThingy");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasAddThingyDocs()
        {
            return this.client.HasLinkDoc("addThingy");
        }

        public async Task<MultipartInput1Result> BeginAddMultipart()
        {
            var result = await this.client.LoadLink("BeginAddMultipart");
            return new MultipartInput1Result(result);

        }

        public bool CanBeginAddMultipart()
        {
            return this.client.HasLink("BeginAddMultipart");
        }

        public async Task<HalEndpointDoc> GetBeginAddMultipartDocs()
        {
            var result = await this.client.LoadLinkDoc("BeginAddMultipart");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasBeginAddMultipartDocs()
        {
            return this.client.HasLinkDoc("BeginAddMultipart");
        }

        public async Task<HalEndpointDoc> GetUpdateThingyDocs()
        {
            var result = await this.client.LoadLinkDoc("updateThingy");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasUpdateThingyDocs()
        {
            return this.client.HasLinkDoc("updateThingy");
        }

        public async Task TestTakeListInput(IEnumerable<ThingyView> data)
        {
            var result = await this.client.LoadLinkWithBody("TestTakeListInput", data);
        }

        public bool CanTestTakeListInput()
        {
            return this.client.HasLink("TestTakeListInput");
        }

        public async Task<HalEndpointDoc> GetTestTakeListInputDocs()
        {
            var result = await this.client.LoadLinkDoc("TestTakeListInput");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasTestTakeListInputDocs()
        {
            return this.client.HasLinkDoc("TestTakeListInput");
        }

        public async Task FileInput(SingleFileInput data)
        {
            var result = await this.client.LoadLinkWithForm("FileInput", data);
        }

        public bool CanFileInput()
        {
            return this.client.HasLink("FileInput");
        }

        public async Task<HalEndpointDoc> GetFileInputDocs()
        {
            var result = await this.client.LoadLinkDoc("FileInput");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasFileInputDocs()
        {
            return this.client.HasLinkDoc("FileInput");
        }

        public async Task FileInputMultiple(MultiFileInput data)
        {
            var result = await this.client.LoadLinkWithForm("FileInputMultiple", data);
        }

        public bool CanFileInputMultiple()
        {
            return this.client.HasLink("FileInputMultiple");
        }

        public async Task<HalEndpointDoc> GetFileInputMultipleDocs()
        {
            var result = await this.client.LoadLinkDoc("FileInputMultiple");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasFileInputMultipleDocs()
        {
            return this.client.HasLinkDoc("FileInputMultiple");
        }

        public async Task FileInputQuery(PagedCollectionQuery query, SingleFileInput data)
        {
            var result = await this.client.LoadLinkWithQueryAndForm("FileInputQuery", query, data);
        }

        public bool CanFileInputQuery()
        {
            return this.client.HasLink("FileInputQuery");
        }

        public async Task<HalEndpointDoc> GetFileInputQueryDocs()
        {
            var result = await this.client.LoadLinkDoc("FileInputQuery");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasFileInputQueryDocs()
        {
            return this.client.HasLinkDoc("FileInputQuery");
        }

        public async Task FileInputMultipleQuery(PagedCollectionQuery query, MultiFileInput data)
        {
            var result = await this.client.LoadLinkWithQueryAndForm("FileInputMultipleQuery", query, data);
        }

        public bool CanFileInputMultipleQuery()
        {
            return this.client.HasLink("FileInputMultipleQuery");
        }

        public async Task<HalEndpointDoc> GetFileInputMultipleQueryDocs()
        {
            var result = await this.client.LoadLinkDoc("FileInputMultipleQuery");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasFileInputMultipleQueryDocs()
        {
            return this.client.HasLinkDoc("FileInputMultipleQuery");
        }

        public async Task<HttpResponseMessage> ReturnActionResult()
        {
            var result = await this.client.LoadRawLink("ReturnActionResult");
            return result;
        }

        public bool CanReturnActionResult()
        {
            return this.client.HasLink("ReturnActionResult");
        }

        public async Task<HalEndpointDoc> GetReturnActionResultDocs()
        {
            var result = await this.client.LoadLinkDoc("ReturnActionResult");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasReturnActionResultDocs()
        {
            return this.client.HasLinkDoc("ReturnActionResult");
        }
    }

    public class SubThingyCollectionViewResult
    {
        private HalEndpointClient client;

        public SubThingyCollectionViewResult(HalEndpointClient client)
        {
            this.client = client;
        }

        private SubThingyCollectionView strongData = default(SubThingyCollectionView);
        public SubThingyCollectionView Data
        {
            get
            {
                if (this.strongData == default(SubThingyCollectionView))
                {
                    this.strongData = this.client.GetData<SubThingyCollectionView>();
                }
                return this.strongData;
            }
        }

        private List<SubThingyViewResult> strongItems = null;
        public List<SubThingyViewResult> Items
        {
            get
            {
                if (this.strongItems == null)
                {
                    var embeds = this.client.GetEmbed("values");
                    var clients = embeds.GetAllClients();
                    this.strongItems = new List<SubThingyViewResult>(clients.Select(i => new SubThingyViewResult(i)));
                }
                return this.strongItems;
            }
        }

        public async Task<SubThingyCollectionViewResult> Refresh()
        {
            var result = await this.client.LoadLink("self");
            return new SubThingyCollectionViewResult(result);

        }

        public bool CanRefresh()
        {
            return this.client.HasLink("self");
        }

        public async Task<HalEndpointDoc> GetRefreshDocs()
        {
            var result = await this.client.LoadLinkDoc("self");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasRefreshDocs()
        {
            return this.client.HasLinkDoc("self");
        }

        public async Task<SubThingyCollectionViewResult> ListSubThingies()
        {
            var result = await this.client.LoadLink("listSubThingies");
            return new SubThingyCollectionViewResult(result);

        }

        public bool CanListSubThingies()
        {
            return this.client.HasLink("listSubThingies");
        }

        public async Task<HalEndpointDoc> GetListSubThingiesDocs()
        {
            var result = await this.client.LoadLinkDoc("listSubThingies");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasListSubThingiesDocs()
        {
            return this.client.HasLinkDoc("listSubThingies");
        }
    }

    public class SubThingyViewResult
    {
        private HalEndpointClient client;

        public SubThingyViewResult(HalEndpointClient client)
        {
            this.client = client;
        }

        private SubThingyView strongData = default(SubThingyView);
        public SubThingyView Data
        {
            get
            {
                if (this.strongData == default(SubThingyView))
                {
                    this.strongData = this.client.GetData<SubThingyView>();
                }
                return this.strongData;
            }
        }

        public async Task<SubThingyViewResult> Refresh()
        {
            var result = await this.client.LoadLink("self");
            return new SubThingyViewResult(result);

        }

        public bool CanRefresh()
        {
            return this.client.HasLink("self");
        }

        public async Task<HalEndpointDoc> GetRefreshDocs()
        {
            var result = await this.client.LoadLinkDoc("self");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasRefreshDocs()
        {
            return this.client.HasLinkDoc("self");
        }

        public async Task<SubThingyCollectionViewResult> ListSubThingies()
        {
            var result = await this.client.LoadLink("listSubThingies");
            return new SubThingyCollectionViewResult(result);

        }

        public bool CanListSubThingies()
        {
            return this.client.HasLink("listSubThingies");
        }

        public async Task<HalEndpointDoc> GetListSubThingiesDocs()
        {
            var result = await this.client.LoadLinkDoc("listSubThingies");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasListSubThingiesDocs()
        {
            return this.client.HasLinkDoc("listSubThingies");
        }

        public async Task<SubThingyViewResult> UpdateSubThingy(SubThingyView data)
        {
            var result = await this.client.LoadLinkWithBody("updateSubThingy", data);
            return new SubThingyViewResult(result);

        }

        public bool CanUpdateSubThingy()
        {
            return this.client.HasLink("updateSubThingy");
        }

        public async Task<HalEndpointDoc> GetUpdateSubThingyDocs()
        {
            var result = await this.client.LoadLinkDoc("updateSubThingy");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasUpdateSubThingyDocs()
        {
            return this.client.HasLinkDoc("updateSubThingy");
        }

        public async Task DeleteSubThingy()
        {
            var result = await this.client.LoadLink("deleteSubThingy");
        }

        public bool CanDeleteSubThingy()
        {
            return this.client.HasLink("deleteSubThingy");
        }

        public async Task<SubThingyViewResult> GetSubThingy()
        {
            var result = await this.client.LoadLink("getSubThingy");
            return new SubThingyViewResult(result);

        }

        public bool CanGetSubThingy()
        {
            return this.client.HasLink("getSubThingy");
        }

        public async Task<HalEndpointDoc> GetGetSubThingyDocs()
        {
            var result = await this.client.LoadLinkDoc("getSubThingy");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasGetSubThingyDocs()
        {
            return this.client.HasLinkDoc("getSubThingy");
        }

        public async Task<ThingyViewResult> GetThingy()
        {
            var result = await this.client.LoadLink("getThingy");
            return new ThingyViewResult(result);

        }

        public bool CanGetThingy()
        {
            return this.client.HasLink("getThingy");
        }

        public async Task<HalEndpointDoc> GetGetThingyDocs()
        {
            var result = await this.client.LoadLinkDoc("getThingy");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasGetThingyDocs()
        {
            return this.client.HasLinkDoc("getThingy");
        }
    }

    public class ThingyCollectionViewResult
    {
        private HalEndpointClient client;

        public ThingyCollectionViewResult(HalEndpointClient client)
        {
            this.client = client;
        }

        private ThingyCollectionView strongData = default(ThingyCollectionView);
        public ThingyCollectionView Data
        {
            get
            {
                if (this.strongData == default(ThingyCollectionView))
                {
                    this.strongData = this.client.GetData<ThingyCollectionView>();
                }
                return this.strongData;
            }
        }

        private List<ThingyViewResult> strongItems = null;
        public List<ThingyViewResult> Items
        {
            get
            {
                if (this.strongItems == null)
                {
                    var embeds = this.client.GetEmbed("values");
                    var clients = embeds.GetAllClients();
                    this.strongItems = new List<ThingyViewResult>(clients.Select(i => new ThingyViewResult(i)));
                }
                return this.strongItems;
            }
        }

        public async Task<ThingyCollectionViewResult> Refresh()
        {
            var result = await this.client.LoadLink("self");
            return new ThingyCollectionViewResult(result);

        }

        public bool CanRefresh()
        {
            return this.client.HasLink("self");
        }

        public async Task<HalEndpointDoc> GetRefreshDocs()
        {
            var result = await this.client.LoadLinkDoc("self");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasRefreshDocs()
        {
            return this.client.HasLinkDoc("self");
        }

        public async Task<ThingyCollectionViewResult> ListThingies(PagedCollectionQuery query)
        {
            var result = await this.client.LoadLinkWithQuery("listThingies", query);
            return new ThingyCollectionViewResult(result);

        }

        public bool CanListThingies()
        {
            return this.client.HasLink("listThingies");
        }

        public async Task<HalEndpointDoc> GetListThingiesDocs()
        {
            var result = await this.client.LoadLinkDoc("listThingies");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasListThingiesDocs()
        {
            return this.client.HasLinkDoc("listThingies");
        }

        public async Task<ThingyViewResult> AddThingy(ThingyView data)
        {
            var result = await this.client.LoadLinkWithBody("addThingy", data);
            return new ThingyViewResult(result);

        }

        public bool CanAddThingy()
        {
            return this.client.HasLink("addThingy");
        }

        public async Task<HalEndpointDoc> GetAddThingyDocs()
        {
            var result = await this.client.LoadLinkDoc("addThingy");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasAddThingyDocs()
        {
            return this.client.HasLinkDoc("addThingy");
        }

        public async Task<ThingyCollectionViewResult> Next()
        {
            var result = await this.client.LoadLink("next");
            return new ThingyCollectionViewResult(result);

        }

        public bool CanNext()
        {
            return this.client.HasLink("next");
        }

        public async Task<HalEndpointDoc> GetNextDocs()
        {
            var result = await this.client.LoadLinkDoc("next");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasNextDocs()
        {
            return this.client.HasLinkDoc("next");
        }

        public async Task<ThingyCollectionViewResult> Previous()
        {
            var result = await this.client.LoadLink("previous");
            return new ThingyCollectionViewResult(result);

        }

        public bool CanPrevious()
        {
            return this.client.HasLink("previous");
        }

        public async Task<HalEndpointDoc> GetPreviousDocs()
        {
            var result = await this.client.LoadLinkDoc("previous");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasPreviousDocs()
        {
            return this.client.HasLinkDoc("previous");
        }

        public async Task<ThingyCollectionViewResult> First()
        {
            var result = await this.client.LoadLink("first");
            return new ThingyCollectionViewResult(result);

        }

        public bool CanFirst()
        {
            return this.client.HasLink("first");
        }

        public async Task<HalEndpointDoc> GetFirstDocs()
        {
            var result = await this.client.LoadLinkDoc("first");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasFirstDocs()
        {
            return this.client.HasLinkDoc("first");
        }

        public async Task<ThingyCollectionViewResult> Last()
        {
            var result = await this.client.LoadLink("last");
            return new ThingyCollectionViewResult(result);

        }

        public bool CanLast()
        {
            return this.client.HasLink("last");
        }

        public async Task<HalEndpointDoc> GetLastDocs()
        {
            var result = await this.client.LoadLinkDoc("last");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasLastDocs()
        {
            return this.client.HasLinkDoc("last");
        }
    }

    public class ThingyViewResult
    {
        private HalEndpointClient client;

        public ThingyViewResult(HalEndpointClient client)
        {
            this.client = client;
        }

        private ThingyView strongData = default(ThingyView);
        public ThingyView Data
        {
            get
            {
                if (this.strongData == default(ThingyView))
                {
                    this.strongData = this.client.GetData<ThingyView>();
                }
                return this.strongData;
            }
        }

        public async Task<ThingyViewResult> Refresh()
        {
            var result = await this.client.LoadLink("self");
            return new ThingyViewResult(result);

        }

        public bool CanRefresh()
        {
            return this.client.HasLink("self");
        }

        public async Task<HalEndpointDoc> GetRefreshDocs()
        {
            var result = await this.client.LoadLinkDoc("self");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasRefreshDocs()
        {
            return this.client.HasLinkDoc("self");
        }

        public async Task<ThingyViewResult> GetThingy()
        {
            var result = await this.client.LoadLink("getThingy");
            return new ThingyViewResult(result);

        }

        public bool CanGetThingy()
        {
            return this.client.HasLink("getThingy");
        }

        public async Task<HalEndpointDoc> GetGetThingyDocs()
        {
            var result = await this.client.LoadLinkDoc("getThingy");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasGetThingyDocs()
        {
            return this.client.HasLinkDoc("getThingy");
        }

        public async Task<ThingyViewResult> UpdateThingy(ThingyView data)
        {
            var result = await this.client.LoadLinkWithBody("updateThingy", data);
            return new ThingyViewResult(result);

        }

        public bool CanUpdateThingy()
        {
            return this.client.HasLink("updateThingy");
        }

        public async Task<HalEndpointDoc> GetUpdateThingyDocs()
        {
            var result = await this.client.LoadLinkDoc("updateThingy");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasUpdateThingyDocs()
        {
            return this.client.HasLinkDoc("updateThingy");
        }

        public async Task DeleteThingy()
        {
            var result = await this.client.LoadLink("deleteThingy");
        }

        public bool CanDeleteThingy()
        {
            return this.client.HasLink("deleteThingy");
        }

        public async Task<SubThingyCollectionViewResult> ListThingySubThingies()
        {
            var result = await this.client.LoadLink("listThingySubThingies");
            return new SubThingyCollectionViewResult(result);

        }

        public bool CanListThingySubThingies()
        {
            return this.client.HasLink("listThingySubThingies");
        }

        public async Task<HalEndpointDoc> GetListThingySubThingiesDocs()
        {
            var result = await this.client.LoadLinkDoc("listThingySubThingies");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasListThingySubThingiesDocs()
        {
            return this.client.HasLinkDoc("listThingySubThingies");
        }

        public async Task<SubThingyViewResult> AddSubThingy(SubThingyView data)
        {
            var result = await this.client.LoadLinkWithBody("addSubThingy", data);
            return new SubThingyViewResult(result);

        }

        public bool CanAddSubThingy()
        {
            return this.client.HasLink("addSubThingy");
        }

        public async Task<HalEndpointDoc> GetAddSubThingyDocs()
        {
            var result = await this.client.LoadLinkDoc("addSubThingy");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasAddSubThingyDocs()
        {
            return this.client.HasLinkDoc("addSubThingy");
        }

        public async Task AuthorizedpropertiesThingies()
        {
            var result = await this.client.LoadLink("authorizedpropertiesThingies");
        }

        public bool CanAuthorizedpropertiesThingies()
        {
            return this.client.HasLink("authorizedpropertiesThingies");
        }

        public async Task RolepropertiesThingies()
        {
            var result = await this.client.LoadLink("rolepropertiesThingies");
        }

        public bool CanRolepropertiesThingies()
        {
            return this.client.HasLink("rolepropertiesThingies");
        }

        public async Task TestDeclareLinkToRel(PagedCollectionQuery query)
        {
            var result = await this.client.LoadLinkWithQuery("testDeclareLinkToRel", query);
        }

        public bool CanTestDeclareLinkToRel()
        {
            return this.client.HasLink("testDeclareLinkToRel");
        }

        public async Task<HalEndpointDoc> GetTestDeclareLinkToRelDocs()
        {
            var result = await this.client.LoadLinkDoc("testDeclareLinkToRel");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasTestDeclareLinkToRelDocs()
        {
            return this.client.HasLinkDoc("testDeclareLinkToRel");
        }
    }

    public class MultipartInput1Result
    {
        private HalEndpointClient client;

        public MultipartInput1Result(HalEndpointClient client)
        {
            this.client = client;
        }

        private MultipartInput1 strongData = default(MultipartInput1);
        public MultipartInput1 Data
        {
            get
            {
                if (this.strongData == default(MultipartInput1))
                {
                    this.strongData = this.client.GetData<MultipartInput1>();
                }
                return this.strongData;
            }
        }

        public async Task<MultipartInput1Result> Previous()
        {
            var result = await this.client.LoadLink("previous");
            return new MultipartInput1Result(result);

        }

        public bool CanPrevious()
        {
            return this.client.HasLink("previous");
        }

        public async Task<HalEndpointDoc> GetPreviousDocs()
        {
            var result = await this.client.LoadLinkDoc("previous");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasPreviousDocs()
        {
            return this.client.HasLinkDoc("previous");
        }

        public async Task<MultipartInput2Result> Next()
        {
            var result = await this.client.LoadLink("next");
            return new MultipartInput2Result(result);

        }

        public bool CanNext()
        {
            return this.client.HasLink("next");
        }

        public async Task<HalEndpointDoc> GetNextDocs()
        {
            var result = await this.client.LoadLinkDoc("next");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasNextDocs()
        {
            return this.client.HasLinkDoc("next");
        }

        public async Task<MultipartInput1Result> Save(MultipartInput1 data)
        {
            var result = await this.client.LoadLinkWithBody("save", data);
            return new MultipartInput1Result(result);

        }

        public bool CanSave()
        {
            return this.client.HasLink("save");
        }

        public async Task<HalEndpointDoc> GetSaveDocs()
        {
            var result = await this.client.LoadLinkDoc("save");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasSaveDocs()
        {
            return this.client.HasLinkDoc("save");
        }
    }

    public class MultipartInput2Result
    {
        private HalEndpointClient client;

        public MultipartInput2Result(HalEndpointClient client)
        {
            this.client = client;
        }

        private MultipartInput2 strongData = default(MultipartInput2);
        public MultipartInput2 Data
        {
            get
            {
                if (this.strongData == default(MultipartInput2))
                {
                    this.strongData = this.client.GetData<MultipartInput2>();
                }
                return this.strongData;
            }
        }

        public async Task<MultipartInput1Result> Previous()
        {
            var result = await this.client.LoadLink("previous");
            return new MultipartInput1Result(result);

        }

        public bool CanPrevious()
        {
            return this.client.HasLink("previous");
        }

        public async Task<HalEndpointDoc> GetPreviousDocs()
        {
            var result = await this.client.LoadLinkDoc("previous");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasPreviousDocs()
        {
            return this.client.HasLinkDoc("previous");
        }

        public async Task<MultipartInput3Result> Next()
        {
            var result = await this.client.LoadLink("next");
            return new MultipartInput3Result(result);

        }

        public bool CanNext()
        {
            return this.client.HasLink("next");
        }

        public async Task<HalEndpointDoc> GetNextDocs()
        {
            var result = await this.client.LoadLinkDoc("next");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasNextDocs()
        {
            return this.client.HasLinkDoc("next");
        }

        public async Task<MultipartInput2Result> Save(MultipartInput2 data)
        {
            var result = await this.client.LoadLinkWithBody("save", data);
            return new MultipartInput2Result(result);

        }

        public bool CanSave()
        {
            return this.client.HasLink("save");
        }

        public async Task<HalEndpointDoc> GetSaveDocs()
        {
            var result = await this.client.LoadLinkDoc("save");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasSaveDocs()
        {
            return this.client.HasLinkDoc("save");
        }
    }

    public class MultipartInput3Result
    {
        private HalEndpointClient client;

        public MultipartInput3Result(HalEndpointClient client)
        {
            this.client = client;
        }

        private MultipartInput3 strongData = default(MultipartInput3);
        public MultipartInput3 Data
        {
            get
            {
                if (this.strongData == default(MultipartInput3))
                {
                    this.strongData = this.client.GetData<MultipartInput3>();
                }
                return this.strongData;
            }
        }

        public async Task<MultipartInput2Result> Previous()
        {
            var result = await this.client.LoadLink("previous");
            return new MultipartInput2Result(result);

        }

        public bool CanPrevious()
        {
            return this.client.HasLink("previous");
        }

        public async Task<HalEndpointDoc> GetPreviousDocs()
        {
            var result = await this.client.LoadLinkDoc("previous");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasPreviousDocs()
        {
            return this.client.HasLinkDoc("previous");
        }

        public async Task<MultipartInput3Result> Next()
        {
            var result = await this.client.LoadLink("next");
            return new MultipartInput3Result(result);

        }

        public bool CanNext()
        {
            return this.client.HasLink("next");
        }

        public async Task<HalEndpointDoc> GetNextDocs()
        {
            var result = await this.client.LoadLinkDoc("next");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasNextDocs()
        {
            return this.client.HasLinkDoc("next");
        }

        public async Task<MultipartInput3Result> Save(MultipartInput3 data)
        {
            var result = await this.client.LoadLinkWithBody("save", data);
            return new MultipartInput3Result(result);

        }

        public bool CanSave()
        {
            return this.client.HasLink("save");
        }

        public async Task<HalEndpointDoc> GetSaveDocs()
        {
            var result = await this.client.LoadLinkDoc("save");
            return result.GetData<HalEndpointDoc>();
        }

        public bool HasSaveDocs()
        {
            return this.client.HasLinkDoc("save");
        }
    }
}
//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v9.4.5.0 (http://NJsonSchema.org)
// </auto-generated>
//----------------------

namespace TestHalcyonApi.ServiceClient
{
#pragma warning disable // Disable all warnings

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.4.5.0")]
    public partial class EntryPoints
    {
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static EntryPoints FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<EntryPoints>(data);
        }
    }

    /// <summary>Default implementation of ICollectionQuery.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.4.5.0")]
    public partial class PagedCollectionQuery
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

        public static PagedCollectionQuery FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<PagedCollectionQuery>(data);
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.4.5.0")]
    public partial class ThingyCollectionView
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

        public static ThingyCollectionView FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ThingyCollectionView>(data);
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.4.5.0")]
    public partial class ComplexObject
    {
        [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("number", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Number { get; set; }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static ComplexObject FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ComplexObject>(data);
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.4.5.0")]
    public partial class ThingyView
    {
        [Newtonsoft.Json.JsonProperty("complexObjects", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.ObjectModel.ObservableCollection<ComplexObject> ComplexObjects { get; set; }

        [Newtonsoft.Json.JsonProperty("thingyId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int ThingyId { get; set; }

        [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Name { get; set; }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static ThingyView FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ThingyView>(data);
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.4.5.0")]
    public partial class MultipartInput1
    {
        [Newtonsoft.Json.JsonProperty("part1Number", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Part1Number { get; set; }

        [Newtonsoft.Json.JsonProperty("part1String", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Part1String { get; set; }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static MultipartInput1 FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MultipartInput1>(data);
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.4.5.0")]
    public partial class SingleFileInput
    {
        [Newtonsoft.Json.JsonProperty("daFile", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public byte[] DaFile { get; set; }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static SingleFileInput FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SingleFileInput>(data);
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.4.5.0")]
    public partial class MultiFileInput
    {
        [Newtonsoft.Json.JsonProperty("daFile", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.ObjectModel.ObservableCollection<byte[]> DaFile { get; set; }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static MultiFileInput FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MultiFileInput>(data);
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.4.5.0")]
    public partial class SubThingyCollectionView
    {
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static SubThingyCollectionView FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SubThingyCollectionView>(data);
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.4.5.0")]
    public enum TestEnum
    {
        [System.Runtime.Serialization.EnumMember(Value = "TestValue1")]
        TestValue1 = 0,

        [System.Runtime.Serialization.EnumMember(Value = "TestValue2")]
        TestValue2 = 1,

        [System.Runtime.Serialization.EnumMember(Value = "TestValue3")]
        TestValue3 = 2,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.4.5.0")]
    public partial class SubThingyView
    {
        [Newtonsoft.Json.JsonProperty("otherWeirdThing", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid OtherWeirdThing { get; set; }

        [Newtonsoft.Json.JsonProperty("subThingyId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int SubThingyId { get; set; }

        [Newtonsoft.Json.JsonProperty("amount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal Amount { get; set; }

        [Newtonsoft.Json.JsonProperty("thingyId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int ThingyId { get; set; }

        [Newtonsoft.Json.JsonProperty("enumTest", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string EnumTest { get; set; }

        [Newtonsoft.Json.JsonProperty("enumTestNullable", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string EnumTestNullable { get; set; }

        [Newtonsoft.Json.JsonProperty("enumTestNullableRelabel", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string EnumTestNullableRelabel { get; set; }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static SubThingyView FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SubThingyView>(data);
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.4.5.0")]
    public partial class MultipartInput2
    {
        [Newtonsoft.Json.JsonProperty("part2Number", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Part2Number { get; set; }

        [Newtonsoft.Json.JsonProperty("part2String", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Part2String { get; set; }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static MultipartInput2 FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MultipartInput2>(data);
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.4.5.0")]
    public partial class MultipartInput3
    {
        [Newtonsoft.Json.JsonProperty("part3Number", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Part3Number { get; set; }

        [Newtonsoft.Json.JsonProperty("part3String", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Part3String { get; set; }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static MultipartInput3 FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MultipartInput3>(data);
        }
    }
}

