using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Client
{
    /// <summary>
    /// This class makes it easier to wrap paged results and get all the items
    /// across all pages. The algorithm is pretty abstract and you must provide
    /// functions to move to the next page and to get the current page's items.
    /// It works the same as a normal .net Enumerator where the pointer starts
    /// before the first element. The biggest difference is MoveNext is async
    /// since it might have to load a new page.
    /// </summary>
    /// <typeparam name="TPage">The type of the page class.</typeparam>
    /// <typeparam name="TItem">The type of the page's item class.</typeparam>
    public class PageEnumerator<TPage, TItem>
        where TPage : class
    {
        private Func<TPage, Task<TPage>> getNextPage;
        private Func<TPage, IEnumerable<TItem>> getItems;
        private IEnumerator<TItem> pageItems;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="firstPage">The first page of data to enumerate over.</param>
        /// <param name="getNextPage">A function that will return a Task to get a page or null to signify the last page has been reached. Can return null directly, it does not need to be wrapped in a Task.FromResult.</param>
        /// <param name="getItems">Get the items for a page of data. Null can be returned, which will force the iterator onto the next page.</param>
        public PageEnumerator(TPage firstPage, Func<TPage, Task<TPage>> getNextPage, Func<TPage, IEnumerable<TItem>> getItems)
        {
            this.getNextPage = getNextPage;
            this.getItems = getItems;
            this.CurrentPage = firstPage;
        }

        /// <summary>
        /// The current item.
        /// </summary>
        public TItem Current
        {
            get
            {
                return pageItems.Current;
            }
        }

        public TPage CurrentPage { get; private set; }

        public async Task<bool> MoveNext()
        {
            if (CurrentPage == null)
            {
                //If the current page is null we are past the end of the iter and are finished
                return false;
            }

            if (pageItems == null)
            {
                pageItems = getItems(CurrentPage).GetEnumerator();
            }

            if (pageItems == null || !pageItems.MoveNext())
            {
                //End of current page, load next
                var pageTask = getNextPage(CurrentPage);
                if (pageTask != null)
                {
                    CurrentPage = await pageTask;
                }
                else
                {
                    CurrentPage = default(TPage);
                }
                pageItems = null;
                return await MoveNext();
            }

            return true;
        }
    }
}
