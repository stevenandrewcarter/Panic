using System.Collections.Generic;
using Panic.Model;

namespace Panic.Repository
{
  // A delegate type for hooking up change notifications.
  public delegate void LinksChangedHandler(Link aLink);

  public interface ILinkRepository : IRepository<Link>
  {
    #region Events
    event LinksChangedHandler LinksChanged;
    #endregion

    List<Link> Populate();
  }
}
