using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using Panic.Model;

namespace Panic.Repository.SQLRepository
{
  public class HardwareSQL : IHardwareRepository
  {
    #region Private Variables
    private SqlCeConnection connection;    
    private Panic.Repository.Linq.Panic context;
    private Dictionary<int, Hardware> data;
    #endregion

    #region Constructor

    /// <summary>
    /// Creates a Site Repository which contains the sites
    /// </summary>
    /// <param name="aConnection"></param>
    public HardwareSQL(SqlCeConnection aConnection)
    {
      connection = aConnection;
      context = new Linq.Panic(aConnection);
      data = new Dictionary<int, Hardware>();
    }

    #endregion

    public event AddedHardwareHandler AddedHardware;

    public List<Hardware> GetAll()
    {
      return data.Values.ToList();
    }

    public List<Hardware> Populate()
    {
      var result = from n in context.Hardware
                   select n;
      foreach (Panic.Repository.Linq.Hardware s in result)
      {
        Hardware entity = BuildHardware(s);
        Add(entity);
      }
      return data.Values.ToList<Hardware>();
    }

    public Hardware GetByID(int id)
    {
      if (data.ContainsKey(id))
      {
        return data[id];
      }
      else
      {
        throw new IndexOutOfRangeException("Hardware (" + id.ToString() + ") does not exist");
      }
    }

    public bool Add(Hardware entity)
    {
      bool added = false;
      if (entity.ID == 0)
      {
        entity.SetID(InsertHardware(entity));
      }
      if (!data.ContainsKey(entity.ID))
      {
        data.Add(entity.ID, entity);
        if (AddedHardware != null)
        {
          AddedHardware(entity);
        }
        added = true;
      }
      return added;
    }

    public bool Remove(Hardware entity)
    {
      bool removed = false;
      if (data.ContainsKey(entity.ID))
      {
        data.Remove(entity.ID);
        removed = true;
      }
      return removed;
    }

    #region Private Methods

    private int InsertHardware(Hardware aHardware)
    {
      int lastID = 0;
      string query = string.Format(@"INSERT INTO HARDWARE
        (Description, Make, ModelNumber, TXCapacity, RXCapacity, Enabled, Notes)
        VALUES
        ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
        aHardware.Description,
        aHardware.Make,
        aHardware.ModelNumber,
        aHardware.TXCapacity,
        aHardware.RXCapacity,
        aHardware.Enabled,
        aHardware.Notes);
      SqlCeCommand command = new SqlCeCommand(query, connection);
      connection.Open();
      int result = command.ExecuteNonQuery();
      if (result > 0)
      {
        SqlCeCommand command1 = new SqlCeCommand("select @@identity ", connection);
        lastID = Convert.ToInt32(command1.ExecuteScalar());
      }
      connection.Close();
      return lastID;
    }

    private Hardware BuildHardware(Panic.Repository.Linq.Hardware aHardware)
    {
      Hardware hardware = new Hardware(aHardware.HardwareID)
      {
        Enabled = (bool)aHardware.Enabled,
        Description = aHardware.Description,
        Make = aHardware.Make,
        ModelNumber = aHardware.ModelNumber,
        RXCapacity = (double)aHardware.RXCapacity,
        TXCapacity = (double)aHardware.TXCapacity,
        Notes = aHardware.Notes       
      };
      return hardware;
    }

    #endregion
  }
}
