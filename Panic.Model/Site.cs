using System;

namespace Panic.Model {
  public delegate void SiteChangedEvent(Site aSite);

  public class Site {
    #region Constructor

    public Site(int aID, string aName) {
      id = aID;
      name = aName;
    }

    #endregion

    #region Events

    public event SiteChangedEvent Changed;

    #endregion

    #region Properties

    public int ID {
      get { return id; }
      private set { id = value; }
    }

    public String Name {
      get { return name; }
      set {
        name = value;
        if (Changed != null) { Changed(this); }
      }
    }

    public double Latitude {
      get { return latitude; }
      set {
        latitude = value;
        if (Changed != null) { Changed(this); }
      }
    }

    public double Longitude {
      get { return longitude; }
      set {
        longitude = value;
        if (Changed != null) { Changed(this); }
      }
    }

    public double LocalTX {
      get { return localTX; }
      set {
        localTX = value;
        if (Changed != null) { Changed(this); }
      }
    }

    public double LocalRX {
      get { return localRX; }
      set {
        localRX = value;
        if (Changed != null) { Changed(this); }
      }
    }

    public bool Enabled {
      get { return enabled; }
      set {
        enabled = value;
        if (Changed != null) { Changed(this); }
      }
    }

    public String Notes {
      get { return notes; }
      set {
        notes = value;
        if (Changed != null) { Changed(this); }
      }
    }

    #endregion

    #region Private Variables

    private int id;
    private string name;
    private double latitude;
    private double longitude;
    private double localTX;
    private double localRX;
    private bool enabled;
    private string notes;

    #endregion

    #region Public Methods

    public void SetID(int aID) {
      if (id == 0) {
        ID = aID;
      }
    }

    #endregion

    public override string ToString() {
      return name;
    }
  }
}
