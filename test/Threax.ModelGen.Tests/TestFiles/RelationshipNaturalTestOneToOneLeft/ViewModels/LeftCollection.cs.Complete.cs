using Halcyon.HAL.Attributes;
using Test.Controllers.Api;
using Test.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace Test.ViewModels
{
    [HalModel]
    [HalSelfActionLink(typeof(LeftsController), nameof(LeftsController.List))]
    [HalActionLink(typeof(LeftsController), nameof(LeftsController.Get), DocsOnly = true)] //This provides access to docs for showing items
    [HalActionLink(typeof(LeftsController), nameof(LeftsController.List), DocsOnly = true)] //This provides docs for searching the list
    [HalActionLink(typeof(LeftsController), nameof(LeftsController.Update), DocsOnly = true)] //This provides access to docs for updating items if the ui has different modes
    [HalActionLink(typeof(LeftsController), nameof(LeftsController.Add))]
    [DeclareHalLink(typeof(LeftsController), nameof(LeftsController.List), PagedCollectionView<Object>.Rels.Next, ResponseOnly = true)]
    [DeclareHalLink(typeof(LeftsController), nameof(LeftsController.List), PagedCollectionView<Object>.Rels.Previous, ResponseOnly = true)]
    [DeclareHalLink(typeof(LeftsController), nameof(LeftsController.List), PagedCollectionView<Object>.Rels.First, ResponseOnly = true)]
    [DeclareHalLink(typeof(LeftsController), nameof(LeftsController.List), PagedCollectionView<Object>.Rels.Last, ResponseOnly = true)]
    public partial class LeftCollection : PagedCollectionViewWithQuery<Left, LeftQuery>
    {
        public LeftCollection(LeftQuery query, int total, IEnumerable<Left> items) : base(query, total, items)
        {
            
        }
    }
}