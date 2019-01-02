using Halcyon.HAL.Attributes;
using Test.Controllers.Api;
using Test.Models;
using Test.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace Test.ViewModels
{
    [HalModel]
    [HalSelfActionLink(typeof(RightsController), nameof(RightsController.List))]
    [HalActionLink(typeof(RightsController), nameof(RightsController.Get), DocsOnly = true)] //This provides access to docs for showing items
    [HalActionLink(typeof(RightsController), nameof(RightsController.List), DocsOnly = true)] //This provides docs for searching the list
    [HalActionLink(typeof(RightsController), nameof(RightsController.Update), DocsOnly = true)] //This provides access to docs for updating items if the ui has different modes
    [HalActionLink(typeof(RightsController), nameof(RightsController.Add))]
    [DeclareHalLink(typeof(RightsController), nameof(RightsController.List), PagedCollectionView<Object>.Rels.Next, ResponseOnly = true)]
    [DeclareHalLink(typeof(RightsController), nameof(RightsController.List), PagedCollectionView<Object>.Rels.Previous, ResponseOnly = true)]
    [DeclareHalLink(typeof(RightsController), nameof(RightsController.List), PagedCollectionView<Object>.Rels.First, ResponseOnly = true)]
    [DeclareHalLink(typeof(RightsController), nameof(RightsController.List), PagedCollectionView<Object>.Rels.Last, ResponseOnly = true)]
    public partial class RightCollection : PagedCollectionViewWithQuery<Right, RightQuery>
    {
        public RightCollection(RightQuery query, int total, IEnumerable<Right> items) : base(query, total, items)
        {
            
        }
    }
}