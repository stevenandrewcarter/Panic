using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using Panic.Model;

namespace Panic.Repository.SQLRepository {
  public class SiteSQL : ISiteRepository {
    #region Private Variables
    private Panic.Repository.Linq.Panic context;
    private SqlCeConnection connection;
    private Dictionary<int, Site> data;
    #endregion

    #region Constructor

    /// <summary>
    /// Creates a Site Repository which contains the sites
    /// </summary>
    /// <param name="aConnection"></param>
    public SiteSQL(SqlCeConnection aConnection) {
      connection = aConnection;
      context = new Linq.Panic(aConnection);
      data = new Dictionary<int, Model.Site>();
    }

    #endregion

    #region Events
    public event SitesChangedHandler SitesChanged;
    #endregion

    public List<Site> GetAll() {
      return data.Values.ToList();
    }

    public List<Site> Populate() {
      var result = from n in context.Site
                   select n;
      foreach (Panic.Repository.Linq.Site s in result) {
        Site site = BuildSite(s);
        Add(site);
      }
      return data.Values.ToList<Site>();
    }

    public Site GetByID(int id) {
      if (data.ContainsKey(id)) {
        return data[id];
      } else {
        throw new IndexOutOfRangeException("Site (" + id.ToString() + ") does not exist");
      }
    }

    public bool Add(Site entity) {
      bool added = false;
      if (entity.ID == 0) {
        // Insert a new site
        entity.SetID(InsertSite(entity));
      }
      if (!data.ContainsKey(entity.ID)) {
        data.Add(entity.ID, entity);
        if (SitesChanged != null) {
          SitesChanged(entity);
        }
        entity.Changed += new SiteChangedEvent(entity_Changed);
        added = true;
      }
      return added;
    }

    private void entity_Changed(Site aSite) {
      // Update the site in the Database
      if (UpdateSite(aSite)) {
        if (SitesChanged != null) {
          SitesChanged(aSite);
        }
      }
    }

    public bool Remove(Site entity) {
      bool removed = false;
      if (data.ContainsKey(entity.ID)) {
        data.Remove(entity.ID);
        removed = true;
      }
      return removed;
    }

    #region Private Methods

    private bool UpdateSite(Site aSite) {
      string query = string.Format(@"UPDATE SITE
        SET
          SiteName = '{0}', 
          Latitude = {1}, 
          Longitude = {2}, 
          LocalTX = {3}, 
          LocalRX = {4}, 
          Enabled = '{5}', 
          Notes = '{6}'
        WHERE
         SiteID = {7}",
        aSite.Name,
        aSite.Latitude,
        aSite.Longitude,
        aSite.LocalTX,
        aSite.LocalRX,
        aSite.Enabled,
        aSite.Notes,
        aSite.ID);
      SqlCeCommand command = new SqlCeCommand(query, connection);
      connection.Open();
      int result = command.ExecuteNonQuery();
      connection.Close();
      return result > 0;
    }

    private int InsertSite(Site aSite) {
      int lastID = 0;
      string query = string.Format(@"INSERT INTO SITE
        (SiteName, Latitude, Longitude, LocalTX, LocalRX, Enabled, Notes)
        VALUES
        ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
        aSite.Name,
        aSite.Latitude,
        aSite.Longitude,
        aSite.LocalTX,
        aSite.LocalRX,
        aSite.Enabled,
        aSite.Notes);
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

    private Site BuildSite(Panic.Repository.Linq.Site aSite) {
      Site site = new Site(aSite.SiteID, aSite.SiteName) {
        Latitude = (double)aSite.Latitude,
        Longitude = (double)aSite.Longitude,
        LocalRX = (double)aSite.LocalRX,
        LocalTX = (double)aSite.LocalTX,
        Enabled = (bool)aSite.Enabled
      };
      return site;
    }

    #endregion
  }
}
