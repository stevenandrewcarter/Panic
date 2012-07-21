using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Panic.Model
{
  public delegate void LinkChangedEvent(Link aLink);

  public class Link
  {
    private int fromSiteID;
    private int toSiteID;
    private int hardwareID;
    private bool enabled;
    private double rxOverride;
    private double txOverride;

    #region Constructor

    public Link(int aID)
    {
      ID = aID;
    }

    #endregion

    #region Events

    public event LinkChangedEvent Changed;

    #endregion

    public int ID { get; private set; }    

    public bool Enabled 
    {
      get { return enabled; }
      set
      {
        enabled = value;
        if (Changed != null) { Changed(this); }
      }
    }

    public int FromSiteID 
    {
      get { return fromSiteID; }
      set
      {
        fromSiteID = value;
        if (Changed != null) { Changed(this); }
      }
    }

    public int ToSiteID
    {
      get { return toSiteID; }
      set
      {
        toSiteID = value;
        if (Changed != null) { Changed(this); }
      }
    }

    public int HardwareID 
    {
      get { return hardwareID; }
      set
      {
        hardwareID = value;
        if (Changed != null) { Changed(this); }
      }
    }

    public string Notes { get; set; }

    public double RXOverride 
    {
      get { return rxOverride; }
      set
      {
        rxOverride = value;
        if (Changed != null) { Changed(this); }
      }
    }

    public double TXOverride 
    {
      get { return txOverride; }
      set
      {
        txOverride = value;
        if (Changed != null) { Changed(this); }
      }
    }

    public void SetID(int aID)
    {
      if (ID == 0)
      {
        ID = aID;
      }
    }
  }
}
