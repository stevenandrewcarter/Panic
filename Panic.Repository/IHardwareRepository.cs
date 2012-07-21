using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Panic.Model;

namespace Panic.Repository
{
  public delegate void AddedHardwareHandler(Hardware aHardware);

  public interface IHardwareRepository : IRepository<Hardware>
  {
    #region Events
    event AddedHardwareHandler AddedHardware;
    #endregion

    List<Hardware> Populate();

    List<Hardware> GetAll();
  }
}
