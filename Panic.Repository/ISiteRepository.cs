using System.Collections.Generic;
using Panic.Model;

namespace Panic.Repository {
  // A delegate type for hooking up change notifications.
  public delegate void SitesChangedHandler(Site aSite);

  public interface ISiteRepository : IRepository<Site> {
    #region Events
    event SitesChangedHandler SitesChanged;
    #endregion
    List<Model.Site> Populate();
  }
}
