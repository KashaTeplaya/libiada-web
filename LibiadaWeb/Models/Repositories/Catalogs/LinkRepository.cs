namespace LibiadaWeb.Models.Repositories.Catalogs
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// The link repository.
    /// </summary>
    public class LinkRepository : ILinkRepository
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LibiadaWebEntities db;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkRepository"/> class.
        /// </summary>
        /// <param name="db">
        /// The db.
        /// </param>
        public LinkRepository(LibiadaWebEntities db)
        {
            this.db = db;
        }

        /// <summary>
        /// The get select list items.
        /// </summary>
        /// <param name="links">
        /// The links.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<SelectListItem> GetSelectListItems(IEnumerable<Link> links)
        {
            HashSet<int> linkIds = links != null
                                         ? new HashSet<int>(links.Select(c => c.Id))
                                         : new HashSet<int>();
            var allLinks = db.Link;
            var linksList = new List<SelectListItem>();
            foreach (var link in allLinks)
            {
                linksList.Add(new SelectListItem
                    {
                        Value = link.Id.ToString(), 
                        Text = link.Name, 
                        Selected = linkIds.Contains(link.Id)
                    });
            }

            return linksList;
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
