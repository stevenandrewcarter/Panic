using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using Panic.Model;

namespace Panic.Repository.SQLRepository {
  public class LinkSQL : ILinkRepository {
    #region Private Variables
    private Panic.Repository.Linq.Panic context;
    private SqlCeConnection connection;
    private Dictionary<int, Link> data;
    #endregion

    #region Constructor

    /// <summary>
    /// Creates a Site Repository which contains the sites
    /// </summary>
    /// <param name="aConnection"></param>
    public LinkSQL(SqlCeConnection aConnection) {
      connection = aConnection;
      context = new Linq.Panic(aConnection);
      data = new Dictionary<int, Link>();
    }

    #endregion

    #region Events
    public event LinksChangedHandler LinksChanged;
    #endregion


    public List<Link> GetAll() {
      return data.Values.ToList();
    }

    public List<Link> Populate() {
      var result = from n in context.Link
                   select n;
      foreach (Panic.Repository.Linq.Link s in result) {
        Link entity = BuildLink(s);
        Add(entity);
      }
      return data.Values.ToList<Link>();
    }

    public Link GetByID(int id) {
      if (data.ContainsKey(id)) {
        return data[id];
      } else {
        throw new IndexOutOfRangeException("Link (" + id.ToString() + ") does not exist");
      }
    }

    public bool Add(Link entity) {
      bool added = false;
      if (entity.ID == 0) {
        entity.SetID(InsertLink(entity));
      }
      if (!data.ContainsKey(entity.ID)) {
        data.Add(entity.ID, entity);
        if (LinksChanged != null) {
          LinksChanged(entity);
        }
        entity.Changed += new LinkChangedEvent(entity_Changed);
        added = true;
      }
      return added;
    }

    private void entity_Changed(Link aLink) {
      // Update the site in the Database
      if (UpdateLink(aLink)) {
        if (LinksChanged != null) {
          LinksChanged(aLink);
        }
      }
    }

    public bool Remove(Link entity) {
      bool removed = false;
      if (data.ContainsKey(entity.ID)) {
        data.Remove(entity.ID);
        removed = true;
      }
      return removed;
    }

    #region Private Methods

    private bool UpdateLink(Link aLink) {
      string query = string.Format(@"UPDATE LINK
        SET
          FromSiteID = '{0}', 
          ToSiteID = {1}, 
          HardwareID = {2}, 
          TXOverride = {3}, 
          RXOverride = {4}, 
          Enabled = '{5}', 
          Notes = '{6}'
        WHERE
         LinkID = {7}",
        aLink.FromSiteID,
        aLink.ToSiteID,
        aLink.HardwareID,
        aLink.TXOverride,
        aLink.TXOverride,
        aLink.Enabled,
        aLink.Notes,
        aLink.ID);
      SqlCeCommand command = new SqlCeCommand(query, connection);
      connection.Open();
      int result = command.ExecuteNonQuery();
      connection.Close();
      return result > 0;
    }

    private int InsertLink(Link aLink) {
      int lastID = 0;
      string query = string.Format(@"INSERT INTO LINK
        (FromSiteID, ToSiteID, HardwareID, TXOverride, RXOverride, Enabled, Notes)
        VALUES
        ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
        aLink.FromSiteID,
        aLink.ToSiteID,
        aLink.HardwareID,
        aLink.TXOverride,
        aLink.RXOverride,
        aLink.Enabled,
        aLink.Notes);
      SqlCeCommand command = new SqlCeCommand(query, connection);
      connection.Open();
      int result = command.ExecuteNonQuery();
      if (result > 0) {
        SqlCeCommand command1 = new SqlCeCommand("select @@identity ", connection);
        lastID = Convert.ToInt32(command1.ExecuteScalar());
      }
      connection.Close();
      return lastID;
    }

    private Link BuildLink(Panic.Repository.Linq.Link aLink) {
      Link link = new Link(aLink.LinkID) {
        Enabled = (bool)aLink.Enabled,
        FromSiteID = aLink.FromSiteID,
        ToSiteID = aLink.ToSiteID,
        HardwareID = aLink.HardwareID,
        Notes = aLink.Notes,
        RXOverride = (double)aLink.RXOverride,
        TXOverride = (double)aLink.TXOverride
      };
      return link;
    }

    #endregion
  }
}
